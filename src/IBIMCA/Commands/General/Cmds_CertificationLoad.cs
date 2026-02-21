// Revit API
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

// TaskDialog 충돌 방지 (System.Windows.Forms.TaskDialog vs Revit TaskDialog)
using RvtTaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace IBIMCA.Commands.General
{
    // ================================================
    // BF (Barrier Free) 인증 로드
    // ================================================
    [Transaction(TransactionMode.Manual)]
    public class Cmd_LoadCertification_BF : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            RvtTaskDialog.Show("IBIMCA", "BF 인증 로드 (TODO)");
            return Result.Succeeded;
        }
    }

    // ================================================
    // G-SEED 인증 로드
    // ================================================
    [Transaction(TransactionMode.Manual)]
    public class Cmd_LoadCertification_GSEED : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            RvtTaskDialog.Show("IBIMCA", "G-SEED 인증 로드 (TODO)");
            return Result.Succeeded;
        }
    }

    // ================================================
    // CPTED 인증 로드
    // ================================================
    [Transaction(TransactionMode.Manual)]
    public class Cmd_LoadCertification_CPTED : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            RvtTaskDialog.Show("IBIMCA", "CPTED 인증 로드 (TODO)");
            return Result.Succeeded;
        }
    }
}