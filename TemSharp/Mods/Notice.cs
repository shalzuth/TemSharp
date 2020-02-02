using Sfs2X;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
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
        GUIStyle windowStyle;
        void Awake()
        {
            Width = Screen.width * 3 / 4;
            Height = Screen.height * 3 / 4;
            WindowRect = new Rect(Screen.width / 2 - Width / 2, Screen.height / 2 - Height / 2, Width, Height);
            WindowId = GetHashCode();
            labelStyle = new GUIStyle("label");
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.fontSize = 18;
            windowStyle = new GUIStyle("window");
            //GUI.skin.label.alignment = TextAnchor.UpperCenter;
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
            GUILayout.Label("THIS IS FREE AND OPEN SOURCE SOFTWARE", labelStyle);
            GUILayout.Label("THIS IS FREE AND OPEN SOURCE SOFTWARE", labelStyle);
            GUILayout.Label("THIS IS FREE AND OPEN SOURCE SOFTWARE", labelStyle);
            GUILayout.Label("If you paid for this mod, you got scammed", labelStyle);
            GUILayout.Label("If you downloaded this from anywhere but https://github.com/shalzuth/Tem" + "Sharp/releases/download/beta/Tem" + "Sharp.Loader.exe, you probably have a virus.", labelStyle);
            GUILayout.Label("If you downloaded this from anywhere but https://github.com/shalzuth/Tem" + "Sharp/releases/download/beta/Tem" + "Sharp.Loader.exe, you probably have a virus.", labelStyle);
            GUILayout.Label("If you downloaded this from anywhere but https://github.com/shalzuth/Tem" + "Sharp/releases/download/beta/Tem" + "Sharp.Loader.exe, you probably have a virus.", labelStyle);
            GUILayout.Label("If you downloaded this from anywhere but https://github.com/shalzuth/Tem" + "Sharp/releases/download/beta/Tem" + "Sharp.Loader.exe, you probably have a virus.", labelStyle);
            if (GUILayout.Button("Close Notice"))
                enabled = false;
            GUILayout.Label("I'm adding a notice, so I guess I'll add patch notes");
            GUILayout.Label("Patch Notes");
            GUILayout.Label("Teleporter - added right click to teleport in game while running around");
            GUILayout.Label("Zoom - removes beautifier effects so it doesn't get blurry");
            GUILayout.Label("Shiny Hunter - more stability, can disable gfx now, will select benched Tems when one dies");
            GUILayout.Label("Tem" + "Sharp - added this notice because people are selling this and packing it with viruses");
            if (GUILayout.Button("GitHub Page"))
                Application.OpenURL("https://github.com/shalzuth/Tem" + "Sharp");
            if (GUILayout.Button("Discord Server (for learning and community, not support)"))
                Application.OpenURL("https://discord.gg/799qSNR");
            GUI.DragWindow();
        }
        void OnEnable()
        {
            //Application.OpenURL("https://github.com/shalzuth/Tem" + "Sharp");
        }
    }
}
