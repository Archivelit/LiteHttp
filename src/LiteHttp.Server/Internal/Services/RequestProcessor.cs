namespace LiteHttp.Server.Services;

/// <summary>
/// <see cref="RequestProcessor"/> processes entire request. The processing in this service based
/// on 4 stages. The 1st stage is request parsing. 2nd stage is routing. 3rd stage is endpoint handler
/// executing. 4th stage is building response. 
/// </summary>
internal sealed class RequestProcessor : IDisposable, IRequestProcessor
{
    private readonly Router _router = new();
    private readonly Parser _parser = Parser.Instance;
    private readonly ResponseBuilder _responseBuilder = new();
    private readonly ILogger<RequestProcessor> _logger;
    
    /// <summary>
    /// Initializes the <see cref="RequestProcessor"/> with specified configuration.
    /// </summary>
    /// <param name="endpointContext">Context that contains all server endpoints.</param>
    /// <param name="address">Address where server works and listens. Needed for Host header.</param>
    /// <param name="port">Host where server works and listens. Needed for Host header.</param>
    /// <param name="logger">Logger used by whole app.</param>
    public RequestProcessor(IEndpointContext endpointContext, string address, int port, ILogger logger)
    {
        _router.SetContext(endpointContext);
        _logger = logger.ForContext<RequestProcessor>();

        _responseBuilder.Address = address;
        _responseBuilder.Port = port;
    }

    /// <summary>
    /// Changes the port that uses <see cref="IResponseBuilder"/> to create a Host header.
    /// </summary>
    /// <param name="port">
    /// Port where server listens. Needed for Host header.
    /// </param>
    public void SetHostPort(int port) =>
        _responseBuilder.Port = port;
    
    /// <summary>
    /// Changes the address that uses <see cref="IResponseBuilder"/> to create a Host header.
    /// </summary>
    /// <param name="address">
    /// Address where server listens. Needed for Host header.
    /// </param>
    public void SetHostAddress(string address) =>
        _responseBuilder.Address = address;

    /// <summary>
    /// Processes the entire request bytes.
    /// </summary>
    /// <param name="request">
    /// Bytes of entire request that should be processed
    /// </param>
    /// <returns><see cref="Result{ReadOnlyMemory{byte}}"/> wrapee that contains response or error.</returns>
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public Result<ReadOnlyMemory<byte>> Process(Memory<byte> request)
    {
        var context = _parser.Parse(request);

        if (!context.Success)
            return new(context.Error!);
        
        var action = _router.GetAction(context.Value);

        if (action is null)
        {
            _logger.LogInformation($"Endpoint not found");
            var notFoundResponse = _responseBuilder.Build(ActionResultFactory.Instance.NotFound());
            
            return new Result<ReadOnlyMemory<byte>>(notFoundResponse);
        }

        var actionResult = action();

        return new (actionResult is IActionResult<object> result
            ? _responseBuilder.Build(result, Encoding.UTF8.GetBytes(result.Result.ToString() ?? string.Empty))
            : _responseBuilder.Build(actionResult));
    }

    public void Dispose() => _responseBuilder.Dispose();
}