namespace TimePunches.Models
{
    public class EmployeeResult
    {
        public int Id { get; set; }
        public string employee { get; set; }
        public double regular { get; set; }
        public double overtime { get; set; }
        public double doubletime { get; set; }
        public double wageTotal { get; set; }
        public double benefitTotal { get; set; }

    }
}