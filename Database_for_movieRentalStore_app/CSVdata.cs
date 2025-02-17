/// <summary>
/// a class that works as an object in the strategy pattern
/// </summary>
public class CSVdata : ICommand
{
    private string cstmr;
    private string employee;
    private string _outpt;
    /// <summary>
    /// constructor for the class with 2 inputs
    /// </summary>
    /// <param name="customerCSV"></param>
    /// <param name="employeeCSV"></param>
    public CSVdata(string customerCSV = null, string employeeCSV = null)
    {
        cstmr = customerCSV;
        employee = employeeCSV;
    }
    /// <summary>
    /// a method that calls the right method from db and prints the output
    /// </summary>
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

    public void Execute1()
    {
    }
}

