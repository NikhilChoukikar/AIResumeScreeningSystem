namespace Backend_API
{
    public class ResumeUploadRequest
    {
        public IFormFile File { get; set; }
        public string JobDescription { get; set; }

    }
}
