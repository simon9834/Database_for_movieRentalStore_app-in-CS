using LamarCodeGeneration.Util.TextWriting;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Numerics;

public class MyDatabase
{
    private string name;
    private string password;
    private string databaseName = "hloska";
    private string srvr_num;
    private SqlConnection _connection;
    private static readonly Lazy<MyDatabase> _instance = new Lazy<MyDatabase>(() => new MyDatabase());
    private MyDatabase()
    {
        ReadFromFile rff = new ReadFromFile();
        rff.ReadFroFi("prihlas_udaj.txt");
        name = rff.data["jmeno"];
        password = rff.data["heslo"];
        srvr_num = rff.data["cislo serveru"];
    }
    public static MyDatabase Instance => _instance.Value;
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
    //check metoda od chatGPT
    public SqlConnection OpenConnection()
    {
        if (_connection.State == System.Data.ConnectionState.Closed)
        {
            _connection.Open();
        }
        return _connection;
    }
    public void CloseConnection()
    {
        if (_connection.State == System.Data.ConnectionState.Open)
        {
            _connection.Close();
        }
    }

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
    public void createMainDB()
    {
        try
        {
            //the query is written by chatGPT
            string queryCreateDb_W_Tbls = "CREATE TABLE Customers ( CustomerID INT PRIMARY KEY IDENTITY(1,1), FirstName VARCHAR(50) NOT NULL, LastName VARCHAR(50) NOT NULL, Email VARCHAR(100) UNIQUE NOT NULL, Phone VARCHAR(15), Address TEXT, RegistrationDate DATE DEFAULT GETDATE() ); CREATE TABLE Movies ( MovieID INT PRIMARY KEY IDENTITY(1,1), Title VARCHAR(255) NOT NULL, Genre VARCHAR(50), ReleaseYear INT, Rating FLOAT, StockQuantity INT DEFAULT 0 ); CREATE TABLE Employees ( EmployeeID INT PRIMARY KEY IDENTITY(1,1), FirstName VARCHAR(50) NOT NULL, LastName VARCHAR(50) NOT NULL, Position VARCHAR(50), HireDate DATE DEFAULT GETDATE() ); CREATE TABLE Rentals ( RentalID INT PRIMARY KEY IDENTITY(1,1), CustomerID INT, EmployeeID INT, RentalDate DATETIME DEFAULT GETDATE(), ReturnDate DATETIME, Status VARCHAR(10) DEFAULT 'Active', FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID) ON DELETE CASCADE, FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID) ON DELETE SET NULL ); CREATE TABLE RentalDetails ( RentalID INT, MovieID INT, Quantity FLOAT DEFAULT 1, PRIMARY KEY (RentalID, MovieID), FOREIGN KEY (RentalID) REFERENCES Rentals(RentalID) ON DELETE CASCADE, FOREIGN KEY (MovieID) REFERENCES Movies(MovieID) ON DELETE CASCADE ); CREATE TABLE Payments ( PaymentID INT PRIMARY KEY IDENTITY(1,1), RentalID INT UNIQUE, Amount DECIMAL(10,2) NOT NULL, PaymentDate DATETIME DEFAULT GETDATE(), PaymentMethod VARCHAR(20) DEFAULT 'Cash', IsRefunded BIT DEFAULT 0, FOREIGN KEY (RentalID) REFERENCES Rentals(RentalID) ON DELETE CASCADE );";
                SqlCommand command1 = new SqlCommand(queryCreateDb_W_Tbls, OpenConnection());
            command1.ExecuteNonQuery();
            Console.WriteLine("The database was succesfully built");
            CloseConnection();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    public string RemoveARental(string movieName, string? cstmrFiName = null, string? cstmrLaName = null, string? email = null)
    {
        try
        {
            string query = "DELETE FROM Rentals WHERE RentalID IN (SELECT RentalID FROM RentalDetails WHERE MovieID IN (SELECT MovieID FROM Movies WHERE title = @title)) AND" +
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
                return $"{rowsAffected} rentals deleted!";
            }
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    public void insertCSV(string customersCSV, string employeesCSV)
    {
        string line;
        bool isFirstLine = true;
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
                    command.Parameters.AddWithValue("@FirstName", values[1]);
                    command.Parameters.AddWithValue("@LastName", values[2]);
                    command.Parameters.AddWithValue("@Email", values[3]);
                    command.Parameters.AddWithValue("@Phone", values[4]);
                    command.Parameters.AddWithValue("@Address", values[5]);
                    command.Parameters.AddWithValue("@RegistrationDate", DateTime.Parse(values[6]));
                    command.ExecuteNonQuery();
                    CloseConnection();
                }
            }
        }
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
}