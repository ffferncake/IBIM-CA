using Autodesk.Revit.UI;

namespace IBIMCA.UI
{
    public class EvaluationPanelProvider : IDockablePaneProvider
    {
        public static readonly DockablePaneId PaneId =
            new DockablePaneId(new System.Guid("E2D2A99A-7B40-4D88-A6F0-7C9D1C6B92B1"));

        public static EvaluationPanelProvider? Instance { get; private set; }

        public EvaluationPanelViewModel ViewModel { get; } = new EvaluationPanelViewModel();

        // ✅ Revit이 DockablePane 만들 때 이 메서드를 호출함 (시그니처 정확해야 함)
        public void SetupDockablePane(DockablePaneProviderData data)
        {
            Instance = this;

            var view = new EvaluationPanelView();   // UserControl
            view.DataContext = ViewModel;

            data.FrameworkElement = view;

            data.InitialState = new DockablePaneState
            {
                DockPosition = DockPosition.Left
            };

            data.VisibleByDefault = false;
        }
    }
}