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
    /// Interaction logic for ConfigEditorControl.xaml
    /// </summary>
    public partial class ConfigEditorControl : UserControl
    {
        public ConfigEditorControl()
        {
            InitializeComponent();
            var storage = ((App)Application.Current).Storage;
            vm = new ConfigEditorViewModel(storage);
            DataContext = vm;
        }

        private ConfigEditorViewModel vm;

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var text = (e.AddedItems[0] as ConfigItemViewModel).HelpText;
            HelpTextViewer.Navigate(text);
        }
    }
}
