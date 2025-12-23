namespace LiteHttp.ErrorHandling.ErrorCodes;

public static class ParserErrors
{
    public const int InvalidRequestSyntax = 101;
    public const int InvalidHeaderValue = 102;
    public const int TwoSameHeadersMet = 103;
    // 151 reserved for StateUpdateRequested in HeaderParsingResult
    public const int Unexpected = 199;
}