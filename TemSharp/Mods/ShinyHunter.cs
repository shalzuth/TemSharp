using System;
using System.Collections;
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
        Boolean needToEnterBattle = true;
        Boolean needToCloseLevelUpDelay = true;
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
        Boolean luma = true;
        Single sv_hp = 0;
        Single sv_stam = 0;
        Single sv_atk = 0;
        Single sv_def = 0;
        Single sv_spatk = 0;
        Single sv_spdef = 0;
        Single sv_speed = 0;
        void ShinyWindowMethod(Int32 id)
        {
            fight = GUILayout.Toggle(fight, fight ? "Fight" : "Flee");
            luma = GUILayout.Toggle(luma, "Luma required");
            GUILayout.Label("sv minimums to stop");
            GUILayout.BeginHorizontal();
            GUILayout.Label("hp");
            sv_hp = GUILayout.HorizontalScrollbar(sv_hp, 1, 0, 50);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("stam");
            sv_stam = GUILayout.HorizontalScrollbar(sv_stam, 1, 0, 50);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("atk");
            sv_atk = GUILayout.HorizontalScrollbar(sv_atk, 1, 0, 50);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("def");
            sv_def = GUILayout.HorizontalScrollbar(sv_def, 1, 0, 50);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("spatk");
            sv_spatk = GUILayout.HorizontalScrollbar(sv_spatk, 1, 0, 50);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("spdef");
            sv_spdef = GUILayout.HorizontalScrollbar(sv_spdef, 1, 0, 50);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("speed");
            sv_speed = GUILayout.HorizontalScrollbar(sv_speed, 1, 0, 50);
            GUILayout.EndHorizontal();
            GUI.DragWindow();
        }
        IEnumerator HealAndSpawnMonster(Single seconds)
        {
            yield return new WaitForSeconds(seconds * Time.timeScale);
            typeof(Temtem.Network.NetworkLogic).GetField<SmartFox>().Send(new ExtensionRequest("gameplay.HealTeam", new SFSObject()));
            var isfsobject = new SFSObject();
            var zone = GetSpawnZone();
            isfsobject.PutShort("sid", zone.GetField<Int16>("sceneId"));
            isfsobject.PutShort("spid", zone.GetField<Int16>("id"));
            typeof(Temtem.Network.NetworkLogic).GetField<SmartFox>().Send(new ExtensionRequest("spawnMonster", isfsobject));
        }
        IEnumerator CloseReport(Single seconds)
        {
            yield return new WaitForSeconds(seconds * Time.timeScale);
            typeof(SystemReportUI).GetField<SystemReportUI>().Invoke("qminholrkqm");
        }
        void Update()
        {
            var minimap = FindObjectsOfType<MinimapFogController>().FirstOrDefault();
            if (minimap != null && minimap.gameObject.activeInHierarchy)
            {
                if (needToEnterBattle)
                {
                    needToEnterBattle = false;
                    StartCoroutine(HealAndSpawnMonster(0.5f));
                }
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
                {
                    UIButton button;
                    if (fight) button = FindObjectsOfType<BattleButtonTechUI>().FirstOrDefault(b => b.name == "BattleTechniqueButton");
                    else button = FindObjectsOfType<BattleButtonRestSwapBagRunUI>().FirstOrDefault(b => b.name == "Run");
                    if (button != null && button.gameObject.activeInHierarchy)
                    {
                        var detailMonsters = typeof(Temtem.Battle.BattleClient).GetField<Temtem.Battle.BattleClient>().GetField<Temtem.Monsters.firikfgomje[]>("jrnfkcppiql");

                        var monsters = typeof(Temtem.Battle.BattleClient).GetField<Temtem.Battle.BattleClient>().GetField<temtem.networkserialized.NetworkBattleMonster[]>("jhfmqrkmgep");
                        for (var i = 0; i < monsters.Count(); i++)
                        {
                            var monster = monsters[i];
                            if (monster == null) continue;
                            var detailedInfo = detailMonsters[i];
                            if (monster.luma)
                            {

                                if (monster.luma == luma
                                    && sv_hp >= (Single)detailedInfo.GetField<Int16>("hqoqompkoko")
                                    && sv_stam >= (Single)detailedInfo.GetField<Int16>("lkfqjncqjlh")
                                    && sv_atk >= (Single)detailedInfo.GetField<Int16>("feefnfjirce")
                                    && sv_def >= (Single)detailedInfo.GetField<Int16>("kgddimqgcgl")
                                    && sv_spatk >= (Single)detailedInfo.GetField<Int16>("qmnfcgkfkje")
                                    && sv_spdef >= (Single)detailedInfo.GetField<Int16>("foqcikgkjfi")
                                    && sv_speed >= (Single)detailedInfo.GetField<Int16>("ljpogjmlrhd"))
                                {
                                    enabled = false;
                                    return;
                                }
                            }
                            //Debug.Log(monster.rjflhcnqnif.OriginalName + " : " + (monster.hpfkeknnqiq ? "Shiny" : "Normal"));
                            //Debug.Log(monster.Nickname + " : " +monster.ToString());
                        }
                        needToCloseLevelUpDelay = needToEnterBattle = true;
                        button.gameObject.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
                    }

                    nameplate.gameObject.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
                }
            }
            var report = typeof(SystemReportUI).GetField<SystemReportUI>().GetField<GameObject>("acceptGO");
            if (report.activeInHierarchy && (nameplate == null || !nameplate.gameObject.activeInHierarchy))
            {
                if (needToCloseLevelUpDelay)
                {
                    needToCloseLevelUpDelay = false;
                    StartCoroutine(CloseReport(0.5f));
                }
                    
            }
        }
    }
}
