﻿using System.Net;
using System.Net.Sockets;
using System.Text;
using Spectre.Console;
namespace IrcNetCoreClient
{
    class Program
    {

        static void Main(string[] args)
        {
            ClientManager clientManager = new();
            clientManager.Run();
        }
    }
}