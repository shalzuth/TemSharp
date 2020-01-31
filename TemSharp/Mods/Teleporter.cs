using UnityEngine;

namespace TemSharp
{
    public class Teleporter : MonoBehaviour
    {
        void Update()
        {
            var position = Temtem.Players.LocalPlayerAvatar.nkqrjhelndm.ckchddpfgip.position;
            if (Input.GetKeyDown(KeyCode.Keypad4))
                Temtem.Network.NetworkLogic.nkqrjhelndm.elennjqknrp(Temtem.Network.NetworkLogic.nkqrjhelndm.npqcecmqpio, new Vector3(position.x - 5, position.y, position.z));
            if (Input.GetKeyDown(KeyCode.Keypad6))
                Temtem.Network.NetworkLogic.nkqrjhelndm.elennjqknrp(Temtem.Network.NetworkLogic.nkqrjhelndm.npqcecmqpio, new Vector3(position.x + 5, position.y, position.z));
            if (Input.GetKeyDown(KeyCode.Keypad8))
                Temtem.Network.NetworkLogic.nkqrjhelndm.elennjqknrp(Temtem.Network.NetworkLogic.nkqrjhelndm.npqcecmqpio, new Vector3(position.x, position.y, position.z + 5));
            if (Input.GetKeyDown(KeyCode.Keypad2))
                Temtem.Network.NetworkLogic.nkqrjhelndm.elennjqknrp(Temtem.Network.NetworkLogic.nkqrjhelndm.npqcecmqpio, new Vector3(position.x, position.y, position.z - 5));
        }
    }
}