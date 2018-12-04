using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;

namespace Wi_Fi_Map
{

    public sealed class Database
    {
        // Единственные заполняемые данные для подключения к базе данных
        private static readonly string _dataTable = "WiFi";
        private static readonly string _connectionString = (new SqlConnectionStringBuilder
        {
            DataSource = "wifisqlserver.database.windows.net",
            UserID = "User",
            Password = "QyPc17ebb4GT",
            InitialCatalog = "wifidb"
        }).ConnectionString;
        //

        public Database()
        {
            
        }

        public void AddSignal(WiFiSignalWithGeoposition wiFiSignal)
        {
            var arr = new WiFiSignalWithGeoposition[1];
            arr[0] = wiFiSignal;
            AddSignals(arr);
        }
      
        public void AddSignals(IEnumerable<WiFiSignalWithGeoposition> wiFiSignals)
        {
            // название процедуры
            string sqlExpression = "InsertWiFi";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand
                {
                    CommandText = sqlExpression,
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                foreach (var signal in wiFiSignals)
                {
                    command.Parameters.Clear();
                    command = AddParameters(command, signal);
                    command.ExecuteNonQuery();
                }
            }
        }  
        
        private SqlCommand AddParameters(SqlCommand command, WiFiSignalWithGeoposition signal)
        {
            command.Parameters.AddWithValue("@bssid", signal.BSSID);
            command.Parameters.AddWithValue("@ssid", signal.SSID);
            command.Parameters.AddWithValue("@latitude",
                signal.Latitude.ToString(new CultureInfo("en-US")));
            command.Parameters.AddWithValue("@longitude",
                signal.Longitude.ToString(new CultureInfo("en-US")));
            command.Parameters.AddWithValue("@signalStrength",
                signal.SignalStrength.ToString(new CultureInfo("en-US")));
            command.Parameters.AddWithValue("@encryption", signal.Encryption);
            return command;
        }

        public IEnumerable<WiFiSignalWithGeoposition> GetAllSignals()
        {
            string sqlExpression = $@"SELECT * FROM {_dataTable}";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            var wifiSignal = new WiFiSignalWithGeoposition
                            {
                                BSSID = reader.GetString(0),
                                SSID = reader.GetString(1),
                                Latitude = reader.GetDouble(2),
                                Longitude = reader.GetDouble(3),
                                SignalStrength = reader.GetInt16(4),
                                Encryption = reader.GetString(5),
                            };
                            yield return wifiSignal;
                        }
                    }
                }      
            }
        }

        public async Task<IEnumerable<WiFiSignalWithGeoposition>> GetAllSignalsAsync()
        {
            var list = new List<WiFiSignalWithGeoposition>();
            string sqlExpression = $@"SELECT * FROM {_dataTable}";
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(sqlExpression, connection);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        while (await reader.ReadAsync()) // построчно считываем данные
                        {
                            var wifiSignal = new WiFiSignalWithGeoposition
                            {
                                BSSID = reader.GetString(0),
                                SSID = reader.GetString(1),
                                Latitude = reader.GetDouble(2),
                                Longitude = reader.GetDouble(3),
                                SignalStrength = reader.GetInt16(4),
                                Encryption = reader.GetString(5),
                            };
                            list.Add(wifiSignal);
                        }
                    }
                }
            }
            return list;
        }
    }
}