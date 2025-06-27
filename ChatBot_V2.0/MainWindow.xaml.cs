using ChatBot_V2._0;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading.Tasks;

using System.IO;
using static QuizSystem;
using System.Windows.Threading;

namespace ChatBot_V2._0;

// MainWindow.xaml.cs
public partial class MainWindow : Window
{
    private ResponseSystem responseSystem;
    private string? userName;

    public MainWindow()
    {
        InitializeComponent();
        responseSystem = new ResponseSystem();
        DispatcherTimer reminderTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(30)
        };

        reminderTimer.Tick += (s, e) => {
            foreach (var task in responseSystem.taskSystem.GetDueTasks())
            {
                ChatDisplayTextBlock.Text += $"\nCABBY: ⏰ Reminder! {task.Description} (scheduled for {task.ReminderTime:t})\n";
                responseSystem.taskSystem.MarkAsReminded(task);
            }
        };

        reminderTimer.Start();

        AskForName();

    }

    private async void AskForName()
    {
        string namePrompt = "CABBY: What is your name?\n";
        ChatDisplayTextBlock.Text += await TypingEffectAsync(namePrompt);
        ChatDisplayTextBlock.ScrollToEnd();
    }

    private async void SendButton_Click(object sender, RoutedEventArgs e)
    {
        string userInput = UserInputTextBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(userInput)) return;

        ChatDisplayTextBlock.Text += $"\nYou: {userInput}\n";
        UserInputTextBox.Clear();

        if (string.IsNullOrEmpty(userName))
        {
            userName = userInput;
            File.WriteAllText("Memory.txt", userName);
            string welcomeMessage = $"\nCABBY: What can I assist you with {userName}?\n";
            ChatDisplayTextBlock.Text += await TypingEffectAsync(welcomeMessage);
            return;
        }

        string response = responseSystem.ProcessInput(userInput, userName);
        ChatDisplayTextBlock.Text += await TypingEffectAsync("\nCABBY: " + response + "\n");
        ChatDisplayTextBlock.ScrollToEnd();

    }

    private async Task<string> TypingEffectAsync(string text, int delayMs = 20)
    {
        StringBuilder sb = new();
        foreach (char c in text)
        {
            sb.Append(c);
            await Task.Delay(delayMs);
        }
        return sb.ToString();
    }

    private string FormatQuizQuestion(QuizQuestions question)
    {
        var lines = new List<string> { question.QuestionText };

        if (question.Type == QuestionType.TrueFalse)
        {
            lines.Add("A: True");
            lines.Add("B: False");
        }
        else
        {
            var labels = new[] { "A", "B", "C", "D" };
            for (int i = 0; i < question.Options.Length; i++)
            {
                lines.Add($"{labels[i]}: {question.Options[i]}");
            }
        }

        return string.Join("\n", lines);
    }

    private async void StartQuizButton_Click(object sender, RoutedEventArgs e)
    {
        
        responseSystem.quiz.BeginQuiz();
        var firstQuestion = responseSystem.quiz.GetCurrentQuestion();

        string formattedQuestion = FormatQuizQuestion(firstQuestion);

        ChatDisplayTextBlock.Text += await TypingEffectAsync($"\nCABBY: Let's begin the cybersecurity quiz!\n{formattedQuestion}\n");
        ChatDisplayTextBlock.ScrollToEnd();

    }


}


