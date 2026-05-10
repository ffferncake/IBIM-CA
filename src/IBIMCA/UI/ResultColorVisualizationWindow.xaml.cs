using System.Windows;

namespace IBIMCA.UI
{
    public partial class ResultColorVisualizationWindow : Window
    {
        public ResultColorVisualizationWindow()
        {
            InitializeComponent();
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}