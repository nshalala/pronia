namespace Pronia.Helpers.Exceptions;

public class SizeLimitException:Exception
{
    public SizeLimitException(string message) : base(message) { }
}