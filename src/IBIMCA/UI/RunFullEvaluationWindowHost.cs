namespace IBIMCA.UI
{
    public static class RunFullEvaluationWindowHost
    {
        public static RunFullEvaluationWindow? Window { get; private set; }

        public static RunFullEvaluationViewModel EnsureShown()
        {
            if (Window == null || !Window.IsVisible)
            {
                Window = new RunFullEvaluationWindow();
                Window.Closed += (_, __) => Window = null;
                Window.Show();
            }
            else
            {
                Window.Activate();
            }

            return (RunFullEvaluationViewModel)Window.DataContext;
        }
    }
}
