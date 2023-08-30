/* Revit 2017 Add-in : Room Data Exporter
 * Query room data from Revit model and generate a basic SQL report showing room names, numbers, areas, etc.
 */

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

using System.Data.SQLite;

#endregion

namespace J_DB_Tools
{
    [Transaction(TransactionMode.Manual)]

    public class Command_01_RoomDataExporter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // --- Get application and document objects
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            // Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // --- Declare SQLite db file path
            string connectionString = @"Data Source=E:\98_cs\SQLite\J_DB_Tools.db;Version=3;"; // -→ SQLite db file path : SQLite lib will auto create the db if it does not exist
            SQLiteConnection sqliteConnection = new SQLiteConnection(connectionString);

            // --- Open connection to SQLite db
            sqliteConnection.Open();

            // --- Create table if it does not exist
            SQLiteCommand sqliteCommand_CreateRoomTable = new SQLiteCommand(sqliteConnection);
            sqliteCommand_CreateRoomTable.CommandText = "CREATE TABLE IF NOT EXISTS Rooms (Id INTEGER PRIMARY KEY, RoomName TEXT, RoomNumber TEXT, RoomArea REAL, RoomVolume REAL, RoomLevel TEXT, RoomHeight REAL, RoomComments TEXT)";
            sqliteCommand_CreateRoomTable.ExecuteNonQuery(); // -→ Create table if it does not exist
            sqliteConnection.Close();

            // --- Query Revit document for Room Elements
            FilteredElementCollector roomCollector = new FilteredElementCollector(doc)
            .OfCategory(BuiltInCategory.OST_Rooms)
            .WhereElementIsNotElementType();

            IList<Element> rooms = roomCollector.ToElements();

            // --- Loop over elemetns in the roomCollector
            foreach (Element room in roomCollector)
            {
                // --- Cast element to a room
                Room roomObj = room as Room;

                // --- Get room properties
                Parameter nameParam = roomObj.get_Parameter(BuiltInParameter.ROOM_NAME);
                string name = nameParam.AsString();

                Parameter numberParam = roomObj.get_Parameter(BuiltInParameter.ROOM_NUMBER);
                string number = numberParam.AsString();

                Parameter areaParam = roomObj.get_Parameter(BuiltInParameter.ROOM_AREA);
                double area = areaParam.AsDouble();

                Parameter volumeParam = roomObj.get_Parameter(BuiltInParameter.ROOM_VOLUME);
                double volume = volumeParam.AsDouble();

                Parameter levelParam = roomObj.get_Parameter(BuiltInParameter.ROOM_LEVEL_ID);
                ElementId levelId = levelParam.AsElementId();

                Parameter heightParam = roomObj.get_Parameter(BuiltInParameter.ROOM_HEIGHT);
                double height = heightParam.AsDouble();

                Parameter commentParam = roomObj.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS);
                string comment = commentParam.AsString();

                // --- Open connection to SQLite db
                sqliteConnection.Open();

                // --- Insert data into SQLite db
                SQLiteCommand sqliteCommand_InsertRoomData = new SQLiteCommand(sqliteConnection);
                sqliteCommand_InsertRoomData.CommandText = "INSERT INTO Rooms (RoomName, RoomNumber, RoomArea, RoomVolume, RoomLevel, RoomHeight, RoomComments) " +
                    "VALUES ('" + name + "', '" + number + "', '" + area + "', '" + volume + "', '" + levelId + "', '" + height + "', '" + comment + "')";
                sqliteCommand_InsertRoomData.ExecuteNonQuery(); // -→ Insert data into SQLite db
                sqliteConnection.Close();
            }   
            

            return Result.Succeeded;

        }

    }
}
