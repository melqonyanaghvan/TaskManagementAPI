namespace TaskManagement.Domain.Enums;

public static class TaskStatus
{
    public const string Pending = "Pending";
    public const string InProgress = "InProgress";
    public const string Completed = "Completed";
    
    public static readonly string[] All = { Pending, InProgress, Completed };
    
    public static bool IsValid(string status)
    {
        return All.Contains(status);
    }
}
