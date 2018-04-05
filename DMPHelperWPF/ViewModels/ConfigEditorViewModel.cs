using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DMPHelperWPF.ViewModels
{
    public class ConfigEditorViewModel : NotifyChangedBase
    {
        private StorageHelper storage;
        private Dictionary<string, DataFile> fileTypes = new Dictionary<string, DataFile>()
        {
            {"Cities", DataFile.City },
            {"Dungeons", DataFile.Dungeon },
            {"Items", DataFile.ItemRank },
            {"Nations", DataFile.Nation },
            {"Names", DataFile.NpcName },
            {"Personalities", DataFile.Personality },
            {"Professions", DataFile.Profession },
            {"Cultures", DataFile.Race },
            {"Regions", DataFile.Region },
            //{"Rumors", DataFile.Rumor },
            {"Settlement Types", DataFile.SettlementType },
            {"Settlement Roles", DataFile.SettlementRole }
        };
        private string displayText;
        private IConfigDisplay displayItem;

        public ConfigEditorViewModel(StorageHelper s)
        {
            storage = s;
            Labels = new ObservableCollection<ConfigLabel>
            {
                new ConfigLabel(){ ConfigType=DataFile.City, Icon="People", Label="Cities"},
                new ConfigLabel(){ ConfigType=DataFile.Race, Icon="Contact2", Label="Cultures"},
                new ConfigLabel(){ ConfigType=DataFile.Dungeon, Icon="Map", Label="Dungeons"},
                new ConfigLabel(){ ConfigType=DataFile.ItemRank, Icon="Emoji", Label="Items"},
                new ConfigLabel(){ ConfigType=DataFile.NpcName, Icon="AddFriend", Label="Names"},
                new ConfigLabel(){ ConfigType=DataFile.Nation, Icon="World", Label="Nations"},
                new ConfigLabel(){ ConfigType=DataFile.Personality, Icon="Emoji2", Label="Personalities"},
                new ConfigLabel(){ ConfigType=DataFile.Profession, Icon="Account", Label="Professions"},
                new ConfigLabel(){ ConfigType=DataFile.Region, Icon="World", Label="Regions"},
                //new ConfigLabel(){ ConfigType=DataFile.Rumor, Icon="PostUpdate", Label="Rumors"},
                new ConfigLabel(){ ConfigType=DataFile.SettlementType, Icon="Street", Label="Town Types"},
                new ConfigLabel(){ ConfigType=DataFile.SettlementRole, Icon="Filter", Label="Town Roles"}
            };
        }

        public IConfigDisplay DisplayItem
        {
            get => displayItem;
            set => SetProperty(ref displayItem, value);
        }

        public string DisplayText { get => displayText; set => SetProperty(ref displayText, value); }

        public ObservableCollection<ConfigLabel> Labels { get; private set; }
        public bool RichContentViewVisible => DisplayItem != null;
        public bool TextContentViewVisible => !RichContentViewVisible;

        private void DidSelectItem(string label)
        {
        }

        private void DidSaveItem()
        {   
        }
    }

    public class ConfigLabel : NotifyChangedBase
    {
        public string Label { get; set; }
        public string Icon { get; set; }
        public DataFile ConfigType { get; set; }
        public ICommand Command { get; set; }
        public string Tag { get; set; }
    }

    public interface IConfigDisplay
    {
        ICommand AddItemCommand { get; }
        ICommand RemoveItemCommand { get; }
        ICommand SelectItemCommand { get; }
        ICommand SaveConfigCommand { get; }
        void SetItems();
    }

    public class GenericDisplay : NotifyChangedBase, IConfigDisplay
    {
        public ICommand AddItemCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand SelectItemCommand { get; }
        public ICommand SaveConfigCommand { get; }

        public void SetItems()
        {
            return;
        }
        public string DisplayText { get; set; }
    }
}
