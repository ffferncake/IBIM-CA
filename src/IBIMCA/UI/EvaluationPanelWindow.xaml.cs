using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace IBIMCA.UI
{
    public partial class EvaluationPanelWindow : Window
    {
        public EvaluationPanelViewModel VM { get; } = new EvaluationPanelViewModel();

        public EvaluationPanelWindow()
        {
            InitializeComponent();
            DataContext = VM;
        }

        // Changed visibility to public to ensure XAML event hookup resolves correctly
        public void OnSave(object sender, RoutedEventArgs e)
        {
            Autodesk.Revit.UI.TaskDialog.Show("IBIMCA", "저장 완료");
        }

        public void OnClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}