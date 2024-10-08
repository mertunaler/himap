using System.Net.Security;
using System.Net.Sockets;
using System.Text;

namespace IMAP.Core;

public class TcpClient : IDisposable
{
    private System.Net.Sockets.TcpClient _client;
    private SslStream _sslStream;
    private StreamReader _streamReader;
    private StreamWriter _streamWriter;
    public async Task<bool> ConnectAsync(string hostName, int port, bool useSsl = true)
    {
        try
        {
            _client = new System.Net.Sockets.TcpClient();
            await _client.ConnectAsync(hostName, port);

            NetworkStream networkStream = _client.GetStream();

            if (useSsl)
            {
                _sslStream = new SslStream(networkStream, false,
                    new RemoteCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true));
                await _sslStream.AuthenticateAsClientAsync(hostName);
                _streamReader = new StreamReader(_sslStream, Encoding.ASCII);
                _streamWriter = new StreamWriter(_sslStream, Encoding.ASCII);

            }
            else
            {
                _streamReader = new StreamReader(networkStream, Encoding.ASCII);
                _streamWriter = new StreamWriter(networkStream, Encoding.ASCII);
            }
            _streamWriter.AutoFlush = true;
            return true;
        }
        catch
        {
            return false;
        }
    }
    public async Task SendAsync(string data)
    {
        if (_streamWriter is null)
            throw new InvalidOperationException("Connection is not established yet.");

        await _streamWriter.WriteAsync(data + "\r\n");
    }
    public async Task<string> RetrieveAsync(string tag)
    {
        if (_streamReader is null)
            throw new InvalidOperationException("Connection is not established yet.");

        var sb = new StringBuilder();
        string line;
        while ((line = await _streamReader.ReadLineAsync()) != null)
        {

            sb.AppendLine(line);
            if (line.StartsWith("* BYE") || line.StartsWith(tag))
                break;
        }
        return sb.ToString();
    }
    public void Dispose()
    {
        _client.Dispose();
        _sslStream.Dispose();
        _streamReader.Dispose();
        _streamWriter.Dispose();
    }
   
}
