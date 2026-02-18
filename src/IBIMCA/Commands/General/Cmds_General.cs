// Revit API
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
// IBIMCA
using gFil = IBIMCA.Utilities.File_Utils;

// The class belongs to the Commands namespace
namespace IBIMCA.Cmds_General
{
    #region Cmd_About

    /// <summary>
    /// Opens the project Github page.
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class Cmd_About : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Open the Url
            string linkPath = @"https://github.com/aussieBIMguru/IBIMCA";
            return gFil.OpenLinkPath(linkPath);
        }
    }

    #endregion
}