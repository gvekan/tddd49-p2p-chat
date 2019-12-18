using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TDDD49.Models;
using Newtonsoft.Json;
using System.Windows;
using System.Net;
using TDDD49.Helpers;
using System.Windows.Media.Imaging;

namespace TDDD49.Services
{
    class DataService
    {
        static string PATH = Environment.CurrentDirectory;
        static string APPDATA_PATH = PATH + "/appdata";
        static object AppDataLock = new Object();

        private MainModel _Model;
        private bool isLoading = false;

        public DataService()
        {
            MainModel.MainModelParams Params = new MainModel.MainModelParams();
            Params.Port = 6536;
            Params.Username = "username";
            string hostName = Dns.GetHostName(); 
            Params.IP = Dns.GetHostByName(hostName).AddressList[0].ToString(); // Private ip

            Params.Connections = new ConnectionObservableCollection();


            Params.CurrentConnection = new ConnectionModel(Guid.Empty, "No current connection", "");

            _Model = new MainModel(Params);
            _Model.PropertyChanged += _Model_PropertyChanged;

            // TODO: Create task
            loadModel();
            
        }

        private void _Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "CurrentConnection":
                case "Connections":
                case "LastMessage":
                case "Port":
                case "Username":
                    writeAppData();
                    break;
            }
        }

        private void _Connection_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Messages":
                    writeConnection((ConnectionModel)sender);
                    break;
            }
        }

        public MainModel Model
        {
            get
            {
                return _Model;
            }
        }

        public string SaveImage(BitmapImage image, Guid id)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            string dirPath = "/" + id.ToString() + "images/";
            string filePath = dirPath + Guid.NewGuid();
            if(!Directory.Exists(PATH + dirPath))
            {
                Directory.CreateDirectory(PATH + dirPath);
            }
            using (var fileStream = new System.IO.FileStream(PATH + filePath, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
            return PATH + filePath;
        }

        private void loadModel()
        {
            if (!File.Exists(APPDATA_PATH))
            {
                isLoading = true;
                Model.id = Guid.NewGuid();
                isLoading = false;
                writeAppData();
            } else
            {

                isLoading = true;
                // TODO: load model from file in thread
                string data = File.ReadAllText(APPDATA_PATH);
                AppData appData = JsonConvert.DeserializeObject<AppData>(data, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
                Model.Username = appData.Username;
                Model.Port = appData.Port;
                Model.id = appData.id;

                foreach (Guid id in appData.connections)
                {
                    data = File.ReadAllText(PATH + "/" + id);
                    ConnectionModel cm = JsonConvert.DeserializeObject<ConnectionModel>(data, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });

                    addConnection(cm);

                    if (id == appData.lastChatId)
                    {
                        Model.CurrentConnection = cm;
                    }
                }
                isLoading = false;
            }
        }

        private void addConnection(ConnectionModel cm)
        {
            cm.PropertyChanged += _Connection_PropertyChanged;
            Model.Connections.Add(cm);

        }

        public ConnectionModel getConnection(Guid id)
        {
            return Model.Connections.SingleOrDefault(con => con.id == id);
        }

        public ConnectionModel newConnection(Guid id, String Username, String IPAddrPort)
        {
            ConnectionModel res = new ConnectionModel(id, Username, IPAddrPort);
            addConnection(res);
            writeConnection(res);
            return res;
        }

        private void writeConnection(ConnectionModel cm)
        {
            new Task(() =>
            {
                lock (cm)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        File.WriteAllText(PATH + "/" + cm.id, JsonConvert.SerializeObject(cm, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        }));
                    });
                }
            }).Start();
        }

        private void writeAppData()
        {
            if (!isLoading)
            {
                Task a = new Task(() =>
                {
                    lock(AppDataLock) { 
                        AppData appData = new AppData(Model);
                        File.WriteAllText(APPDATA_PATH, JsonConvert.SerializeObject(appData, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        }));
                    }
                });
                a.Start();
            }
        }

        class AppData
        {

            public string Username;
            public int Port;
            public Guid id;
            public Guid lastChatId;
            public List<Guid> connections;

            public AppData(MainModel Model)
            {
                Username = Model.Username;
                Port = Model.Port;
                id = Model.id;
                lastChatId = Model.CurrentConnection.id;
                connections = Model.Connections.Select(con => con.id).ToList();
            }

            [JsonConstructor]
            public AppData()
            {

            }
        }
    }
}
