﻿using System.IO;
namespace ChatBot_V2._0;
class MemorySystem
{
    private readonly string memoryPath = "Memory.txt";
    private Dictionary<string, string> Memory = new();

    public void SaveInterest(string topic)
    {
        string key = "interest";
        Memory[key] = topic;
        File.WriteAllText(memoryPath, $"{key}|{topic}");
    }

    public string? RecallInterest()
    {
        string key = "interest";
        if (Memory.ContainsKey(key))
            return Memory[key];

        if (File.Exists(memoryPath))
        {
            var line = File.ReadAllText(memoryPath);
            var parts = line.Split('|', 2);
            if (parts.Length == 2 && parts[0] == key)
                return parts[1];
        }
        return null;
    }
}