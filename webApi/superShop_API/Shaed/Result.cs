namespace superShop_API.Shared;

public class Result
{
    /// <summary>
    /// Determine if a Task has been executed Succesfully
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Message to send
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// In case we have an exception performing our task
    /// </summary>
    public Exception? Exception { get; set; }

    private object? _Data { get; set; }

    private Result()
    {
        IsSuccess = true;
        Message = string.Empty;
    }

    public Result Fail(string errorMessage, Exception? exception = null, object? data = null)
    {
        if (exception != null)
        {
            Exception = exception;
        }

        if (data != null)
        {
            _Data = data;
        }

        Message = errorMessage;

        IsSuccess = false;
        return this;
    }

    public Result Success(string message, object? data = null)
    {
        if (data != null)
        {
            _Data = data;
        }

        IsSuccess = true;
        Message = message;
        return this;
    }

    public object? Data()
    {
        return _Data;
    }

    public T? Data<T>()
    {
        return (T?)_Data;
    }

    public Tt? Data<Tt>(Tt pbj)
    {
        return (Tt?)_Data;
    }

    public static Result Instance()
    {
        return new Result();
    }
}