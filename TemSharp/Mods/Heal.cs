using UnityEngine;
using Sfs2X;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
namespace TemSharp
{
    public class Heal : MonoBehaviour
    {
        void OnEnable()
        {
            typeof(Temtem.Network.NetworkLogic).GetField<SmartFox>().Send(new ExtensionRequest("gameplay.HealTeam", new SFSObject()));
        }
    }
}
