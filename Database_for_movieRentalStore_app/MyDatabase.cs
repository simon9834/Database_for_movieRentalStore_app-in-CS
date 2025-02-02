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
                string query1 = $"DECLARE @sql NVARCHAR(MAX) = N'';\r\n\r\n-- Step 1: Drop all Foreign Key Constraints\r\nSELECT @sql += 'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id)) + '.' +\r\n               QUOTENAME(OBJECT_NAME(parent_object_id)) + \r\n               ' DROP CONSTRAINT ' + QUOTENAME(name) + '; ' \r\nFROM sys.foreign_keys;\r\n\r\nPRINT @sql; -- Optional: Print the statements for debugging\r\nEXEC sp_executesql @sql;\r\n\r\n-- Reset the SQL variable\r\nSET @sql = N'';\r\n\r\n-- Step 2: Drop all Tables\r\nSELECT @sql += 'DROP TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(object_id)) + '.' + QUOTENAME(name) + '; ' \r\nFROM sys.tables;\r\n\r\nPRINT @sql; -- Optional: Print the statements for debugging\r\nEXEC sp_executesql @sql;";
                SqlCommand command = new SqlCommand(query1, connection);
                command.ExecuteNonQuery();
                //nacteni dat z tabulky
                /*string query1 = "select * from Tabulka";
                SqlCommand command1 = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader[0].ToString() + " " + reader[1].ToString());
                }*/
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}