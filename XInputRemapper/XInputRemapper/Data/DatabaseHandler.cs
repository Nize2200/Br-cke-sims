using System;
using Npgsql;

namespace XInputRemapper
{
    public class DatabaseHandler
    {
        private string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=;Database=seeeds";

        public void AddToDatabase(int index, string stateJson)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(@"INSERT INTO xinput_list (index_, state_) VALUES (@index, @state::jsonb) ON CONFLICT (index_) DO UPDATE SET state_ = EXCLUDED.state_", conn))
                    {
                        cmd.Parameters.AddWithValue("index", index);
                        cmd.Parameters.AddWithValue("state", NpgsqlTypes.NpgsqlDbType.Jsonb, stateJson);
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
