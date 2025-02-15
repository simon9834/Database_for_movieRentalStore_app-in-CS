

using System.Text.RegularExpressions;

public class ButtonExecution
{
    private string moviename;
    private string firstname;
    private string lastname;
    private string email;
    private string filepath1;
    private string filepath2;
    private CmndInvoker invoker = new CmndInvoker();
    public void delete()
    {
        Console.Clear();

        moviename = null;
        firstname = null;
        lastname = null;
        email = null;

        while (true)
        {
            if (moviename == null)
            {
                Console.Write("Enter the movie name: ");
                moviename = Console.ReadLine();
                Console.Clear();
                if (string.IsNullOrEmpty(moviename)) continue;
            }
            if (firstname == null)
            {
                Console.Write("Enter the customers first name if you know it.\nIf you dont know it, enter none: ");
                firstname = nullChecker(Console.ReadLine());
            }
            if (lastname == null)
            {
                Console.Write("Enter the customers last name if you know it.\nIf you dont know it, enter none: ");
                lastname = nullChecker(Console.ReadLine());
            }
            Console.Write("Enter the user's email if you know it.\nIf you dont know it, enter none: ");
            email = nullChecker(Console.ReadLine());
            Regex reg = new Regex(@"^(?!.*\.\.)[a-zA-Z0-9!#$%&'*+/=?^_`{|}~.-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            if (reg.IsMatch(email) || string.IsNullOrEmpty(email))
            {
                break;
            }
            else
            {
                Console.WriteLine("incorrect email");
                try
                {
                    Thread.Sleep(25000);
                }catch (ThreadInterruptedException e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.Clear();
            }
        }
        ICommand usrInptCommand = new UsrInptCommand(moviename, firstname, lastname, email);

        invoker.SetCommand(usrInptCommand);
        invoker.ExecuteCommand();
    }

    public void insertCSVData()
    {
        Console.WriteLine("You can insert you'r file of customers.csv and employees.csv into the bin/debug/net8.0 file," +
            "\nor you can insert you'r data into the already created file named employees.csv and customers.csv which" +
            "\nare to be found at the same location where you would insert you'r own files.\n" +
            "\nIf you copy data to already created files, write here none. If you insert your own files," +
            "\nwrite here 'yourfileofcustomers.csv' of them (with .csv) and press enter, do the same for the 'yourfileofemployees.csv'.");
        filepath1 = nullChecker(Console.ReadLine(), false);
        filepath2 = nullChecker(Console.ReadLine());
        if(!string.IsNullOrEmpty(filepath1) && !string.IsNullOrEmpty(filepath2))
        {
            ICommand csv = new CSVdata(filepath1, filepath2);
            invoker.SetCommand(csv);
        }
        else
        {
            ICommand csv = new CSVdata();
            invoker.SetCommand(csv);
        }
            
            invoker.ExecuteCommand();
        
    }

    public string nullChecker(string s, bool clear = true)
    {
        if(clear) Console.Clear();
        if (string.Equals(s.Trim().ToLower(), "none"))
        {
            return "";
        }
        else
        {
            return s;
        }
    }
}

