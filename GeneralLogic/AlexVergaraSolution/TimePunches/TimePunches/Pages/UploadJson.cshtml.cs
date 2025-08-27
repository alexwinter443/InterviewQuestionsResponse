using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Punchlogictest.Services;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TimePunches.Models;

namespace TimePunches.Pages
{
    public class UploadJsonModel : PageModel
    {
        // bind json file
        [BindProperty]
        public IFormFile JsonFile { get; set; }

        [BindProperty]
        public string SelectedFile { get; set; }

        public string UploadMessage { get; set; }

        public string ProcessedFileLink { get; set; }

        public List<string> PreselectedFiles { get; set; } = new List<string>();

        private string PreselectFolder => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "preselectfiles");
        private string OutputFolder => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "outputfiles");

        public void OnGet()
        {
            // Load all preselected files to show as tiles
            PreselectedFiles.Clear();
            if (Directory.Exists(PreselectFolder))
            {
                // only json files
                var files = Directory.GetFiles(PreselectFolder, "*.json");
                // loops through all files
                foreach (var file in files)
                {
                    PreselectedFiles.Add(Path.GetFileName(file));
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            AllData allData;
            string fileName;

            // User uploaded a file
            if (JsonFile != null && JsonFile.Length > 0)
            {
                // only allows json files; else return error
                if (!JsonFile.FileName.EndsWith(".json"))
                {
                    UploadMessage = "Only JSON files are allowed.";
                    OnGet();
                    return Page();
                }
                // assigns file name
                fileName = JsonFile.FileName;

                // Read uploaded file into memory
                using var ms = new MemoryStream();
                // copies the uploaded file's contents into the memory
                await JsonFile.CopyToAsync(ms);
                // resets stream position to beginning so it can be read from the start.
                ms.Position = 0;
                // allows reading it as text.
                using var reader = new StreamReader(ms);
                // reads the entire content
                string jsonContent = await reader.ReadToEndAsync();
                // convert the JSON string into a C# object of type AllData
                allData = JsonSerializer.Deserialize<AllData>(jsonContent)!;
            }
            // User selected a preselected file
            else if (!string.IsNullOrEmpty(SelectedFile))
            {
                // file name is selected file
                fileName = SelectedFile;
                // gets file path of preselected file
                string selectedFilePath = Path.Combine(PreselectFolder, SelectedFile);
                // if file does not exist 
                if (!System.IO.File.Exists(selectedFilePath))
                {
                    UploadMessage = "Selected file does not exist.";
                    OnGet();
                    return Page();
                }
                // Reads the entire contents of a file
                string jsonContent = await System.IO.File.ReadAllTextAsync(selectedFilePath);
                // Converts the JSON string into a C# object.
                allData = JsonSerializer.Deserialize<AllData>(jsonContent)!;
            }
            // if user clicks on upload without file attached
            else
            {
                UploadMessage = "No file uploaded or selected.";
                OnGet();
                return Page();
            }

            try
            {
                // Ensure output folder exists
                Directory.CreateDirectory(OutputFolder);

                // sets output file name 
                string outputFileName = Path.GetFileNameWithoutExtension(fileName) + "_results.json";
                // gets output filepath
                string outputFilePath = Path.Combine(OutputFolder, outputFileName);
                // for getting json results
                JsonOutput jsonOutput = new JsonOutput();
                // creates output json and saves to outputFilePath
                jsonOutput.getResults(allData, outputFilePath);
                // success message
                UploadMessage = $"File '{fileName}' processed successfully!";
                // file path
                ProcessedFileLink = "/outputfiles/" + outputFileName;
            }
            catch (System.Exception ex)
            {
                UploadMessage = $"Error processing JSON: {ex.Message}";
            }
            // reload preselected tiles
            OnGet();
            // reload page
            return Page();
        }
    }
}
