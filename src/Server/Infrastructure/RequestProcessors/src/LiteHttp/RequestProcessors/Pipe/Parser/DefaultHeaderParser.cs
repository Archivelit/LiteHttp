namespace LiteHttp.RequestProcessors.PipeContext.Parser;

internal sealed class DefaultHeaderParser : IHeaderParser
{
    /// <inheritdoc />
    public HeaderParsingResult ParseHeader(ReadOnlySequence<byte> line, HeaderCollection headerCollection)
    {
        // Line integrity is already validated during input reading

        // Request separator line ("\r\n") encountered
        if (line.Length < 3)
        {
            return HeaderParsingResult.StateUpdateRequested;
        }

        var reader = new SequenceReader<byte>(line);

        if (!reader.TryReadTo(out ReadOnlySequence<byte> headerTitleSequence, RequestSymbolsAsBytes.Colon, true))
            return HeaderParsingResult.HeaderSyntaxError;

        if (!reader.TryReadTo(out ReadOnlySequence<byte> headerValueSequence, RequestSymbolsAsBytes.CarriageReturnSymbol, true))
            return HeaderParsingResult.HeaderSyntaxError;

        var headerTitleMemory = headerTitleSequence.GetReadOnlyMemoryFromSequence();
        var headerValueMemory = headerValueSequence.GetReadOnlyMemoryFromSequence();
        var addingResult = headerCollection.TryAdd(headerTitleMemory, TrimStart(headerValueMemory));

        if (!addingResult.Success)
            return HeaderParsingResult.TwoSameHeaders;

        return HeaderParsingResult.Successful;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ReadOnlyMemory<byte> TrimStart(ReadOnlyMemory<byte> memory)
    {
        int bytesToSkip = 0, current = 0;
        while (current < memory.Length && memory.Span[current] == ' ')
        {
            bytesToSkip += 1;
            current += 1;
        }

        return memory[bytesToSkip..];
    }
}
