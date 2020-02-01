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
    public class MonsterInfo : MonoBehaviour
    {
        static Int32 Margin = 5;
        static Int32 WindowWidth = 150;
        Int32 WindowId;
        Rect Window = new Rect(Margin + WindowWidth + Margin, Margin, WindowWidth, 50);
        void Awake()
        {
            WindowId = GetHashCode();
        }
        void OnGUI()
        {
            if (Cursor.visible)
            {
                Window = GUILayout.Window(WindowId, Window, WindowMethod, "monsters info", GUILayout.ExpandHeight(true));
            }
        }
        void WindowMethod(Int32 id)
        {
            var monsters = typeof(Temtem.Battle.BattleClient).GetField<Temtem.Battle.BattleClient>().GetField<temtem.networkserialized.NetworkBattleMonster[]>("jhfmqrkmgep");
            var detailMonsters = typeof(Temtem.Battle.BattleClient).GetField<Temtem.Battle.BattleClient>().GetField<Temtem.Monsters.firikfgomje[]>("jrnfkcppiql");

            GUILayout.Label("fighting");
            for (var i = 0; i < monsters.Count(); i++)
            {
                var monster = monsters[i];
                if (monster == null) continue;
                GUILayout.Label(monster.Nickname + " (lvl " + monster.level + ")");
                var detailedInfo = detailMonsters[i];
                GUILayout.Label("sv_hp : " + detailedInfo.GetField<Int16>("hqoqompkoko"));
                GUILayout.Label("sv_stam : " + detailedInfo.GetField<Int16>("lkfqjncqjlh"));
                GUILayout.Label("sv_atk : " + detailedInfo.GetField<Int16>("feefnfjirce"));
                GUILayout.Label("sv_def : " + detailedInfo.GetField<Int16>("kgddimqgcgl"));
                GUILayout.Label("sv_spatk : " + detailedInfo.GetField<Int16>("qmnfcgkfkje"));
                GUILayout.Label("sv_spdef : " + detailedInfo.GetField<Int16>("foqcikgkjfi"));
                GUILayout.Label("sv_speed : " + detailedInfo.GetField<Int16>("ljpogjmlrhd"));
            }
            GUI.DragWindow();
        }
    }
}
