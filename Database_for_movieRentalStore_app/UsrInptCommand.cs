public class UsrInptCommand : ICommand
{
    //class and content by chatGPT
    private string _movieName;
    private string _firstName;
    private string _lastName;
    private string _email;
    private string _outpt;

    public UsrInptCommand(string moviename, string? firstname, string? lastname, string? email)
    {
        _movieName = moviename;
        _firstName = firstname;
        _lastName = lastname;
        _email = email;
    }
    public void Execute()
    {
        var mdb = MyDatabase.Instance;
        _outpt = mdb.RemoveARental(_movieName, _firstName, _lastName, _email);
        Console.WriteLine(_outpt);
    }

    public void Execute1()
    {

    }
}

