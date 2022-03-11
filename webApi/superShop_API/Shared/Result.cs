namespace superShop_API.Shared;

public class Result<T>
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

    public T? Data { get; set; }

    internal Result()
    {
        IsSuccess = true;
        Message = string.Empty;
    }

    public Result<T> Fail(string errorMessage, Exception? exception = null)
    {
        if (exception != null)
        {
            Exception = exception;
        }

        Message = errorMessage;

        IsSuccess = false;
        return this;
    }

    public Result<T> Fail(string errorMessage, T data, Exception? exception = null)
    {
        if (exception != null)
        {
            Exception = exception;
        }

        if (data != null)
        {
            Data = data;
        }

        Message = errorMessage;

        IsSuccess = false;
        return this;
    }

    public Result<T> Success(string message)
    {
        IsSuccess = true;
        Message = message;
        return this;
    }

    public Result<T> Success(string message, T data)
    {
        if (data != null)
        {
            Data = data;
        }

        IsSuccess = true;
        Message = message;
        return this;
    }
}

public class Result
{
    public static Result<T> Instance<T>(T instanceType)
    {
        return new Result<T>();
    }

    public static Result<T> Instance<T>()
    {
        return new Result<T>();
    }

    public static Result<Object> Instance()
    {
        return new Result<Object>();
    }
}