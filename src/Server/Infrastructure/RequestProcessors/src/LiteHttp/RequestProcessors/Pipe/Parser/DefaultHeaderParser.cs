namespace LiteHttp.RequestProcessors.PipeContext.Parser;

internal sealed class DefaultHeaderParser : IHeaderParser
{
    /// <inheritdoc />
    public HeaderParsingResult ParseHeader(in ReadOnlySequence<byte> line, HeaderCollection headerCollection)
    {
        // Line integrity is already validated during input reading

        // Request separator line ("\r\n") encountered
        if (line.Length < 3)
            return HeaderParsingResult.StateUpdateRequested;

        var reader = new SequenceReader<byte>(line);

        if (!reader.TryReadTo(out ReadOnlySequence<byte> headerTitleSequence, RequestSymbolsAsBytes.Colon, true))
            return HeaderParsingResult.HeaderSyntaxError;


        if (!reader.TryReadExact((int)reader.UnreadSequence.Length, out var headerValueSequence))
            return HeaderParsingResult.HeaderSyntaxError;

        var headerTitleMemory = headerTitleSequence.GetReadOnlyMemoryFromSequence();
        var headerValueMemory = headerValueSequence.GetReadOnlyMemoryFromSequence();
        Trim(ref headerValueMemory);

        if (headerValueMemory.IsEmpty)
            return HeaderParsingResult.HeaderSyntaxError;

        var addingResult = headerCollection.TryAdd(headerTitleMemory, headerValueMemory);

        if (!addingResult.Success)
            return addingResult.Error;

        return HeaderParsingResult.Successful;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Trim(ref ReadOnlyMemory<byte> memory)
    {
        int current = 0;
        while (current < memory.Length && memory.Span[current] == ' ')
        {
            current += 1;
        }

        memory = memory[current..];

        current = memory.Length - 1; // - 1 to point to the last character
        while (current >= 0 && (memory.Span[current] == RequestSymbolsAsBytes.Space || memory.Span[current] == RequestSymbolsAsBytes.CarriageReturnSymbol || memory.Span[current] == RequestSymbolsAsBytes.NewLine))
        {
            current -= 1;
        }

        //return memory[..current];
        memory = current >= 0 ? memory[..(current + 1)] : ReadOnlyMemory<byte>.Empty;
    }
}
