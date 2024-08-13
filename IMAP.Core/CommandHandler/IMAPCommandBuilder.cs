using System;
using System.ComponentModel;

namespace IMAP.Core.CommandHandler;

public class IMAPCommandBuilder : ICommandBuilder
{
    private string _command;
    private string _params;
    private int _tagCounter = 0;

    public string Build()
    {
        string tagPrefix = $"A{_tagCounter++:D4}";

        return $"{tagPrefix} {_command} {_params}".Trim();
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
