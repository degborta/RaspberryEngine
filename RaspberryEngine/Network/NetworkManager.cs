using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Lidgren.Network;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;
using RaspberryEngine.Assets;
using RaspberryEngine.Components;
using RaspberryEngine.Screens;
using RaspberryEngine.Network;

namespace RaspberryEngine.Network
{
    public class NetworkManager
    {
        #region Fields

        public string ServerUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        #endregion

        public NetworkManager(string serverUrl)
        {
            ServerUrl = serverUrl;
        }

        public T GetUrl<T>(string method)
        {
            return GetUrl<T>(method, string.Empty);
        }
        public T GetUrl<T>(string method, string query)
        {
            string url = ServerUrl + "?method=" + method + query;
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();

            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
                JsonSerializerSettings jsSettings = new JsonSerializerSettings();
                jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //jsSettings.ContractResolver = new HidePropertiesContractResolver(new List<string>() { "Costs" });
                return (T)JsonConvert.DeserializeObject(streamReader.ReadToEnd(),typeof(T), jsSettings);
            }
        }

    }
}
