
public class CmndInvoker
{
    //class and content by chatGPT
    private ICommand _command;

    public void SetCommand(ICommand command)
    {
        _command = command;
    }

    public void ExecuteCommand(bool sw = true)
    {
        if(sw) _command.Execute();
        if(!sw) _command.Execute1();
    }
}

