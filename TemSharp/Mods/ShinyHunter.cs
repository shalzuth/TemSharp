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
                    isfsobject.PutShort("sid", 1);
                    isfsobject.PutShort("spid", 0);
                    typeof(Temtem.Network.NetworkLogic).GetField<SmartFox>().Send(new ExtensionRequest("spawnMonster", isfsobject));
                }
            }
            var button = FindObjectsOfType<Temtem.UI.BattleButtonRestSwapBagRunUI>().FirstOrDefault(b => b.name == "Run");
            if (button != null && button.gameObject.activeInHierarchy)
            {
                var monsters = typeof(Temtem.Battle.BattleClient).GetField<Temtem.Battle.BattleClient>().GetField<Temtem.Monsters.firikfgomje[]>();
                foreach (var monster in monsters)
                {
                    if (monster == null) continue;
                    if (monster.hpfkeknnqiq)
                    {
                        enabled = false;
                        return;
                    }
                    //Debug.Log(monster.rjflhcnqnif.OriginalName + " : " + (monster.hpfkeknnqiq ? "Shiny" : "Normal"));
                }

                delayNextBattle = needToEnterBattle = true;
                var clickButton = button.gameObject.GetComponent<UnityEngine.UI.Button>();
                clickButton.OnPointerClick(new PointerEventData(EventSystem.current) { button = PointerEventData.InputButton.Left });
            }
        }
    }
}
