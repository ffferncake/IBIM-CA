using Autodesk.Revit.UI;

namespace IBIMCA.UI
{
    public static class EvaluationPanelWindowHost
    {
        public static EvaluationPanelWindow? Window { get; private set; }

        public static EvaluationPanelViewModel EnsureShown()
        {
            if (Window == null || !Window.IsLoaded)
            {
                Window = new EvaluationPanelWindow();
                Window.DataContext = new EvaluationPanelViewModel();
                Window.Show();
            }
            else
            {
                Window.Activate();
            }

            return (EvaluationPanelViewModel)Window.DataContext;
        }
    }
}