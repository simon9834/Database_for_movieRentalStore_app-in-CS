using System.Data.SqlClient;
/// <summary>
/// a class that works as a gate to my actuall database (uses singleton pattern for safety)
/// </summary>
public class MyDatabase
{
    private string name;
    private string password;
    private string databaseName;
    private string srvr_num;
    private SqlConnection _connection;
    private static readonly Lazy<MyDatabase> _instance = new Lazy<MyDatabase>(() => new MyDatabase());
    /// <summary>
    /// constructor of the class setting the basic values for a db connection
    /// </summary>
    private MyDatabase()
    {
        ReadFromFile rff = new ReadFromFile();
        rff.ReadFroFi("prihlas_udaj.txt");
        databaseName = rff.data["database name"];
        name = rff.data["jmeno"];
        password = rff.data["heslo"];
        srvr_num = rff.data["cislo serveru"];
    }
    /// <summary>
    /// singleton of my db
    /// </summary>
    public static MyDatabase Instance => _instance.Value;
    /// <summary>
    /// method that sets all the data needed to connect to the db
    /// </summary>
    public void ConnectionConfig()
    {
        var consStringBuilder = new SqlConnectionStringBuilder();
        consStringBuilder.UserID = name; //prihlasovaci jmeno
        consStringBuilder.Password = password; //heslo
        consStringBuilder.InitialCatalog = databaseName; //jmeno databaze
        consStringBuilder.DataSource = srvr_num; //cislo serveru (popr. skolniho pc)
        /*consStringBuilder.ConnectTimeout = 30;*/
        _connection = new SqlConnection(consStringBuilder.ConnectionString);
    }
    /// <summary>
    /// a method that opens the connection to the database
    /// </summary>
    /// <returns></returns>
    private SqlConnection OpenConnection()
    {
        if (_connection.State == System.Data.ConnectionState.Closed)
        {
            _connection.Open();
        }
        return _connection;
    }
    /// <summary>
    /// a method that closes a connection to the db
    /// </summary>
    private void CloseConnection()
    {
        if (_connection.State == System.Data.ConnectionState.Open)
        {
            _connection.Close();
        }
    }
    /// <summary>
    /// a method to remove all tables and all possible values in them, in the right order
    /// </summary>
    public void RemoveAllTables()
    {
        try
        {
            Console.WriteLine("Connection succesfull");
            //the query is written by chatGPT
            string query = $"DECLARE @sql NVARCHAR(MAX) = N'';\r\n\r\nSELECT @sql += 'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id)) + '.' +\r\n               QUOTENAME(OBJECT_NAME(parent_object_id)) + \r\n               ' DROP CONSTRAINT ' + QUOTENAME(name) + '; ' \r\nFROM sys.foreign_keys;\r\n\r\nPRINT @sql; -- Optional: Print the statements for debugging\r\nEXEC sp_executesql @sql;\r\n\r\n-- Reset the SQL variable\r\nSET @sql = N'';\r\n\r\n-- Step 2: Drop all Tables\r\nSELECT @sql += 'DROP TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(object_id)) + '.' + QUOTENAME(name) + '; ' \r\nFROM sys.tables;\r\n\r\nPRINT @sql; -- Optional: Print the statements for debugging\r\nEXEC sp_executesql @sql;";
            SqlCommand command = new SqlCommand(query, OpenConnection());
            command.ExecuteNonQuery();
            CloseConnection();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    /// <summary>
    /// a method that creates the all the tables inside a db and adds all attributes
    /// </summary>
    public void createMainDB()
    {
        try
        {
            //the query is written by chatGPT
            string queryCreateDb_W_Tbls = "Create Database @Database; Use @Database; CREATE TABLE Customers ( CustomerID INT PRIMARY KEY IDENTITY(1,1), FirstName VARCHAR(50) NOT NULL, LastName VARCHAR(50) NOT NULL, Email VARCHAR(100) UNIQUE NOT NULL, Phone VARCHAR(15), Address TEXT, RegistrationDate DATE DEFAULT GETDATE() ); CREATE TABLE Movies ( MovieID INT PRIMARY KEY IDENTITY(1,1), Title VARCHAR(255) NOT NULL UNIQUE, Genre VARCHAR(50), ReleaseYear INT, Rating FLOAT, StockQuantity INT DEFAULT 0 ); CREATE TABLE Employees ( EmployeeID INT PRIMARY KEY IDENTITY(1,1), FirstName VARCHAR(50) NOT NULL, LastName VARCHAR(50) NOT NULL, Position VARCHAR(50), HireDate DATE DEFAULT GETDATE() ); CREATE TABLE Rentals ( RentalID INT PRIMARY KEY IDENTITY(1,1), CustomerID INT, EmployeeID INT, RentalDate DATETIME DEFAULT GETDATE(), ReturnDate DATETIME, Status VARCHAR(10) DEFAULT 'Active', FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID) ON DELETE CASCADE, FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID) ON DELETE SET NULL ); CREATE TABLE RentalDetails ( RentalID INT, MovieID INT, Quantity FLOAT DEFAULT 1, PRIMARY KEY (RentalID, MovieID), FOREIGN KEY (RentalID) REFERENCES Rentals(RentalID) ON DELETE CASCADE, FOREIGN KEY (MovieID) REFERENCES Movies(MovieID) ON DELETE CASCADE ); CREATE TABLE Payments ( PaymentID INT PRIMARY KEY IDENTITY(1,1), RentalID INT UNIQUE, Amount DECIMAL(10,2) NOT NULL, PaymentDate DATETIME DEFAULT GETDATE(), PaymentMethod VARCHAR(20) DEFAULT 'Cash', IsRefunded BIT DEFAULT 0, FOREIGN KEY (RentalID) REFERENCES Rentals(RentalID) ON DELETE CASCADE );";
            SqlCommand command1 = new SqlCommand(queryCreateDb_W_Tbls, OpenConnection());
            command1.Parameters.Add(new SqlParameter("@Database", databaseName));
            command1.ExecuteNonQuery();
            Console.WriteLine("The database was succesfully built");
            CloseConnection();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    /// <summary>
    /// a method that removes a rental from the table of rentals (if exists)
    /// </summary>
    /// <param name="movieName"></param>
    /// <param name="cstmrFiName"></param>
    /// <param name="cstmrLaName"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    public string RemoveARental(string movieName, string? cstmrFiName = null, string? cstmrLaName = null, string? email = null)
    {
        try
        {
            string query = "DELETE FROM Rentals WHERE RentalID IN (SELECT RentalID FROM RentalDetails WHERE MovieID IN (SELECT Movies.MovieID FROM Movies WHERE Title = @Title)) AND" +
            " CustomerID IN (SELECT CustomerID FROM Customers WHERE 1=1";

            //sql injection protection
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@title", movieName)
            };

            if (!string.IsNullOrEmpty(cstmrFiName))
            {
                query += " AND FirstName = @FirstName";
                parameters.Add(new SqlParameter("@FirstName", cstmrFiName));
            }

            if (!string.IsNullOrEmpty(cstmrLaName))
            {
                query += " AND LastName = @LastName";
                parameters.Add(new SqlParameter("@LastName", cstmrLaName));
            }
            if (!string.IsNullOrEmpty(email))
            {
                query += " AND Email = @Email";
                parameters.Add(new SqlParameter("@Email", email));
            }
            query += ");";
            using (SqlCommand command = new SqlCommand(query, OpenConnection()))
            {
                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }
                int rowsAffected = command.ExecuteNonQuery();
                CloseConnection();
                return $"{rowsAffected} rentals deleted.";
            }
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    /// <summary>
    /// a method to insert csv data into the database
    /// </summary>
    /// <param name="customersCSV"></param>
    /// <param name="employeesCSV"></param>
    /// <returns></returns>
    public string insertCSV(string? customersCSV = "customers.csv", string? employeesCSV = "employees.csv")
    {
        string line;
        bool isFirstLine = true;
        try
        {
            using (StreamReader sr = new StreamReader(customersCSV))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }
                    string[] values = line.Split(',');
                    string query = "INSERT INTO Customers (FirstName, LastName, Email, Phone, Address, RegistrationDate) " +
                                   "VALUES (@FirstName, @LastName, @Email, @Phone, @Address, @RegistrationDate)";

                    using (SqlCommand command = new SqlCommand(query, OpenConnection()))
                    {
                        command.Parameters.AddWithValue("@FirstName", values[0]);
                        command.Parameters.AddWithValue("@LastName", values[1]);
                        command.Parameters.AddWithValue("@Email", values[2]);
                        command.Parameters.AddWithValue("@Phone", values[3]);
                        command.Parameters.AddWithValue("@Address", values[4]);
                        command.Parameters.AddWithValue("@RegistrationDate", DateTime.Parse(values[5]));
                        command.ExecuteNonQuery();
                        CloseConnection();
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message, "\ncustomers weren't inserted\n");
        }
        try
        {
            using (StreamReader sr1 = new StreamReader(employeesCSV))
            {
                while ((line = sr1.ReadLine()) != null)
                {
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }
                    string[] values = line.Split(',');
                    string query = "INSERT INTO Employees (FirstName, LastName, Position, HireDate) " +
                                   "VALUES (@FirstName, @LastName, @Position, @HireDate)";
                    using (SqlCommand command = new SqlCommand(query, OpenConnection()))
                    {
                        // Add parameters for each column in the Employees table
                        command.Parameters.AddWithValue("@FirstName", values[1]);
                        command.Parameters.AddWithValue("@LastName", values[2]);
                        command.Parameters.AddWithValue("@Position", values[3]);
                        command.Parameters.AddWithValue("@HireDate", DateTime.Parse(values[4]));
                        command.ExecuteNonQuery();
                        CloseConnection();
                    }
                }
            }
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e.Message, "\ncustomers weren't inserted\n");
        }
        return "Employees and Customers inserted.";
    }
    /// <summary>
    /// a method that adds an employee to the table of employees
    /// </summary>
    /// <param name="firstname"></param>
    /// <param name="lastname"></param>
    /// <param name="position"></param>
    /// <param name="hiredate"></param>
    public void AddAnEmployee(string firstname, string lastname, string position, string hiredate)
    {
        string query = "INSERT INTO Employees (FirstName, LastName, Position, HireDate) " +
                                   "VALUES (@FirstName, @LastName, @Position, @HireDate)";
        using (SqlCommand command = new SqlCommand(query, OpenConnection()))
        {
            command.Parameters.AddWithValue("@FirstName", firstname);
            command.Parameters.AddWithValue("@LastName", lastname);
            command.Parameters.AddWithValue("@Position", string.IsNullOrEmpty(position) ? DBNull.Value : position);
            command.Parameters.AddWithValue("@HireDate", string.IsNullOrEmpty(hiredate) ? DBNull.Value : DateTime.Parse(hiredate));
            command.ExecuteNonQuery();
            CloseConnection();
        }
        Console.WriteLine("Employee added.");
    }
    /// <summary>
    /// a method that adds a movie to the table of movies
    /// </summary>
    /// <param name="title"></param>
    /// <param name="genre"></param>
    /// <param name="releaseyear"></param>
    /// <param name="rating"></param>
    /// <param name="stockquantity"></param>
    public void AddAMovie(string title, string genre, int releaseyear, float? rating, int stockquantity)
    {
        string query = "INSERT INTO Movies (Title, Genre, ReleaseYear, Rating, StockQuantity) " +
                                   "VALUES (@Title, @Genre, @ReleaseYear, @Rating, @StockQuantity)";
        using (SqlCommand command = new SqlCommand(query, OpenConnection()))
        {
            command.Parameters.AddWithValue("@Title", title);
            command.Parameters.AddWithValue("@Genre", string.IsNullOrEmpty(genre) ? DBNull.Value : genre);
            command.Parameters.AddWithValue("@ReleaseYear", releaseyear);
            command.Parameters.AddWithValue("@Rating", rating.HasValue ? DBNull.Value : rating);
            command.Parameters.AddWithValue("@StockQuantity", stockquantity);
            command.ExecuteNonQuery();
            CloseConnection();
        }
        Console.WriteLine("Movie added.");
    }
    /// <summary>
    /// a method that can change the quantity of a film in shop
    /// </summary>
    /// <param name="movie_title"></param>
    /// <param name="newQuantity"></param>
    public void updtMovieQuantity(string movie_title, int newQuantity)
    {
        string query = "UPDATE Movies " +
                       "SET StockQuantity = @NewQuantity " +
                       "WHERE Title = @MovieTitle";

        using (SqlCommand command = new SqlCommand(query, OpenConnection()))
        {
            command.Parameters.AddWithValue("@NewQuantity", newQuantity);
            command.Parameters.AddWithValue("@MovieTitle", movie_title);
            command.ExecuteNonQuery();

            Console.WriteLine("Movie quantity updated successfully.");
        }
    }
}