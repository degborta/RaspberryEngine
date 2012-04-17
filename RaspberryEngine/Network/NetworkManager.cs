using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using RaspberryEngine.Assets;
using RaspberryEngine.Components;
using RaspberryEngine.Screens;
using RaspberryEngine.Network;

namespace RaspberryEngine.Network
{
    public class NetworkManager
    {
        // Client Object
        NetClient Client;
        List<NetIncomingMessage> Incomming;
        public List<NetIncomingMessage> IncommingMessages { get { return Incomming; } }

        // Game Data
        public string Username { get { return Name; } }
        string Name; //will be used for login later
        string Password; //will be used for login later
        string Server_ip;
        int Port;

        bool Connected = false;

        public NetworkManager(string Server_ip, int Port, string Name, string Password)
        {
            this.Name = Name;
            this.Password = Password;
            this.Server_ip = Server_ip;
            this.Port = Port;

            //Create client
            NetPeerConfiguration Config = new NetPeerConfiguration("kycklingstuds"); // Create new instance of configs. Parameter is "application Id". It has to be same on client and server.
            Client = new NetClient(Config); // Create new client, with previously created configs
            Client.Start(); // Start client

            //lines commented out bellow will be used later when we have login suport on the server program

            //Create the first message to the server that contains login data
            NetOutgoingMessage outmsg = Client.CreateMessage(); // Create new outgoing message
            outmsg.Write(this.Name);
            outmsg.Write(this.Password);
            Client.Connect(this.Server_ip, this.Port, outmsg); // Connect client, to ip in the properties file

            Incomming = new List<NetIncomingMessage>();
        }

        public void Update()
        {
            Incomming.Clear();

            //Add incomming messages to Incomming
            bool fetching = true;
            while (fetching)
            {
                NetIncomingMessage incom = Client.ReadMessage();
                if (incom != null)
                    Incomming.Add(incom);
                else fetching = false;
            }

            //Confirm Connection
            if (!Connected && Incomming.Count > 0)
                Connected = true;

            //Remove all messages that are not of type Data.
            //This is for safety if an other message type would slip through.
            for (int i = Incomming.Count-1; i >= 0; i--)
                if (Incomming[i].MessageType != NetIncomingMessageType.Data)
                {
                    Incomming.RemoveAt(i);
                }
        }

        public void Disconnect()
        {
            Client.Disconnect(Name + " Disconnected");
            Connected = false;
        }

        public NetOutgoingMessage NewMessage()
        { return Client.CreateMessage(); }
        public void SendMessage(NetOutgoingMessage Out, NetDeliveryMethod Method)
        { Client.SendMessage(Out, Method); }
    }
}
