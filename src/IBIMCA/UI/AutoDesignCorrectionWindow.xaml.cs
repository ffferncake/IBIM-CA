using System.Windows;

namespace IBIMCA.UI
{
    public partial class AutoDesignCorrectionWindow : Window
    {
        public AutoDesignCorrectionWindow()
        {
            InitializeComponent();
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}