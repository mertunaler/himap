using System;
using System.ComponentModel;

namespace IMAP.Core.CommandHandler;

public class IMAPCommandBuilder : ICommandBuilder
{
    private string _command;
    private string _params;

    public string Build()
    {
        return $@"{_command} {_params}";
    }
    public ICommandBuilder WithCommand(string command)
    {
        _command = command;
        return this;
    }
    public ICommandBuilder WithParameters(params string[] parameters)
    {
        _params = string.Join(" ", parameters);
        return this;
    }
}
