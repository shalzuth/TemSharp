using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using Sfs2X.Util;
using Sfs2X.Protocol.Serialization;
using Steamworks;

namespace TemBot
{
	public class ShinyFinder
	{
		public SmartFox SFClient;
		Int32 LagValue;
		Boolean GameStarted = false;
		Boolean InBattle = false;
		String SessionTicket = "";
		Single X;
		Single Y;
		Boolean LoadedGeneral = false;
		Boolean LoadedDialogues = false;
		ConfigData ConfigData = new ConfigData
		{
			Host = "51.161.14.64",
			Port = 9933,
			Zone = "2Lobby",
			UdpHost = "51.161.14.64",
			UdpPort = 9933
		};
		public void InitSteam()
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
		public ShinyFinder()
		{
			InitSteam();
			if (DefaultSFSDataSerializer.RunningAssembly == null)
				DefaultSFSDataSerializer.RunningAssembly = Assembly.Load("Assembly-CSharp");

			SFClient = new SmartFox();
			SFClient.Log.LoggingLevel = Sfs2X.Logging.LogLevel.INFO;
			SFClient.ThreadSafeMode = false;
			//SFClient.RemoveAllEventListeners();
			SFClient.AddEventListener(SFSEvent.EXTENSION_RESPONSE, new EventListenerDelegate(OnResponse));
			SFClient.AddEventListener(SFSEvent.PING_PONG, new EventListenerDelegate(OnPing));
			SFClient.AddEventListener(SFSEvent.CONNECTION_LOST, new EventListenerDelegate(OnDisconnect));

			SFClient.AddEventListener(SFSEvent.CONNECTION, new EventListenerDelegate(OnConnect));
			SFClient.AddEventListener(SFSEvent.LOGIN, new EventListenerDelegate(OnLogin));
			SFClient.AddEventListener(SFSEvent.LOGOUT, new EventListenerDelegate(OnLogout));
			SFClient.AddEventListener(SFSEvent.LOGIN_ERROR, new EventListenerDelegate(OnLoginError));

			SFClient.AddEventListener(SFSEvent.USER_VARIABLES_UPDATE, new EventListenerDelegate(OnUserVarUpdate));
			SFClient.AddEventListener(SFSEvent.PUBLIC_MESSAGE, new EventListenerDelegate(OnPublicMessage));

			SFClient.AddEventListener(SFSEvent.USER_ENTER_ROOM, new EventListenerDelegate(Dummy));
			SFClient.AddEventListener(SFSEvent.USER_EXIT_ROOM, new EventListenerDelegate(Dummy));
			SFClient.AddEventListener(SFSEvent.ROOM_JOIN, new EventListenerDelegate(Dummy));
			SFClient.AddEventListener(SFSEvent.ROOM_VARIABLES_UPDATE, new EventListenerDelegate(Dummy));
			SFClient.Connect(ConfigData);
		}
		void OnPing(BaseEvent eventParam)
		{
			LagValue = (int)eventParam.Params["lagValue"];
		}
		void OnDisconnect(BaseEvent eventParam)
		{
			if (false)
			{
				Console.WriteLine("OnDisconnect : " + eventParam.Type);
				foreach (var kvp in eventParam.Params)
					Console.WriteLine("\t" + kvp.Key + " : " + kvp.Value);
				//Console.WriteLine(eventParam.Params["reason"]);
			}
		}
		void OnLogout(BaseEvent eventParam)
		{
			if (false)
			{
				Console.WriteLine("OnLogout : " + eventParam.Type);
				foreach (var kvp in eventParam.Params)
					Console.WriteLine("\t" + kvp.Key + " : " + kvp.Value);
			}
			Task.Run(() => { Login(); });
			//Console.WriteLine(eventParam.Params["reason"]);
		}
		void Dummy(BaseEvent eventParam)
		{
			//Console.WriteLine(eventParam);
		}
		void OnLoginError(BaseEvent eventParam)
		{
			Console.WriteLine(eventParam.Params["reason"]);
		}
		void OnConnect(BaseEvent eventParam)
		{
			if (false)
			{
				Console.WriteLine("OnConnect : " + eventParam.Type);
				foreach (var kvp in eventParam.Params)
					Console.WriteLine("\t" + kvp.Key + " : " + kvp.Value);
			}
			if ((Boolean)eventParam.Params["success"])
			{
				Login();
			}
		}
		void Login()
		{
			var zoneName = !GameStarted ? "2Lobby" : InBattle ? "1BattleZone" : "3Game";
			var isfsobject = new SFSObject();
			isfsobject.PutByte("LT", 0);
			isfsobject.PutUtfString("authSessionTicket", SessionTicket);
			isfsobject.PutShort("v", 30);
			//Thread.Sleep(4000);
			SFClient.Send(new LoginRequest(SteamUser.GetSteamID().m_SteamID.ToString(), string.Empty, zoneName, isfsobject));
		}
		void OnLogin(BaseEvent eventParam)
		{
			Task.Run(() =>
			{
				if (false)
				{
					Console.WriteLine("OnLogin : " + eventParam.Type);
					foreach (var kvp in eventParam.Params)
						Console.WriteLine("\t" + kvp.Key + " : " + kvp.Value);
				}
				if (!SFClient.UdpInited)
				{
					SFClient.AddEventListener(SFSEvent.UDP_INIT, new EventListenerDelegate(OnUdpInit));
					SFClient.InitUDP();
					return;
				}
				if (eventParam.Params["zone"].ToString() == "1BattleZone")
				{
					//System.Threading.Thread.Sleep(8000);
					//Login();
					//System.Threading.Thread.Sleep(1000);
					SFClient.Send(new ExtensionRequest("joinRoom", new SFSObject()));
				}

				if (!InBattle)
				{
					var isfsobject = new SFSObject();
					isfsobject.PutShort("sid", 0);
					isfsobject.PutBool("sst", true);
					var isfsarray = new SFSArray();
					isfsarray.AddFloat(X);
					isfsarray.AddFloat(0.0f);
					isfsarray.AddFloat(Y);
					isfsobject.PutSFSArray("pos", isfsarray);
					SFClient.Send(new ExtensionRequest("joinRoom", isfsobject));

					isfsobject = new SFSObject();
					isfsobject.PutShort("sid", 1);
					SFClient.Send(new ExtensionRequest("joinRoom", isfsobject));

					isfsobject = new SFSObject();
					isfsobject.PutShort("sid", 8);
					SFClient.Send(new ExtensionRequest("joinRoom", isfsobject));


					InBattle = true;
					isfsobject = new SFSObject();
					isfsobject.PutShort("sid", 1);
					isfsobject.PutShort("spid", 0);
					SFClient.Send(new ExtensionRequest("spawnMonster", isfsobject));


				}
			});
		}
		void OnUdpInit(BaseEvent eventParam)
		{
			if (false)
			{
				Console.WriteLine("OnUdpInit : " + eventParam.Type);
				foreach (var kvp in eventParam.Params)
					Console.WriteLine("\t" + kvp.Key + " : " + kvp.Value);
			}
			if (!(Boolean)eventParam.Params["success"])
				throw new Exception("");
			Task.Run(() =>
			{
				if (!GameStarted)
				{
					var isfsobject = new SFSObject();
					isfsobject.PutInt("tmpdV", 0);// hjjfkrmqrjc.inqinlfoilq);
					SFClient.Send(new ExtensionRequest("gameStart", isfsobject));
				}
			});
		}
		void OnUserVarUpdate(BaseEvent eventParam)
		{
			if (false)
			{
				Console.WriteLine("OnUserVarUpdate : " + eventParam.Type);
				foreach (var kvp in eventParam.Params)
					Console.WriteLine("\t" + kvp.Key + " : " + kvp.Value);
			}
			if (!LoadedGeneral)
			{
				LoadedGeneral = true;
				var isfsobject = new SFSObject();
				isfsobject.PutInt("v", 4);
				//SFClient.Send(new ExtensionRequest("loadSave.General", isfsobject));
			}
			if (!LoadedDialogues)
			{
				LoadedDialogues = true;
				var isfsobject = new SFSObject();
				isfsobject.PutInt("v", 10);
				//SFClient.Send(new ExtensionRequest("loadSave.Dialogues", isfsobject));

				//SFClient.Send(new ExtensionRequest("temtemWelfare.GetPlayerData", new SFSObject()));


			}
		}
		void OnPublicMessage(BaseEvent hqhciklmlhl)
		{
			var room = (Room)hqhciklmlhl.Params["room"];
			for (int i = 0; i < room.UserList.Count; i++)
			{
				var user = room.UserList[i];
				if (!user.IsItMe && user.ContainsVariable("v"))
				{
					var val = user.GetVariable("v").GetSFSArrayValue().GetInt(0);
					//this.epkjhcinpik[val] = user;
				}
			}
		}
		Boolean trigger = false;
		Boolean fleed = false;
		Boolean fleeBug = false;
		public Thread fleeThread;
		List<Int32> battleTicks = new List<int> { Environment.TickCount };
		List<Int32> battleTimes = new List<int>();
		void OnResponse(BaseEvent eventParam)
		{
			if (false)
			{
				Console.WriteLine("OnResponse : " + eventParam.Type);
				foreach (var kvp in eventParam.Params)
					Console.WriteLine("\t" + kvp.Key + " : " + kvp.Value);
			}
			var varst = (SFSObject)eventParam.Params["params"];
			//Console.WriteLine(varst.GetDump());

			Task.Run(() =>
			{
				var vars = (SFSObject)eventParam.Params["params"];
				var array = eventParam.Params["cmd"].ToString().Split('.');
				var text = array[0];
				if (text == "Chat")
					Console.WriteLine(text);
				else if (text == "TemtemWelfare")
					Console.WriteLine(text);
				else if (text == "PA")
					Console.WriteLine(text);
				else if (text == "GameStart")
				{
					if (vars.ContainsKey("inbattle"))
						InBattle = vars.GetBool("inbattle");
					GameStarted = true;
					X = vars.GetFloat("x");
					Y = vars.GetFloat("z");
					//Thread.Sleep(700);
					SFClient.Send(new LogoutRequest());
					//Task.Run(() => { SFClient.Send(new LogoutRequest()); });
				}
				else if (text == "Monsters")
					Console.WriteLine(text);
				else if (text == "PickAndBan")
					Console.WriteLine(text);
				else if (text == "Notifications")
					Console.WriteLine(text);
				else if (text == "UCP")
					Console.WriteLine(text);
				else if (text == "Coop")
					Console.WriteLine(text);
				else if (text == "Friends")
					Console.WriteLine(text);
				else if (text == "Gameplay")
				{
					//Console.WriteLine(text);
				}
				else if (text == "Battle")
				{
					if (array.Length > 1)
					{
						if (array[1] == "CurrentState")
						{
							var enemies = vars.GetSFSArray("aiMon");
							var count = 0;

							foreach (temtem.networkserialized.NetworkMonster monster in enemies)
							{
								if (monster == null)
									continue;
								count++;
							}

							if (!fleed)
							{
								battleTicks.Add(Environment.TickCount);
								battleTimes.Add(battleTicks.Last() - battleTicks[battleTicks.Count - 2]);
								Console.WriteLine(DateTime.Now + " : new battle, last battle time " + (battleTimes.Last() / 1000.0f) + ", average time " + battleTimes.Average() / 1000.0f);
								fleed = true;
								while (fleeBug) { }
								fleeBug = true;
								fleeThread = new Thread(() =>
								{

									while (fleeBug)
									{
										if (!SFClient.IsConnected)
										{
											SFClient.KillConnection();
											Console.WriteLine("reseting");
											Thread.Sleep(2000);
											return;
										}
										var actype4 = new SFSObject();
										actype4.PutByte("actype", 4);
										SFClient.Send(new ExtensionRequest("battle", actype4));
										SFClient.Send(new ExtensionRequest("battle", actype4));

										var aioArray = new SFSArray();
										for (Byte i = 0; i < count; i++)
										{
											var aioObj = new SFSObject();
											aioObj.PutByte("slot", (Byte)(64 + i));
											aioObj.PutByte("techI", i);
											aioObj.PutByte("actype", 0);
											aioArray.AddSFSObject(aioObj);
										}
										var aio = new SFSObject();
										aio.PutSFSArray("aiO", aioArray);
										Thread.Sleep(2500);
										if (fleeBug)
											SFClient.Send(new ExtensionRequest("battle", aio));
										Thread.Sleep(1000);
									}
								});
								fleeThread.Start();
							}
							foreach (temtem.networkserialized.NetworkMonster monster in enemies)
							{
								if (monster == null)
									continue;
								var monsterCache = Temtems.AllTemtems[monster.monsterNumber];
								Console.WriteLine("    " + monsterCache.name + " (lvl" + monster.level + ") : " + (monster.luma ? "Shiny" : "Normal") + " : " + (monster.gender ? "Female" : "Male"));

							}
						}
						if (array[1] == "BattleFinished")
						{
							fleeBug = false;
							InBattle = false;
							fleed = false;
							//Thread.Sleep(2000);
							SFClient.Send(new LogoutRequest());
						}
						if (array[1] == "BattleTurnResult")
						{
							if (vars.ContainsKey("result"))
							{
								var result = vars.GetSFSArray("result");
								foreach (SFSObject r in result)
								{
									if (r.GetBool("runres"))
									{
										//Console.WriteLine("run success");
									}
									//Console.WriteLine("run fail");
								}
							}
							else
								Console.WriteLine("no result");
						}
					}
				}
				else if (text == "Inventory")
					Console.WriteLine(text);
				else if (text == "PromoCode")
					Console.WriteLine(text);
				else if (text == "UPP")
				{
					//Console.WriteLine(text);
				}
				else if (text == "Movement")
					Console.WriteLine(text);
				else if (text == "Breeding")
					Console.WriteLine(text);
				else if (text == "World")
				{
					if (!trigger)
					{
						trigger = true;
						/*Task.Run(() =>
						{
							System.Threading.Thread.Sleep(1000);
							Login();
						});
						Task.Run(() =>
						{
							System.Threading.Thread.Sleep(2000);
							SFClient.Send(new ExtensionRequest("joinRoom", new SFSObject()));
						});*/
					}
					Console.WriteLine(text);
				}
				else if (text == "SetNickname")
					Console.WriteLine(text);
				else if (text == "Trade")
					Console.WriteLine(text);
				else if (text == "PCControl")
					Console.WriteLine(text);
				else
					Console.WriteLine("unk : " + text);
			});
		}
		static uint ComputeFNVHash(string s)
		{
			var num = 0x811C9DC5u;
			for (var i = 0; i < s.Length; i++)
				num = (s[i] ^ num) * 0x10000193u;
			return num;
		}
	}
}
