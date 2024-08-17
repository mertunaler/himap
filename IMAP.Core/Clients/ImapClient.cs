using System;
using System.Diagnostics;
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

    if (!ProcessAuthResponse(response, tag))
        throw new AuthenticationException("Invalid email or password.");

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

        var mailBoxList = ProcessMailBoxListResponse(response);

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
    private bool ProcessAuthResponse(string response, string tag)
    {
        if (string.IsNullOrEmpty(response))
            return false;

        var responseLines = response.Split("\n").Where(w=> w != string.Empty);

        return responseLines.Last().StartsWith($"{tag} OK");
    }
    private List<string> ProcessMailBoxListResponse(string boxResponse)
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
                    mailList.Add(parts[5].Trim('"'));
                }
            }
        }
        return mailList;
    }

    private string GetCommandTag(string cmd)
    {
        return cmd.Substring(0, cmd.IndexOf(' '));
    }
}
