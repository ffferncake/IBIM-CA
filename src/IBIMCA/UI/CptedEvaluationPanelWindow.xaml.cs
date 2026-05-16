using System.Windows;

namespace IBIMCA.UI
{
    public partial class CptedEvaluationPanelWindow : Window
    {
        public CptedEvaluationPanelViewModel VM { get; } = new();

        public CptedEvaluationPanelWindow()
        {
            InitializeComponent();
            DataContext = VM;
        }

        public void OnSave(object sender, RoutedEventArgs e)
        {
            Autodesk.Revit.UI.TaskDialog.Show("IBIMCA", "CPTED 평가 패널 저장 완료");
        }

        public void OnClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
