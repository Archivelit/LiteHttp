namespace LiteHttp.RequestProcessors.PipeContext.Parser;

public interface IHeaderParser
{
    public HeaderParsingResult ParseHeader(ReadOnlySequence<byte> line, HeaderCollection headerCollection);
}
