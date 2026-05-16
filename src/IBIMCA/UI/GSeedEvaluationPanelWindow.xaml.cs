using System.Windows;

namespace IBIMCA.UI
{
    public partial class GSeedEvaluationPanelWindow : Window
    {
        public GSeedEvaluationPanelViewModel VM { get; } = new();

        public GSeedEvaluationPanelWindow()
        {
            InitializeComponent();
            DataContext = VM;
        }

        public void OnSave(object sender, RoutedEventArgs e)
        {
            Autodesk.Revit.UI.TaskDialog.Show("IBIMCA", "G-SEED 평가 패널 저장 완료");
        }

        public void OnClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
