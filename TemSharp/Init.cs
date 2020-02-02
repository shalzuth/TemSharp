using System.Reflection;
using System.Linq;
namespace TemSharp
{
    public class Init
    {
        public static UnityEngine.GameObject BaseObject;
        public static void DisableAnticheat()
        {
            UnityEngine.Object.DestroyImmediate(UnityEngine.Object.FindObjectOfType<CodeStage.AntiCheat.Detectors.ActDetectorBase>());
            UnityEngine.Object.DestroyImmediate(UnityEngine.Object.FindObjectOfType<CodeStage.AntiCheat.Detectors.InjectionDetector>());
            UnityEngine.Object.DestroyImmediate(UnityEngine.Object.FindObjectOfType<CodeStage.AntiCheat.Detectors.ObscuredCheatingDetector>());
            UnityEngine.Object.DestroyImmediate(UnityEngine.Object.FindObjectOfType<CodeStage.AntiCheat.Detectors.SpeedHackDetector>());
            UnityEngine.Object.DestroyImmediate(UnityEngine.Object.FindObjectOfType<CodeStage.AntiCheat.Detectors.TimeCheatingDetector>());
        }
        public static void Load()
        {
            DisableAnticheat();
            while (BaseObject = UnityEngine.GameObject.Find("TemSharp"))
                UnityEngine.Object.Destroy(BaseObject);
            BaseObject = new UnityEngine.GameObject("TemSharp");
            UnityEngine.Object.DontDestroyOnLoad(BaseObject);
            BaseObject.SetActive(false);
            var types = Assembly.GetExecutingAssembly().GetTypes().ToList().Where(t => t.BaseType == typeof(UnityEngine.MonoBehaviour) && !t.IsNested);
            foreach (var type in types)
            {
                var component = (UnityEngine.MonoBehaviour)BaseObject.AddComponent(type);
                component.enabled = false;
            }
            BaseObject.GetComponent<Notice>().enabled = true;
            BaseObject.GetComponent<Menu>().enabled = true;
            BaseObject.SetActive(true);
        }

        public static void Unload()
        {
            UnityEngine.Object.Destroy(BaseObject);
        }
    }
}