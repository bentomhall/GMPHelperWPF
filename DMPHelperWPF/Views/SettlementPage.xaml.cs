using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using LibGenerator.Settlement;
using DMPrepHelper.ViewModels;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DMPrepHelper.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettlementPage : Page
    {
        public SettlementPage()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var storage = ((App)Application.Current).Storage;
            if (vm == null || storage.ShouldReload("settlement"))
            {
                vm = new SettlementGeneratorViewModel(storage);
            }   
            base.OnNavigatedTo(e);
        }

        public SettlementGeneratorViewModel ViewModel { get => vm; }
        private SettlementGeneratorViewModel vm;

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var removed = e.RemovedItems;
            var selected = e.AddedItems;
            if (removed.Count != 0)
            {
                foreach (var r in removed)
                {
                    vm.SelectedViewModels.Remove(r as SettlementViewModel);
                }
            }
            if (selected.Count != 0)
            {
                foreach (var a in selected)
                {
                    vm.SelectedViewModels.Add(a as SettlementViewModel);
                }
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            DataList.SelectAll();
        }
    }
}
