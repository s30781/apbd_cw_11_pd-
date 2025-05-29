namespace apbd_cw11.Exceptions;


public class MyExceptionWhenNotFound : Exception
{
    public MyExceptionWhenNotFound()
    {
    }

    public MyExceptionWhenNotFound(string? message) : base(message)
    {
    }

    public MyExceptionWhenNotFound(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}