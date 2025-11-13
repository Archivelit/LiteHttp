namespace LiteHttp.RequestProcessors;

public sealed class ResponseBuilder : IResponseBuilder, IDisposable
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

    public ResponseBuilder()
    {
        Address = AddressConstants.IPV4_LOOPBACK.ToString();
        Port = AddressConstants.DEFAULT_SERVER_PORT;
    }

    public void Dispose() => 
        _owner.Dispose();
    
    public ReadOnlyMemory<byte> Build(IActionResult actionResult, ReadOnlyMemory<byte>? responseBody = null)
    {
        ResetMessage();

        var memory = _owner.Memory;

        Append(HttpVersionsAsBytes.Http_1_1);
        Append(actionResult.ResponseCode.AsByteString());

        BuildHeaders(responseBody);

        Append(RequestSymbolsAsBytes.RequestSplitter);

        if (responseBody is not null)
            Append(responseBody.Value);
        
        return memory[.._length];
    }

    private void BuildHeaders(ReadOnlyMemory<byte>? body)
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
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Append(byte[] bytes)
    {
        Debug.Assert(bytes.Length <= _owner.Memory.Length - _length);

        bytes.CopyTo(_owner.Memory[_length..]);
        _length += bytes.Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Append(ReadOnlySpan<byte> bytes)
    {
        Debug.Assert(bytes.Length <= _owner.Memory.Length - _length);

        bytes.CopyTo(_owner.Memory.Span[_length..]);
        _length += bytes.Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Append(ReadOnlyMemory<byte> bytes)
    {
        Debug.Assert(bytes.Length <= _owner.Memory.Length - _length);

        bytes.CopyTo(_owner.Memory[_length..]);
        _length += bytes.Length;
    }

    [SkipLocalsInit]
    private void UpdateHost() => _host = Encoding.UTF8.GetBytes($"{Address}:{Port}");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ResetMessage() => _length = 0;
}