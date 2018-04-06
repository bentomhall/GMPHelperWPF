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
using DMPHelperWPF.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DMPHelperWPF.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConfigItemPage : Page
    {
        private ConfigItemViewModel model;
        private StorageHelper storage;

        public ConfigItemPage()
        {
            this.InitializeComponent();
            storage = ((App)Application.Current).Storage;
        }

        private void SetupViewModel(string type)
        {
            var dType = (DataFile)Enum.Parse(typeof(DataFile), type);
            model = new ConfigItemViewModel(storage, dType);
            return;
        }

        public ConfigItemViewModel ViewModel { get => model; set => model = value; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SetupViewModel(e.Parameter as string);
            base.OnNavigatedTo(e);
        }


    }
}
