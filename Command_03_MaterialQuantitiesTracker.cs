/* Revit 2017 Add-in : Material Quantities Tracker
 * Description: Track material usage across model. Calculate total quantities used for each material. Output to SQLite db.
 */

/* UnderConstruction */

#region Namespaces

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
//using Autodesk.Revit.DB.Architecture;
//using Autodesk.Revit.DB.Events;

using System.Data.SQLite;

#endregion

namespace J_DB_Tools
{
    [Transaction(TransactionMode.Manual)]

    public class Command_03_MaterialQuantitiesTracker : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // --- Get application and document objects
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            //Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // --- Declare SQLite db file path
            string connectionString = @"Data Source=E:\98_cs\SQLite\J_DB_Tools.db;Version=3;"; // -→ SQLite db file path : SQLite lib will auto create the db if it does not exist
            SQLiteConnection sqliteConnection = new SQLiteConnection(connectionString);

            // --- Open connection to SQLite db
            sqliteConnection.Open();

            // --- Create table if it does not exist
            SQLiteCommand sQLiteCommand_ParameterTracker = new SQLiteCommand(sqliteConnection);
            sQLiteCommand_ParameterTracker.CommandText = "CREATE TABLE IF NOT EXISTS MaterialQuantities (MaterialId TEXT, MaterialVolume REAL)";
            sQLiteCommand_ParameterTracker.ExecuteNonQuery(); // -→ Create table if it does not exist

            // --- Get all materials in model
            FilteredElementCollector materialCollector = new FilteredElementCollector(doc)
                .OfClass(typeof(Material));

            // --- Loop through all materials, get volume, insert to db.
            foreach(Material mat in materialCollector)
            {
                double matVolume = 0;

                FilteredElementCollector elementCollector = new FilteredElementCollector(doc)
                    .OfClass(typeof(Wall));

                // --- Loop through all elements
                foreach(Element e in elementCollector)
                {
                    // --- Get elementId of material
                    ElementId eId = e.GetMaterialIds(false).First();
                    matVolume += e.GetMaterialVolume(eId);
                }

                // --- Get material ID string
                string matId = mat.GetMaterialIds(false).First().IntegerValue.ToString();

                // --- Insert material volume into SQLite db
                SQLiteCommand sQLiteCommand_InsertMaterialVolume = new SQLiteCommand(sqliteConnection);

                sQLiteCommand_InsertMaterialVolume.CommandText = "INSERT INTO MaterialQuantities (MaterialId, MaterialVolume)" + "VALUES ('" + matId + "', '" + matVolume + "')";
                sQLiteCommand_InsertMaterialVolume.ExecuteNonQuery(); // -→ Insert material volume into SQLite db
                sqliteConnection.Close();
            }

            return Result.Succeeded;

        }

    }
}
