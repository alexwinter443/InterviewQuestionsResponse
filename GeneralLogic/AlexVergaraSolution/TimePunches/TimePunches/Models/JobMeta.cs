using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimePunches.Models
{
    public class JobMeta
    {
        public string job { get; set; }
        public decimal rate { get; set; }
        public decimal benefitsRate { get; set; }

    }
}
