public class AddValue : ICommand
{
    private string firstname, lastname, position, hiredate, title, genre;
    private int stockQuantity, releaseYear;
    private float? rating;
    public AddValue(string firstname, string lastname, string position, string hiredate)
    {
        this.firstname = firstname;
        this.lastname = lastname;
        this.position = position;
        this.hiredate = hiredate;
    }
    public AddValue(string title, string genre, int releaseyear, float? rating, int stockQuantity)
    {
        this.title = title;
        this.genre = genre;
        this.releaseYear = releaseyear;
        this.rating = rating;
        this.stockQuantity = stockQuantity;
    }
    public void Execute()
    {
        var mdb = MyDatabase.Instance;
        mdb.AddAnEmployee(firstname, lastname, position, hiredate);
    }

    public void Execute1()
    {
        var mdb = MyDatabase.Instance;
        mdb.AddAMovie(title, genre, releaseYear, rating, stockQuantity);
    }
}

