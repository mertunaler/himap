using IMAP.Core;
using IMAP.Core.Configurations.ConfigModels;

namespace Core.UnitTests;

public class ImapClientTests
{
    [Fact]
    public void ConnectsToImapServer()
    {
        var sut = GetImapClient();

        bool isConnected = sut.ConnectAsync("imap.gmail.com", 993).Result;

        Assert.True(isConnected);
    }
    [Fact]
    public void AuthenticatesSuccessfully()
    {
        var sut = GetImapClient();
        bool isConnected = sut.ConnectAsync("imap.gmail.com", 993).Result;
        var cfg = GetConfigValues();
        
        bool isAuthenticated = sut.AuthenticateAsync(cfg.EmailAdress, cfg.EmailPassword).Result;

        Assert.True(isAuthenticated);
    }
    [Fact]
    public void ListsMailBoxes()
    {
        var sut = GetImapClient();
        bool isConnected = sut.ConnectAsync("imap.gmail.com", 993).Result;
        var cfg = GetConfigValues();
        bool isAuthenticated = sut.AuthenticateAsync(cfg.EmailAdress, cfg.EmailPassword).Result;

        var mailBoxes = sut.ListMailBoxesAsync().Result;


        Assert.True(mailBoxes.Count > 0);
    }

    private ImapClient GetImapClient()
    {
        return new ImapClient();
    }
    private CfgEmail GetConfigValues()
    {
        var cfgProvider = new ConfigProvider();
        return cfgProvider.GetConfigValueByKey<CfgEmail>("Email");
    }


}