using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LibGenerator.NPC;

namespace DMPHelperWPF.ViewModels
{
    public class NPCGeneratorViewModel : NotifyChangedBase
    {
        private NPCGenerator generator;
        private bool canGenerate = true;
        private RelayCommand<object> generateCommand;
        private RelayCommand<object> exportCommand;
        private ObservableCollection<PersonViewModel> selectedModels = new ObservableCollection<PersonViewModel>();
        private ObservableCollection<string> nations;
        private ObservableCollection<PersonViewModel> models = new ObservableCollection<PersonViewModel>() { };
        private string selected;
        private int number = 1;
        private StorageHelper storageHelper;

        public ObservableCollection<PersonViewModel> SelectedModels
        {
            get => selectedModels ?? new ObservableCollection<PersonViewModel>();
            set => SetProperty(ref selectedModels, value);
        }

        public ObservableCollection<string> Nations
        {
            get => nations;
            set
            {
                SetProperty(ref nations, value);
                selected = nations[0];
            }
        }
        public ObservableCollection<PersonViewModel> ViewModels { get => models; set => SetProperty(ref models, value); }

        public string SelectedNation
        {
            get => selected;
            set => SetProperty(ref selected, value);
        }

        public int Number { get => number; set => SetProperty(ref number, value); }

        public NPCGeneratorViewModel(StorageHelper storage)
        {
            storageHelper = storage;
            generator = storage.GetNPCGenerator();
            Nations = new ObservableCollection<string>(generator.GetValidNations());
            name = "NPC Generator";
        }

        public bool CanGenerate { get => canGenerate; }

        public ICommand GenerateCommand
        {
            get
            {
                if (generateCommand == null)
                {
                    generateCommand = new RelayCommand<object>(param => CreateNPCs(), param => canGenerate);
                }
                return generateCommand;
            }
        }

        private void CreateNPCs()
        {
            var viewModels = new ObservableCollection<PersonViewModel>();
            for (int i = 0; i < number; i++)
            {
                try
                {
                    var npc = generator.GenerateNPC(selected);
                    viewModels.Add(new PersonViewModel(npc));
                    DisplayError = false;
                }
                catch (Exception)
                {
                    ErrorText = "An error occurred. Check the following files for mismatched keys: Names, Cultures, and Nations.";
                    DisplayError = true;
                    return;
                }

            }
            ViewModels = viewModels;
            OnPropertyChanged(nameof(ViewModels));

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

        public void DidSelectAll()
        {
            SelectedModels = ViewModels;
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
            var data = SelectedModels.Select(x => x.RawData);
            storageHelper.WriteFile(Export.ExportTypes.Person, data);
        }
    }
}
