using System.Windows;

namespace IBIMCA.UI
{
    public partial class UpdateExternalDBWindow : Window
    {
        public UpdateExternalDBWindow()
        {
            InitializeComponent();
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}