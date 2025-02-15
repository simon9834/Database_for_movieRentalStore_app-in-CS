
public class CSVdata : ICommand
{
    private string cstmr;
    private string employee;
    private string _outpt;

    public CSVdata(string? customerCSV = null, string? employeeCSV = null)
    {
        cstmr = customerCSV;
        employee = employeeCSV;
    }
    public void Execute()
    {
        var mdb = MyDatabase.Instance;
        if (!string.IsNullOrEmpty(cstmr) && !string.IsNullOrEmpty(employee))
        {
            _outpt = mdb.insertCSV(cstmr, employee);

        }
        else
        {
            _outpt = mdb.insertCSV();
        }
        Console.WriteLine(_outpt);
    }
}

