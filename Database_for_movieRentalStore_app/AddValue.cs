public class AddValue : ICommand
{
    private string firstname, lastname, position, hiredate;
    public AddValue(string firstname, string lastname, string position, string hiredate)
    {
        this.firstname = firstname;
        this.lastname = lastname;
        this.position = position;
        this.hiredate = hiredate;
    }
    public AddValue()
    {
        
    }
    public void Execute()
    {
        var mdb = MyDatabase.Instance;
        mdb.AddAnEmployee(firstname, lastname, position, hiredate);
    }
    public void Execute(string np)
    {

    }
}

