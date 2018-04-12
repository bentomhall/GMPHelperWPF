using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGenerator;

namespace DMPHelperWPF.ViewModels
{
    public class SavedItemsViewModel : NotifyChangedBase
    {
        private ObservableCollection<PersonViewModel> savedNPCModels = new ObservableCollection<PersonViewModel>();
        private ObservableCollection<SettlementViewModel> savedSettlementModels = new ObservableCollection<SettlementViewModel>();
        private ObservableCollection<DungeonViewModel> savedDungeonModels = new ObservableCollection<DungeonViewModel>();
        private StorageHelper storage;

        public SavedItemsViewModel(StorageHelper s)
        {
            storage = s;
        }

        public ObservableCollection<PersonViewModel> SavedNPCs { get => savedNPCModels; set => SetProperty(ref savedNPCModels, value); }
        public ObservableCollection<SettlementViewModel> SavedSettlements { get => savedSettlementModels; set => SetProperty(ref savedSettlementModels, value); }
        public ObservableCollection<DungeonViewModel> SavedDungeons { get => savedDungeonModels; set => SetProperty(ref savedDungeonModels, value); }

        public void AddViewModel(NotifyChangedBase vm, Export.ExportTypes type)
        {
            switch (type)
            {
                case Export.ExportTypes.Dungeon:
                    SavedDungeons.Add(vm as DungeonViewModel);
                    return;
                case Export.ExportTypes.Person:
                    SavedNPCs.Add(vm as PersonViewModel);
                    return;
                case Export.ExportTypes.Settlement:
                    SavedSettlements.Add(vm as SettlementViewModel);
                    return;
            }
        }
    }
}
