using System;
using UnityEngine;

namespace TemSharp
{
    public class Zoom : MonoBehaviour
    {
        Single MinValue = 2;
        Single MaxValue = 40;
        Single ScalingValue = 7;
        void OnEnable()
        {
            Camera.allCameras[0].GetComponent<BeautifyEffect.Beautify>().enabled = false;
            Camera.allCameras[0].GetComponent<HxVolumetricImageEffectOpaque>().enabled = false;
            Camera.allCameras[0].GetComponent<UnityEngine.Rendering.PostProcessing.PostProcessLayer>().enabled = false;
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
            Camera.allCameras[0].GetComponent<BeautifyEffect.Beautify>().enabled = true;
            Camera.allCameras[0].GetComponent<HxVolumetricImageEffectOpaque>().enabled = true;
            Camera.allCameras[0].GetComponent<UnityEngine.Rendering.PostProcessing.PostProcessLayer>().enabled = true;
            Menu.ScalingOptions.Remove(GetType());
        }
    }
}