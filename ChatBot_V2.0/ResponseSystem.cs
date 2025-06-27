using com.sun.org.apache.bcel.@internal.generic;
using System.IO;
using System.Text.Json;
using static QuizSystem;
using SharpNL.Tokenize;
using SharpNL.POSTag;
using SharpNL.Utility;


namespace ChatBot_V2._0;
class ResponseSystem
{
    private Dictionary<string, List<string>> CabbyResponses = new();
    private Dictionary<string, string> lastResponses = new();
    public QuizSystem quiz = new QuizSystem();
    public TaskSystem taskSystem = new();
    // public NlpEngine nlp = new NlpEngine();
    public MemorySystem memory = new();
    public MoodSystem mood = new();

    public ResponseSystem()
    {
        LoadResponses("ChatbotResponses.txt");
    }

    private void LoadResponses(string filePath)
    {
        foreach (var line in File.ReadAllLines("ChatbotResponses.txt"))
        {
            if (!line.Contains('|')) continue;

            var parts = line.Split('|', 2);
            string key = parts[0].Trim().ToLower();
            string jsonList = parts[1].Trim();

            try
            {
                var responseList = JsonSerializer.Deserialize<List<string>>(jsonList);
                if (responseList != null)
                    CabbyResponses[key] = responseList;
            }
            catch { }
        }
    }

    string FormatQuestion(QuizQuestions question)
    {
        var lines = new List<string?> { question.QuestionText };

        if (question.Type == QuestionType.TrueFalse)
        {
            lines.Add("A: True");
            lines.Add("B: False");
        }
        else
        {
            var labels = new[] { "A", "B", "C", "D" };
            for (int i = 0; i < question?.Options?.Length; i++)
            {
                lines.Add($"{labels[i]}: {question.Options[i]}");
            }
        }

        return string.Join("\n", lines);
    }

    public string ProcessInput(string input, string userName)
    {


        if (string.IsNullOrWhiteSpace(input))
            return "I didn't quite get that, could you tell me again?";

        string userInput = input.ToLower();

        if (userInput.Contains("start quiz"))
        {
            quiz.BeginQuiz();
            var question = quiz.GetCurrentQuestion();
            return $"Quiz started!\n\n{FormatQuestion(question)}";
        }

        MoodSystem.Mood currentMood = mood.DetermineMood(userInput);
        string? moodKey = currentMood switch
        {
            MoodSystem.Mood.Positive => "mood_positive",
            MoodSystem.Mood.Negative => "mood_negative",
            _ => null
        };

        if (userInput.StartsWith("add task"))
        {
            string content = input.Substring("add task".Length).Trim();

            string title = "Untitled Task";
            string description = "";
            DateTime? reminderTime = null;

            if (content.Contains("-"))
            {
                var parts = content.Split("-", 2);
                title = parts[0].Trim();
                description = parts[1].Trim();
            }
            else
            {
                title = content;
                description = "No description provided.";
            }

            reminderTime = ReminderParser.ParseReminder(input);

            var task = new SecurityTask
            {
                Title = title,
                Description = description,
                ReminderTime = reminderTime
            };

            taskSystem.AddTask(task);

            string confirm = $"Task added: \"{title}\" - {description}";
            if (reminderTime.HasValue)
                confirm += $"\n⏰ Reminder set for {reminderTime.Value:g}.";
            else
                confirm += "\nNo reminder was set.";

            return confirm;
        }

        if (userInput.StartsWith("view tasks"))
        {
            var all = taskSystem.GetAllTasks();
            if (all.Count == 0) return "You currently have no tasks.";

            return "🗂️ Your tasks:\n" + string.Join("\n\n", all.Select(t => t.ToString()));
        }

        if (userInput.StartsWith("complete task"))
        {
            string title = input.Substring("complete task".Length).Trim();
            bool success = taskSystem.CompleteTask(title);
            return success ? $"✅ Task \"{title}\" marked as completed." : $"Task \"{title}\" not found.";
        }

        if (userInput.StartsWith("delete task"))
        {
            string title = input.Substring("delete task".Length).Trim();
            bool success = taskSystem.DeleteTask(title);
            return success ? $"🗑️ Task \"{title}\" deleted." : $"Task \"{title}\" not found.";
        }

        //Start Quiz logic bellow  

        int answerIndex = -1;
        if (quiz.quizActive)
        {
            var current = quiz.GetCurrentQuestion();
            if (current.Type == QuestionType.MultipleChoice)
            {
                answerIndex = userInput switch
                {
                    "a" => 0,
                    "b" => 1,
                    "c" => 2,
                    "d" => 3,
                    _ => -1
                };
            }
            else if (current.Type == QuestionType.TrueFalse)
            {
                answerIndex = userInput.ToLower() switch
                {
                    "true" => 0,
                    "false" => 1,
                    _ => -1
                };
            }

            if (answerIndex == -1)
                return "Please answer with A, B, C, D or True/False.";

            string result = quiz.sendAnswer(answerIndex);
            if (quiz.NextQuestion())
            {
                var next = quiz.GetCurrentQuestion();
                return $"{result}\n\nNext:\n{FormatQuestion(next)}";
            }
            else
            {
                int finalScore = quiz.score;
                quiz.EndQuiz();

                if (finalScore <= 5)
                {
                    return $"{result}\n\nQuiz complete! You scored {finalScore} out of {quiz.TotalQuestions}.\n Keep learning to stay safe online!";
                }
                if (finalScore > 6)
                {
                    return $"{result}\n\nQuiz complete! You scored {finalScore} out of {quiz.TotalQuestions}.\n Great job ! You're a CyberSecurity pro!";
                }
            }
        }

        if (moodKey != null && CabbyResponses.ContainsKey(moodKey))
        {
            var moodResponses = CabbyResponses[moodKey];
            return moodResponses[new Random().Next(moodResponses.Count)];
        }

        if (userInput.Contains("what was i interested in"))
        {
            string? topic = memory.RecallInterest();
            return topic != null ? $"You said you were interested in {topic}." : "I don't remember you mentioning an interest yet.";
        }

        if (userInput.StartsWith("im interested in") ||
            userInput.StartsWith("i'm interested in") ||
            userInput.StartsWith("i am interested in"))
        {
            string topic = input.Replace("i'm interested in", "", StringComparison.OrdinalIgnoreCase)
                                .Replace("i am interested in", "", StringComparison.OrdinalIgnoreCase)
                                .Replace("im interested in", "", StringComparison.OrdinalIgnoreCase)
                                .Trim();
            memory.SaveInterest(topic);
            return $"Got it! I've noted that you're interested in {topic}.";
        }

        if (userInput.Contains("my name"))
        {
            string? name = File.Exists("Memory.txt") ? File.ReadAllText("Memory.txt") : null;
            return name != null ? $"Your name is {name}." : "I don't know your name.";
        }

        string? matched = CabbyResponses.Keys
            .OrderByDescending(k => k.Length)
            .FirstOrDefault(key => userInput.Contains(key.Replace("_", " ")));

        if (matched != null)
        {
            var responses = CabbyResponses[matched];
            return responses[new Random().Next(responses.Count)];
        }
        return "I don't quite understand, could you rephrase?";
    }
}
