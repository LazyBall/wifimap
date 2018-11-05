using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;


namespace Wi_Fi_Map
{
    public static class DataBase
    {
        private static readonly string _dataSource = "wifisqlserver.database.windows.net";
        private static readonly string _userID = "User";
        private static readonly string _password = "QyPc17ebb4GT";
        private static readonly string _initialCatalog = "wifidb";
        private static readonly string _dataTable = "WiFi";
        private static readonly string _connectionString = (new SqlConnectionStringBuilder
        {
            DataSource = _dataSource,
            UserID = _userID,
            Password = _password,
            InitialCatalog = _initialCatalog
        }).ConnectionString;

        public static void Insert(WiFiPointData wiFiPoint)
        {
            foreach (var signal in wiFiPoint.WiFiSignals)
            {
                WiFiSignalWithGeoposition sg = new WiFiSignalWithGeoposition
                {
                    BSSID = signal.BSSID,
                    Encryption = signal.Encryption,
                    Latitude = wiFiPoint.Latitude,
                    Longitude = wiFiPoint.Longitude,
                    SignalStrength = signal.SignalStrength,
                    SSID = signal.SSID,
                };
                Insert(sg);
            }
        }

        public static void Insert(WiFiSignalWithGeoposition wiFiSignal)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand
                {
                    CommandText = SqlCheckRecord(wiFiSignal.BSSID),
                    Connection = connection
                };
                object _signalStregth = command.ExecuteScalar();
                if (_signalStregth == null)
                {
                    command.CommandText =
                        SqlInsert(wiFiSignal.BSSID, wiFiSignal.SSID, wiFiSignal.Latitude,
                        wiFiSignal.Longitude, wiFiSignal.SignalStrength,
                        wiFiSignal.Encryption);
                    command.ExecuteNonQuery();
                }
                else if (wiFiSignal.SignalStrength > ((short)_signalStregth))
                {
                    command.CommandText =
                        SqlUpdate(wiFiSignal.BSSID, wiFiSignal.SSID, wiFiSignal.Latitude,
                        wiFiSignal.Longitude, wiFiSignal.SignalStrength,
                        wiFiSignal.Encryption);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<WiFiSignalWithGeoposition> SelectAll()
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

        private static string SqlInsert(string BSSID, string SSID, double Latitude, double Longitude,
            short SignalStrength, string Encryption)
        {
            return $@"INSERT INTO {_dataTable} 
                      (BSSID, SSID, Latitude, Longitude, SignalStrength, Encryption)
                                           VALUES 
                      ('{BSSID}', '{SSID}', {Latitude.ToString(new CultureInfo("en-US"))},
                        {Longitude.ToString(new CultureInfo("en-US"))}, {SignalStrength}, '{Encryption}')";
        }

        private static string SqlUpdate(string BSSID, string SSID, double Latitude, double Longitude,
            short SignalStrength, string Encryption)
        {
            return $@"UPDATE {_dataTable} 
                    SET SSID='{SSID}', Latitude={Latitude.ToString(new CultureInfo("en-US"))},
                    Longitude={Longitude.ToString(new CultureInfo("en-US"))}, SignalStrength={SignalStrength},
                    Encryption='{Encryption}' WHERE BSSID='{BSSID}'";
        }

        private static string SqlCheckRecord(string BSSID)
        {
            return $@"SELECT TOP(1) SignalStrength FROM {_dataTable} WHERE BSSID='{BSSID}'";
        }

        private static string SqlSelectAll()
        {
            return $@"SELECT * FROM {_dataTable}";
        }
    }
}