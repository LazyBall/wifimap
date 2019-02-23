using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;

//Если не будет запускаться, то в свойствах Wi-Fi AzureCloudService "Интернет" ->
//"Эмулятор" -> "Использовать полный эмулятор"
namespace WCFService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.
    public class WiFiMapDataService : IWiFiMapDataService
    {
        private static readonly string _dataTable = "WiFi";
        private static readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public IEnumerable<WiFiSignalWithGeoposition> GetData()
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

        public void SendData(IEnumerable<WiFiSignalWithGeoposition> signals)
        {
            // название хранимой процедуры
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
                foreach (var signal in signals)
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
    }
}