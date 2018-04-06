using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LibGenerator.Dungeon;

namespace DMPHelperWPF.ViewModels
{
    public class DungeonGeneratorViewModel : NotifyChangedBase
    {
        private int number = 1;
        private string region = "";
        private ObservableCollection<string> regions;
        private RelayCommand<object> exportCommand;
        private DungeonGenerator generator;
        private bool CanExecute = true;
        private RelayCommand<object> generateCommand;
        private ObservableCollection<DungeonViewModel> vms = new ObservableCollection<DungeonViewModel>();
        private ObservableCollection<DungeonViewModel> selectedVMs;
        private StorageHelper storage;

        public DungeonGeneratorViewModel(StorageHelper s)
        {
            storage = s;
            generator = s.GetDungeonGenerator();
            Regions = new ObservableCollection<string>(generator.GetValidRegions());
            name = "Adventure Site Generator";
        }

        public ObservableCollection<DungeonViewModel> SelectedViewModels
        {
            get
            {
                if (selectedVMs == null) { selectedVMs = new ObservableCollection<DungeonViewModel>(); }
                return selectedVMs;
            }
            set => SetProperty(ref selectedVMs, value);
            
        }


        public ObservableCollection<string> Regions
        {
            get
            {
                if (regions == null) { regions = new ObservableCollection<string>(generator.GetValidRegions()); }
                return regions;
            }
            set
            {
                SetProperty(ref regions, value);
                SelectedRegion = value[0];
            }
        }

        public ObservableCollection<DungeonViewModel> ViewModels { get => vms; set => SetProperty(ref vms, value); }

        public int AdventuresToGenerate { get => number; set => SetProperty(ref number, value); }
        public string SelectedRegion { get => region; set => SetProperty(ref region, value); }
        public ICommand GenerateCommand
        {
            get
            {
                if (generateCommand == null)
                {
                    generateCommand = new RelayCommand<object>(param => CreateVM(), param => CanExecute);
                }
                return generateCommand;
            }
        }

        private void CreateVM()
        {
            for (int i=0; i < number; i++)
            {
                DungeonViewModel vm;
                try
                {
                    vm = new DungeonViewModel(generator.GenerateAdventure(SelectedRegion));
                    vms.Add(vm);
                    DisplayError = false;
                } catch (Exception)
                {
                    ErrorText = "An error occured. Check the following files for mismatching keys: Region, Dungeon.";
                    DisplayError = true;
                    break;
                }
                
                
            }
            OnPropertyChanged(nameof(ViewModels));
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
            storage.WriteFile(Export.ExportTypes.Dungeon, selectedVMs.Select(x => x.RawData));
        }

    }
}
