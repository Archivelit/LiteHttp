namespace LiteHttp.RequestProcessors.PipeContext.Parser;

public interface IHeaderParser
{
    /// <summary>
    /// Parses a single HTTP header line from the provided byte sequence and updates headerCollection if header was parsed.
    /// </summary>
    /// <param name="line">The sequence of bytes representing a single header line to parse. Never called when sequence is not
    /// full line.</param>
    /// <param name="headerCollection">Collection of headers for storing parsed headers. Cannot be null.</param>
    /// <returns>A result indicating the outcome of the header parsing operation. Must return <see cref="HeaderParsingErrors.StateUpdateRequested"/> 
    /// for proper header parsing, otherwise parsing must not work with body. Other <see cref="HeaderParsingErrors"/> can be returned.</returns>
    public HeaderParsingResult ParseHeader(ReadOnlySequence<byte> line, [DisallowNull] HeaderCollection headerCollection);
}
