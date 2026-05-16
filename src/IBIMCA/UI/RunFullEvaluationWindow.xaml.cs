using System.Windows;

namespace IBIMCA.UI
{
    public partial class RunFullEvaluationWindow : Window
    {
        public RunFullEvaluationViewModel VM { get; } = new();

        public RunFullEvaluationWindow()
        {
            InitializeComponent();
            DataContext = VM;
        }

        public void OnPreview(object sender, RoutedEventArgs e)
        {
            EvaluationPanelWindowHost.EnsureShown();
        }

        public void OnRun(object sender, RoutedEventArgs e)
        {
            EvaluationPanelWindowHost.EnsureShown();
            Autodesk.Revit.UI.TaskDialog.Show("IBIMCA", "전체 평가 실행 완료");
        }

        public void OnClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
