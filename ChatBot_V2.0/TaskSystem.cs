using System.Globalization;
using System.Text.RegularExpressions;
public enum TaskStatus
{
    Pending,
    Completed
}

//get and set methods for the tasksystem
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


//complete,delete and add task methods that allow for adding, completing, and deleting tasks, as well as viewing all tasks 
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


//determines the reminder time based on user input, "in 10 minutes" and specific times like "at 3:00 PM"

public static class ReminderParser
{
    public static DateTime? ParseReminder(string input)
    {
        input = input.ToLower();

        var inMatch = Regex.Match(input, @"remind me in (\d+)\s*(minutes?|hours?|days?)");
        if (inMatch.Success)
        {
            int amount = int.Parse(inMatch.Groups[1].Value);
            string unit = inMatch.Groups[2].Value;

            return unit switch
            {
                "minute" or "minutes" => DateTime.Now.AddMinutes(amount),
                "hour" or "hours" => DateTime.Now.AddHours(amount),
                "day" or "days" => DateTime.Now.AddDays(amount),
                _ => null
            };
        }

        var atMatch = Regex.Match(input, @"remind me at (\d{1,2}):(\d{2})");
        if (atMatch.Success)
        {
            int hour = int.Parse(atMatch.Groups[1].Value);
            int minute = int.Parse(atMatch.Groups[2].Value);
            DateTime now = DateTime.Now;
            DateTime reminder = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);
            if (reminder < now)
                reminder = reminder.AddDays(1);
            return reminder;
        }

        if (input.Contains("remind me tomorrow"))
        {
            return DateTime.Today.AddDays(1).AddHours(9);
        }
        return null;
    }
}