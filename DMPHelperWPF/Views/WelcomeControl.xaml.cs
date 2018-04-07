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
using System.IO;

namespace DMPHelperWPF.Views
{
    /// <summary>
    /// Interaction logic for WelcomeControl.xaml
    /// </summary>
    public partial class WelcomeControl : UserControl
    {
        public WelcomeControl()
        {
            InitializeComponent();
            this.Loaded += OnLoad;
        }

        private void OnLoad(object sender, RoutedEventArgs args)
        {
            
            var text = File.ReadAllBytes("pack://application,,,/HelpText/WelcomeText.rtf");
            TextViewer.SelectAll();
            using (MemoryStream s = new MemoryStream(text))
            {
                s.Position = 0;
                TextViewer.Selection.Load(s, DataFormats.Rtf);
            }
                
            //throw new NotImplementedException();
        }
    }
}
