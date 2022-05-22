using System.Windows;

namespace WpfApp3.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class PopUpWindow : Window
    {
        string popupTitleVar;
        public PopUpWindow()
        {
            InitializeComponent();
        }


        private void OnConfirmClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
    
}
