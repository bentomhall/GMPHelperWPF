using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGenerator.Settlement;

namespace DMPHelperWPF.ViewModels
{
    public class SettlementViewModel : NotifyChangedBase
    {
        private Settlement settlement;
        private ObservableCollection<PersonViewModel> personViewModels;
        private ObservableCollection<ItemsViewModel> items = new ObservableCollection<ItemsViewModel>();
        private ObservableCollection<DictItemViewModel> tech = new ObservableCollection<DictItemViewModel>();
        private ObservableCollection<DictItemViewModel> races = new ObservableCollection<DictItemViewModel>();

        public SettlementViewModel(Settlement s)
        {
            settlement = s;
            var vms = settlement.NPCs.Select(x => new PersonViewModel(x));
            ImportantPeople = new ObservableCollection<PersonViewModel>(vms);
            foreach (KeyValuePair<string, List<string>> kvp in settlement.UnavailableItems)
            {
                items.Add(new ItemsViewModel(kvp.Key, kvp.Value));
            }
            foreach (KeyValuePair<string, int> kvp in settlement.Demographics)
            {
                races.Add(new DictItemViewModel(kvp));
            }
            foreach (KeyValuePair<string, int> kvp in settlement.TechLevels)
            {
                tech.Add(new DictItemViewModel(kvp));
            }
            OnPropertyChanged(nameof(Size));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(TechLevels));
            OnPropertyChanged(nameof(Demographics));
        }

        public Settlement RawData { get => settlement; }

        public string Size
        {
            get => $"{settlement.Size} ({settlement.Role}): Population {settlement.Population}";
        }

        public string Name { get => settlement.Name; }
        public string NearestCity { get => settlement.NearestCity; }
        public ObservableCollection<PersonViewModel> ImportantPeople
        {
            get => personViewModels;
            set => SetProperty(ref personViewModels, value);
        }
        
        public ObservableCollection<ItemsViewModel> Items { get => items; }
        public ObservableCollection<DictItemViewModel> TechLevels { get => tech; }
        public ObservableCollection<DictItemViewModel> Demographics { get => races; }
    }

    public class ItemsViewModel
    {
        public ItemsViewModel(string key, List<string> values)
        {
            Category = key;
            Items = new ObservableCollection<string>(values);
        }

        public string Category { get; private set; }
        public ObservableCollection<string> Items { get; private set; }
    }

    public class DictItemViewModel
    {
        public DictItemViewModel(KeyValuePair<string, int> kvp)
        {
            Key = kvp.Key;
            Value = kvp.Value;
        }

        public string Key { get; private set; }
        public int Value { get; private set; }
    }
}
