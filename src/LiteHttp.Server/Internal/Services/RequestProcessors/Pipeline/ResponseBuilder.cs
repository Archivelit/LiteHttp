using HttpContext = LiteHttp.Models.PipeContextModels.HttpContext;

namespace LiteHttp.RequestProcessors.Pipeline;

internal sealed class ResponseBuilder
{
    public int Port
    {
        get;

        set
        {
            if (value < 0) 
                throw new ArgumentOutOfRangeException(nameof(value), ExceptionStrings.NonNegativeNumberRequired);

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

    public async ValueTask Build(Pipe requestPipe, HttpContext context, IActionResult actionResult)
    {
        Write(requestPipe.Writer, HttpVersionsAsBytes.Http11);
        Write(requestPipe.Writer, actionResult.ResponseCode.AsByteString());

        BuildHeaders(requestPipe, context);
        Write(requestPipe.Writer, RequestSymbolsAsBytes.RequestSplitter);

        if (context.Body is not null)
            Write(requestPipe.Writer, context.Body.Value);

        await requestPipe.Writer.FlushAsync();
        await requestPipe.Writer.CompleteAsync();
    }

    private void BuildHeaders(Pipe requestPipe, HttpContext context)
    {
        Write(requestPipe.Writer, HeadersAsBytes.Host);

        Write(requestPipe.Writer, _host);

        if (context.Body is null)
            return;

        Write(requestPipe.Writer, RequestSymbolsAsBytes.NewRequestLine);

        Write(requestPipe.Writer, HeadersAsBytes.ContentType);

        Write(requestPipe.Writer, HeaderValuesAsBytes.ContentTextPlain);

        Write(requestPipe.Writer, HeadersAsBytes.ContentLength);

        Write(requestPipe.Writer, Encoding.ASCII.GetBytes(context.Body.Value.Length.ToString()));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Write(PipeWriter writer, ReadOnlyMemory<byte> bytes)
    {
        var buffer = writer.GetMemory(bytes.Length);
        bytes.CopyTo(buffer);
        writer.Advance(bytes.Length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Write(PipeWriter writer, ReadOnlySequence<byte> sequence)
    {
        var sequenceReader = new SequenceReader<byte>(sequence);

        while (sequenceReader.TryRead(out var @byte))
        {
            var span = writer.GetSpan(1);
            span[0] = @byte;
            writer.Advance(1);
        }
    }

    private void UpdateHost() => _host = Encoding.UTF8.GetBytes($"{Address}:{Port}");
}