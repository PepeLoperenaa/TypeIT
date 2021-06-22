namespace TypeIT.Models
{
    public class AchievementModel
    {
        public string Title { get; set; }
        public string Desc { get; set; }

        public AchievementModel(string title, string desc)
        {
            Title = title;
            Desc = desc;
        }
    }
}
