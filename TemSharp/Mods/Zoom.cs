using System;
using UnityEngine;

namespace TemSharp
{
    public class Zoom : MonoBehaviour
    {
        Single MinValue = 7;
        Single MaxValue = 20;
        Single ScalingValue = 7;
        void OnEnable()
        {
            Menu.ScalingOptions.Add(GetType());
        }
        void Update()
        {
            // fix minimap zooming...
            //var minimap = Temtem.UI.InGameMenuUI.nkqrjhelndm.GetField<Transform>("fcmdqepnjgl").GetComponent<Temtem.UI.GenericMinimap>();
            //RectTransform component = Temtem.UI.InGameMenuUI.nkqrjhelndm.GetField<Transform>("fcmdqepnjgl").GetComponent<RectTransform>();
            //component.anchoredPosition = mreneodkfed;
            //component.localEulerAngles = new Vector3(781f, 2f, 0f);
            //component.localEulerAngles = new Vector3(0, 0f, 0f);
            //component.anchoredPosition = vector;
            //component.sizeDelta = new Vector2(100, 100);
            typeof(Temtem.Configuration.VisualSettings).SetField("cameraDistance", ScalingValue);
        }
        void OnDisable()
        {
            typeof(Temtem.Configuration.VisualSettings).SetField("cameraDistance", 7);
            Menu.ScalingOptions.Remove(GetType());
        }
    }
}