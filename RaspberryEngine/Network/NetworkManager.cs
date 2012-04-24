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
        #region Fields

        NetClient _client;
        NetPeerConfiguration _config;

        private List<NetIncomingMessage> _incommingMessages;
        public List<NetIncomingMessage> IncommingMessages
        { 
            get { return _incommingMessages; }
        }

        private string _appId;
        public string AppId
        {
            get { return _appId; }
            set { _appId = value; }
        }

        private string _serverIp;
        public string ServerIp
        {
            get { return _serverIp; }
            set { _serverIp = value; }
        }

        private int _port;
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        private string _userName;
        public string Username 
        {
            get { return _userName; }
            set { _userName = value; } 
        }

        private string _password;
        public string PassWord
        {
            get { return _password; }
            set { _password = value; }
        }

        private bool _connected = false;

        #endregion

        public NetworkManager()
        {
            _incommingMessages = new List<NetIncomingMessage>();
        }

        public void Update()
        {
            //trash old messages
            _incommingMessages.Clear();

            //get new messages
            bool fetching = true;
            while (fetching)
            {
                NetIncomingMessage incom = _client.ReadMessage();

                if (incom != null)
                {
                    _incommingMessages.Add(incom);
                }
                else
                {
                    fetching = false;
                }
            }

            //If we could find any messages
            //Confirm Connection
            if (!_connected && _incommingMessages.Count > 0)
                _connected = true;

            //Remove all messages that are not of type Data.
            //This is for safety if an other message type would slip through.
            for (int i = _incommingMessages.Count - 1; i >= 0; i--)
            {
                if (_incommingMessages[i].MessageType != NetIncomingMessageType.Data)
                {
                    _incommingMessages.RemoveAt(i);
                }
            }
        }

        public void Connect()
        {
            // Create new instance of configs. 
            //Parameter is "application Id". It has to be same on client and server.
            _config = new NetPeerConfiguration(_appId);

            // Start client
            // Create new client, with previously created configs
            _client = new NetClient(_config);
            _client.Start();

            //Create the first message to the server that contains login data
            NetOutgoingMessage outmsg = _client.CreateMessage();
            outmsg.Write(_userName);
            outmsg.Write(_password);
            _client.Connect(_serverIp, _port, outmsg);
        }

        public void Disconnect()
        {
            _client.Disconnect(_userName + " Disconnected");
            _connected = false;
        }

        public NetOutgoingMessage NewMessage()
        {
            return _client.CreateMessage();
        }

        public void SendMessage(NetOutgoingMessage Out, NetDeliveryMethod Method)
        {
            _client.SendMessage(Out, Method);
        }
    }
}
