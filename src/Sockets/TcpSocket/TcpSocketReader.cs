namespace LiteHttp.Sockets.TcpSocket;

/// <summary>
/// Provides functionality for receiving request from a TCP connection.
/// </summary>
internal sealed class TcpSocketReader
{
    private const int MinBufferSize = 1024;

#if RELEASE
    /// <summary>
    /// Asynchronously receives data from specified TCP <see cref="Socket"/> and writes
    /// them into the provided <see cref="Pipe"/>.
    /// </summary>
    /// <param name="socket">The connected socket from which the request data is received.</param>
    /// <param name="pipe">The pipe used to store received request data.</param>
    public async Task ReceiveAsync(Socket socket, Pipe pipe)
    {
        while (true)
        {
            try
            {
                Memory<byte> buffer = pipe.Writer.GetMemory(MinBufferSize);
                var bytesRead = await socket.ReceiveAsync(buffer);

                if (bytesRead == 0)
                    break;

                pipe.Writer.Advance(bytesRead);
            }
            catch
            {
                break;
            }

            var flushResult = await pipe.Writer.FlushAsync();
            if (flushResult.IsCompleted)
                break;
        }

        await pipe.Writer.CompleteAsync();
    }
#endif

#if DEBUG
    /// <summary>
    /// Asynchronously receives data from specified TCP <see cref="Socket"/> and writes
    /// them into the provided <see cref="Pipe"/>.
    /// </summary>
    /// <param name="socket">The connected socket from which the request data is received.</param>
    /// <param name="pipe">The pipe used to store received request data.</param>
    public async Task ReceiveAsync(Socket socket, Pipe pipe)
    {
        while (true)
        {
            try
            {
                var buffer = pipe.Writer.GetMemory(MinBufferSize);
                var bytesRead = await socket.ReceiveAsync(buffer);

                if (bytesRead == 0)
                    break;

                pipe.Writer.Advance(bytesRead);
            }
            catch
            {
                break;
            }

            var flushResult = await pipe.Writer.FlushAsync();
            if (flushResult.IsCompleted)
                break;
        }

        await pipe.Writer.CompleteAsync();
    }
#endif
}
