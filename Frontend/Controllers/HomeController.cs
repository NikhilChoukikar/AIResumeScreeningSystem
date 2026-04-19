using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Frontend.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(new ResumeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(ResumeViewModel model)
        {

            Console.WriteLine("JobDescription: " + model.JobDescription);

            if (string.IsNullOrEmpty(model.JobDescription))
            {
                return Content("Job Description is empty");
            }

            if (model.ResumeFile == null)
            {
                return Content("File not uploaded");
            }

            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            var client = new HttpClient(handler);

            using var content = new MultipartFormDataContent();


            //content.Add(new StreamContent(model.ResumeFile.OpenReadStream()), "file", model.ResumeFile.FileName);
            // content.Add(new StringContent(model.JobDescription), "jobDescription");


            var jobDesc = model.JobDescription?.Trim();

            //content.Add(new StreamContent(model.ResumeFile.OpenReadStream()), "file", model.ResumeFile.FileName);
            //content.Add(new StringContent(jobDesc ?? "", Encoding.UTF8), "jobDescription");

            content.Add(new StreamContent(model.ResumeFile.OpenReadStream()), "File", model.ResumeFile.FileName);
            content.Add(new StringContent(model.JobDescription), "JobDescription");

            var response = await client.PostAsync(
                "https://localhost:7234/api/Resume/match-pdf",
                content);

            var raw = await response.Content.ReadAsStringAsync();
            Console.WriteLine("RAW RESPONSE: " + raw);

            var result = JsonSerializer.Deserialize<MatchResponse>(raw);


            if (result != null)
            {
                model.Score = result.score;
                model.ExtractedText = result.extracted_text;
                model.MatchedSkills = result.matched_skills;
                model.MissingSkills = result.missing_skills;
                model.IsProcessed = true;
            }

            model.IsProcessed = true;


            return View(model);
        }
    }
}