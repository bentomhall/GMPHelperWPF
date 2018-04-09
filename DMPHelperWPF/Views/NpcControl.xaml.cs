using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DMPHelperWPF.ViewModels;

namespace DMPHelperWPF.Views
{
    /// <summary>
    /// Interaction logic for NpcControl.xaml
    /// </summary>
    public partial class NpcControl : UserControl
    {
        public NpcControl()
        {
            InitializeComponent();/*
            var storage = ((App)Application.Current).Storage;
            vm = new NPCGeneratorViewModel(storage);
            DataContext = vm;*/
        }

        private NPCGeneratorViewModel vm;

        public NPCGeneratorViewModel ViewModel { get => DataContext as NPCGeneratorViewModel; }

        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel == null) { return; } // viewmodel gets changed out first as things change.
            var addedItems = e.AddedItems;
            var removedItems = e.RemovedItems;
            
            if (removedItems.Count != 0)
            {
                foreach (var r in removedItems)
                {
                    ViewModel?.SelectedModels.Remove(r as PersonViewModel);
                }
            }
            if (addedItems.Count != 0)
            {
                foreach (var a in addedItems)
                {
                    var model = a as PersonViewModel;
                    ViewModel?.SelectedModels.Add(model);
                }
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid.SelectAll();
            vm.DidSelectAll();
        }
    }
}
