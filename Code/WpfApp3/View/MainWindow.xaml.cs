using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp3.ModelView;
using WpfApp3.Objects;
using SearchManager = System.Windows.Forms;

namespace WpfApp3.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Backup> backups = new List<Backup>();

        private List<Backup> recentBackups = new List<Backup>();

        private ViewModel viewModel;

        Thickness dynamicMargin = new Thickness();
        public MainWindow()
        {
            viewModel = new ViewModel();
            InitializeComponent();
            ReloadBackups();
            this.Resources["marginLefComponent"] = "100,0,0,0";
            backups = viewModel.LoadBackUpWork();
            
            ListBackupList.ItemsSource = backups;
            recentBackups = backups.OrderByDescending(backup => backup.date).Take(3).ToList();
            ListBackupHistoric.ItemsSource = recentBackups;
            ListBackupHistoricDaskBoard.ItemsSource = recentBackups;
            ButtonSetter(Visibility.Visible);
        }
        
        private void ReloadBackups()
        {
            backups = viewModel.LoadBackUpWork();
            recentBackups = viewModel.LoadBackUpWork();

            ListBackupList.ItemsSource = backups;
            ListBackupHistoric.ItemsSource = backups;
            ListBackupHistoricDaskBoard.ItemsSource = recentBackups;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(ActualWidth < 500)
            {
                dynamicMargin.Left = 30;
                leftPanelMenue.Width = 30;
                this.Resources["marginLeftComponent"] = dynamicMargin;
            }
            else
            {
                dynamicMargin.Left = 100;
                leftPanelMenue.Width = 100;
                this.Resources["marginLeftComponent"] = dynamicMargin;
            }
        }

        private void OnHomeClick(object sender, RoutedEventArgs e)
        {
            if(this.FindResource("homeVisible").ToString() != "Visible")
            {
                ButtonSetter(Visibility.Visible);
            }
        }

        private void OnSettingClick(object sender, RoutedEventArgs e)
        {
            LoadSettings();
            if (this.FindResource("settingVisible").ToString() != "Visible")
            {
                ButtonSetter(Visibility.Hidden,Visibility.Hidden, Visibility.Hidden,Visibility.Visible);
            }
        }

        //to force float type in floatTexbox
        private void textBoxFloatType_ValueChanged(object sender, RoutedEventArgs e)
        {
            TextBox floatTextbox = (sender as TextBox);
            string correctFloatInput = "";
            foreach(char a in floatTextbox.Text)
            {
                if (Char.IsDigit(a) || a==',')
                {
                    correctFloatInput += a;
                }
            }
            floatTextbox.Text = correctFloatInput;
        }

        //button to search file in fileExplorer
        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            string textBoxNameLinkWithButton = (sender as Button).Tag.ToString();
            TextBox textBoxConcerned = this.FindName(textBoxNameLinkWithButton) as TextBox;

            SearchManager.OpenFileDialog openFileDialog = new SearchManager.OpenFileDialog();
            SearchManager.DialogResult result = openFileDialog.ShowDialog();
            if (result == SearchManager.DialogResult.OK && !string.IsNullOrWhiteSpace(openFileDialog.FileName))
            {
                textBoxConcerned.Text = System.IO.Path.GetFullPath(openFileDialog.FileName);
            }
        }

        //button to search folder in fileExplorer
        private void btnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            string textBoxNameLinkWithButton = (sender as Button).Tag.ToString();
            TextBox textBoxConcerned = this.FindName(textBoxNameLinkWithButton) as TextBox;

            SearchManager.FolderBrowserDialog openFolderDialog = new SearchManager.FolderBrowserDialog();
            SearchManager.DialogResult result = openFolderDialog.ShowDialog();
            if (result == SearchManager.DialogResult.OK && !string.IsNullOrWhiteSpace(openFolderDialog.SelectedPath))
            {
                textBoxConcerned.Text = openFolderDialog.SelectedPath;
            }
        }

        private void OnConfirmSettingsClick(object sender, RoutedEventArgs e)
        {
            //apply settings modifications
            string businessTool = txtInputBusinessTool.Text;
            try
            {
                viewModel.ChangeSettings(txtInputBusinessTool.Text, txtInputFileSize.Text, choiceInputLanguage.Text, txtExtensionRequired.Text);
                PopUpSetter("", "Les modifications ont été apportées", Visibility.Hidden);
                
            }
            catch(Exception)
            {
                PopUpSetter("", "Paramètres incorrects", Visibility.Hidden);
            }
        }
        private void OnCancelSettingsClick(object sender, RoutedEventArgs e)
        {
            txtInputBusinessTool.Clear();
            txtInputFileSize.Clear();
            txtExtensionRequired.Clear();
            choiceInputLanguage.SelectedItem = "French";
        }

        private void OnFrenchSelectionClick(object sender, RoutedEventArgs e)
        {
            //apply change language modification
        }
        private void OnEnglishSelectionClick(object sender, RoutedEventArgs e)
        {
            //apply change language modification
        }

        private void OnAddBackupClick(object sender, RoutedEventArgs e)
        {
            if (this.FindResource("addBackupVisible").ToString() != "Visible")
            {
                ButtonSetter(Visibility.Hidden,Visibility.Visible);
            }
            
        }

        private void OnConfirmNewBackupClick(object sender, RoutedEventArgs e)
        {

            if (txtInputBackupName.Text != "" && txtInputBackupSource.Text != "" && txtInputBackupDestination.Text != "")
            {
                if (viewModel.AddBackUpWorks(txtInputBackupName.Text, txtInputBackupSource.Text,
                        txtInputBackupDestination.Text, choiceInputSavingType.Text) == 0)
                {
                    PopUpSetter("", "La backup a été ajoutée!", Visibility.Hidden);
                }
                else
                {
                    PopUpSetter("", "Erreur lors de l'ajout!", Visibility.Hidden);
                }

            }
            else
            {
                PopUpSetter("", "Paramètres incorrects!", Visibility.Hidden);
            }
            
            ReloadBackups();

        }
        private void OnCancelNewBackupClick(object sender, RoutedEventArgs e)
        {
            txtInputBackupName.Text = "";
            txtInputBackupSource.Text = "";
            txtInputBackupDestination.Text = "";
            choiceInputSavingType.SelectedItem = "Full";
        }

        private void OnBackupListClick(object sender, RoutedEventArgs e)
        {
            if (this.FindResource("backupListVisible").ToString() != "Visible")
            {
                ButtonSetter(Visibility.Hidden, Visibility.Hidden, Visibility.Visible);
            }
        }

        private void OnExecuteBackupClick(object sender, RoutedEventArgs e)
        {
            //execute the following backup when the user agree to the following popup
            Backup selectedBackup = (Backup)((Button)sender).DataContext;
            
            PopUpSetter("Execute Backup", "Are you sure you want to execute the selected backup ?", Visibility.Visible, selectedBackup.id);
        }

        private void OnRemoveBackupClick(object sender, RoutedEventArgs e)
        {
            //remove the following backup when the user agree to the following popup
            Backup selectedBackup = (Backup)((Button)sender).DataContext;
            PopUpSetter("Remove Backup", "Are you sure you want to remove the selected backup ?", Visibility.Visible, selectedBackup.id);

        }

        private void ButtonSetter(Visibility homeVisible = Visibility.Hidden, Visibility addBackupVisible = Visibility.Hidden, 
            Visibility backupListVisible = Visibility.Hidden, Visibility settingVisible = Visibility.Hidden)
        {
            this.Resources["homeVisible"] = homeVisible;
            this.Resources["addBackupVisible"] = addBackupVisible;
            this.Resources["backupListVisible"] = backupListVisible;
            this.Resources["settingVisible"] = settingVisible;

            Color color = Color.FromRgb(217, 217, 217);
            SolidColorBrush brushButton = new SolidColorBrush(color);
            //Color borderColor = Color.FromRgb(179, 179, 179);
            //SolidColorBrush borderBrushButton = new SolidColorBrush(borderColor);

            if (homeVisible == Visibility.Visible)
            {
                btnHome.Background = brushButton;
                //btnHome.BorderBrush = borderBrushButton;
                labPageTitle.Content = "Home";
            }
            else
            {
                btnHome.Background = Brushes.Transparent;
            }
            if (addBackupVisible == Visibility.Visible)
            {
                btnAddBakup.Background = brushButton;
                labPageTitle.Content = "Add Backup";
            }
            else
            {
                btnAddBakup.Background = Brushes.Transparent;
            }
            if (backupListVisible == Visibility.Visible)
            {
                btnBackupList.Background = brushButton;
                labPageTitle.Content = "Backup List";
            }
            else
            {
                btnBackupList.Background = Brushes.Transparent;
            }
            if (settingVisible == Visibility.Visible)
            {
                labPageTitle.Content = "Settings";
            }

            ListBackupList.ItemsSource = backups;
        }

        private void PopUpSetter(string popupTitle, string messageContent, Visibility cancelOption, int id = 0)
        {
            PopUpWindow windowToPopUp = new PopUpWindow();
            windowToPopUp.Title = popupTitle;
            windowToPopUp.Resources["text"] = messageContent;
            windowToPopUp.Resources["cancelOption"] = cancelOption;
            windowToPopUp.ShowDialog();
            
            if (windowToPopUp.DialogResult == true)
            {
                if (popupTitle == "Execute Backup")
                {
                    viewModel.ExecuteBackUpWork(id);
                    ProgressionWindow loadingWindow = new ProgressionWindow();
                    loadingWindow.Show();
                }
                else if (popupTitle == "Remove Backup")
                {
                    viewModel.DeleteBackUpWork(id);
                    ReloadBackups();
                }
            }
        }

        private void LoadSettings()
        {
            var settings = viewModel.LoadSettings();
            txtInputBusinessTool.Text = settings.businessSoftware;
            txtInputFileSize.Text = settings.fileSize;
            choiceInputLanguage.Text = settings.language;
            var extensions = String.Join(" ", settings.cryptoSoftExtensions.ToArray());
            txtExtensionRequired.Text = extensions;
        }

        private void OnExecuteAllBackupClick(object sender, RoutedEventArgs e)
        {
            viewModel.DeleteBackUpWork(-1);
        }
    }
}
