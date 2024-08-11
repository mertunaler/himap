using System.Text;

namespace IMAP.Core;

public class TcpClient : IDisposable
{
    private System.Net.Sockets.TcpClient _client;
    private Stream _stream;
    private StreamReader _streamReader;
    private StreamWriter _streamWriter;
    public async Task ConnectAsync(string hostName, int port)
    {
        _client = new System.Net.Sockets.TcpClient();
        await _client.ConnectAsync(hostName, port);
        _stream = _client.GetStream();
        _streamReader = new StreamReader(_stream, Encoding.UTF8);
        _streamWriter = new StreamWriter(_stream, Encoding.UTF8);
    }
    public async Task SendAsync(string data)
    {
        if (_streamWriter is null)
            throw new InvalidOperationException("Connection is not established yet.");

        await _streamWriter.WriteLineAsync(data);
    }
    public async Task<string> RetrieveAsync()
    {
        if (_streamReader is null)
            throw new InvalidOperationException("Connection is not established yet.");

        return await _streamReader.ReadLineAsync();
    }
    public void Dispose()
    {
        _client.Dispose();
        _stream.Dispose();
        _streamReader.Dispose();
        _streamWriter.Dispose();
    }
}
