using System;
using System.Collections.Generic;
using System.IO;

namespace ChatBot_V2._0
{
    public class ActivityLogSystem
    {
        private readonly List<string> logEntries = new();
        private readonly string logFilePath = "ActivityLog.txt";

        public void Log(string message)
        {
            string entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm}] {message}";
            logEntries.Add(entry);

            File.AppendAllLines(logFilePath, new[] { entry });
        }

        public List<string> GetRecentLogs(int maxEntries = 10)
        {
            return logEntries.Count <= maxEntries
                ? new List<string>(logEntries)
                : logEntries.GetRange(logEntries.Count - maxEntries, maxEntries);
        }

        public void LoadLogFromFile()
        {
            if (File.Exists(logFilePath))
            {
                var lines = File.ReadAllLines(logFilePath);
                logEntries.Clear();
                logEntries.AddRange(lines);
            }
        }
    }
}
