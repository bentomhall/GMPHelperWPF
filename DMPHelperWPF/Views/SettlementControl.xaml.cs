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
    /// Interaction logic for SettlementControl.xaml
    /// </summary>
    public partial class SettlementControl : UserControl
    {
        public SettlementControl()
        {
            InitializeComponent();/*
            var storage = ((App)Application.Current).Storage;
            vm = new SettlementGeneratorViewModel(storage);
            DataContext = vm;*/
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel == null) { return; } // viewmodel gets changed out first as things change.
            var removed = e.RemovedItems;
            var selected = e.AddedItems;
            if (removed.Count != 0)
            {
                foreach (var r in removed)
                {
                    ViewModel.SelectedViewModels.Remove(r as SettlementViewModel);
                }
            }
            if (selected.Count != 0)
            {
                foreach (var a in selected)
                {
                    ViewModel.SelectedViewModels.Add(a as SettlementViewModel);
                }
            }
        }

        public SettlementGeneratorViewModel ViewModel { get => DataContext as SettlementGeneratorViewModel; }
        private SettlementGeneratorViewModel vm;

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            DataList.SelectAll();
        }
    }
}
