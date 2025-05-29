namespace apbd_cw11.Exceptions;

public class MyExceptionWhenConflict : Exception
{
    public MyExceptionWhenConflict()
    {
    }

    public MyExceptionWhenConflict(string? message) : base(message)
    {
    }

    public MyExceptionWhenConflict(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}