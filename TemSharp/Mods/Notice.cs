using System;
using UnityEngine;
namespace TemSharp
{
    public class Notice : MonoBehaviour
    {
        Int32 Width = 200;
        Int32 Height = 200;
        Int32 WindowId;
        Rect WindowRect;
        GUIStyle labelStyle;
        void Awake()
        {
            Width = Screen.width * 3 / 4;
            Height = Screen.height * 3 / 4;
            WindowRect = new Rect(Screen.width / 2 - Width / 2, Screen.height / 2 - Height / 2, Width, Height);
            WindowId = GetHashCode();
            labelStyle = new GUIStyle("label");
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.fontSize = 18;
            if (Environment.UserName == "shalzuth") enabled = false;
        }
        void OnGUI()
        {
            GUI.color = Color.black;
            WindowRect = GUILayout.Window(WindowId, WindowRect, WindowMethod, "shalzuth's news");
        }

        void WindowMethod(Int32 id)
        {
            GUILayout.Label("Tem" + "Sharp by shalzuth", labelStyle);
            GUILayout.Label("THIS IS FREE AND OPEN SOURCE SOFTWARE", labelStyle);
            GUILayout.Label("If you paid for this mod, you got scammed", labelStyle);
            GUILayout.Label("If you downloaded this from anywhere but https://github.com/shalzuth/Tem" + "Sharp/releases/download/beta/Tem" + "Sharp.Loader.exe, you probably have a virus.", labelStyle);
            GUILayout.Label("Disabled hacks due to ban wave. Monster Info and Zoom only now.", labelStyle);
            if (GUILayout.Button("Close Notice"))
                enabled = false;
            if (GUILayout.Button("GitHub Page"))
                Application.OpenURL("https://github.com/shalzuth/Tem" + "Sharp");
            if (GUILayout.Button("Discord Server (for learning and community, not support)"))
                Application.OpenURL("https://discord.gg/799qSNR");
            GUI.DragWindow();
        }
    }
}
