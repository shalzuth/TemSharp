using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace TemSharp
{
    public class SceneDebugger : MonoBehaviour
    {
        Int32 Margin = 5;
        Int32 HierarchyWindowId;
        Int32 ProjectWindowId { get { return HierarchyWindowId + 1; } }
        Int32 InspectorWindowId { get { return ProjectWindowId + 1; } }

        Rect HierarchyWindow;
        Int32 HierarchyWidth = 400;
        Vector2 HierarchyScrollPos;
        String SearchText = "";
        Vector2 PropertiesScrollPos;
        Transform SelectedGameObject;
        HashSet<object> ExpandedObjs = new HashSet<object>();

        Rect ProjectWindow;
        Int32 ProjectWidth = 400;
        Vector2 ProjectScrollPos;
        ConcurrentDictionary<object, Boolean> ExpandedObjects = new ConcurrentDictionary<object, Boolean>();

        Rect InspectorWindow;
        Int32 InspectorWidth = 350;
        void Start()
        {
            HierarchyWindowId = GetHashCode();

            HierarchyWindow = new Rect(Screen.width - HierarchyWidth - Margin, Margin, HierarchyWidth, Screen.height - Margin * 2);
            ProjectWindow = new Rect(HierarchyWindow.x - Margin - ProjectWidth, Margin, ProjectWidth, Screen.height - Margin * 2);
            InspectorWindow = new Rect(ProjectWindow.x - Margin - InspectorWidth, Margin, InspectorWidth, Screen.height - Margin * 2);
        }
        void LateUpdate()
        {
            Cursor.visible = true;
        }
        void OnGUI()
        {
            HierarchyWindow = GUILayout.Window(HierarchyWindowId, HierarchyWindow, HierarchyWindowMethod, "Hierarchy");
            ProjectWindow = GUILayout.Window(ProjectWindowId, ProjectWindow, ProjectWindowMethod, "Project");
        }
        #region Hierarchy GUI
        void DisplayGameObject(GameObject gameObj, Int32 level)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(level * 20);
                var color = GUI.color;
                if (SelectedGameObject == gameObj.transform)
                    GUI.color = Color.green;
                if (!gameObj.activeSelf && gameObj.transform.childCount == 0)
                    GUI.color = Color.magenta;
                else if (gameObj.transform.childCount == 0)
                    GUI.color = Color.yellow;
                else if (!gameObj.activeSelf)
                    GUI.color = Color.red;
                if (GUILayout.Toggle(ExpandedObjs.Contains(gameObj), gameObj.name, GUILayout.ExpandWidth(false)))
                {
                    if (!ExpandedObjs.Contains(gameObj))
                    {
                        ExpandedObjs.Add(gameObj);
                        SelectedGameObject = gameObj.transform;
                    }
                }
                else
                {
                    if (ExpandedObjs.Contains(gameObj))
                    {
                        ExpandedObjs.Remove(gameObj);
                        SelectedGameObject = gameObj.transform;
                    }
                }
                GUI.color = color;
            }
            GUILayout.EndHorizontal();
            if (ExpandedObjs.Contains(gameObj))
                for (var i = 0; i < gameObj.transform.childCount; ++i)
                    DisplayGameObject(gameObj.transform.GetChild(i).gameObject, level + 1);
        }
        void HierarchyWindowMethod(Int32 id)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            {
                GUILayout.BeginHorizontal();
                {
                    SearchText = GUILayout.TextField(SearchText, GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("Search", GUILayout.ExpandWidth(false)))
                    { }
                }
                GUILayout.EndHorizontal();
                var rootObjects = new List<GameObject>();
                foreach (Transform xform in GameObject.FindObjectsOfType<Transform>())
                    if (xform.parent == null)
                        rootObjects.Add(xform.gameObject);
                //var rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
                if (SelectedGameObject == null)
                    SelectedGameObject = rootObjects.First().transform;
                HierarchyScrollPos = GUILayout.BeginScrollView(HierarchyScrollPos, GUILayout.Height(HierarchyWindow.height / 3), GUILayout.ExpandWidth(true));
                {
                    foreach (var rootObject in rootObjects)
                        DisplayGameObject(rootObject, 0);
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical(GUI.skin.box);
            {
                PropertiesScrollPos = GUILayout.BeginScrollView(PropertiesScrollPos, GUI.skin.box);
                {
                    var fullName = SelectedGameObject.name;
                    var parentTransform = SelectedGameObject.parent;
                    while (parentTransform != null)
                    {
                        fullName = parentTransform.name + "/" + fullName;
                        parentTransform = parentTransform.parent;
                    }
                    GUILayout.Label(fullName);
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label(SelectedGameObject.gameObject.layer + " : " + LayerMask.LayerToName(SelectedGameObject.gameObject.layer));
                        GUILayout.FlexibleSpace();
                        SelectedGameObject.gameObject.SetActive(GUILayout.Toggle(SelectedGameObject.gameObject.activeSelf, "Active", GUILayout.ExpandWidth(false)));
                        if (GUILayout.Button("?"))
                            Console.WriteLine("?");
                        if (GUILayout.Button("X"))
                            Destroy(SelectedGameObject.gameObject);
                    }
                    GUILayout.EndHorizontal();
                    foreach (var component in SelectedGameObject.GetComponents<Component>())
                    {
                        GUILayout.BeginHorizontal(GUI.skin.box);
                        {

                            if (component is Behaviour)
                                (component as Behaviour).enabled = GUILayout.Toggle((component as Behaviour).enabled, "", GUILayout.ExpandWidth(false));

                            GUILayout.Label(component.GetType().Name + " : " + component.GetType().Namespace);
                            GUILayout.FlexibleSpace();
                            if (GUILayout.Button("?"))
                                Console.WriteLine("?");
                            if (!(component is Transform))
                                if (GUILayout.Button("X"))
                                    Destroy(component);
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
            GUI.DragWindow();
        }
        #endregion
        #region Project GUI
        void ProjectWindowMethod(Int32 id)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            {
                ProjectScrollPos = GUILayout.BeginScrollView(ProjectScrollPos, GUILayout.Height(ProjectWindow.height / 3), GUILayout.ExpandWidth(true));
                {
                    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    foreach (var assembly in assemblies)
                    {
                        ExpandedObjects[assembly] = GUILayout.Toggle(ExpandedObjects.ContainsKey(assembly) ? ExpandedObjects[assembly] : false, assembly.GetName().Name, GUILayout.ExpandWidth(false));
                        if (ExpandedObjects[assembly])
                        {
                            var types = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && !t.ContainsGenericParameters).ToList();
                            foreach (var type in types)
                            {
                                var staticfields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy).Count(f => f.Name != "OffsetOfInstanceIDInCPlusPlusObject");
                                if (staticfields == 0)
                                    continue;
                                GUILayout.BeginHorizontal();
                                {
                                    var color = GUI.color;
                                    GUILayout.Space(20);
                                    ExpandedObjects[type] = GUILayout.Toggle(ExpandedObjects.ContainsKey(type) ? ExpandedObjects[type] : false, type.Name, GUILayout.ExpandWidth(false));
                                    GUI.color = color;
                                }
                                GUILayout.EndHorizontal();
                                if (ExpandedObjects[type])
                                {
                                    var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                                    foreach (var field in fields)
                                    {
                                        if (field.Name == "OffsetOfInstanceIDInCPlusPlusObject") continue;
                                        //var val = field.GetValue(null);
                                        GUILayout.BeginHorizontal();
                                        {
                                            GUILayout.Space(40);
                                            ExpandedObjects[field] = GUILayout.Toggle(ExpandedObjects.ContainsKey(field) ? ExpandedObjects[field] : false, field.Name + " : " + field.FieldType, GUI.skin.label, GUILayout.ExpandWidth(false));
                                        }
                                        GUILayout.EndHorizontal();
                                    }
                                }
                            }
                        }
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
            GUI.DragWindow();
        }
        #endregion
    }
}