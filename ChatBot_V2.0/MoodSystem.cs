namespace ChatBot_V2._0;
    class MoodSystem
    {
        public enum Mood { Neutral, Positive, Negative }

        public Mood DetermineMood(string input)
        {
            string lower = input.ToLower();
            if (lower.Contains("happy") || lower.Contains("great") || lower.Contains("good"))
                return Mood.Positive;
            if (lower.Contains("sad") || lower.Contains("angry") || lower.Contains("bad"))
                return Mood.Negative;
            return Mood.Neutral;
        }
    }