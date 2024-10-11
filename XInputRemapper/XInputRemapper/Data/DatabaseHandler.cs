using System;
using Npgsql;

namespace XInputRemapper
{
    public class DatabaseHandler
    {
        private string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=Nirre123;Database=seeeds";

        public void AddToDatabase(int index, string stateJson, string commandJson)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("INSERT INTO xinput_list (index_, state_, command_) VALUES (@index, @state::jsonb, @command::jsonb)", conn))
                    {
                        cmd.Parameters.AddWithValue("index", index);
                        cmd.Parameters.AddWithValue("state", NpgsqlTypes.NpgsqlDbType.Jsonb, stateJson);
                        cmd.Parameters.AddWithValue("command", NpgsqlTypes.NpgsqlDbType.Jsonb, commandJson);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding value to database: {ex.Message}");
            }
        }
    }
}
