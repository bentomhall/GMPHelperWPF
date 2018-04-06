using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibGenerator.Dungeon;
using LibGenerator.NPC;
using LibGenerator.Settlement;
using Newtonsoft.Json;
using System.IO;

namespace DMPHelperWPF
{
    public class StorageHelper
    {
        private string localPath;

        public StorageHelper()
        {
            localPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DMPHelper");
            Directory.CreateDirectory(localPath);
            foreach (DataFile value in Enum.GetValues(typeof(DataFile)))
            {
                dataText[value] = GetConfigData(value);
            }
        }

        public bool ShouldReload(string page)
        {
            return isDataDirty[page];
        }

        string GetConfigData(DataFile fileType)
        {
            string data;
            var filename = dataFiles[fileType];
            var localFile = Path.Combine(localPath, filename);
            try
            {
                data = File.ReadAllText(localFile);
            } catch (IOException)
            {
                var uri = new Uri(Path.Combine("/Data", filename), UriKind.Relative);
                var info = System.Windows.Application.GetContentStream(uri);
                using (StreamReader r = new StreamReader(info.Stream, System.Text.Encoding.UTF8))
                {
                    data = r.ReadToEnd();
                }
                File.WriteAllText(localFile, data);
            }
            return data;
        }

        public NPCGenerator GetNPCGenerator()
        {
            if (nPCGenerator == null || isDataDirty["npc"]) {
                var culture = Deserialize<CultureData>(DataFile.Race);
                var names = Deserialize<NameData>(DataFile.NpcName);
                var personalities = Deserialize<string>(DataFile.Personality);
                var professions = Deserialize<string>(DataFile.Profession);
                var nations = Deserialize<NationData>(DataFile.Nation);
                nPCGenerator = new NPCGenerator(culture, names, personalities, professions, nations);
                isDataDirty["npc"] = false;
            }
            return nPCGenerator;
        }

        public SettlementGenerator GetSettlementGenerator()
        {
            if (sGenerator == null || isDataDirty["settlement"])
            {
                var npc = GetNPCGenerator();
                var cities = Deserialize<CityData>(DataFile.City);
                var items = Deserialize<ItemData>(DataFile.ItemRank);
                var settlements = Deserialize<SettlementData>(DataFile.SettlementType);
                var roles = Deserialize<SettlementRole>(DataFile.SettlementRole);
                sGenerator = new SettlementGenerator(cities, items, settlements, roles, npc);
                isDataDirty["settlement"] = false;
            }

            return sGenerator;
        }

        public DungeonGenerator GetDungeonGenerator()
        {
            if (dGenerator == null || isDataDirty["dungeon"])
            {
                var regions = Deserialize<RegionData>(DataFile.Region);
                var locations = Deserialize<LocationData>(DataFile.Dungeon);
                dGenerator = new DungeonGenerator(regions, locations);
                isDataDirty["dungeon"] = false;
            }
            return dGenerator;
        }

        public string GetConfigText(DataFile type)
        {
            return dataText[type];
        }

        public string GetBlankEntry<T>() where T : new()
        {
            var entry = new T();
            return "," + Environment.NewLine + JsonConvert.SerializeObject(entry, Formatting.Indented);
        }

        public void SaveConfigText(DataFile type, string text)
        {
            var file = Path.Combine(localPath, dataFiles[type]);
            dataText[type] = text;
            MarkDirty(type);
            File.WriteAllText(file, text, System.Text.Encoding.UTF8);
            return;
        }

        public void SaveConfigText(DataFile type, object data)
        {
            var text = JsonConvert.SerializeObject(data, Formatting.Indented);
            SaveConfigText(type, text);
        }

        private void MarkDirty(DataFile type)
        {
            switch (type)
            {
                case DataFile.Dungeon:
                case DataFile.Region:
                    isDataDirty["dungeon"] = true;
                    break;
                case DataFile.City:
                case DataFile.ItemRank:
                case DataFile.SettlementRole:
                case DataFile.SettlementType:
                    isDataDirty["settlement"] = true;
                    break;
                case DataFile.Nation:
                case DataFile.NpcName:
                case DataFile.Race:
                case DataFile.Personality:
                case DataFile.Profession:
                    isDataDirty["settlement"] = true;
                    isDataDirty["npc"] = true;
                    break;
            }
            return;
        }

        private SettlementGenerator sGenerator;
        private NPCGenerator nPCGenerator;
        private DungeonGenerator dGenerator;
        private Dictionary<string, bool> isDataDirty = new Dictionary<string, bool> { {"npc", false }, {"dungeon", false }, {"settlement", false } };

        /*
        public async Task<List<T>> DeserializeAsync<T>(DataFile type)
        {
            var data = dataText[type];
            return await data.ContinueWith(x => JsonConvert.DeserializeObject<List<T>>(x.Result));
        }*/

        public List<T> Deserialize<T>(DataFile type)
        {
            var data = dataText[type];
            return JsonConvert.DeserializeObject<List<T>>(data);
        }

        private string ChooseThemePackage()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                FileName = "theme",
                DefaultExt = ".zip",
                Filter = "Compressed Theme Files (.zip)|*.zip"
            };

            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                return dlg.FileName;
            }
            else
            {
                return null;
            }
        }

        private string ChooseFileLocation(Export.ExportTypes type)
        {
            string filename;
            switch (type)
            {
                case Export.ExportTypes.Person:
                    filename = "generated NPC";
                    break;
                case Export.ExportTypes.Dungeon:
                    filename = "generated location";
                    break;
                case Export.ExportTypes.Settlement:
                    filename = "generated settlement";
                    break;
                default:
                    filename = "generated data";
                    break;
            }

            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = filename,
                DefaultExt = ".txt",
                Filter = "Text documents (.txt)|*.txt"
            };

            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                return dlg.FileName;
            }
            else
            {
                return null;
            }

        }

        public void WriteFile<T>(Export.ExportTypes export, IEnumerable<T> data)
        {
            var file = ChooseFileLocation(export);
            if (file == null)
            {
                return;
            }
            var writer = new Export.ExportWriter(file);
            switch (export)
            {
                case Export.ExportTypes.Dungeon:
                    WriteDungeon(writer, data as IEnumerable<AdventureData>);
                    break;
                case Export.ExportTypes.Person:
                    WriteNPC(writer, data as IEnumerable<PersonData>);
                    break;
                case Export.ExportTypes.Settlement:
                    WriteSettlement(writer, data as IEnumerable<Settlement>);
                    break;
            }
            return;

        }

        private void WriteNPC(Export.ExportWriter e, IEnumerable<PersonData> data)
        {
            var exporter = new Export.PersonExporter();
            e.WriteFile(exporter, data);
        }

        private void WriteSettlement(Export.ExportWriter e, IEnumerable<Settlement> data)
        {
            var exporter = new Export.SettlementExporter();
            e.WriteFile(exporter, data);
        }

        private void WriteDungeon(Export.ExportWriter e, IEnumerable<AdventureData> data)
        {
            var exporter = new Export.DungeonExporter();
            e.WriteFile(exporter, data);
        }

        Dictionary<DataFile, string> dataText = new Dictionary<DataFile, string>();
        Dictionary<DataFile, string> dataFiles = new Dictionary<DataFile, string>
        {
            {DataFile.City, "cityData.json" },
            {DataFile.Dungeon, "dungeonData.json" },
            {DataFile.ItemRank, "itemRanks.json" },
            {DataFile.Nation, "nations.json" },
            {DataFile.NpcName, "npcNames.json" },
            {DataFile.Personality, "personality.json" },
            {DataFile.Profession, "professions.json" },
            {DataFile.Race, "races.json" },
            {DataFile.Region, "regionData.json" },
            //{DataFile.Rumor, "rumors.json" },
            {DataFile.SettlementRole, "settlementRoles.json" },
            {DataFile.SettlementType, "settlementTypes.json" }
        };
    }

    public enum DataFile
    {
        City,
        Dungeon,
        ItemRank,
        NpcName,
        Nation,
        Personality,
        Profession,
        Race,
        Region,
        SettlementRole,
        SettlementType,
        //Rumor

    }
}
