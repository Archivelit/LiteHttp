namespace LiteHttp.Sockets.TcpSocket;

/// <summary>
/// This class is used for sending response data to a TCP client.
/// </summary>
internal class TcpSocketWriter
{
#if RELEASE
    /// <summary>
    /// Asynchronously reads response data from the provided <see cref="Pipe"/> and sends it to the
    /// specified TCP <see cref="Socket"/>.
    /// </summary>
    /// <param name="socket">A socket with connection where the response has to be sent.</param>
    /// <param name="pipe">Pipe with response stored in it.</param>
    public async Task SendAsync(Socket socket, Pipe pipe)
    {
        while (true)
        {
            var result = await pipe.Reader.ReadAsync();
            if (result.IsCompleted)
                break;
            
            try
            {
                foreach (var segment in result.Buffer)
                    await socket.SendAsync(segment);
            }
            catch
            {
                break;
            }

            pipe.Reader.AdvanceTo(result.Buffer.Start, result.Buffer.End);
        }

        await pipe.Reader.CompleteAsync();
    }
#endif

#if DEBUG
    /// <summary>
    /// Asynchronously reads response data from the provided <see cref="Pipe"/> and sends it to the
    /// specified TCP <see cref="Socket"/>.
    /// </summary>
    /// <param name="socket">A socket with connection where the response has to be sent.</param>
    /// <param name="pipe">Pipe with response stored in it.</param>
    public async Task SendAsync(Socket socket, Pipe pipe)
    {
        while (true)
        {
            var result = await pipe.Reader.ReadAsync();
            if (result.IsCompleted)
                break;

            try
            {
                foreach (var segment in result.Buffer)
                    await socket.SendAsync(segment);
            }
            catch
            {
                break;
            }

            pipe.Reader.AdvanceTo(result.Buffer.Start, result.Buffer.End);
        }

        await pipe.Reader.CompleteAsync();
    }
#endif
}