/// <summary>
/// an additional class for nice text position in the console
/// </summary>
public class TextHandler
{
    private int width;
    private int height;
    /// <summary>
    /// a method that calculates where should the inputted text be placed
    /// </summary>
    /// <param name="inputsWidth">the width of a string</param>
    /// <returns>returns a number of spaces that should be before the text in the console</returns>
    public string calculateSpaces(int inputsWidth)
    {
        width = Console.WindowWidth;
        height = Console.WindowHeight;
        int padding = (width - inputsWidth) / 2;
        if (padding < 0) padding = 0;
        string spaces = new string(' ', padding);
        return spaces;
    }
}

