using Backend_API;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace Backend_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ResumeController()
        {
            _httpClient = new HttpClient();
        }

        [HttpPost("match")]
        public async Task<IActionResult> MatchResume([FromBody] ResumeRequest request)
        {
            var pythonApiUrl = "http://127.0.0.1:5000/match";

            var data = new
            {
                resume = request.ResumeText,
                job_desc = request.JobDescription
            };

            var response = await _httpClient.PostAsJsonAsync(pythonApiUrl, data);

            if (!response.IsSuccessStatusCode)
                return StatusCode(500, "Python API failed");

            var result = await response.Content.ReadFromJsonAsync<Dictionary<string, double>>();

            return Ok(result);
        }


        [HttpPost("match-pdf")]
        public async Task<IActionResult> MatchResumePdf([FromForm] ResumeUploadRequest request)
        {
            // 🔐 Safety checks
            if (request.File == null)
                return BadRequest("File is required");

            if (string.IsNullOrWhiteSpace(request.JobDescription))
                return BadRequest("JobDescription is required");

            var pythonApiUrl = "http://127.0.0.1:5000/match-pdf";

            using var content = new MultipartFormDataContent();

            using var stream = request.File.OpenReadStream();

            // ✅ Send to Python (match Python keys)
            content.Add(new StreamContent(stream), "resume", request.File.FileName);
            content.Add(new StringContent(request.JobDescription.Trim()), "job_desc");

            Console.WriteLine("JOB DESC: " + request.JobDescription);

            var response = await _httpClient.PostAsync(pythonApiUrl, content);

            var raw = await response.Content.ReadAsStringAsync();
            Console.WriteLine("PYTHON RESPONSE: " + raw);

            if (!response.IsSuccessStatusCode)
                return StatusCode(500, "Python API failed");

            var result = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();

            return Ok(result);
        }
    }
}