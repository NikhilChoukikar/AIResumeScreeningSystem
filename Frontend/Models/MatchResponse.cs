namespace Frontend.Models
{
    public class MatchResponse
    {
        public double score { get; set; }
        public string extracted_text { get; set; }

            public List<string> matched_skills { get; set; }
            public List<string> missing_skills { get; set; }
        }

    }

