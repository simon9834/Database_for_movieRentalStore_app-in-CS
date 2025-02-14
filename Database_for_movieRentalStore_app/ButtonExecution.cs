

using System.Text.RegularExpressions;

public class ButtonExecution
{
    private string moviename;
    private string firstname;
    private string lastname;
    private string email;
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

        CmndInvoker invoker = new CmndInvoker();
        invoker.SetCommand(usrInptCommand);
        invoker.ExecuteCommand();
    }

    public void insertCSVData()
    {
        try
        {
            ICommand usrInputCmnd = 
        }catch(FileNotFoundException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public string nullChecker(string s)
    {
        Console.Clear();
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

