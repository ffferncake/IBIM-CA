using System.Windows;

namespace IBIMCA.UI
{
    public partial class ExportExcelReportWindow : Window
    {
        public ExportExcelReportWindow()
        {
            InitializeComponent();
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}