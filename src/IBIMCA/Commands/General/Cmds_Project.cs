using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace IBIMCA.Commands.General
{
    [Transaction(TransactionMode.Manual)]
    public class Cmd_StandardEvaluationSettings : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                var uiapp = commandData.Application;

                // create window
                var win = new IBIMCA.UI.StandardEvaluationSettingsWindow();

                // set Revit as owner (modal)
                var helper = new System.Windows.Interop.WindowInteropHelper(win);
                helper.Owner = uiapp.MainWindowHandle;

                win.ShowDialog();
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                Autodesk.Revit.UI.TaskDialog.Show("IBIMCA", ex.ToString());
                return Result.Failed;
            }
        }
    }
}