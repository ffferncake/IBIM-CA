// Revit API
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using IBIMCA.UI;

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
            try
            {
                var window = new BFCertificationOverviewWindow();
                window.ShowDialog();
                return Result.Succeeded;
            }
            catch (System.Exception ex)
            {
                message = ex.ToString();
                RvtTaskDialog.Show("IBIMCA Error - BF Certification", ex.ToString());
                return Result.Failed;
            }
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
            try
            {
                var window = new GSeedCertificationOverviewWindow();
                window.ShowDialog();
                return Result.Succeeded;
            }
            catch (System.Exception ex)
            {
                message = ex.ToString();
                RvtTaskDialog.Show("IBIMCA Error - G-SEED Certification", ex.ToString());
                return Result.Failed;
            }
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
            try
            {
                var window = new CptedCertificationOverviewWindow();
                window.ShowDialog();
                return Result.Succeeded;
            }
            catch (System.Exception ex)
            {
                message = ex.ToString();
                RvtTaskDialog.Show("IBIMCA Error - CPTED Certification", ex.ToString());
                return Result.Failed;
            }
        }
    }
}
