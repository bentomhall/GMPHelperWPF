using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibGenerator.Dungeon;
using LibGenerator.NPC;
using LibGenerator.Settlement;
using Newtonsoft.Json;

namespace DMPHelperWPF
{
    public class StorageHelper
    {
        public StorageHelper()
        {
            foreach (DataFile value in Enum.GetValues(typeof(DataFile)))
            {
                dataText[value] = GetConfigData(value);
            }
        }

        public bool ShouldReload(string page)
        {
            return isDataDirty[page];
        }

        async Task<string> GetConfigData(DataFile fileType)
        {
            /*
            var file = await GetModifiedFile(fileType);
            string text;
            if (file == null)
            {
                var defaultFile = await GetDefaultFile(fileType);
                text = await FileIO.ReadTextAsync(defaultFile);
                file = await ApplicationData.Current.LocalFolder.CreateFileAsync(dataFiles[fileType]);
                await FileIO.WriteTextAsync(file, text);
            }
            else
            {
                text = await FileIO.ReadTextAsync(file);
            }
            localFiles[fileType] = file;*/
            return "";
        }

        async Task<StorageFile> GetModifiedFile(DataFile fileType)
        {
            if (localFiles.TryGetValue(fileType, out StorageFile savedFile))
            {
                return savedFile;
            }
            var folder = ApplicationData.Current.LocalFolder;
            var filename = dataFiles[fileType];
            StorageFile file = await folder.TryGetItemAsync(filename) as StorageFile;
            return file;

        }

        async Task<StorageFile> GetDefaultFile(DataFile fileType)
        {
            var filename = new Uri(@"ms-appx:////Assets/" + dataFiles[fileType]);
            return await StorageFile.GetFileFromApplicationUriAsync(filename);
        }

        public NPCGenerator GetNPCGenerator()
        {
            if (nPCGenerator == null || isDataDirty["npc"]) {
                var culture = DeserializeAsync<CultureData>(DataFile.Race);
                var names = DeserializeAsync<NameData>(DataFile.NpcName);
                var personalities = DeserializeAsync<string>(DataFile.Personality);
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
            return dataText[type].Result;
        }

        public string GetBlankEntry<T>() where T : new()
        {
            var entry = new T();
            return "," + Environment.NewLine + JsonConvert.SerializeObject(entry, Formatting.Indented);
        }

        public async Task SaveConfigText(DataFile type, string text)
        {
            var file = localFiles[type];
            dataText[type] = Task.FromResult(text);
            MarkDirty(type);
            await FileIO.WriteTextAsync(file, text, Windows.Storage.Streams.UnicodeEncoding.Utf8);
            return;
        }

        public async Task SaveConfigText(DataFile type, object data)
        {
            var file = localFiles[type];
            var text = JsonConvert.SerializeObject(data, Formatting.Indented);
            dataText[type] = Task.FromResult(text);
            MarkDirty(type);
            await FileIO.WriteTextAsync(file, text, Windows.Storage.Streams.UnicodeEncoding.Utf8);
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

        public async Task<List<T>> DeserializeAsync<T>(DataFile type)
        {
            var data = dataText[type];
            return await data.ContinueWith(x => JsonConvert.DeserializeObject<List<T>>(x.Result));
        }

        public List<T> Deserialize<T>(DataFile type)
        {
            var data = dataText[type];
            if (!data.IsCompletedSuccessfully)
            {
                data.Wait(-1); //will block thread.
            }
            return JsonConvert.DeserializeObject<List<T>>(data.Result);
        }

        private async Task<StorageFile> ChooseThemePackage()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary,
            };
            picker.FileTypeFilter.Add(".zip");
            picker.FileTypeFilter.Add(".rpgsetting");

            return await picker.PickSingleFileAsync();
            
        }

        private async Task<StorageFile> ChooseFileLocation(Export.ExportTypes type)
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

            var picker = new Windows.Storage.Pickers.FileSavePicker
            {
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary,
                SuggestedFileName = filename
            };
            picker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            var file = await picker.PickSaveFileAsync();
            return file;
        }

        public async Task WriteFile<T>(Export.ExportTypes export, IEnumerable<T> data)
        {
            var file = await ChooseFileLocation(export);
            var writer = new Export.ExportWriter(file);
            switch (export)
            {
                case Export.ExportTypes.Dungeon:
                    await WriteDungeon(writer, data as IEnumerable<AdventureData>);
                    break;
                case Export.ExportTypes.Person:
                    await WriteNPC(writer, data as IEnumerable<PersonData>);
                    break;
                case Export.ExportTypes.Settlement:
                    await WriteSettlement(writer, data as IEnumerable<Settlement>);
                    break;
            }
            return;

        }

        private async Task WriteNPC(Export.ExportWriter e, IEnumerable<PersonData> data)
        {
            var exporter = new Export.PersonExporter();
            await e.WriteFile(exporter, data);
        }

        private async Task WriteSettlement(Export.ExportWriter e, IEnumerable<Settlement> data)
        {
            var exporter = new Export.SettlementExporter();
            await e.WriteFile(exporter, data);
        }

        private async Task WriteDungeon(Export.ExportWriter e, IEnumerable<AdventureData> data)
        {
            var exporter = new Export.DungeonExporter();
            await e.WriteFile(exporter, data);
        }

        Dictionary<DataFile, StorageFile> localFiles = new Dictionary<DataFile, StorageFile>();
        Dictionary<DataFile, Task<string>> dataText = new Dictionary<DataFile, Task<string>>();
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
