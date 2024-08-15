using System;
using System.Net.Security;
using System.Security.Authentication;
using IMAP.Core.CommandHandler;

namespace IMAP.Core;

public class ImapClient : IDisposable
{
    TcpClient _tcpClient;
    IMAPCommandBuilder _builder;
    public ImapClient()
    {
        _tcpClient = new TcpClient();
        _builder = new IMAPCommandBuilder();
    }
    public async Task<bool> ConnectAsync(string hostName, int port)
    {
        return await _tcpClient.ConnectAsync(hostName, port);
    }
    public async Task<bool> AuthenticateAsync(string username, string password)
    {
        string command = _builder.WithCommand("LOGIN")
                                 .WithParameters(username, password)
                                 .Build();

        string tag = GetCommandTag(command);
        await _tcpClient.SendAsync(command);
        string response = await _tcpClient.RetrieveAsync(tag);

        if (!IsAuthSuccessful(response))
            return false;
        return true;
    }
    public async Task<List<string>> ListMailBoxesAsync()
    {
        string command = _builder.WithCommand("LIST")
                                 .WithParameters("\"\"", "\"*\"") //start from the root folder and get all the mailboxes
                                 .Build();
        string tag = GetCommandTag(command);
        await _tcpClient.SendAsync(command);
        string response = await _tcpClient.RetrieveAsync(tag);

        var mailBoxList = ProcessMailBoxList(response);

        return mailBoxList;
    }
    public async Task SelectMailBoxAsync(string mailBoxName)
    {
        string command = _builder.WithCommand("SELECT")
                                 .WithParameters(mailBoxName)
                                 .Build();
        string tag = GetCommandTag(command);
        await _tcpClient.SendAsync(command);
        string response = await _tcpClient.RetrieveAsync(tag);

        if (!response.Contains("SELECT completed"))
            throw new InvalidOperationException("Couldn't select mailbox.");
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    private List<string> ProcessMailBoxList(string boxResponse)
    {
        var responseLines = boxResponse.Split("\n");
        List<string> mailList = new List<string>();
        //example response * LIST (\HasNoChildren) "/" "INBOX"
        foreach (var line in responseLines)
        {
            if (line.StartsWith("* LIST"))
            {
                var parts = line.Split(new[] { ' ' }, 6, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 6)
                {
                    mailList.Append(parts[5].Trim('"'));
                }
            }
        }
        return mailList;
    }
    private bool IsAuthSuccessful(string response)
    {
        if (string.IsNullOrEmpty(response))
            return false;

        return response.StartsWith("* OK", StringComparison.InvariantCultureIgnoreCase);

    }
    private string GetCommandTag(string cmd)
    {
        return cmd.Substring(0, cmd.IndexOf(' '));
    }
}
