using Newtonsoft.Json;
using Punchlogictest.Models;
using Punchlogictest.Services;
using System;
using System.Text.Json;

public class Program
{
    public static void Main(string[] args)
    {
        // Instantiate Json
        LoadJson loadjson = new LoadJson();
        // get json data
        AllData allData = loadjson.loadJsonData();
        // Instantiate jsonOutput class
        JsonOutput jsonOutput = new JsonOutput();
        // pass json into a method that does all the calculation for regular hours, overtime, etc for each employee
        jsonOutput.getResults(allData);
    }

}