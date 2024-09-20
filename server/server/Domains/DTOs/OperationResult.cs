using server.Domains.Enums;

namespace server.Domains.DTOs;

public class OperationResult<T>
{
    public bool IsSuccess { get; }
    public ErrorCode ErrorCode { get; }
    public T Data { get; }

    private OperationResult(bool isSuccess, ErrorCode errorCode, T data = default!)
    {
        IsSuccess = isSuccess;
        ErrorCode = errorCode;
        Data = data;
    }

    public static OperationResult<T> Success(T data) => new OperationResult<T>(true, 0, data);

    public static OperationResult<T> Failure(ErrorCode statusCode)
    {
        return new OperationResult<T>(false, statusCode);
    }
}
public class OperationResult
{
    public bool IsSuccess { get; }
    public object Data { get; }
    public ErrorCode ErrorCode { get; }

    private OperationResult(bool isSuccess, ErrorCode errorCode)
    {
        IsSuccess = isSuccess;
        ErrorCode = errorCode;
    }

    public static OperationResult Success() => new OperationResult(true, 0);

    public static OperationResult Failure(ErrorCode statusCode)
    {
        return new OperationResult(false, statusCode);
    }
}