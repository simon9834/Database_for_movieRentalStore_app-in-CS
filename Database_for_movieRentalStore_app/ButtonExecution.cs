using System.Text.RegularExpressions;
/// <summary>
/// class that gets all needed data from the user after clicking on a button and calls invoker
/// </summary>
public class ButtonExecution
{
    private CmndInvoker invoker = new CmndInvoker();
    /// <summary>
    /// a method that gets the needed data from user for deletion of some data
    /// </summary>
    public void delete()
    {
        Console.Clear();

        string moviename = null;
        string firstname = null;
        string lastname = null;
        string email = null;

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
                firstname = nullChecker(Console.ReadLine().Trim());
            }
            if (lastname == null)
            {
                Console.Write("Enter the customers last name if you know it.\nIf you dont know it, enter none: ");
                lastname = nullChecker(Console.ReadLine().Trim());
            }
            Console.Write("Enter the user's email if you know it.\nIf you dont know it, enter none: ");
            email = nullChecker(Console.ReadLine().Trim());
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
                }
                catch (ThreadInterruptedException e)
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
    /// <summary>
    /// a method that gets the needed data from user for updating the quantity of some atribute
    /// </summary>
    public void updateQuantity()
    {
        string movie_title;
        Console.Write("write the movie title: ");
        movie_title = nullChecker(Console.ReadLine());
        int quantity = 0;
        while (true)
        {
            Console.Write("write the new quantity (numerical): ");
            if (int.TryParse(Console.ReadLine(), out int res))
            {
                quantity = res;
                Console.Clear();
                break;
            }
        }
        
        ICommand updateQuantity = new UpdateQ(movie_title, quantity);
        invoker.SetCommand(updateQuantity);
        invoker.ExecuteCommand();
    }
    /// <summary>
    /// a method that gets the needed data from user for importing data from csv to employees and custonmers
    /// </summary>
    public void insertCSVData()
    {
        Console.WriteLine("You can insert you'r file of customers.csv and employees.csv into the bin/debug/net8.0 file," +
            "\nor you can insert you'r data into the already created file named employees.csv and customers.csv which" +
            "\nare to be found at the same location where you would insert you'r own files.\n" +
            "\nIf you copy data to already created files, write here none. If you insert your own files," +
            "\nwrite here 'yourfileofcustomers.csv' of them (with .csv) and press enter, do the same for the 'yourfileofemployees.csv'.\n\n");
        Console.Write("name of you'r customers file csv: ");
        string filepath1 = nullChecker(Console.ReadLine().Trim(), false);
        Console.WriteLine();
        Console.Write("name of you'r employees file csv: ");
        string filepath2 = nullChecker(Console.ReadLine().Trim());
        if (!string.IsNullOrEmpty(filepath1) && !string.IsNullOrEmpty(filepath2))
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
    /// <summary>
    /// a method that gets the needed data from user for adding an employee to the db
    /// </summary>
    public void addAnEmployee()
    {
        string firstname;
        string lastname;
        string position;
        string date;
        Regex reg = new Regex(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$");
        Console.Write("write employees first name: ");
        firstname = nullChecker(Console.ReadLine());
        Console.Write("write employees last name: ");
        lastname = nullChecker(Console.ReadLine());
        Console.Write("write employees position or write 'none' if he doesn't have a postion yet: ");
        position = nullChecker(Console.ReadLine().Trim());
        while (true)
        {
            Console.Write("write employees hire date in yyyy-mm-dd format or write 'none' if he's not employed: ");
            date = nullChecker(Console.ReadLine().Trim());
            if (reg.IsMatch(date) || string.Equals(date, ""))
            {
                ICommand addemployee = new AddValue(firstname, lastname, position, date);
                invoker.SetCommand(addemployee);
                invoker.ExecuteCommand();
                break;
            }
            else
            {
                Console.WriteLine("the hire date isn't in the given format, try again");
            }
        }
    }
    /// <summary>
    /// a method that gets the needed data from user for adding a film to the db
    /// </summary>
    public void addAMovie()
    {
        string title;
        string genre;
        int releaseyear;
        float? rating;
        int stockQuantity;
        while (true)
        {
            Console.Write("write movies title: ");
            title = nullChecker(Console.ReadLine());
            if (title == "") continue;
            Console.Write("write movies genre or write 'none' if you don't know it: ");
            genre = nullChecker(Console.ReadLine().Trim());
            while (true)
            {
                Console.Write("write movies release year (in numerical): ");
                if (int.TryParse(Console.ReadLine(), out int res))
                {
                    releaseyear = res;
                    Console.Clear();
                    break;
                }
            }
            while (true)
            {
                Console.Write("write movies rating (numerical) or 'none' if you dont know it: ");
                var smth = Console.ReadLine();
                if (float.TryParse(smth, out float res))
                {
                    rating = res;
                    Console.Clear();
                    break;
                }
                else
                {
                    if (smth == "none")
                    {
                        rating = null;
                        Console.Clear();
                        break;
                    }
                }
            }
            while (true)
            {
                Console.Write("write movies stockQuantity (in numerical): ");
                if (int.TryParse(Console.ReadLine(), out int res))
                {
                    stockQuantity = res;
                    Console.Clear();
                    break;
                }
            }
            ICommand addmovie = new AddValue(title, genre, releaseyear, rating, stockQuantity);
            invoker.SetCommand(addmovie);
            invoker.ExecuteCommand(false);
            break;
        }
    }
    /// <summary>
    /// a method that works as a checker if the user does know what to put in or doesnt and based on that returns specific data
    /// </summary>
    /// <param name="s"> users input </param>
    /// <param name="clear"> bool thats being checked, if true, it should clear the console </param>
    /// <returns></returns>
    public string nullChecker(string s, bool clear = true)
    {
        if (clear) Console.Clear();
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

