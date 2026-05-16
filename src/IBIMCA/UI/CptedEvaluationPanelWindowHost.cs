namespace IBIMCA.UI
{
    public static class CptedEvaluationPanelWindowHost
    {
        public static CptedEvaluationPanelWindow? Window { get; private set; }

        public static CptedEvaluationPanelViewModel EnsureShown()
        {
            if (Window == null || !Window.IsVisible)
            {
                Window = new CptedEvaluationPanelWindow();
                Window.DataContext = new CptedEvaluationPanelViewModel();
                Window.Closed += (_, __) => Window = null;
                Window.Show();
            }
            else
            {
                Window.Activate();
            }

            return (CptedEvaluationPanelViewModel)Window.DataContext;
        }
    }
}
