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
using System.Windows.Shapes;
using ValheimPlusManagerWPF.Models;
using ValheimPlusManagerWPF.SupportClasses;

namespace ValheimPlusManagerWPF
{
    /// <summary>
    /// Interaction logic for ConfigurationManagerWindow.xaml
    /// </summary>
    public partial class ConfigurationManagerWindow : Window
    {
        private ValheimPlusConf ValheimPlusConf { get; set; }

        private bool ManageClient { get; set; }

        public ConfigurationManagerWindow(bool manageClient)
        {
            InitializeComponent();
            this.ManageClient = manageClient;

            if (manageClient)
            {
                ValheimPlusConf = ConfigManager.ReadConfigFile(true);
                DataContext = ValheimPlusConf;
            }
            else
            {
                ValheimPlusConf = ConfigManager.ReadConfigFile(false);
                DataContext = ValheimPlusConf;
            }
        }

        private void saveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = ConfigManager.WriteConfigFile(ValheimPlusConf, ManageClient);

            if (success)
            {
                //saveChangesLabel.Text = String.Format("Changes saved!");
                //saveChangesLabel.ForeColor = Color.Green;
            }
            else
            {
                //saveChangesLabel.Text = String.Format("Error, changes not saved!");
                //saveChangesLabel.ForeColor = Color.Red;
            }

            //var t = new Timer
            //{
            //    Interval = 3000 // it will Tick in 3 seconds
            //};
            //t.Tick += (s, e) =>
            //{
            //    saveChangesLabel.Hide();
            //    t.Stop();
            //};
            //t.Start();
        }
    }
}
