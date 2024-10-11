namespace server.Domains.Enums;

public enum ErrorCode
{
    None = 0,
    ErrorWhileCreatingDirectory = 10,
    UnauthorizedAccessException,
    PathIsWrong,
    DirectoryNotFoundException,
}