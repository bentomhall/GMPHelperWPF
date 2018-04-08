using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using LibGenerator.Settlement;

namespace DMPHelperWPF.ViewModels
{
    public class SettlementGeneratorViewModel : NotifyChangedBase
    {
        private SettlementGenerator generator;
        private ObservableCollection<string> sizes;
        private string selected;
        private string selectedCity;
        private ObservableCollection<string> cities;
        private RelayCommand<object> generateCommand;
        private RelayCommand<object> exportCommand;
        private ObservableCollection<SettlementViewModel> vms = new ObservableCollection<SettlementViewModel>();
        private bool CanGenerate => (!String.IsNullOrEmpty(selected) && ! String.IsNullOrEmpty(selectedCity));
        private int number = 1;
        private ObservableCollection<SettlementViewModel> selectedViewModels = new ObservableCollection<SettlementViewModel>();
        private StorageHelper storageHelper;

        public SettlementGeneratorViewModel(StorageHelper storage)
        {
            storageHelper = storage;
            generator = storage.GetSettlementGenerator();
            Sizes = new ObservableCollection<string>(generator.GetPossibleSettlementTypes());
            Sizes.Insert(0, "Random");
            SelectedSize = Sizes[0];
            Cities = new ObservableCollection<string>(generator.GetPossibleCities());
            name = "Settlement Generator";
        }

        public ObservableCollection<SettlementViewModel> SelectedViewModels
        {
            get => selectedViewModels;
            set => SetProperty(ref selectedViewModels, value);
        }


        public int Number { get => number; set => SetProperty(ref number, value); }

        public ObservableCollection<string> Sizes
        {
            get => sizes;
            set
            {
                SetProperty(ref sizes, value);
                SelectedSize = value[0];
            }
        }

        public ObservableCollection<string> Cities
        {
            get => cities;
            set
            {
                SetProperty(ref cities, value);
                SelectedCity = value[0];
            }
        }
   

        public string SelectedSize { get => selected; set => SetProperty(ref selected, value); }
        public string SelectedCity { get => selectedCity; set => SetProperty(ref selectedCity, value); }

        public ObservableCollection<SettlementViewModel> SettlementModels { get => vms; set => SetProperty(ref vms, value); }

        public ICommand GenerateCommand
        {
            get
            {
                if (generateCommand == null)
                {
                    generateCommand = new RelayCommand<object>(param => CreateVM(), param => CanGenerate);
                }
                return generateCommand;
            }
        }

        private void CreateVM()
        {
            SettlementModels.Clear();
            for (int i=0; i < Number; i++)
            {
                try
                {
                    string size;
                    if (selected == "Random")
                    {
                        var r = new Random();
                        var index = r.Next(1, sizes.Count);
                        size = sizes[index];
                    }
                    else
                    {
                        size = selected;
                    }
                    var settlement = generator.GenerateSettlement(size, selectedCity);
                    SettlementModels.Add(new SettlementViewModel(settlement));
                    DisplayError = false;
                } catch (Exception)
                {
                    ErrorText = "An error occurred. Please check the following for mismatched keys: Culture, Name, Nation, City, Items, Town Type, and Town Role.";
                    DisplayError = true;
                    return;
                }
                
            }

            if (DisplayError)
            {
                ClearFlag();
            }
            
        }

        private void ClearFlag()
        {
            System.Threading.Timer timer = null;
            timer = new System.Threading.Timer((obj) =>
            {
                DisplayError = false;
            },
            null, 2000, System.Threading.Timeout.Infinite);
        }

        public ICommand ExportCommand
        {
            get
            {
                if (exportCommand == null) { exportCommand = new RelayCommand<object>(param => ExportSelected()); }
                return exportCommand;
            }
        }

        private void ExportSelected()
        {
            var data = selectedViewModels.Select(x => x.RawData);
            storageHelper.WriteFile(Export.ExportTypes.Settlement, data);
        }
    }
}
