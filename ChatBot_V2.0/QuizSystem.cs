using System;
using static QuizSystem;

/* the quiz contains the actual question, the correct answer of that question and an explanation of the answer as well as the various options a user can select
 * these are all defined in the QuizQuestions class
*/
public class QuizQuestions
{
    public string? QuestionText { get; set; }
    public string[]? Options { get; set; }
    public int CorrectIndex { get; set; }
    public QuestionType Type { get; set; }
    public string? Explanation { get; set; }
}

//main quiz classes that control the logic of the quiz,
public class QuizSystem
{
    public List<QuizQuestions> questions;
    private int currentQuestionIndex = 0;
    public int TotalQuestions => questions.Count;

    public int score { get; set; } = 0;
    public bool quizActive { get; set; } = false;

    public enum QuestionType
    {
        MultipleChoice,
        TrueFalse
    }

    public QuizSystem()
    {

        questions = LoadQuestions();

    }
    // Initializes the quiz, setting the score to 0 and the current question index to 0
    public void BeginQuiz()
    {
        score = 0;
        currentQuestionIndex = 0;
        quizActive = true;
    }

    public QuizQuestions GetCurrentQuestion()
    {
        return questions[currentQuestionIndex];
    }

    /* Retrieves the current question based on the current question index,if the index is incorrect the correct answer will be displayed
     * and the score will be incremented if the answer is correct, additionally, an explanation of the answer will be provided
    */
    public string sendAnswer(int answerIndex)
    {
        var current = GetCurrentQuestion();

        string response;
        if (answerIndex == current.CorrectIndex)
        {
            score++;
            response = "✅ Correct!";
        }
        else
        {
            response = $"❌ Incorrect! The correct answer was: {current?.Options?[current.CorrectIndex]}";
        }

        response += $"\n💡 {current?.Explanation}";
        return response;
    }

    public bool NextQuestion()
    {
        currentQuestionIndex++;
        return currentQuestionIndex < questions.Count;
    }

    public void EndQuiz()
    {
        quizActive = false;
    }

    //all the quiz questions are bellow
    public List<QuizQuestions> LoadQuestions()
    {
        return new List<QuizQuestions>
        {
            new QuizQuestions // 1 MC
            {
             QuestionText = "What does Phishing refer to?",
             Options = new[] { "A fake website", "Tricking users into giving sensitive info", "A type of firewall", "An antivirus method" },
             CorrectIndex = 1,
             Type = QuestionType.MultipleChoice,
             Explanation = "Phishing is a social engineering attack where users are tricked into revealing personal information, often via fake emails or websites."
            },

            new QuizQuestions // 2 MC
            {
             QuestionText = "What should you do if you receive an email asking for your password?",
             Options = new[] { "Send them your password", "Delete the email", "Report the email as phishing", "Converse with the person who sent the email" },
             CorrectIndex = 2,
             Type = QuestionType.MultipleChoice,
             Explanation = "Fake emails often aim to steal your credentials. Always report them as phishing."
            },

            new QuizQuestions // 3 TF
            {
             QuestionText = "(True/False) Using the same password for multiple accounts is safe as long as it's a strong password.",
             Options = new[] { "True", "False" },
             CorrectIndex = 1,
             Type = QuestionType.TrueFalse,
             Explanation = "Even strong passwords become weak if reused. Use unique ones for each account."
            },

            new QuizQuestions // 4 TF
            {
             QuestionText = "(True/False) Password managers are unsafe because they store all your passwords in one place.",
             Options = new[] { "True", "False" },
             CorrectIndex = 1,
             Type = QuestionType.TrueFalse,
             Explanation = "Password managers are secure tools that encrypt and store your credentials safely."
            },

            new QuizQuestions // 5 MC
            {
             QuestionText = "What does \"HTTPS\" in a website URL indicate?",
             Options = new[] { "The site is government-approved", "The connection is encrypted", "The site is free from malware", "The site is a social media platform" },
             CorrectIndex = 1,
             Type = QuestionType.MultipleChoice,
             Explanation = "HTTPS indicates that data sent to and from the site is encrypted for security."
            },

            new QuizQuestions // 6 TF
            {
             QuestionText = "(True/False) Public Wi-Fi networks are always safe for online banking if you’re using a VPN.",
             Options = new[] { "True", "False" },
             CorrectIndex = 1,
             Type = QuestionType.TrueFalse,
             Explanation = "VPNs add security, but public Wi-Fi still carries risks. Avoid sensitive tasks on public networks."
            },

            new QuizQuestions // 7 MC
            {
             QuestionText = "Which browser practice helps avoid malicious sites?",
             Options = new[] { "Clicking on pop-up ads", "Disabling automatic updates", "Using ad-blockers and script blockers", "Ignoring browser security warnings" },
             CorrectIndex = 2,
             Type = QuestionType.MultipleChoice,
             Explanation = "Using blockers and keeping your browser updated helps avoid malicious scripts and ads."
            },

            new QuizQuestions // 8 MC
            {
             QuestionText = "You receive an email claiming your bank account is locked and asks you to click a link. What should you do?",
             Options = new[] { "Click the link and log in", "Forward the email to your bank’s fraud department", "Reply with your password", "Ignore it—banks never send such emails" },
             CorrectIndex = 1,
             Type = QuestionType.MultipleChoice,
             Explanation = "Always report suspicious emails to your bank. Do not click links or reply with sensitive info."
            },

            new QuizQuestions // 9 TF
            {
             QuestionText = "(True/False) Phishing attacks only happen through email.",
             Options = new[] { "True", "False" },
             CorrectIndex = 1,
             Type = QuestionType.TrueFalse,
             Explanation = "Phishing can also occur via phone calls, SMS, and social media."
            },

            new QuizQuestions // 10 MC
            {
             QuestionText = "What is considered a strong password?",
             Options = new[] { "It contains a mix of many different numbers and characters", "It contains my name and birthdate", "It contains personal info", "It's easy to remember" },
             CorrectIndex = 0,
             Type = QuestionType.MultipleChoice,
             Explanation = "A strong password is long, unpredictable, and includes uppercase, lowercase, symbols, and numbers."
            }
        };
    }
}
