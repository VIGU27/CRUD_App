namespace CRUD_App.Exceptions;

/// <summary>Thrown when a requested entity does not exist. Mapped to HTTP 404.</summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}

/// <summary>Thrown when a request references invalid/non-existent related data. Mapped to HTTP 400.</summary>
public class ValidationException : Exception
{
    public ValidationException(string message) : base(message)
    {
    }
}
