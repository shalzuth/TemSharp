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
            GUILayout.Label("", labelStyle);
            GUILayout.Label("NOTE - there's a chance your account will be banned for cheating. Use with caution.", labelStyle);
            GUILayout.Label("People have been banned for trading on flagged accounts", labelStyle);
            GUILayout.Label("", labelStyle);
            GUILayout.Label("Patch Notes");
            GUILayout.Label("Better teleporter method. Can use in dungeons. ~less~ likely to get flagged/banned");
            GUILayout.Label("", labelStyle);
            GUILayout.Label("Old Patch Notes");
            GUILayout.Label("Added shiny hunter configuration");
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
    }
}
