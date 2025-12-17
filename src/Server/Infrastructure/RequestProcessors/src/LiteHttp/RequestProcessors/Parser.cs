using HttpContext = LiteHttp.Models.HttpContext;

namespace LiteHttp.RequestProcessors;

#nullable disable
public sealed class Parser
{
    public static readonly Parser Instance = new();
    
    /// <summary>
    /// Parses the entire request bytes into <see cref="HttpContext"/> model.
    /// </summary>
    /// <param name="request">Entire request bytes.</param>
    /// <returns><see cref="Result{TResult}"/> wrappee with result or exception wrapped</returns>
    [SkipLocalsInit]
    public Result<HttpContext> Parse(Memory<byte> request)
    {
        var requestParts = SplitRequest(request);

        var firstLine = GetFirstLine(requestParts.Headers);

        var method = GetMethod(firstLine);

        if (!method.Success)
            return new(method.Error.Value);

        var route = GetRoute(firstLine);

        if (!route.Success)
            return new(route.Error.Value);

        var headerSection = requestParts.Headers[(firstLine.Length + RequestSymbolsAsBytes.NewRequestLine.Length)..]; // First line of request does not contain any header

        var headers = MapHeaders(headerSection);

        return !headers.Success
            ? new(headers.Error.Value)
            : new Result<HttpContext>(new HttpContext(method.Value, route.Value, headers.Value, requestParts.Body));
    }

    /// <summary>
    /// Extracts route from <paramref name="firstRequestLine"/>.
    /// </summary>
    /// <param name="firstRequestLine">The first line of http request represented in byte array</param>
    /// <returns><see cref="Result{TReult}"/> wrapee with result or exception wrapped</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Result<Memory<byte>> GetRoute(Memory<byte> firstRequestLine)
    {
        var firstSpaceIndex = firstRequestLine.Span.IndexOf(RequestSymbolsAsBytes.Space);
        var lastSpaceIndex = firstRequestLine.Span.LastIndexOf(RequestSymbolsAsBytes.Space);

        if (firstSpaceIndex == lastSpaceIndex)
            return new(new Error(ParserErrors.InvalidRequestSyntax,"The request has wrong format"));

        return new(firstRequestLine[(firstSpaceIndex + 1)..lastSpaceIndex]); // space index + 1 to exclude whitespace and get first symbol of route
    }

    /// <summary>
    /// Extracts first request line represented in bytes
    /// </summary>
    /// <param name="request">Entire request needed to be parsed</param>
    /// <returns>First request line represented in bytes</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Memory<byte> GetFirstLine(Memory<byte> request) =>
        request[..request.Span.IndexOf(RequestSymbolsAsBytes.CarriageReturnSymbol)];

    /// <summary>
    /// Extracts request method from entire request
    /// </summary>
    /// <param name="firstRequestLine">First line of the entire request</param>
    /// <returns>The <see cref="Result{TResult}"/> wrapee with exception or method represented in bytes</returns>
    /// <exception cref="ArgumentException">Returned if request does not contain method</exception>
    private Result<Memory<byte>> GetMethod(Memory<byte> firstRequestLine)
    {
        var spaceIndex = firstRequestLine.Span.IndexOf(RequestSymbolsAsBytes.Space);

        if (spaceIndex == -1)
            return new(new Error(ParserErrors.InvalidRequestSyntax, "The request has wrong format"));

        return new(firstRequestLine[..spaceIndex]);
    }

    /// <summary>
    /// Splits the entire request to Headers and Body parts
    /// </summary>
    /// <param name="request">Entire request</param>
    /// <returns>Tuple with slices of entire request parts. Body is null if request does not contain it</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private (Memory<byte> Headers, Memory<byte>? Body) SplitRequest(Memory<byte> request)
    {
        var splitterIndex = request.Span.IndexOf(RequestSymbolsAsBytes.RequestSplitter);

        if (splitterIndex == -1)
            return (request, null);

        return (request[..(splitterIndex + RequestSymbolsAsBytes.NewRequestLine.Length)], // NOTE: do not change, it is breaking change. Adding 1 new line symbol for proper header parsing 
            request[(splitterIndex + RequestSymbolsAsBytes.RequestSplitter.Length)..]);
    }

    /// <summary>
    /// Maps the entire request header section on specified headers.
    /// </summary>
    /// <param name="headers">The entire request header section</param>
    /// <returns><see cref="Result{TResult}"/> wrapee with exception or headers dictionary.
    /// The dictionaries key is header title without column</returns>
    [SkipLocalsInit]
    private Result<Dictionary<ReadOnlyMemory<byte>, ReadOnlyMemory<byte>>> MapHeaders(Memory<byte> headers)
    {
        var headersDictionary = new Dictionary<ReadOnlyMemory<byte>, ReadOnlyMemory<byte>>(8);

        while (headers.Length > 2)
        {
            var eol = headers.Span.IndexOf(RequestSymbolsAsBytes.NewLine);

            if (eol == -1)
            {
                var colonIndex = headers.Span.IndexOf(RequestSymbolsAsBytes.Colon);

                if (colonIndex == -1)
                    return new(headersDictionary);

                headersDictionary.Add(headers[..colonIndex], headers[(colonIndex + 2)..]); // +2 to exclude colon and space,

                return new(headersDictionary);
            }

            var colon = headers.Span[..eol].IndexOf(RequestSymbolsAsBytes.Colon);

            if (colon == -1)
                return new(new Error(ParserErrors.InvalidRequestSyntax, "The headers had wrong format"));

            var key = headers[..colon];
            var value = headers[
                (colon + 2)..(eol - 1)]; // +2 to exclude colon and space, -1 to exclude carriage return (\r) symbol

            headersDictionary[key] = value;
            headers = headers[(eol + 1)..]; // +1 to exclude eol symbol
        }

        return new(headersDictionary);
    }
}