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
using DMPrepHelper.ViewModels;
using System.Diagnostics;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DMPHelperWPF.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NpcPage : Page
    {
        public NpcPage()
        {
            this.InitializeComponent();
            
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var storage = ((App)Application.Current).Storage;
            if (ViewModel == null || storage.ShouldReload("npc"))
            {
                ViewModel = new NPCGeneratorViewModel(storage);
                ViewModel.GenerateCommand.CanExecuteChanged += GenerateCommand_CanExecuteChanged;
            }
            base.OnNavigatedTo(e);
        }

        private void GenerateCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            CreateButton.IsEnabled = true;
        }

        private NPCGeneratorViewModel vm;

        public NPCGeneratorViewModel ViewModel { get => vm; set => vm = value; }

        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var addedItems = e.AddedItems;
            var removedItems = e.RemovedItems;
            if (removedItems.Count != 0)
            {
                foreach (var r in removedItems)
                {
                    vm.SelectedModels.Remove(r as PersonViewModel);
                }
            }
            if (addedItems.Count != 0)
            {
                foreach (var a in addedItems)
                {
                    var model = a as PersonViewModel;
                    vm.SelectedModels.Add(model);
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
