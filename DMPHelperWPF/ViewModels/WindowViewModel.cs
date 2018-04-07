using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DMPHelperWPF.ViewModels
{
    public class WindowViewModel : NotifyChangedBase
    {
        private ObservableCollection<NotifyChangedBase> viewModels = new ObservableCollection<NotifyChangedBase> { new WelcomeViewModel() };
        private NotifyChangedBase selected;
        private RelayCommand<string> changePageCommand;

        public WindowViewModel(StorageHelper s)
        {
            viewModels.Add(new NPCGeneratorViewModel(s));
            viewModels.Add(new SettlementGeneratorViewModel(s));
            viewModels.Add(new DungeonGeneratorViewModel(s));
            viewModels.Add(new ConfigEditorViewModel(s));
            selected = viewModels[0];
        }

        public NotifyChangedBase SelectedModel { get => selected; set => SetProperty(ref selected, value); }
        public ObservableCollection<NotifyChangedBase> ViewModels { get => viewModels; }

        public ICommand ChangePageCommand
        {
            get
            {
                if (changePageCommand == null)
                {
                    changePageCommand = new RelayCommand<string>(x => OnPageChange(x));
                }
                return changePageCommand;
            }
        }

        private void OnPageChange(string page)
        {
            SelectedModel = viewModels.FirstOrDefault(x => x.Name == page) ?? viewModels[0];
        }


    }
}
