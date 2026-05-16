// Revit API
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using IBIMCA.UI;
using System.Collections.ObjectModel;
using System.Windows.Interop;


// Revit TaskDialog alias (충돌 방지)
using RvtTaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace IBIMCA.Commands.Tools
{
    // ================================================
    // 1. 평가 패널 열기
    // ================================================
    [Transaction(TransactionMode.Manual)]
    public class Cmd_OpenEvaluationPanel : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                // Just show the window. Do NOT touch Result here.
                EvaluationPanelWindowHost.EnsureShown();
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                Autodesk.Revit.UI.TaskDialog.Show("IBIMCA Error - OpenEvaluationPanel", ex.ToString());
                return Result.Failed;
            }
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Cmd_OpenEvaluationPanel_GSEED : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                GSeedEvaluationPanelWindowHost.EnsureShown();
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                Autodesk.Revit.UI.TaskDialog.Show("IBIMCA Error - OpenGSeedEvaluationPanel", ex.ToString());
                return Result.Failed;
            }
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Cmd_OpenEvaluationPanel_CPTED : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                CptedEvaluationPanelWindowHost.EnsureShown();
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                Autodesk.Revit.UI.TaskDialog.Show("IBIMCA Error - OpenCptedEvaluationPanel", ex.ToString());
                return Result.Failed;
            }
        }
    }

    // ================================================
    // 2. 전체 평가 실행
    // ================================================
    [Transaction(TransactionMode.Manual)]
    public class Cmd_RunFullEvaluation : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                RunFullEvaluationWindowHost.EnsureShown();
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                Autodesk.Revit.UI.TaskDialog.Show("IBIMCA Error - RunFullEvaluation", ex.ToString());
                return Result.Failed;
            }
        }

        private static IEnumerable<EvaluationNode> FlattenAll(ObservableCollection<EvaluationNode> nodes)
        {
            foreach (var n in nodes)
            {
                yield return n;
                foreach (var c in FlattenAll(n.Children))
                    yield return c;
            }
        }

        private static void Rollup(EvaluationNode node)
        {
            if (!node.HasChildren)
                return;

            foreach (var ch in node.Children)
                Rollup(ch);

            // ✅ Group score = sum of children scores
            node.Score = node.Children.Sum(x => x.Score);

            // ✅ Group max score = sum of children max scores
            // If you want to KEEP the original group MaxScore (like 64), comment this out.
            //node.MaxScore = node.Children.Sum(x => x.MaxScore);

            // ✅ Group result rule:
            // all leaf nodes under this node are "우수" => "우수", otherwise "N/A"
            bool allExcellent = FlattenLeaves(node).All(x => x.Result == "우수");
            node.Result = allExcellent ? "우수" : "N/A";
        }

        private static IEnumerable<EvaluationNode> FlattenLeaves(EvaluationNode node)
        {
            if (!node.HasChildren)
            {
                yield return node;
                yield break;
            }

            foreach (var ch in node.Children)
            {
                foreach (var leaf in FlattenLeaves(ch))
                    yield return leaf;
            }
        }
    }
    // ================================================
    // 3. AI 챗봇 어시스턴트
    // ================================================
    [Transaction(TransactionMode.Manual)]
    internal static class AIAssistantWindowHost
    {
        private static AIAssistantWindow? _window;

        public static void Show()
        {
            if (_window == null || !_window.IsLoaded)
            {
                _window = new AIAssistantWindow
                {
                    DataContext = new AIAssistantViewModel()
                };

                _window.Closed += (_, __) => _window = null;
                _window.Show();
            }
            else
            {
                if (_window.WindowState == System.Windows.WindowState.Minimized)
                    _window.WindowState = System.Windows.WindowState.Normal;

                _window.Activate();
            }
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Cmd_AIAssistant : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                AIAssistantWindowHost.Show();
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                Autodesk.Revit.UI.TaskDialog.Show("IBIMCA Error - AIAssistant", ex.ToString());
                return Result.Failed;
            }
        }
    }

    // ================================================
    // 4. 외부 DB 자동 갱신
    // ================================================
    [Transaction(TransactionMode.Manual)]
    public class Cmd_UpdateExternalDB : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                var w = new UpdateExternalDBWindow
                {
                    DataContext = new UpdateExternalDBViewModel()
                };

                w.ShowDialog();
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                Autodesk.Revit.UI.TaskDialog.Show("IBIMCA Error - UpdateExternalDB", ex.ToString());
                return Result.Failed;
            }
        }
    }
    // ================================================
    // 5. 설계 대안 자동수정
    // ================================================
    [Transaction(TransactionMode.Manual)]
    public class Cmd_AutoDesignCorrection : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                var w = new AutoDesignCorrectionWindow
                {
                    DataContext = new AutoDesignCorrectionViewModel()
                };

                w.ShowDialog();
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                Autodesk.Revit.UI.TaskDialog.Show("IBIMCA Error - AutoDesignCorrection", ex.ToString());
                return Result.Failed;
            }
        }
    }
    // ================================================
    // 6. 결과 색상 가시화
    // ================================================
    [Transaction(TransactionMode.Manual)]
    public class Cmd_ResultColorVisualization : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData,
                              ref string message,
                              ElementSet elements)
        {
            try
            {
                var vm = new ResultColorVisualizationViewModel();
                var w = new ResultColorVisualizationWindow();
                w.DataContext = vm;

                w.ShowDialog();

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                Autodesk.Revit.UI.TaskDialog.Show("DEBUG ERROR", ex.ToString());
                message = ex.ToString();
                return Result.Failed;
            }
        }
    }
    // ================================================
    // 7. 엑셀 보고서 출력
    // ================================================
    [Transaction(TransactionMode.Manual)]
    public class Cmd_ExportExcelReport : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                var w = new ExportExcelReportWindow
                {
                    DataContext = new ExportExcelReportViewModel()
                };

                // Revit 메인창을 owner로 잡고 싶으면 아래 줄(선택)
                // w.Owner = System.Windows.Application.Current?.MainWindow;

                w.ShowDialog();
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                Autodesk.Revit.UI.TaskDialog.Show("IBIMCA Error - ExportExcelReport", ex.ToString());
                return Result.Failed;
            }
        }
    }
}
