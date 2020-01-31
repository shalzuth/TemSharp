using UnityEngine;
namespace TemSharp
{
    public class Credits : MonoBehaviour
    {
        void OnEnable()
        {
            Application.OpenURL("https://github.com/shalzuth/Tem" + "Sharp");
        }
    }
}
