using System;
using System.Collections.Generic;
using System.Linq;
using LibGenerator.Dungeon;
using LibGenerator.NPC;
using LibGenerator.Settlement;

namespace DMPHelperWPF.ViewModels
{
    public class ConfigItemViewModel : ConfigBaseViewModel
    {
        private string configText;
        private string helpText;
        private DataFile configType;

        public ConfigItemViewModel(StorageHelper s, DataFile type) : base(s)
        {
            configType = type;
            configText = GetData();
            HelpText = @"ms-appx-web:///Assets/" + helpResources[configType];
        }

        public string ConfigText { get => configText; set => SetProperty(ref configText, value); }
        public string HelpText { get => helpText; set => SetProperty(ref helpText, value); }

        private string GetData()
        {
            return storage.GetConfigText(configType);
        }

        protected override void DidSaveConfig()
        {
            var dummy = storage.SaveConfigText(configType, configText);
        }

        protected override void DidAddItem()
        {
            string json;
            switch (configType)
            {
                case DataFile.City:
                    json = storage.GetBlankEntry<CityData>();
                    break;
                case DataFile.Dungeon:
                    json = storage.GetBlankEntry<LocationData>();
                    break;
                case DataFile.ItemRank:
                    json = storage.GetBlankEntry<ItemData>();
                    break;
                case DataFile.Nation:
                    json = storage.GetBlankEntry<NationData>();
                    break;
                case DataFile.NpcName:
                    json = storage.GetBlankEntry<NationData>();
                    break;
                case DataFile.Race:
                    json = storage.GetBlankEntry<CultureData>();
                    break;
                case DataFile.Region:
                    json = storage.GetBlankEntry<RegionData>();
                    break;
                case DataFile.SettlementRole:
                    json = storage.GetBlankEntry<SettlementRole>();
                    break;
                case DataFile.SettlementType:
                    json = storage.GetBlankEntry<SettlementData>();
                    break;
                default:
                    return;
            }

            ConfigText = configText.Insert(configText.Length - 1, json);
            return;
        }

        private Dictionary<DataFile, string> helpResources = new Dictionary<DataFile, string>
        {
            {DataFile.City, "CityDataHelpText.html"},
            {DataFile.Dungeon, "DungeonDataHelpText.html" },
            {DataFile.ItemRank, "ItemRanksHelpText.html" },
            {DataFile.Nation, "NationDataHelpText.html" },
            {DataFile.NpcName, "NameHelpText.html" },
            {DataFile.Personality, "PersonalityAndProfession.html" },
            {DataFile.Profession, "PersonalityAndProfession.html"},
            {DataFile.Race, "CultureDataHelpText.html" },
            {DataFile.Region, "RegionDataHelpText.html" },
            //{DataFile.Rumor, "RumorDataHelpText"},
            {DataFile.SettlementRole, "SettlementRoleDataHelpText.html" },
            {DataFile.SettlementType, "SettlementTypesHelpText.html" }
        };

    }


}
