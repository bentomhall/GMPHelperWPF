using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DMPHelperWPF.ViewModels
{
    public class ConfigBaseViewModel : NotifyChangedBase, IConfigDisplay
    {
        protected StorageHelper storage;
        protected ObservableCollection<string> items;

        public ConfigBaseViewModel(StorageHelper s)
        {
            storage = s;
        }
        public ConfigBaseViewModel() { }

        public void SetItems()
        {
            throw new NotImplementedException();
        }

        protected virtual void DidAddItem()
        {
            throw new NotImplementedException();
        }

        protected virtual void DidRemoveItem(string p)
        {
            throw new NotImplementedException();
        }

        protected virtual void DidSelectItem(string p)
        {
            throw new NotImplementedException();
        }

        protected virtual void DidSaveConfig()
        {
            throw new NotImplementedException();
        }

        public virtual void DidAddListItem(string p)
        {
            throw new NotImplementedException();
        }

        public virtual void DidRemoveListItem(string p)
        {
            throw new NotImplementedException();
        }

        #region Commands


        private RelayCommand<object> addItemCommand;
        private RelayCommand<string> removeItemCommand;
        private RelayCommand<string> selectItemCommand;
        private RelayCommand<object> saveConfigCommand;
        private RelayCommand<string> addListItemCommand;
        private RelayCommand<string> removeListItemCommand;
        public ICommand AddItemCommand
        {
            get
            {
                if (addItemCommand == null)
                {
                    addItemCommand = new RelayCommand<object>(p => DidAddItem());
                }
                return addItemCommand;
            }
        }

        public ICommand RemoveItemCommand
        {
            get
            {
                if (removeItemCommand == null)
                {
                    removeItemCommand = new RelayCommand<string>(p => DidRemoveItem(p));
                }
                return removeItemCommand;
            }
        }

        public ICommand SelectItemCommand
        {
            get
            {
                if (selectItemCommand == null)
                {
                    selectItemCommand = new RelayCommand<string>(p => DidSelectItem(p));
                }
                return selectItemCommand;
            }
        }

        public ICommand SaveConfigCommand
        {
            get
            {
                if (saveConfigCommand == null)
                {
                    saveConfigCommand = new RelayCommand<object>(p => DidSaveConfig());
                }
                return saveConfigCommand;
            }
        }

        public ICommand AddListItemCommand
        {
            get
            {
                if (addListItemCommand == null)
                {
                    addListItemCommand = new RelayCommand<string>(p => DidAddListItem(p));
                }
                return addListItemCommand;
            }
        }

        public ICommand RemoveListItemCommand
        {
            get
            {
                if (removeListItemCommand == null)
                {
                    removeListItemCommand = new RelayCommand<string>(p => DidRemoveListItem(p));
                }
                return removeListItemCommand;
            }
        }
        #endregion
    }
}
