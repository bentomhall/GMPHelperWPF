using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGenerator.NPC;
using LibGenerator.Settlement;
using LibGenerator.Dungeon;

namespace DMPHelperWPF.Export
{
    public abstract class ExportBase<T> : IExporter<T>
    {
        public string Marshal(IEnumerable<T> t)
        {
            string separator = $"{Environment.NewLine}------------------------{Environment.NewLine}";
            var data = t.Select(x => FormatData(x));
            return string.Join(separator, data);
        }

        public string Marshal(IEnumerable<T> t, string separator)
        {
            var data = t.Select(x => FormatData(x));
            return string.Join(separator, data);
        }

        public abstract string FormatData(object o);
    }

    public enum ExportTypes
    {
        Person,
        Dungeon,
        Settlement
    }


    public class PersonExporter : ExportBase<PersonData>, IExporter<PersonData>
    {
        public override string FormatData(object o)
        {
            return FormatData(o as PersonData);
        }

        private string FormatData(PersonData d)
        {
            var output = new List<string>()
            {
                $"Name: {d.Name}"
            };
            if (string.IsNullOrEmpty(d.Subrace))
            {
                output.Add($"Demographics: {d.Age} {d.Gender} {d.Race} from {d.Nation} ({d.Culture})");
            }
            else
            {
                output.Add($"Demographics: {d.Age} {d.Gender} {d.Race} ({d.Subrace}) from {d.Nation} ({d.Culture})");
            }
            output.Add($"Profession: {d.Profession}");
            output.Add($"Religious habits: {d.Religion}");
            output.Add($"Personality: {d.Personality}");
            return string.Join(Environment.NewLine, output);
        }
    }

    public class DungeonExporter : ExportBase<AdventureData>, IExporter<AdventureData>
    {
        public override string FormatData(object o)
        {
            return FormatData(o as AdventureData);
        }

        private string FormatData(AdventureData d)
        {
            var output = new List<string>()
            {
                $"A {d.SubType} {d.AdventureType} adventure for level {d.Level} characters.",
                $"A {d.Scale} site with {d.Size} separate areas.",
                $"Mainly occupied by {d.PrimaryMonster}-type creatures with {(d.HasBoss ? "a" : "no")} boss monster."
            };
            return string.Join(Environment.NewLine, output);
        }
    }

    public class SettlementExporter : ExportBase<Settlement>, IExporter<Settlement>
    {
        public override string FormatData(object o)
        {
            return FormatData(o as Settlement);
        }

        private string FormatData(Settlement d)
        {
            var personExporter = new PersonExporter();
            var internalSeparator = "+++++++++++++++++++++";
            var output = new List<string>()
            {
                $"{d.Name}, a(n) {d.Role} {d.Size} ({d.Population} inhabitants)",
                $"Location: near {d.NearestCity}",
                internalSeparator,
                "Demographics:",
                FormatDictionary(d.Demographics),
                internalSeparator,
                "Tech Levels:",
                FormatDictionary(d.TechLevels),
                internalSeparator,
                "NPCs:",
                personExporter.Marshal(d.NPCs, Environment.NewLine+internalSeparator+Environment.NewLine),
                "Unavailable Items by Category:"
            };
            foreach (KeyValuePair<string, List<string>> kvp in d.UnavailableItems)
            {
                if (kvp.Value.Count != 0)
                {
                    output.Add(internalSeparator);
                    output.Add($"{kvp.Key}");
                    output.AddRange(kvp.Value.Select(x => x.ToString()));
                }
            }
            return string.Join(Environment.NewLine, output);
        }

        private string FormatDictionary(Dictionary<string,int> d)
        {
            var output = new List<string>();
            foreach (KeyValuePair<string, int> kvp in d)
            {
                output.Add($"{kvp.Key} : {kvp.Value}");
            }
            return string.Join(Environment.NewLine, output);
        }
    }
}
