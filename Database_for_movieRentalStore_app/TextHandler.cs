
public class TextHandler
{
    private int width = Console.WindowWidth;
    private int height = Console.WindowHeight;
    public string calculateSpaces(int inputsWidth)
    {
        int padding = (width - inputsWidth) / 2;
        if (padding < 0) padding = 0;
        string spaces = new string(' ', padding);
        return spaces;
    }
}

