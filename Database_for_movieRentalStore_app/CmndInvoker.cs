/// <summary>
/// a class that works as an invoker for other classes working as a strategz pattern objects
/// (inherit ICommand)
/// </summary>
public class CmndInvoker
{
    //class and content by chatGPT
    private ICommand _command;
    /// <summary>
    /// function that sets the command that should be executed
    /// </summary>
    /// <param name="command">class as an object</param>
    public void SetCommand(ICommand command)
    {
        _command = command;
    }
    /// <summary>
    /// function that executes the class that was set as a command. Also checks if it uses the second
    /// method provided from the interface
    /// </summary>
    /// <param name="sw"></param>
    public void ExecuteCommand(bool sw = true)
    {
        if(sw) _command.Execute();
        if(!sw) _command.Execute1();
    }
}

