// Revit API
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

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
            RvtTaskDialog.Show("IBIMCA", "평가 패널 열기 (TODO)");
            return Result.Succeeded;
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
            RvtTaskDialog.Show("IBIMCA", "전체 평가 실행 (TODO)");
            return Result.Succeeded;
        }
    }

    // ================================================
    // 3. AI 챗봇 어시스턴트
    // ================================================
    [Transaction(TransactionMode.Manual)]
    public class Cmd_AIAssistant : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            RvtTaskDialog.Show("IBIMCA", "AI 챗봇 어시스턴트 (TODO)");
            return Result.Succeeded;
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
            RvtTaskDialog.Show("IBIMCA", "외부 DB 자동 갱신 (TODO)");
            return Result.Succeeded;
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
            RvtTaskDialog.Show("IBIMCA", "설계 대안 자동수정 (TODO)");
            return Result.Succeeded;
        }
    }

    // ================================================
    // 6. 결과 색상 가시화
    // ================================================
    [Transaction(TransactionMode.Manual)]
    public class Cmd_ResultColorVisualization : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            RvtTaskDialog.Show("IBIMCA", "결과 색상 가시화 (TODO)");
            return Result.Succeeded;
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
            RvtTaskDialog.Show("IBIMCA", "엑셀 보고서 출력 (TODO)");
            return Result.Succeeded;
        }
    }
}