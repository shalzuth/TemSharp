using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Temtem.Core;
using Temtem.Monsters;
using Temtem.UI;
using Temtem.World;
using Sfs2X;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
namespace TemSharp
{
    public class ShinyHunter : MonoBehaviour
    {
        Int32 tick = Environment.TickCount;
        Boolean needToEnterBattle = true;
        Boolean needToCloseLevelUpDelay = true;
        Boolean needToSelectBench = true;
        UInt64 BattleCount = 0;
        Dictionary<Int16, MonsterStats> TemtemDict;
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
        static Int32 WindowWidth = 300;
        Int32 ShinyId;
        Rect ShinyWindow;
        Rect ConfigWindow;
        void Awake()
        {
            TemtemDict = typeof(ConfigReader).GetField<ConfigReader>().GetField<Tempedia>().GetField<Dictionary<Int16, MonsterStats>>("temtemDict");
               ShinyWindow = new Rect(Margin + 150 + Margin, Margin, WindowWidth, 50);
            ShinyId = GetHashCode();
        }
        void OnEnable()
        {
            needToEnterBattle = needToCloseLevelUpDelay = true;
        }
        void OnGUI()
        {
            if (Cursor.visible)
                ShinyWindow = GUILayout.Window(ShinyId, ShinyWindow, ShinyWindowMethod, "shiny hunter settings", GUILayout.ExpandHeight(true));
            if (addingNewConfig)
                ConfigWindow = GUILayout.Window(ShinyId + 1, ConfigWindow, ConfigWindowMethod, "configuration", GUILayout.ExpandHeight(true));
        }
        Boolean addingNewConfig = false;
        List<Criteria> CriteriaList = new List<Criteria>();
        void ShinyWindowMethod(Int32 id)
        {
            GUILayout.Label("battle # " + BattleCount);
            GUILayout.Label("time avg : " + ((Single)(Environment.TickCount - tick) / 1000 / (Single)BattleCount));
            fight = GUILayout.Toggle(fight, fight ? "Fight" : "Flee");
            disableGfx = GUILayout.Toggle(disableGfx, "Disable gfx");
            var removedAny = false;
            foreach(var c in CriteriaList)
            {
                GUILayout.BeginHorizontal();
                var name = "Any";
                if (TemtemDict.ContainsKey(c.monster_number)) name = TemtemDict[c.monster_number].OriginalName;
                GUILayout.Label(name + (c.luma ? " Luma " : " Normal ") + c.sv_hp + " " + c.sv_stam + " " + c.sv_atk
                    + " " + c.sv_def + " " + c.sv_spatk + " " + c.sv_spdef + " " + c.sv_speed);
                if (GUILayout.Button("X"))
                {
                    removedAny = true;
                    c.remove = true;
                }
                GUILayout.EndHorizontal();
            }
            if (removedAny)
            {
                CriteriaList.RemoveAll(r => r.remove);
                ShinyWindow = new Rect(ShinyWindow.x, ShinyWindow.y, WindowWidth, 10);
            }
            if (GUILayout.Button("Add New Criteria"))
            {
                ConfigWindow = new Rect(ShinyWindow.x + ShinyWindow.width + Margin, ShinyWindow.y, WindowWidth, 50);
                addingNewConfig = true;
            }
            if (GUILayout.Button(botting ? "Stop" : "Start")) botting = !botting;
            GUI.DragWindow();
        }
        Boolean fight = true;
        Boolean disableGfx = false;
        Boolean botting = false;
        String monster_name = "";
        Int16 monster_number = 0;
        Boolean luma = true;
        Int32 sv_hp = 0;
        Int32 sv_stam = 0;
        Int32 sv_atk = 0;
        Int32 sv_def = 0;
        Int32 sv_spatk = 0;
        Int32 sv_spdef = 0;
        Int32 sv_speed = 0;
        class Criteria
        {
            public Int16 monster_number;
            public Boolean luma;
            public Int32 sv_hp = 0;
            public Int32 sv_stam = 0;
            public Int32 sv_atk = 0;
            public Int32 sv_def = 0;
            public Int32 sv_spatk = 0;
            public Int32 sv_spdef = 0;
            public Int32 sv_speed = 0;
            public String Passive;
            public Boolean remove = false;
        }
        void ConfigWindowMethod(Int32 id)
        {
            monster_name = "Any";
            if (TemtemDict.ContainsKey(monster_number)) monster_name = TemtemDict[monster_number].OriginalName;
            GUILayout.BeginHorizontal();
            GUILayout.Label("monster name : " + monster_name);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("monster number");
            monster_number = Int16.Parse(GUILayout.TextField(monster_number.ToString()));
            GUILayout.EndHorizontal();
            luma = GUILayout.Toggle(luma, "Luma");
            GUILayout.BeginHorizontal();
            GUILayout.Label("hp");
            sv_hp = Int32.Parse(GUILayout.TextField(sv_hp.ToString()));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("stam");
            sv_stam = Int32.Parse(GUILayout.TextField(sv_stam.ToString()));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("atk");
            sv_atk = Int32.Parse(GUILayout.TextField(sv_atk.ToString()));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("def");
            sv_def = Int32.Parse(GUILayout.TextField(sv_def.ToString()));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("spatk");
            sv_spatk = Int32.Parse(GUILayout.TextField(sv_spatk.ToString()));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("spdef");
            sv_spdef = Int32.Parse(GUILayout.TextField(sv_spdef.ToString()));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("speed");
            sv_speed = Int32.Parse(GUILayout.TextField(sv_speed.ToString()));
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Add Criteria"))
            {
                addingNewConfig = false;
                if (monster_number == 0 || TemtemDict.ContainsKey(monster_number))
                    CriteriaList.Add(new Criteria
                    {
                        monster_number = monster_number,
                        luma = luma,
                        sv_hp = sv_hp,
                        sv_stam = sv_stam,
                        sv_atk = sv_atk,
                        sv_def = sv_def,
                        sv_spatk = sv_spatk,
                        sv_spdef = sv_spdef,
                        sv_speed = sv_speed,
                        Passive = "todo"
                    });
            }
            GUI.DragWindow();
        }
        IEnumerator HealAndSpawnMonster(Single seconds)
        {
            if (!needToEnterBattle)
                yield break;
            needToEnterBattle = false;
            yield return new WaitForSeconds(seconds * Time.timeScale);
            typeof(Temtem.Network.NetworkLogic).GetField<SmartFox>().Send(new ExtensionRequest("gameplay.HealTeam", new SFSObject()));
            var isfsobject = new SFSObject();
            var zone = GetSpawnZone();

            if (Temtem.Network.NetworkLogic.nkqrjhelndm.chdmkelfjpg != null)
            {
                isfsobject.PutShort("sid", Temtem.Network.NetworkLogic.nkqrjhelndm.chdmkelfjpg.glgirecgqig());
                isfsobject.PutBool("isB", false);
            }
            else
                isfsobject.PutShort("sid", zone.GetField<Int16>("sceneId"));
            isfsobject.PutShort("spid", zone.GetField<Int16>("id"));
            if (zone is SaiparkSpawnZoneDefinition)
                isfsobject.PutBool("sp", false);
            typeof(Temtem.Network.NetworkLogic).GetField<SmartFox>().Send(new ExtensionRequest("spawnMonster", isfsobject));
            BattleCount++;
            yield return new WaitForSeconds(2.0f * Time.timeScale);
            needToEnterBattle = true;
        }
        IEnumerator CloseReport(Single seconds)
        {
            if (!needToCloseLevelUpDelay)
                yield break;
            needToCloseLevelUpDelay = false;
            yield return new WaitForSeconds(seconds * Time.timeScale);
            typeof(SystemReportUI).GetField<SystemReportUI>().Invoke("qminholrkqm");
            yield return new WaitForSeconds(2.0f * Time.timeScale);
            needToCloseLevelUpDelay = true;
        }
        IEnumerator Reconnect(PopupButton button, Single seconds)
        {
            yield return new WaitForSeconds(seconds * Time.timeScale);
            button.gameObject.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
        }
        IEnumerator SelectBenchTem(BattleSquadUI battleSquad, Single seconds)
        {
            if (!needToSelectBench)
                yield break;
            needToSelectBench = false;
            yield return new WaitForSeconds(seconds * Time.timeScale);
            var slots = battleSquad.GetComponentsInChildren<SquadSlotUI>();
            foreach (var slot in slots)
            {
                if (!slot.gameObject.activeInHierarchy)
                    continue;
                slot.OnHover();
                slot.gameObject.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
                yield return new WaitForSeconds(seconds * Time.timeScale);
                if (!battleSquad.transform.GetChild(0).gameObject.activeInHierarchy)
                    break;
            }
            yield return new WaitForSeconds(2.0f * Time.timeScale);
            needToSelectBench = true;
        }
        void Update()
        {
            GfxToggle();
            if (!botting)
                return;
            var minimap = (MonoBehaviour)FindObjectsOfType<MinimapFogController>().FirstOrDefault();
            if (minimap == null) minimap = (MonoBehaviour)FindObjectsOfType<GenericMinimap>().FirstOrDefault();
            if (minimap != null && minimap.gameObject.activeInHierarchy)
                StartCoroutine(HealAndSpawnMonster(0.5f));
            var nameplate = FindObjectsOfType<BattleButtonStatsUI>().LastOrDefault();
            if (nameplate != null && nameplate.gameObject.activeInHierarchy)
                nameplate = FindObjectsOfType<BattleButtonStatsUI>().FirstOrDefault();
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
                            foreach(var c in CriteriaList)
                            {
                                if (c.monster_number == 0 || c.monster_number == monster.monsterNumber)
                                    if (monster.luma == c.luma
                                        && c.sv_hp <= (Single)detailedInfo.GetField<Int16>("hqoqompkoko")
                                        && c.sv_stam <= (Single)detailedInfo.GetField<Int16>("lkfqjncqjlh")
                                        && c.sv_atk <= (Single)detailedInfo.GetField<Int16>("feefnfjirce")
                                        && c.sv_def <= (Single)detailedInfo.GetField<Int16>("kgddimqgcgl")
                                        && c.sv_spatk <= (Single)detailedInfo.GetField<Int16>("qmnfcgkfkje")
                                        && c.sv_spdef <= (Single)detailedInfo.GetField<Int16>("foqcikgkjfi")
                                        && c.sv_speed <= (Single)detailedInfo.GetField<Int16>("ljpogjmlrhd"))
                                    {
                                        botting = false;
                                        disableGfx = false;
                                        GfxToggle();
                                        return;
                                    }
                            }
                            //Debug.Log(monster.rjflhcnqnif.OriginalName + " : " + (monster.hpfkeknnqiq ? "Shiny" : "Normal"));
                            //Debug.Log(monster.Nickname + " : " +monster.ToString());
                        }
                        button.gameObject.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
                    }

                    nameplate.gameObject.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
                }
            }
            var battleSquad = typeof(BattleSquadUI).GetField<BattleSquadUI>();
            if (battleSquad != null && battleSquad.transform.GetChild(0).gameObject.activeInHierarchy)
                StartCoroutine(SelectBenchTem(battleSquad, 0.5f));
            var report = typeof(SystemReportUI).GetField<SystemReportUI>().GetField<GameObject>("acceptGO");
            if (report != null && report.activeInHierarchy && (nameplate == null || !nameplate.gameObject.activeInHierarchy))
                StartCoroutine(CloseReport(0.5f));
            var reconnect = typeof(CommonPopupUI).GetField<CommonPopupUI>().GetField<PopupButton>("choiceA");
            if (reconnect != null && reconnect.gameObject.activeInHierarchy && (nameplate == null || !nameplate.gameObject.activeInHierarchy))
            {
                if (reconnect.GetField<TMPro.TextMeshProUGUI>("text").text == "Reconnect")
                {
                    StartCoroutine(Reconnect(reconnect, 0.5f));
                }                    
            }
        }
        void GfxToggle()
        {
            // buggy need to clean
            //var worldCam = typeof(Temtem.MonstersCamera.WorldCamera).GetField<Temtem.MonstersCamera.WorldCamera>();
            //if (worldCam != null && worldCam.gameObject != null)
            //    worldCam.gameObject.SetActive(!disableGfx);
            /* foreach(var pool in PathologicalGames.PoolManager.Pools)
                pool.Value.gameObject.SetActive(!disableGfx);*/
            var toHide = FindObjectsOfType<Temtem.Utils.BattleZoneGameObjectToHide>();
            foreach (var hide in toHide)
                hide.gameObject.SetActive(!disableGfx);
            var battleCam = typeof(Temtem.MonstersCamera.BattleCamera).GetField<Temtem.MonstersCamera.BattleCamera>();
            if (battleCam != null && battleCam.gameObject != null)
                battleCam.gameObject.SetActive(!disableGfx);
        }
        void OnDisable()
        {
            disableGfx = false;
            GfxToggle();
        }
    }
}
