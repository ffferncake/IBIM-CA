namespace IBIMCA.UI
{
    public static class GSeedEvaluationPanelWindowHost
    {
        public static GSeedEvaluationPanelWindow? Window { get; private set; }

        public static GSeedEvaluationPanelViewModel EnsureShown()
        {
            if (Window == null || !Window.IsVisible)
            {
                Window = new GSeedEvaluationPanelWindow();
                Window.DataContext = new GSeedEvaluationPanelViewModel();
                Window.Closed += (_, __) => Window = null;
                Window.Show();
            }
            else
            {
                Window.Activate();
            }

            return (GSeedEvaluationPanelViewModel)Window.DataContext;
        }
    }
}
