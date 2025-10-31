namespace LiteHttp.RequestProcessors;

#pragma warning disable CS8618
public class ResponseGenerator : IResponseGenerator, IDisposable
{
    private readonly IMemoryOwner<byte> _owner = MemoryPool<byte>.Shared.Rent(4096);
    private int _length = 0;
    
    public int Port
    {
        get;

        set
        {
            field = value;

            UpdateHost();
        }
    }
    public string Address
    {
        get;

        set
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(value, nameof(value));
            
            field = value;

            UpdateHost();
        }
    }
    
    private ReadOnlyMemory<byte> _host;

    public ResponseGenerator()
    {
        Address = AddressConstants.IPV4_LOOPBACK.ToString();
        Port = AddressConstants.DEFAULT_SERVER_PORT;
    }

    public void Dispose() => 
        _owner.Dispose();
    
    public ReadOnlyMemory<byte> Generate(IActionResult actionResult, ReadOnlyMemory<byte>? responseBody = null)
    {
        ResetMessage();

        var memory = _owner.Memory;

        Append(HttpVersionsAsBytes.Http_1_1);
        Append(actionResult.ResponseCode.AsByteString());
        
        GenerateHeaders(responseBody);

        Append(RequestSymbolsAsBytes.NewRequestLine);

        if (responseBody is not null)
            Append(responseBody.Value);
        
        return memory[.._length];
    }

    private void GenerateHeaders(ReadOnlyMemory<byte>? body)
    {
        var memory = _owner.Memory;
        
        Append(HeadersAsBytes.Host);

        Append(_host);

        if (body is not null)
        {
            Append(RequestSymbolsAsBytes.NewRequestLine);

            Append(HeadersAsBytes.ContentType);

            Append(HeaderValuesAsBytes.ContentTextPlain);

            Append(HeadersAsBytes.ContentLength);

            if (body.Value.Length.TryFormat(memory.Span[_length..], out var written))
                _length += written;
        }

        Append(RequestSymbolsAsBytes.NewRequestLine);
    }

    private void Append(byte[] bytes)
    {
        bytes.CopyTo(_owner.Memory[_length..]);
        _length += bytes.Length;
    }

    private void Append(ReadOnlySpan<byte> bytes)
    {
        bytes.CopyTo(_owner.Memory.Span[_length..]);
        _length += bytes.Length;
    }

    private void Append(ReadOnlyMemory<byte> bytes)
    {
        bytes.CopyTo(_owner.Memory[_length..]);
        _length += bytes.Length;
    }
    
    private void UpdateHost() => _host = Encoding.UTF8.GetBytes($"{Address}:{Port}");

    private void ResetMessage() => _length = 0;
}