using System;
using System.Linq;
using UnityEngine;
using Sfs2X;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
namespace TemSharp
{
    public class Heal : MonoBehaviour
    {
        void OnEnable()
        {
            typeof(Temtem.Network.NetworkLogic).GetField<SmartFox>().Send(new ExtensionRequest("gameplay.HealTeam", new SFSObject()));
        }
        Boolean needHeal = false;
        Boolean delayAfterBattle = true;
        Int32 tick = 0;
        void Update()
        {
            var minimap = FindObjectsOfType<Temtem.UI.MinimapFogController>().FirstOrDefault();
            if (minimap != null && minimap.gameObject.activeInHierarchy)
            {
                if (delayAfterBattle)
                {
                    delayAfterBattle = false;
                    tick = Environment.TickCount;
                }
                if (needHeal && Environment.TickCount - tick > 500)
                {
                    typeof(Temtem.Network.NetworkLogic).GetField<SmartFox>().Send(new ExtensionRequest("gameplay.HealTeam", new SFSObject()));
                    needHeal = false;
                }
            }
            var button = FindObjectsOfType<Temtem.UI.BattleButtonRestSwapBagRunUI>().FirstOrDefault(b => b.name == "Run");
            if (button != null && button.gameObject.activeInHierarchy)
                delayAfterBattle = needHeal = true;
        }
    }
}
