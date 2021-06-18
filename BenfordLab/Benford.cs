using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BenfordLab
{
    public class BenfordData
    {
        public int Digit { get; set; }
        public int Count { get; set; }

        public BenfordData() { }
    }

    public class Benford
    {

        public static BenfordData[] calculateBenford(string csvFilePath)        
        {
            // load the data
            var data = File.ReadAllLines(csvFilePath)
                .Skip(1) // For header
                .Select(s => Regex.Match(s, @"^(.*?),(.*?)$"))
                .Select(data => new
                {
                    Country = data.Groups[1].Value,
                    Population = int.Parse(data.Groups[2].Value)
                });

            var dataCountryWise = data.Select(x => new { Country = x.Country, Digit = FirstDigit.getFirstDigit(x.Population) }).ToArray();

            var countSortCountryWise =  dataCountryWise.GroupBy(x => x.Digit)
                                        .Select(country => (Digit: country.Key, Count: country.Count()))
                                        .OrderByDescending(x => x.Digit).ToArray();

            List<BenfordData> m = new List<BenfordData>();
            foreach (var item in countSortCountryWise)
            {
                m.Add(new BenfordData
                {
                    Digit = item.Digit,
                    Count = item.Count
                });
            }

            return m.ToArray();
        }
    }
}
