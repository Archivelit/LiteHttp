namespace LiteHttp.RequestProcessors.PipeContext.Parser;

public readonly struct HeaderParsingResult : IEquatable<HeaderParsingResult>
{
    public static readonly HeaderParsingResult Successful = new();
    public static readonly HeaderParsingResult StateUpdateRequested = new(HeaderParsingErrors.StateUpdateRequested);
    public static readonly HeaderParsingResult TwoSameHeaders = new(HeaderParsingErrors.TwoSameHeaders);
    public static readonly HeaderParsingResult HeaderSyntaxError = new(HeaderParsingErrors.HeaderSyntaxError);

    public readonly Error? Error;
    public readonly bool Success;

    public HeaderParsingResult()
    {
        Error = null;
        Success = true;
    }

    public HeaderParsingResult(Error error)
    {
        Error = error;
        Success = false;
    }

    public HeaderParsingResult(Error? error)
    {
        Error = error;
        Success = false;
    }

    
    public static implicit operator HeaderParsingResult(Error error) =>
        new(error);
    public static implicit operator HeaderParsingResult(Error? error) =>
        new(error);

    public static bool operator ==(HeaderParsingResult a, HeaderParsingResult b) => a.Equals(b);
    public static bool operator !=(HeaderParsingResult a, HeaderParsingResult b) => !a.Equals(b);

    public bool Equals(HeaderParsingResult other) => Success && Error.Equals(other.Error);

    public override bool Equals(object? obj) =>
        obj is HeaderParsingResult r && Equals(r);

    public override int GetHashCode() =>
        HashCode.Combine(Success, Error);
}

