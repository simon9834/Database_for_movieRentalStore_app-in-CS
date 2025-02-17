/// <summary>
/// a class that reads the data for connection to the db from a file
/// </summary>
public class ReadFromFile
{
    public Dictionary<string, string> data { get; set; } = new Dictionary<string, string>();
    private List<string> whatAmIReading = new List<string>();
    /// <summary>
    /// a method to fill my list with string values
    /// </summary>
    private void FillList()
    {
        whatAmIReading.Add("jmeno");
        whatAmIReading.Add("heslo");
        whatAmIReading.Add("cislo serveru");
    }
    /// <summary>
    /// a method that reads the connection data from the file and gives them to the db
    /// </summary>
    /// <param name="pathway"></param>
    public void ReadFroFi(string pathway)
    {
        FillList();
        try
        {
            using (StreamReader sr = new StreamReader(pathway))
            {
                string line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    data.Add(whatAmIReading[i], line);
                    i++;
                }
            }
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine(ex.Message + "\n\n - bro tady user input fakt nezandal");
        }
    }
}
