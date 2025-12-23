namespace LiteHttp.RequestProcessors.PipeContext.Parser;

internal static class HeaderParsingErrors
{
    public static readonly Error HeaderSyntaxError = 
        new Error(HeaderParsingErrorCodes.SyntaxError, HeaderParsingErrorStrings.SyntaxError);
    public static readonly Error StateUpdateRequested = 
        new Error(HeaderParsingErrorCodes.StateUpdateRequested, HeaderParsingErrorStrings.StateUpdateRequested);
    public static readonly Error TwoSameHeaders = 
        new Error(HeaderParsingErrorCodes.TwoSameHeaders, HeaderParsingErrorStrings.TwoSameHeaders);
}

internal static class HeaderParsingErrorCodes
{
    public const int SyntaxError = ParserErrors.InvalidRequestSyntax;
    public const int TwoSameHeaders = ParserErrors.TwoSameHeadersMet;
    public const int StateUpdateRequested = 151;
}

internal static class HeaderParsingErrorStrings
{
    public const string SyntaxError =
        "The header line has an invalid syntax.";
    public const string StateUpdateRequested =
        "A state update has been requested.";
    public const string TwoSameHeaders =
        "The same header has been encountered more than once.";
}