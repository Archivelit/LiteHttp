namespace LiteHttp.Sockets.TcpSocket;

/// <summary>
/// This class is used to receive data from the entire request.
/// </summary>
internal sealed class TcpSocketReader
{
    /// <summary>
    /// Minimal size of buffer that used to receive data from request.
    /// </summary>
    private const int MinBufferSize = 1024;
    
    /// <summary>
    /// Asynchronously receive and write data to the <see cref="Pipe"/> from parameters.
    /// </summary>
    /// <param name="socket">A socket with entire request.</param>
    /// <param name="pipe">Pipe used for storing the request.</param>
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
            catch (Exception ex)
            {
                break;
            }
            
            var flushResult = await pipe.Writer.FlushAsync();

            if (flushResult.IsCompleted)
                break;
        }

        await pipe.Writer.CompleteAsync();
    }
}