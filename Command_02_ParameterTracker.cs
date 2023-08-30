/* Revit 2017 Add-in : Pamaeter Tracker
 * Record parameter changes in the SQLite database along with timestamps and other relevant information.
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
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Events;

using System.Data.SQLite;

#endregion

namespace J_DB_Tools
{
    [Transaction(TransactionMode.Manual)]

    public class Command_02_ParameterTracker : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // --- Get application and document objects
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // --- Declare SQLite db file path
            string connectionString = @"Data Source=E:\98_cs\SQLite\J_DB_Tools.db;Version=3;"; // -→ SQLite db file path : SQLite lib will auto create the db if it does not exist
            SQLiteConnection sqliteConnection = new SQLiteConnection(connectionString);

            // --- Open connection to SQLite db
            sqliteConnection.Open();

            // --- Create table if it does not exist
            SQLiteCommand sQLiteCommand_ParameterTracker = new SQLiteCommand(sqliteConnection);
            sQLiteCommand_ParameterTracker.CommandText = "CREATE TABLE IF NOT EXISTS ParameterEvents ()";
            sQLiteCommand_ParameterTracker.ExecuteNonQuery(); // -→ Create table if it does not exist






            return Result.Succeeded;

        }

    }
}
