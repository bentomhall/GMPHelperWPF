using System;
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
        private ObservableCollection<ConfigItemViewModel> vms = new ObservableCollection<ConfigItemViewModel>();
        private ConfigItemViewModel vm;

        public ConfigEditorViewModel(StorageHelper s)
        {
            storage = s;
            vms.Add(new ConfigItemViewModel(s, DataFile.City));
            vms.Add(new ConfigItemViewModel(s, DataFile.Dungeon));
            vms.Add(new ConfigItemViewModel(s, DataFile.ItemRank));
            vms.Add(new ConfigItemViewModel(s, DataFile.Nation));
            vms.Add(new ConfigItemViewModel(s, DataFile.NpcName));
            vms.Add(new ConfigItemViewModel(s, DataFile.Personality));
            vms.Add(new ConfigItemViewModel(s, DataFile.Profession));
            vms.Add(new ConfigItemViewModel(s, DataFile.Race));
            vms.Add(new ConfigItemViewModel(s, DataFile.Region));
            vms.Add(new ConfigItemViewModel(s, DataFile.SettlementRole));
            vms.Add(new ConfigItemViewModel(s, DataFile.SettlementType));
            OnPropertyChanged(nameof(Models));
            name = "Configuration Editor";
        }

        public ObservableCollection<ConfigItemViewModel> Models { get => vms; }
        public ConfigItemViewModel SelectedVM { get => vm; set => SetProperty(ref vm, value); }
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
