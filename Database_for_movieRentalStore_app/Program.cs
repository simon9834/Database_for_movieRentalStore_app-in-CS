using System;
using System.Diagnostics;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;
var mdb = MyDatabase.Instance;
mdb.ConnectionConfig();
//mdb.RemoveAllTables();
//mdb.createMainDB();
ButtonExecution be = new ButtonExecution();
TextHandler th = new TextHandler();
int selectedIndex = 0;
string[] butns = { "[ remove a rental ]", "[ add an employee ]", "[ add a movie ]", "[ insert data into DB ]" };
string spaces = "";
string selectedCommand;

while (true)
{
    int btnLength = 0;
    Console.Clear();

    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();

    spaces = th.calculateSpaces(36);
    //chatGPT helped here w the menu sign
    string text = spaces + "  __  __   ______   _   _   _    _  " + Environment.NewLine +
                      spaces + " |  \\/  | |  ____| | \\ | | | |  | | " + Environment.NewLine +
                      spaces + " | \\  / | | |__    |  \\| | | |  | | " + Environment.NewLine +
                      spaces + " | |\\/| | |  __|   | . ` | | |  | | " + Environment.NewLine +
                      spaces + " | |  | | | |____  | |\\  | | |__| | " + Environment.NewLine +
                      spaces + " |_|  |_| |______| |_| \\_| |______| ";
    Console.WriteLine(text);

    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();

    foreach (var button in butns)
    {
        if(btnLength != 0)
        {
            btnLength += 3;
        }
        btnLength += button.Length;
    }
    
    Console.Write(th.calculateSpaces(btnLength));

    for (int i = 0; i < butns.Length; i++)
    {
        if (i == selectedIndex)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(butns[i]);
            Console.ResetColor();
        }
        else
        {
            Console.Write(butns[i]);
        }
        Console.Write("   ");
    }

    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();

    text = "(Choose with arrows and select by clicking Enter)";
    Console.Write(th.calculateSpaces(text.Length));
    Console.WriteLine(text);

    ConsoleKeyInfo key = Console.ReadKey();

    if (key.Key == ConsoleKey.LeftArrow)
    {
        selectedIndex = (selectedIndex == 0) ? butns.Length - 1 : selectedIndex - 1;
    }
    else if (key.Key == ConsoleKey.RightArrow)
    {
        selectedIndex = (selectedIndex == butns.Length - 1) ? 0 : selectedIndex + 1;
    }
    else if (key.Key == ConsoleKey.Enter)
    {
        Console.Clear();
        selectedCommand = butns[selectedIndex].Trim();
        switch (selectedCommand)
        {
            case "[ remove a rental ]":
                be.delete();
                break;
            case "[ insert data into DB ]":
                be.insertCSVData();
                break;
            case "[ add an employee ]":
                be.addAnEmployee();
                    break;
            case "[ add a movie ]":
                be.addAMovie();
                break;
        }
        break;
    }
}