using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using ValheimPlusManager.Models;
using ValheimPlusManager.SupportClasses;

namespace ValheimPlusManager
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

        private void IntValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void FloatValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-9]+(\\.[0-9]?)?");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
