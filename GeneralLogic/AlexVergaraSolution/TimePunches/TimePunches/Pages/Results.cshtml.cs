using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace TimePunches.Pages
{
    public class ResultsModel : PageModel
    {
        public List<string> Files { get; set; } = new List<string>();

        public void OnGet()
        {
            // Point to outputfiles folder
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "outputfiles");

            if (Directory.Exists(folderPath))
            {
                // Get all JSON files ending with _results.json
                var files = Directory.GetFiles(folderPath, "*_results.json");
                foreach (var file in files)
                {
                    Files.Add(Path.GetFileName(file));
                }
            }
        }
    }
}
