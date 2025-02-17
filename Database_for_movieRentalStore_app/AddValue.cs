/// <summary>
/// class that inherits ICommand interface to work in a strategy pattern
/// </summary>
public class AddValue : ICommand
{
    private string firstname, lastname, position, hiredate, title, genre;
    private int stockQuantity, releaseYear;
    private float? rating;
    /// <summary>
    /// constructor with 4 inputs
    /// </summary>
    /// <param name="firstname"></param>
    /// <param name="lastname"></param>
    /// <param name="position"></param>
    /// <param name="hiredate"></param>
    public AddValue(string firstname, string lastname, string position, string hiredate)
    {
        this.firstname = firstname;
        this.lastname = lastname;
        this.position = position;
        this.hiredate = hiredate;
    }
    /// <summary>
    /// constructor overload with 5 inputs
    /// </summary>
    /// <param name="title"></param>
    /// <param name="genre"></param>
    /// <param name="releaseyear"></param>
    /// <param name="rating"></param>
    /// <param name="stockQuantity"></param>
    public AddValue(string title, string genre, int releaseyear, float? rating, int stockQuantity)
    {
        this.title = title;
        this.genre = genre;
        this.releaseYear = releaseyear;
        this.rating = rating;
        this.stockQuantity = stockQuantity;
    }
    /// <summary>
    /// execute method that calls the database method
    /// </summary>
    public void Execute()
    {
        var mdb = MyDatabase.Instance;
        mdb.AddAnEmployee(firstname, lastname, position, hiredate);
    }
    /// <summary>
    /// execute method that calls the database method
    /// </summary>
    public void Execute1()
    {
        var mdb = MyDatabase.Instance;
        mdb.AddAMovie(title, genre, releaseYear, rating, stockQuantity);
    }
}

