namespace KrishnaRajamannar.NEA.Models
{
    public class IndependentReviewQuizModel
    {
        public int FeedbackID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
        public string? Option5 { get; set; }
        public string? Option6 { get; set; }
        // This represents the number of points which can be gained when correctly answering
        // a question for the first time
        public int PointsForQuestion { get; set; }
        // This represents the number of points which have been awarded when answering a question
        public int PointsGained { get; set; }
        // This is used to check whether a question was previously answered correctly or not
        public bool IsCorrect { get; set; }
        public int AnswerStreak { get; set; }
    }
}
