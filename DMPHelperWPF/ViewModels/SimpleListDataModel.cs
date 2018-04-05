using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DMPHelperWPF.ViewModels
{
    public class SimpleListDataModel : NotifyChangedBase, IConfigDisplay
    {
        private StorageHelper storage;
        private DataFile dataType;
        private ObservableCollection<ListItem> items;
        private ListItem selectedItem;
        private string newItem;

        public SimpleListDataModel(StorageHelper s, DataFile type)
        {
            storage = s;
            dataType = type;
            ItemNames = new ObservableCollection<ListItem>(s.Deserialize<string>(type).Select(x => new ListItem() { Value = x }));
        }

        public ObservableCollection<ListItem> ItemNames { get => items; set => SetProperty(ref items, value); }
        public ListItem SelectedItem { get => selectedItem; set => SetProperty(ref selectedItem, value); }
        public string NewItem { get => newItem; set => SetProperty(ref newItem, value); }

        public void SetItems()
        {
            throw new NotImplementedException();
        }

        private void DidRemoveItem()
        {
            ItemNames.Remove(selectedItem);
        }
        private void DidSaveConfig()
        {
            var dummy = storage.SaveConfigText(dataType, items.Select(x => x.Value)); //unwrap the display wrapper.
        }

        private void DidAddItem()
        {
            ItemNames.Add(new ListItem { Value = newItem });
            OnPropertyChanged(nameof(ItemNames));
        }

        #region Commands
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
                    removeItemCommand = new RelayCommand<object>(p => DidRemoveItem());
                }
                return removeItemCommand;
            }
        }

        public ICommand SelectItemCommand { get; }
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

        private RelayCommand<object> addItemCommand;
        private RelayCommand<object> removeItemCommand;
        private RelayCommand<object> saveConfigCommand;
        #endregion
    }

    public class ListItem : NotifyChangedBase
    {
        private string v;
        public string Value { get => v; set => SetProperty(ref v, value); }
    }
}
