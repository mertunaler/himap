using IMAP.Core.CommandHandler;

namespace Core.UnitTests;

public class UnitTest1
{
    [Fact]
    public void GeneratesValidCommand()
    {
        string expectedCmd = "A0000 LOGIN w3rt w3rt";
        var sut = GetCommandBuilderObject();

        string generatedCmd = sut.WithCommand("LOGIN")
                                 .WithParameters("w3rt", "w3rt")
                                 .Build();

        Assert.Equal(expectedCmd, generatedCmd);
    }
    [Fact]
    public void ReturnsValidCommandWithoutParams()
    {
        string expectedCmd = "A0000 LOGIN";
        var sut = GetCommandBuilderObject();

        string generatedCmd = sut.WithCommand("LOGIN")
                                  .Build();

        Assert.Equal(expectedCmd, generatedCmd);
    }
    [Fact]
    public void CommandGetCorrectTag()
    {
        var sut = GetCommandBuilderObject();
        string expectedTag = "A0000";
        string generatedCmd = sut.WithCommand("LOGIN")
                             .WithParameters("w3rt")
                             .Build();

        string generatedTag = generatedCmd.Split(" ")[0];

        Assert.Equal(expectedTag, generatedTag);
    }
    [Fact]
    public void MultipleCommandsGeneratesValidCounteredCommands()
    {
        var sut = GetCommandBuilderObject();

        string firstCmd = sut.WithCommand("LOGIN")
                             .WithParameters("w3rt")
                             .Build();

        string secondCmd = sut.WithCommand("LOGIN")
                             .WithParameters("drogba")
                             .Build();

        string firstTag = firstCmd.Split(" ")[0];
        string secondTag = secondCmd.Split(" ")[0];

        Assert.NotEqual(firstTag, secondTag);
    }
    private IMAPCommandBuilder GetCommandBuilderObject()
    {
        return new IMAPCommandBuilder();
    }
}