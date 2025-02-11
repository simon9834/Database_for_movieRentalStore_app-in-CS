using System;
using System.ComponentModel;
using System.Data.SqlClient;

public class MyDatabase
{
    private string name;
    private string password;
    private string databaseName = "hloska";
    private string srvr_num;
    public MyDatabase()
    {
        ReadFromFile rff = new ReadFromFile();
        rff.ReadFroFi("prihlas_udaj.txt");
        name = rff.data["jmeno"];
        password = rff.data["heslo"];
        srvr_num = rff.data["cislo serveru"];
        RemoveAllTables();
    }
    public void RemoveAllTables()
    {
        var consStringBuilder = new SqlConnectionStringBuilder();
        consStringBuilder.UserID = name; //prihlasovaci jmeno
        consStringBuilder.Password = password; //heslo
        consStringBuilder.InitialCatalog = databaseName; //jmeno databaze
        consStringBuilder.DataSource = srvr_num; //cislo serveru (skolniho)
        /*consStringBuilder.ConnectTimeout = 30;*/
        try
        {
            using (SqlConnection connection = new SqlConnection(consStringBuilder.ConnectionString))
            {
                connection.Open();
                Console.WriteLine("Pripojeno");
                //the query is written by chatGPT
                string query1 = $"DECLARE @sql NVARCHAR(MAX) = N'';\r\n\r\nSELECT @sql += 'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id)) + '.' +\r\n               QUOTENAME(OBJECT_NAME(parent_object_id)) + \r\n               ' DROP CONSTRAINT ' + QUOTENAME(name) + '; ' \r\nFROM sys.foreign_keys;\r\n\r\nPRINT @sql; -- Optional: Print the statements for debugging\r\nEXEC sp_executesql @sql;\r\n\r\n-- Reset the SQL variable\r\nSET @sql = N'';\r\n\r\n-- Step 2: Drop all Tables\r\nSELECT @sql += 'DROP TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(object_id)) + '.' + QUOTENAME(name) + '; ' \r\nFROM sys.tables;\r\n\r\nPRINT @sql; -- Optional: Print the statements for debugging\r\nEXEC sp_executesql @sql;";
                SqlCommand command = new SqlCommand(query1, connection);
                command.ExecuteNonQuery();
                //the query is written by chatGPT
                string queryCreateDb_W_Tbls = "CREATE TABLE Customers ( CustomerID INT PRIMARY KEY IDENTITY(1,1), FirstName VARCHAR(50) NOT NULL, LastName VARCHAR(50) NOT NULL, Email VARCHAR(100) UNIQUE NOT NULL, Phone VARCHAR(15), Address TEXT, RegistrationDate DATE DEFAULT GETDATE() ); CREATE TABLE Movies ( MovieID INT PRIMARY KEY IDENTITY(1,1), Title VARCHAR(255) NOT NULL, Genre VARCHAR(50), ReleaseYear INT, Rating FLOAT, StockQuantity INT DEFAULT 0 ); CREATE TABLE Employees ( EmployeeID INT PRIMARY KEY IDENTITY(1,1), FirstName VARCHAR(50) NOT NULL, LastName VARCHAR(50) NOT NULL, Position VARCHAR(50), HireDate DATE DEFAULT GETDATE() ); CREATE TABLE Rentals ( RentalID INT PRIMARY KEY IDENTITY(1,1), CustomerID INT, EmployeeID INT, RentalDate DATETIME DEFAULT GETDATE(), ReturnDate DATETIME, Status VARCHAR(10) DEFAULT 'Active', FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID) ON DELETE CASCADE, FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID) ON DELETE SET NULL ); CREATE TABLE RentalDetails ( RentalID INT, MovieID INT, Quantity FLOAT DEFAULT 1, PRIMARY KEY (RentalID, MovieID), FOREIGN KEY (RentalID) REFERENCES Rentals(RentalID) ON DELETE CASCADE, FOREIGN KEY (MovieID) REFERENCES Movies(MovieID) ON DELETE CASCADE ); CREATE TABLE Payments ( PaymentID INT PRIMARY KEY IDENTITY(1,1), RentalID INT UNIQUE, Amount DECIMAL(10,2) NOT NULL, PaymentDate DATETIME DEFAULT GETDATE(), PaymentMethod VARCHAR(20) DEFAULT 'Cash', IsRefunded BIT DEFAULT 0, FOREIGN KEY (RentalID) REFERENCES Rentals(RentalID) ON DELETE CASCADE );";
                SqlCommand command1 = new SqlCommand(queryCreateDb_W_Tbls, connection);
                command1.ExecuteNonQuery();
                Console.WriteLine("all went great");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}