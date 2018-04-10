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
    /// Interaction logic for ConfigEditorControl.xaml
    /// </summary>
    public partial class ConfigEditorControl : UserControl
    {
        public ConfigEditorControl()
        {
            InitializeComponent();
            DataContextChanged += ConfigEditorControl_DataContextChanged;
        }

        private void ConfigEditorControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            vm = e.NewValue as ConfigEditorViewModel;
        }

        private ConfigEditorViewModel vm;

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            var text =@"file://"+(e.AddedItems[0] as ConfigItemViewModel).HelpText;
            HelpTextViewer.Navigate(text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ContentPanel.ScrollToEnd();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            vm.SelectedVM.SaveConfigCommand.Execute(null);
        }
    }
}
