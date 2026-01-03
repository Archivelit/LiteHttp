using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

using LiteHttp.Constants;
using LiteHttp.Logging;
using LiteHttp.Logging.Abstractions;
using LiteHttp.Models.Events;

namespace LiteHttp.Listener;

#pragma warning disable CS8618
public sealed class Listener : IDisposable
{
    public int ListenerPort 
    { 
        get; 
        
        set
        {
            if (_isListening)
                throw new InvalidOperationException("Ip address cannot be changed while server listening");

            field = value;
            UpdateListenerEndPoint();
        }
    }
    public IPAddress ListenerAddress 
    { 
        get; 
        
        set
        {
            if (_isListening)
                throw new InvalidOperationException("Port cannot be changed while server listening");

            field = value;
            UpdateListenerEndPoint();
        }
    }

    private readonly ILogger<Listener> _logger;
    
    private Socket Socket { get; } = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    private IPEndPoint _endPoint;
    private bool _isListening = false;

    public Listener(ILogger<Listener>? logger = null)
        : this(AddressConstants.IPV4_LOOPBACK, AddressConstants.DEFAULT_SERVER_PORT, logger) { }

    public Listener(IPAddress address)
        : this(address, AddressConstants.DEFAULT_SERVER_PORT) { }

    public Listener(IPAddress address, int port = AddressConstants.DEFAULT_SERVER_PORT, ILogger<Listener>? logger = null)
    {
        logger ??= NullLogger<Listener>.Instance;
        _logger = logger;

        ListenerAddress = address;
        ListenerPort = port;
        Socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        UpdateListenerEndPoint();
    }

    public async ValueTask StartListen(CancellationToken stoppingToken)
    {
        if (_endPoint is null)
            throw new InvalidOperationException("Listener endpoint cannot be null");

        if (!Socket.IsBound)
        {
            Socket.Bind(_endPoint);
            _logger.LogDebug($"Socket bound");
        }

        Socket.Listen();

        _isListening = true;

        _logger.LogInformation($"Listening at {_endPoint.ToString()}");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var connection = await Socket.AcceptAsync(stoppingToken).ConfigureAwait(false);

                OnRequestReceived(new RequestReceivedEvent(connection), stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while listening for incoming connections");
            throw;
        }
        finally
        {
            _isListening = false;
        }
    }

    public void Dispose() =>
        Socket.Dispose();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void UpdateListenerEndPoint() =>
        _endPoint = new(ListenerAddress, ListenerPort);

    public event Func<RequestReceivedEvent, CancellationToken, ValueTask>? RequestReceived;

    public void OnRequestReceived(RequestReceivedEvent connection, CancellationToken ct) =>
        RequestReceived?.Invoke(connection, ct);

    public void SubscribeToRequestReceived(Func<RequestReceivedEvent, CancellationToken, ValueTask> handler) =>
        RequestReceived += handler;

    public void UnsubscribeFromRequestReceived(Func<RequestReceivedEvent, CancellationToken, ValueTask> handler) =>
        RequestReceived -= handler;
}