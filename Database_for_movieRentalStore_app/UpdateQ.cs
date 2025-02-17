/// <summary>
/// a class that works as an object in the strategy pattern
/// </summary>
public class UpdateQ : ICommand
    {
    private int quantity;
    private string movie_title;

    /// <summary>
    /// constructor that has 2 values as parameters and sets them in the class
    /// </summary>
    /// <param name="movie_title"></param>
    /// <param name="quantity"></param>
    public UpdateQ(string movie_title, int quantity)
    {
        this.quantity = quantity;
        this.movie_title = movie_title;
    }
    /// <summary>
    /// a method to call the according method in db and pass in the values that have been set before
    /// </summary>
    public void Execute()
    {
        var mdb = MyDatabase.Instance;
        mdb.updtMovieQuantity(movie_title, quantity);
    }

    public void Execute1() { }
}

