using Punchlogictest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punchlogictest.Services
{
    public class LoadJson
    {
        public AllData loadJsonData()
        {
            // Navigate from bin/... to project root
            string projectRoot = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;

            // Path to Data/data.json in project root
            string filePath = Path.Combine(projectRoot, "Data", "data.json");

            // Check if file exists
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Could not find data.json in the project Data folder.", filePath);

            // Read and deserialize
            string jsonString = File.ReadAllText(filePath);
            AllData mydata = System.Text.Json.JsonSerializer.Deserialize<AllData>(jsonString)!;

            return mydata;
        }

    }
}
