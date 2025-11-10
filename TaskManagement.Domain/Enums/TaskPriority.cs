namespace TaskManagement.Domain.Enums;

public static class TaskPriority
{
    public const string Low = "Low";
    public const string Medium = "Medium";
    public const string High = "High";
    
    public static readonly string[] All = { Low, Medium, High };
    
    public static bool IsValid(string priority)
    {
        return All.Contains(priority);
    }
}
