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
    /// Interaction logic for DungeonControl.xaml
    /// </summary>
    public partial class DungeonControl : UserControl
    {
        public DungeonControl()
        {
            InitializeComponent();
            var storage = ((App)Application.Current).Storage;
            vm = new DungeonGeneratorViewModel(storage);
            DataContext = vm;
        }

        private DungeonGeneratorViewModel vm;
        public DungeonGeneratorViewModel ViewModel { get => vm; }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var removed = e.RemovedItems;
            var selected = e.AddedItems;
            if (removed.Count != 0)
            {
                foreach (var r in removed)
                {
                    vm.SelectedViewModels.Remove(r as DungeonViewModel);
                }
            }
            if (selected.Count != 0)
            {
                foreach (var a in selected)
                {
                    vm.SelectedViewModels.Add(a as DungeonViewModel);
                }
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            DataList.SelectAll();
        }
    }
}
