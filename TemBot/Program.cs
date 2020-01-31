using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Util;
using Sfs2X.Protocol.Serialization;

namespace TemBot
{
    class Program
    {
        static void Main(string[] args)
        {
            typeof(Temtem.Network.NetworkLogic).SetField("dqnppqrrgeo", new SmartFox());
            var q = typeof(Temtem.Network.NetworkLogic).GetField<SmartFox>();
            var monsters = typeof(Temtem.Battle.BattleClient).GetField<Temtem.Battle.BattleClient>();
            while (true)
            {
                var client = new ShinyFinder();
                while (client.SFClient.IsConnecting || client.SFClient.IsConnected || (client.fleeThread != null && client.fleeThread.IsAlive)) System.Threading.Thread.Sleep(1000);
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
