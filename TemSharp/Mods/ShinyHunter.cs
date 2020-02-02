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
        Boolean needToSelectBench = true;
        UInt64 BattleCount = 0;
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
        void OnEnable()
        {
            tick = Environment.TickCount;
            needToEnterBattle = needToCloseLevelUpDelay = true;
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
        Boolean disableGfx = false;
        Single sv_hp = 0;
        Single sv_stam = 0;
        Single sv_atk = 0;
        Single sv_def = 0;
        Single sv_spatk = 0;
        Single sv_spdef = 0;
        Single sv_speed = 0;
        void ShinyWindowMethod(Int32 id)
        {
            GUILayout.Label("battle # " + BattleCount);
            GUILayout.Label("time avg : " + ((Single)(Environment.TickCount - tick) / 1000 / (Single)BattleCount));
            GUILayout.Label("needToEnterBattle " + needToEnterBattle);
            GUILayout.Label("needToCloseLevelUpDelay " + needToCloseLevelUpDelay);
            GUILayout.Label("needToSelectBench " + needToCloseLevelUpDelay);
            fight = GUILayout.Toggle(fight, fight ? "Fight" : "Flee");
            luma = GUILayout.Toggle(luma, "Luma required");
            disableGfx = GUILayout.Toggle(disableGfx, "Disable gfx");
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
                            if (monster.luma == luma
                                && sv_hp <= (Single)detailedInfo.GetField<Int16>("hqoqompkoko")
                                && sv_stam <= (Single)detailedInfo.GetField<Int16>("lkfqjncqjlh")
                                && sv_atk <= (Single)detailedInfo.GetField<Int16>("feefnfjirce")
                                && sv_def <= (Single)detailedInfo.GetField<Int16>("kgddimqgcgl")
                                && sv_spatk <= (Single)detailedInfo.GetField<Int16>("qmnfcgkfkje")
                                && sv_spdef <= (Single)detailedInfo.GetField<Int16>("foqcikgkjfi")
                                && sv_speed <= (Single)detailedInfo.GetField<Int16>("ljpogjmlrhd"))
                            {
                                enabled = false;
                                return;
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
