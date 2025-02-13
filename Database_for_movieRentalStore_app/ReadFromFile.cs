
    public class ReadFromFile
    {
        public Dictionary<string, string> data { get; set; } = new Dictionary<string, string>(); 
        private List<string> whatAmIReading = new List<string>();

        /*data = new Dictionary<string, string>();
        whatAmIReading = new List<string>();
        FillList();*/

        private void FillList()
        {
            whatAmIReading.Add("jmeno");
            whatAmIReading.Add("heslo");
            whatAmIReading.Add("cislo serveru");
        }

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
