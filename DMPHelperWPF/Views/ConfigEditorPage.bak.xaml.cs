using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DMPHelperWPF.ViewModels;
using System.Windows.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DMPHelperWPF.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConfigEditorPage  : Page
    {
        private StorageHelper storage;

        public ConfigEditorPage()
        {
            this.InitializeComponent();
            storage = ((App)Application.Current).Storage;
            ViewModel = new ConfigEditorViewModel(storage);
        }

        public ConfigEditorViewModel ViewModel { get; private set; }
        public IConfigDisplay Display { get; }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clicked = e.ClickedItem as ConfigLabel;
            Navigate(clicked.ConfigType);

        }

        private void Navigate(DataFile dataType)
        {
            ConfigContentFrame.Navigate(typeof(ConfigItemPage), dataType.ToString());
            
        }
    }
}
