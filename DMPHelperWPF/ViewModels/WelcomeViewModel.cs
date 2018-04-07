using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DMPHelperWPF.ViewModels
{
    public class WelcomeViewModel : NotifyChangedBase
    {
        private StorageHelper storage;
        private RelayCommand<object> loadPackageCommand;
        private bool status = false;

        public WelcomeViewModel()
        {
            name = "Welcome";
        }

        public bool LoadStatus { get => status; set => SetProperty(ref status, value); }
        public string LoadSuccess { get => "File Loaded Successfully"; }

        public WelcomeViewModel(StorageHelper s)
        {
            storage = s;
        }

        public ICommand LoadPackageCommand
        {
            get
            {
                if (loadPackageCommand == null)
                {
                    loadPackageCommand = new RelayCommand<object>(x => LoadSettings());
                }
                return loadPackageCommand;
            }
        }

        private void LoadSettings()
        {
            var result = storage.LoadDataPackage();
            if (result)
            {
                LoadStatus = true;
            }
        }
    }
}
