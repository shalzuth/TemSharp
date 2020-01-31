using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamworks;

namespace TemBot
{
	public static class Steam
	{
		public static String SessionTicket = "";
		public static void InitSteam()
		{
			if (!String.IsNullOrEmpty(SessionTicket))
				return;
			if (!System.IO.File.Exists("steam_appid.txt"))
				System.IO.File.WriteAllText("steam_appid.txt", "745920");
			var steamInited = SteamAPI.Init();
			System.IO.File.Delete("steam_appid.txt");
			var rgubTicket = new byte[1024];
			SteamUser.GetEncryptedAppTicket(rgubTicket, 1024, out UInt32 cubTicket);
			SessionTicket = Convert.ToBase64String(rgubTicket, 0, (Int32)cubTicket);
		}
	}
}
