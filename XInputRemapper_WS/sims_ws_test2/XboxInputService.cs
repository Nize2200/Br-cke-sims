using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using SharpDX.XInput;
using System.IO;

namespace XInputRemapper
{
    public partial class XboxInputService : ServiceBase
    {

        private const string folderPath = "C:\\temp\\scan";
        private const string filePath = folderPath + "service_check.txt";

        private ControllerHandler controllerHandler;
        private DatabaseHandler databaseHandler;
        private ButtonMapper buttonMapper;



        public XboxInputService()
        {
            InitializeComponent();
            controllerHandler = new ControllerHandler(HandleInput);
            databaseHandler = new DatabaseHandler();
            buttonMapper = new ButtonMapper();
        }

        protected override void OnStart(string[] args)
        {

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            // Start the controller input monitoring
            Task.Run(() => StartMonitoringController());

        }

        private async Task StartMonitoringController()
        {
            await controllerHandler.StartReadingInput();
        }

        private void HandleInput(Gamepad state)
        {
            try
            {
                // Map the buttons based on user-defined mappings
                var remappedState = buttonMapper.MapButtons(state);

                // Serialize state to JSON
                string stateJson = Newtonsoft.Json.JsonConvert.SerializeObject(remappedState);
                string commandJson = "{\"command\": \"test\"}";

                LogToFile(stateJson, commandJson);

                // Store the remapped state in the database
                databaseHandler.AddToDatabase(1, stateJson, commandJson);
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during input handling or database operations
                EventLog.WriteEntry($"Error handling controller state: {ex.Message}");
                LogToFile($"Error: {ex.Message}", null);
            }
        }

        private void LogToFile(string stateJson, string commandJson)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, true)) // Append to the log file
                {
                    sw.WriteLine($"Timestamp: {DateTime.Now}");
                    if (!string.IsNullOrEmpty(stateJson))
                    {
                        sw.WriteLine($"State JSON: {stateJson}");
                    }
                    if (!string.IsNullOrEmpty(commandJson))
                    {
                        sw.WriteLine($"Command JSON: {commandJson}");
                    }
                    sw.WriteLine(); // Blank line between entries
                }
            }
            catch (Exception ex)
            {
                // If logging fails, write to Event Log
                EventLog.WriteEntry($"Failed to write to log file: {ex.Message}");
            }
        }

        protected override void OnStop()
        {
            // Any cleanup if necessary when the service stops
            controllerHandler.PauseUpdates();
        }
    }
}