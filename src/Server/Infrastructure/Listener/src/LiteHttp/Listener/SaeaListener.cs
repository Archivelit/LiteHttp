using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

using LiteHttp.Constants;
using LiteHttp.Logging;
using LiteHttp.Logging.Abstractions;

namespace LiteHttp.Listener;

#pragma warning disable CS8618
public sealed class SaeaListener : IDisposable
{
    public int ListenerPort 
    { 
        get; 
        
        private set
        {
            if (_isListening)
                throw new InvalidOperationException("Port cannot be changed while server listening");

            field = value;
            UpdateListenerEndPoint();
        }
    }
    public IPAddress ListenerAddress
    {
        get;

        private set
        {
            if (_isListening)
                throw new InvalidOperationException("Ip address cannot be changed while server listening");

            field = value;
            UpdateListenerEndPoint();
        }
    }

    private readonly ILogger<SaeaListener> _logger;

    private Socket Socket { get; }
    private IPEndPoint _endPoint;
    private bool _isListening;
    private CancellationToken _cancellationToken;
    
    public SaeaListener(ILogger<SaeaListener>? logger = null)
        : this(AddressConstants.IPV4_LOOPBACK, AddressConstants.DEFAULT_SERVER_PORT, logger) { }

    public SaeaListener(IPAddress address)
        : this(address, AddressConstants.DEFAULT_SERVER_PORT) { }

    public SaeaListener(IPAddress address, int port = AddressConstants.DEFAULT_SERVER_PORT, ILogger<SaeaListener>? logger = null)
    {
        logger ??= NullLogger<SaeaListener>.Instance;
        _logger = logger;

        _isListening = false;
        ListenerAddress = address;
        ListenerPort = port;
        Socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        UpdateListenerEndPoint();
    }

    public void Dispose()
    {
        Socket.Dispose();
        _isListening = false;
    }

    public bool StartListen(CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;

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
            var acceptEventArg = new SocketAsyncEventArgs();
            acceptEventArg.Completed += AcceptEventArg_Completed;
            
            return ThreadPool.UnsafeQueueUserWorkItem(StartAccept, acceptEventArg, false);
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AcceptEventArg_Completed(object? sender, SocketAsyncEventArgs saea)
    {
        ProcessAccept(saea);

        StartAccept(saea);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void StartAccept(SocketAsyncEventArgs acceptEventArg)
    {
        bool willRaiseEvent = false;
        while (!willRaiseEvent)
        {
            acceptEventArg.AcceptSocket = null;
            willRaiseEvent = Socket.AcceptAsync(acceptEventArg);

            if (!willRaiseEvent)
                ProcessAccept(acceptEventArg);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ProcessAccept(SocketAsyncEventArgs acceptEventArg) => OnRequestReceived(acceptEventArg);

    private void UpdateListenerEndPoint() =>
        _endPoint = new(ListenerAddress, ListenerPort);

    private event Action<SocketAsyncEventArgs> RequestReceived;

    private void OnRequestReceived(SocketAsyncEventArgs saea) =>
        RequestReceived?.Invoke(saea);

    public void SubscribeToRequestReceived(Action<SocketAsyncEventArgs> handler) =>
        RequestReceived += handler;

    public void UnsubscribeFromRequestReceived(Action<SocketAsyncEventArgs> handler) =>
        RequestReceived -= handler;
}