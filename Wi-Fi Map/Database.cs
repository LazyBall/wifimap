using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace Wi_Fi_Map
{
    public class DataBase
    {
        private readonly string _dataTable;
        private readonly string _connectionString;
        public DataBase(string dataSource, string userID, string password, string initialCatalog,
            string dataTable)
        {
            var _sb = new SqlConnectionStringBuilder
            {
                DataSource = dataSource,
                UserID = userID,
                Password = password,
                InitialCatalog = initialCatalog
            };
            _connectionString = _sb.ConnectionString;
            _dataTable = dataTable;
        }

        public void Insert(WiFiSignalWithGeoposition wiFiSignal)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand
                {
                    CommandText = SqlCheckRecord(wiFiSignal.BSSID),
                    Connection = connection
                };
                bool isRecord = ((bool)command.ExecuteScalar());
                if (isRecord)
                {
                    command.CommandText =
                        SqlUpdate(wiFiSignal.BSSID, wiFiSignal.SSID, wiFiSignal.Latitude,
                        wiFiSignal.Longitude, ((short)wiFiSignal.SignalStrength),
                        wiFiSignal.Encryption);
                }
                else
                {
                    command.CommandText =
                        SqlInsert(wiFiSignal.BSSID, wiFiSignal.SSID, wiFiSignal.Latitude,
                        wiFiSignal.Longitude, ((short)wiFiSignal.SignalStrength),
                        wiFiSignal.Encryption);
                }
                command.ExecuteNonQuery();
            }
        }

        public List<WiFiSignalWithGeoposition> SelectAll()
        {
            var _list = new List<WiFiSignalWithGeoposition>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand
                {
                    CommandText = SqlSelectAll(),
                    Connection = connection
                };
                SqlDataReader reader = command.ExecuteReader();
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
                        _list.Add(wifiSignal);
                    }
                }
                reader.Dispose();
            }
            return _list;
        }

        private string SqlInsert(string BSSID, string SSID, double Latitude, double Longitude,
            short SignalStrength, string Encryption)
        {
            return $@"INSERT INTO {_dataTable} 
                         (BSSID, SSID, Latitude, Longitude, SignalStrength, Encryption)
                                       VALUES 
                         ('{BSSID}', '{SSID}', {Latitude}, {Longitude}, {SignalStrength}, '{Encryption}')";
        }

        private string SqlUpdate(string BSSID, string SSID, double Latitude, double Longitude,
            short SignalStrength, string Encryption)
        {
            return $@"UPDATE {_dataTable} 
                    SET SSID='{SSID}', Latitude={Latitude},  Longitude={Longitude},
                    SignalStrength={SignalStrength}, Encryption='{Encryption}'
                    WHERE BSSID='{BSSID}'";
        }

        private string SqlCheckRecord(string BSSID)
        {
            return $@"SELECT Count(BSSID) FROM {_dataTable} WHERE BSSID='{BSSID}' limit 1";
        }

        private string SqlSelectAll()
        {
            return $@"SELECT * FROM {_dataTable}";
        }
    }
}