using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Temtem.Core;
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
                //Debug.Log(spawnZoneDef.GetField<Int16>("id"));
                if (spawnZoneDef.hdmighhrhpl(vector))
                {
                    return spawnZoneDef;
                }
            }
            return null;
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
           // var button = FindObjectsOfType<Temtem.UI.BattleButtonTechUI>().FirstOrDefault(b => b.name == "BattleTechniqueButton");
            var button = FindObjectsOfType<Temtem.UI.BattleButtonRestSwapBagRunUI>().FirstOrDefault(b => b.name == "Run");
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
                var clickButton = button.gameObject.GetComponent<UnityEngine.UI.Button>();
                clickButton.OnPointerClick(new PointerEventData(EventSystem.current) { button = PointerEventData.InputButton.Left });
            }
        }
    }
}
