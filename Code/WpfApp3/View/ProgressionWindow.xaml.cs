using System.ComponentModel;
using System.Windows;

namespace WpfApp3.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ProgressionWindow : Window
    {
        bool closable = false;
        public ProgressionWindow()
        {
            InitializeComponent();

            while (progressDownloadFiles.Value < 100)
            {
                progressDownloadFiles.Value += 1;
            }
        }

        public ProgressionWindow(int progress)
        {
            InitializeComponent();
            progressDownloadFiles.Value = progress;
        }
        void  ProgressionWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!closable)
            {
                e.Cancel = true;
            }
        }

        private void OnConfirmProgression(object sender, RoutedEventArgs e)
        {
            closable = true;
            this.Close();
        }

        private void progressDownloadFiles_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (progressDownloadFiles.Value == 100)
            {
                btnConfirmProgression.IsEnabled = true;
            }
        }
    }
}
