namespace LiteHttp.RequestProcessors;

public sealed class ResponseBuilder
{
    private int _responseLength;

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
            ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

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

    // Body not supported temporarily
    public int Build(IActionResult actionResult, Memory<byte> buffer/*ReadOnlyMemory<byte>? responseBody = null*/)
    {
        ResetMessage();

        Append(buffer, HttpVersionsAsBytes.Http11);
        Append(buffer, actionResult.ResponseCode.AsByteString());

        BuildHeaders(buffer);

        Append(buffer, RequestSymbolsAsBytes.RequestSplitter);

        //if (responseBody is not null)
        //    Append(responseBody.Value);

        return _responseLength;
    }

    private void BuildHeaders(Memory<byte> buffer /*ReadOnlyMemory<byte>? body*/)
    {
        Append(buffer, HeadersAsBytes.Host);

        Append(buffer, _host);

        //if (body is not null)
        //{
        //    Append(RequestSymbolsAsBytes.NewRequestLine);

        //    Append(HeadersAsBytes.ContentType);

        //    Append(HeaderValuesAsBytes.ContentTextPlain);

        //    Append(HeadersAsBytes.ContentLength);

        //    if (body.Value.Length.TryFormat(buffer.Span[_responseLength..], out var written))
        //        _responseLength += written;
        //}
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Append(Memory<byte> buffer, byte[] bytes)
    {
        Debug.Assert(bytes.Length <= buffer.Length - _responseLength);

        bytes.CopyTo(buffer[_responseLength..]);
        _responseLength += bytes.Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Append(Memory<byte> buffer, ReadOnlyMemory<byte> bytes)
    {
        Debug.Assert(bytes.Length <= buffer.Length - _responseLength);

        bytes.CopyTo(buffer[_responseLength..]);
        _responseLength += bytes.Length;
    }

    [SkipLocalsInit]
    private void UpdateHost() => _host = Encoding.UTF8.GetBytes($"{Address}:{Port}");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ResetMessage() => _responseLength = 0;
}