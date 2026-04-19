namespace Frontend
{
    public class ResumeViewModel
    {
        public IFormFile ResumeFile { get; set; }
        public string JobDescription { get; set; }
        public double Score { get; set; }
        public string ExtractedText { get; set; }

        public bool IsProcessed { get; set; }

        public List<string> MatchedSkills { get; set; }
        public List<string> MissingSkills { get; set; }
    }
}

