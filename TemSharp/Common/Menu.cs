using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace TemSharp
{
    public class Menu : MonoBehaviour
    {
        static Int32 Margin = 5;
        static Int32 MenuWidth = 150;
        Int32 MenuId;
        Rect MenuWindow = new Rect(Margin, Margin, MenuWidth, 50);
        void Awake()
        {
            MenuId = GetHashCode();
        }
        void OnGUI()
        {
            if (Cursor.visible)
            {
#if DEBUG
                MenuWindow = GUILayout.Window(MenuId, MenuWindow, MenuMethod, "shalzuth : " + GetType().Namespace, GUILayout.ExpandHeight(true));
#else
                MenuWindow = GUILayout.Window(MenuId, MenuWindow, MenuMethod, "temsharp by shalzuth " + Assembly.GetExecutingAssembly().GetName().Version, GUILayout.ExpandHeight(true));
#endif
            }
        }
        public static List<Type> ScalingOptions = new List<Type>();
        void MenuMethod(Int32 id)
        {
            foreach (var mono in Init.BaseObject.GetComponents<MonoBehaviour>())
            {
                if (mono.GetType() == GetType()) continue;
                mono.enabled = GUILayout.Toggle(mono.enabled, mono.GetType().Name);
                if (ScalingOptions.Contains(mono.GetType()))
                    mono.SetField("ScalingValue", GUILayout.HorizontalScrollbar(mono.GetField<Single>("ScalingValue"), 1.0f, mono.GetField<Single>("MinValue"), mono.GetField<Single>("MaxValue")));
            }
            var unload = GUILayout.Toggle(false, "Unload");
            if (unload) Destroy(Init.BaseObject);
            GUI.DragWindow();
        }
    }
}