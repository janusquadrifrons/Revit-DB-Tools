
#region Namespaces
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion

namespace J_DB_Tools
{
    class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            // --- Create a custom ribbon tab
            string tabname = "J DB Tools";
            string panelname = "Janus Database Tools Panel";

            // --- Create the tab
            application.CreateRibbonTab(tabname);

            // --- Create the panel
            RibbonPanel panel = application.CreateRibbonPanel(tabname, panelname);

            #region Buttons

            // --- Create buttons
            
            var button_01 = new PushButtonData("J DB Tools Room Data Exporter", "J_RoomDataExporter", this.GetType().Assembly.Location, "J_DB_Tools.Command_01_RoomDataExporter");
            button_01.ToolTip = "Exports room data to SQLite database";
            button_01.LongDescription = "Query room data from Revit model and generate a basic SQL report showing room names, numbers, areas, etc.";

            var button_02 = new PushButtonData("J DB Tools deneme", "J_Deneme", this.GetType().Assembly.Location, "J_DB_Tools.Command_02_ParameterTracker");

            var button_03 = new PushButtonData("J DB Tools Material Quantities Tracker", "J_MaterialQuantitiesTracker", this.GetType().Assembly.Location, "J_DB_Tools.Command_03_MaterialQuantitiesTracker");

            #endregion

            // --- Add buttons to the panel
            PushButton btn_01 = panel.AddItem(button_01) as PushButton;
            PushButton btn_02 = panel.AddItem(button_02) as PushButton;
            PushButton btn_03 = panel.AddItem(button_03) as PushButton;

            // --- Creating icons for buttons
            Uri uriImage_01 = new Uri(@"E:\98_cs\REVIT API\J_DB_Tools\J_DB_Tools\Resources\rooms-32.png");
            Uri uriImage_02 = new Uri(@"E:\98_cs\REVIT API\J_DB_Tools\J_DB_Tools\Resources\parameters-32.png");
            Uri uriImage_03 = new Uri(@"E:\98_cs\REVIT API\J_DB_Tools\J_DB_Tools\Resources\measuring-32.png");

            // --- Creating BitmapImage for buttons
            BitmapImage largeImage_01 = new BitmapImage(uriImage_01);
            BitmapImage largeImage_02 = new BitmapImage(uriImage_02);
            BitmapImage largeImage_03 = new BitmapImage(uriImage_03);

            // --- Set the large icons shown on button
            btn_01.LargeImage = largeImage_01;
            btn_02.LargeImage = largeImage_02;
            btn_03.LargeImage = largeImage_03;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
