using TimePunches.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimePunches.Models;

namespace Punchlogictest.Services
{
    public class JsonOutput
    {
        public void getResults(AllData mydata, string outputFilePath)
        {
            // Create Result object
            Result results = new Result();
            // Instantiate the employeeResult list
            results.employeeResult = new List<EmployeeResult>();
            int id = 0;

            foreach (var item in mydata.employeeData)
            {
                // keep track of hours/wages
                double Totalhours = 0;
                double regular = 0;
                double overtime = 0;
                double doubleTime = 0;
                double wageTotal = 0;
                double benefitTotal = 0;

                // for every timepunch
                foreach (var itema in item.timePunch)
                {
                    // get start
                    DateTime start = DateTime.Parse(itema.start);
                    // get end
                    DateTime end = DateTime.Parse(itema.end);
                    // hours worked for timepunch
                    double hoursWorked = (end - start).TotalHours;
                    // remainder to allocate for calculating wage
                    double remaining = hoursWorked;
                    // determine base rate
                    double baseRate = 0;
                    double benefitRate = 0;
                    if (itema.job == "Hospital - Laborer")
                    {
                        baseRate = 20.00;
                        benefitRate = .50;
                    }
                    else if (itema.job == "Hospital - Painter")
                    {
                        baseRate = 31.25;
                        benefitRate = 1;
                    }
                    else
                    {
                        baseRate = 16.25;
                        benefitRate = 1.25;
                    }
                    // --- Regular hours (up to 40) --- ; any hours worked within 40 hours are regular pay
                    if (Totalhours < 40 && remaining > 0)
                    {
                        // hours that are still regular ; doesnt exceed 40 hours
                        double available = 40 - Totalhours;
                        // choose the smaller one; remaining hours in punch or space left in regular hours
                        double used = remaining <= available ? remaining : available;
                        // add these hours to regular hours worked.
                        regular += used;
                        // calculate the pay for this portion.
                        wageTotal += used * baseRate;
                        // increment total hours worked
                        Totalhours += used;
                        // subtract the hours we already counted, leaving remainder for OT/DT.
                        remaining -= used;
                        // get benefit total wage
                        benefitTotal += used * benefitRate;

                    }
                    // --- Overtime hours (40–48) --- ; ensures everything in between 40-48 is counted as overtime
                    if (Totalhours >= 40 && Totalhours < 48 && remaining > 0)
                    {
                        // hours that are within overtime
                        double available = 48 - Totalhours;
                        // get remaining hours or space left in overtime hours ; smallest
                        double used = remaining <= available ? remaining : available;
                        // add to overtime hours
                        overtime += used;
                        // apply 1.5x pay for overtime
                        wageTotal += used * baseRate * 1.5;
                        // add to total hours worked
                        Totalhours += used;
                        // subtract from overtime hours
                        remaining -= used;
                        // calculate benefit total
                        benefitTotal += used * benefitRate;
                    }

                    // --- Doubletime hours (above 48) --- ; any hours left after reg/overtime automatically go here
                    if (remaining > 0)
                    {
                        // add to doubletime hours worked
                        doubleTime += remaining;
                        // pays 2x base rate
                        wageTotal += remaining * baseRate * 2;
                        // calculate benefit total
                        benefitTotal += remaining * benefitRate;
                        // increment total hours worked
                        Totalhours += remaining;
                        // nothing left to allocate
                        remaining = 0;
                    }
                }

                // create employee
                EmployeeResult employeeResult = new EmployeeResult();
                // set hours and wages ; set to 4th decimal place.
                employeeResult.regular = Math.Round(regular, 4);
                employeeResult.overtime = Math.Round(overtime, 4);
                employeeResult.doubletime = Math.Round(doubleTime, 4);
                employeeResult.wageTotal = Math.Round(wageTotal, 4);
                employeeResult.benefitTotal = Math.Round(benefitTotal, 4);
                employeeResult.Id = id++;
                // set name
                employeeResult.employee = item.employee;
                // add employee to list
                results.employeeResult.Add(employeeResult);
            }
            // Ensure directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath)!);

            // Serialize and write directly to final file (overwrites if exists)
            string jsonString = System.Text.Json.JsonSerializer.Serialize(results, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(outputFilePath, jsonString);

        }
    }
}
