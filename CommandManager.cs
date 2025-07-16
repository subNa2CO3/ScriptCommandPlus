public class CommandManager
{
    private readonly Dictionary<string, Action<string[]>> _commands = new();

    // Registers a new command
    public void Register(string name, Action<string[]> action)
    {
        _commands[name.ToLower()] = action;
    }

    // Register a new command
    public bool TryExecute(string name, string[] args)
    {
        if (_commands.TryGetValue(name.ToLower(), out var action))
        {
            action(args);
            return true;
        }
        return false;
    }
}
