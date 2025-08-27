using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimePunches.Models
{
    public class Employee
    {
        public string employee { get; set; }
        public List<Timepunch> Timepunch { get; set; }
    }
}
