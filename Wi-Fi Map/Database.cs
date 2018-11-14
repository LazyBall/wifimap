using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;


namespace Wi_Fi_Map
{
    //Singleton pattern
    public sealed class Database : IDisposable
    {
        private static readonly Lazy<Database> _dataBase = new Lazy<Database>(() => new Database());
        private readonly string _dataTable;
        private readonly SqlConnection _connection;

        private Database()
        {

            // Единственные заполняемые данные для подключения к базе данных
            string dataSource = "wifisqlserver.database.windows.net";
            string userID = "User";
            string password = "QyPc17ebb4GT";
            string initialCatalog = "wifidb";
            _dataTable = "WiFi";
            //

            string connectionString = (new SqlConnectionStringBuilder
            {
                DataSource = dataSource,
                UserID = userID,
                Password = password,
                InitialCatalog = initialCatalog
            }).ConnectionString;
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }

        public static Database Instance
        {
            get
            {
                return _dataBase.Value;
            }
        }

        public void Insert(WiFiPointData wiFiPoint)
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

        public void Insert(WiFiSignalWithGeoposition wiFiSignal)
        {
            var command = new SqlCommand
            {
                CommandText = SqlCheckRecord(wiFiSignal.BSSID),
                Connection = _connection
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

        public List<WiFiSignalWithGeoposition> SelectAll()
        {
            var _list = new List<WiFiSignalWithGeoposition>();

            var command = new SqlCommand
            {
                CommandText = SqlSelectAll(),
                Connection = _connection
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
            return _list;
        }

        private string SqlInsert(string BSSID, string SSID, double Latitude, double Longitude,
            short SignalStrength, string Encryption)
        {
            return $@"INSERT INTO {_dataTable} 
                      (BSSID, SSID, Latitude, Longitude, SignalStrength, Encryption)
                                           VALUES 
                      ('{BSSID}', '{SSID}', {Latitude.ToString(new CultureInfo("en-US"))},
                        {Longitude.ToString(new CultureInfo("en-US"))}, {SignalStrength}, '{Encryption}')";
        }

        private string SqlUpdate(string BSSID, string SSID, double Latitude, double Longitude,
            short SignalStrength, string Encryption)
        {
            return $@"UPDATE {_dataTable} 
                    SET SSID='{SSID}', Latitude={Latitude.ToString(new CultureInfo("en-US"))},
                    Longitude={Longitude.ToString(new CultureInfo("en-US"))}, SignalStrength={SignalStrength},
                    Encryption='{Encryption}' WHERE BSSID='{BSSID}'";
        }

        private string SqlCheckRecord(string BSSID)
        {
            return $@"SELECT TOP(1) SignalStrength FROM {_dataTable} WHERE BSSID='{BSSID}'";
        }

        private string SqlSelectAll()
        {
            return $@"SELECT * FROM {_dataTable}";
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}