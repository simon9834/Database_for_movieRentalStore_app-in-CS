using System;
using System.Diagnostics;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;
MyDatabase mdb = new MyDatabase();
TextHandler th = new TextHandler();
int selectedIndex = 0;
string[] butns = { "[ remove a rental ]", "[ add a movie ]" };
string spaces = "";
string selectedCommand;

while (true)
{
    Console.Clear();
    spaces = th.calculateSpaces(35);
    //chatGPT helped here w the menu sign
    string text = spaces + "  __  __   ______   _   _   _    _  " + Environment.NewLine +
                      spaces + " |  \\/  | |  ____| | \\ | | | |  | | " + Environment.NewLine +
                      spaces + " | \\  / | | |__    |  \\| | | |  | | " + Environment.NewLine +
                      spaces + " | |\\/| | |  __|   | . ` | | |  | |" + Environment.NewLine +
                      spaces + " | |  | | | |____  | |\\  | | |__| | " + Environment.NewLine +
                      spaces + " |_|  |_| |______| |_| \\_| |______| ";
    Console.Write(text);

    Console.WriteLine();


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

    Console.WriteLine("\n\n(Choose with arrows and Enter to select)");

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
        if(selectedCommand == "[ remove a rental ]")
        {
            Console.WriteLine(mdb.RemoveARental("atzijiduchove", "Jarda", "Novak"));
            
        }
        break;
    }
}