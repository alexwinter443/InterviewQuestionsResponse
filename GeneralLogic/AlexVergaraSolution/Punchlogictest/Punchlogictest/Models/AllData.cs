using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punchlogictest.Models
{
    public class AllData
    {
        public List<employeeData> employeeData { get; set; }
        public List<JobMeta> jobMeta { get; set; }
    }
}
