using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Temtem.Core;
using Temtem.UI;
using Temtem.World;
using Sfs2X;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
namespace TemSharp
{
    public class ShinyHunter : MonoBehaviour
    {
        Int32 tick = 0;
        Boolean delayNextBattle = true;
        Boolean needToEnterBattle = true;
        SpawnZoneDefinition GetSpawnZone()
        {
            var spawnZoneDefList = typeof(WildMonstersLogic).GetField<WildMonstersLogic>().GetField<HashSet<SpawnZoneDefinition>>();
            var vector = Temtem.Players.LocalPlayerAvatar.nkqrjhelndm.qqhqkomhdoq;
            foreach (var spawnZoneDef in spawnZoneDefList)
            {
                if (spawnZoneDef.hdmighhrhpl(vector))
                    return spawnZoneDef;
            }
            return null;
        }
        static Int32 Margin = 5;
        static Int32 WindowWidth = 150;
        Int32 ShinyId;
        Rect ShinyWindow = new Rect(Margin + WindowWidth + Margin, Margin, WindowWidth, 50);
        void Awake()
        {
            ShinyId = GetHashCode();
        }
        void OnGUI()
        {
            if (Cursor.visible)
            {
                ShinyWindow = GUILayout.Window(ShinyId, ShinyWindow, ShinyWindowMethod, "shiny hunter settings", GUILayout.ExpandHeight(true));
            }
        }
        Boolean fight = true;
        void ShinyWindowMethod(Int32 id)
        {
            fight = GUILayout.Toggle(fight, fight ? "Fight" : "Flee");
            GUI.DragWindow();
        }
        void Update()
        {
            var minimap = FindObjectsOfType<Temtem.UI.MinimapFogController>().FirstOrDefault();
            if (minimap != null && minimap.gameObject.activeInHierarchy)
            {
                if (delayNextBattle)
                {
                    delayNextBattle = false;
                    tick = Environment.TickCount;
                }
                if (needToEnterBattle && Environment.TickCount - tick > 1000)
                {
                    typeof(Temtem.Network.NetworkLogic).GetField<SmartFox>().Send(new ExtensionRequest("gameplay.HealTeam", new SFSObject()));
                    needToEnterBattle = false;
                    var isfsobject = new SFSObject();
                    var zone = GetSpawnZone();
                    isfsobject.PutShort("sid", zone.GetField<Int16>("sceneId"));
                    isfsobject.PutShort("spid", zone.GetField<Int16>("id"));
                    typeof(Temtem.Network.NetworkLogic).GetField<SmartFox>().Send(new ExtensionRequest("spawnMonster", isfsobject));
                }
            }
            UIButton button;
            if (fight) button = FindObjectsOfType<BattleButtonTechUI>().FirstOrDefault(b => b.name == "BattleTechniqueButton");
            else button = FindObjectsOfType<BattleButtonRestSwapBagRunUI>().FirstOrDefault(b => b.name == "Run");
            if (button != null && button.gameObject.activeInHierarchy)
            {
                var monsters = typeof(Temtem.Battle.BattleClient).GetField<Temtem.Battle.BattleClient>().GetField<temtem.networkserialized.NetworkBattleMonster[]>("jhfmqrkmgep");
                foreach (var monster in monsters)
                {
                    if (monster == null) continue;
                    if (monster.luma)
                    {
                        enabled = false;
                        return;
                    }
                    //Debug.Log(monster.rjflhcnqnif.OriginalName + " : " + (monster.hpfkeknnqiq ? "Shiny" : "Normal"));
                    //Debug.Log(monster.Nickname + " : " +monster.ToString());
                }

                delayNextBattle = needToEnterBattle = true;
                button.gameObject.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
            }
            var nameplate = FindObjectsOfType<BattleButtonStatsUI>().FirstOrDefault();
            if (nameplate != null && nameplate.gameObject.activeInHierarchy)
            {
                var fullName = nameplate.transform.name;
                var parentTransform = nameplate.gameObject.transform;
                while (parentTransform != null)
                {
                    fullName = parentTransform.name + "/" + fullName;
                    parentTransform = parentTransform.parent;
                }
                if (fullName.Contains("Rival"))
                    nameplate.gameObject.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
            }
            var report = typeof(SystemReportUI).GetField<SystemReportUI>().GetField<GameObject>("acceptGO");
            if (report.activeInHierarchy)
            {
                typeof(SystemReportUI).GetField<SystemReportUI>().Invoke("qminholrkqm");
            }
        }
    }
}
