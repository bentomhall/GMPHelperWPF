﻿using System;
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
        }

        public DungeonGeneratorViewModel ViewModel { get => DataContext as DungeonGeneratorViewModel; }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel == null) { return; } // viewmodel gets changed out first as things change.
            var removed = e.RemovedItems;
            var selected = e.AddedItems;
            if (removed.Count != 0)
            {
                foreach (var r in removed)
                {
                    ViewModel.SelectedViewModels.Remove(r as DungeonViewModel);
                }
            }
            if (selected.Count != 0)
            {
                foreach (var a in selected)
                {
                    ViewModel.SelectedViewModels.Add(a as DungeonViewModel);
                }
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            DataList.SelectAll();
        }
    }
}
