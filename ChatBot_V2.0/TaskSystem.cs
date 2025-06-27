using System.Globalization;
using System.Text.RegularExpressions;
public enum TaskStatus
{
    Pending,
    Completed
}

public class SecurityTask
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? ReminderTime { get; set; }
    public bool Reminded { get; set; } = false;
    public TaskStatus Status { get; set; } = TaskStatus.Pending;

    public override string ToString()
    {
        string reminder = ReminderTime.HasValue ? $" ⏰ Reminder: {ReminderTime.Value:g}" : " No reminder set";
        string status = Status == TaskStatus.Completed ? "✅ Completed" : "🕒 Pending";
        return $"• {Title} - {Description}\n   {status}{(ReminderTime.HasValue ? reminder : "")}";
    }
}

public class TaskSystem
{
    private List<SecurityTask> tasks = new();

    public void AddTask(SecurityTask task)
    {
        tasks.Add(task);
    }

    public bool CompleteTask(string title)
    {
        var task = tasks.FirstOrDefault(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (task != null)
        {
            task.Status = TaskStatus.Completed;
            return true;
        }
        return false;
    }

    public bool DeleteTask(string title)
    {
        var task = tasks.FirstOrDefault(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (task != null)
        {
            tasks.Remove(task);
            return true;
        }
        return false;
    }

    public List<SecurityTask> GetAllTasks() => tasks;

    public List<SecurityTask> GetDueTasks()
    {
        var now = DateTime.Now;
        return tasks.Where(t => !t.Reminded && t.ReminderTime.HasValue && t.ReminderTime <= now).ToList();
    }

    public void MarkAsReminded(SecurityTask task)
    {
        task.Reminded = true;
    }
}

public static class ReminderParser
{
    public static DateTime? ParseReminder(string input)
    {
        input = input.ToLower().Trim();

        // Specific date (e.g., "on 2025-07-01")
        if (Regex.IsMatch(input, @"\bon\s+\d{4}-\d{2}-\d{2}\b"))
        {
            string dateString = Regex.Match(input, @"\d{4}-\d{2}-\d{2}").Value;
            if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }
        }

        // In X days or weeks
        var match = Regex.Match(input, @"in\s+(\d+)\s+(day|days|week|weeks)");
        if (match.Success)
        {
            int amount = int.Parse(match.Groups[1].Value);
            string unit = match.Groups[2].Value;

            return unit.StartsWith("week")
                ? DateTime.Now.AddDays(amount * 7)
                : DateTime.Now.AddDays(amount);
        }

        // "Tomorrow"
        if (input.Contains("tomorrow"))
            return DateTime.Now.AddDays(1);

        // Extend here if you want "next Monday", etc.

        return null; // Unable to parse
    }
}




