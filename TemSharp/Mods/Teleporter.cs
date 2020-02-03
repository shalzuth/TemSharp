using System;
using System.Linq;
using UnityEngine;
using Temtem.UI;
using Temtem.Players;
namespace TemSharp
{
    public class Teleporter : MonoBehaviour
    {
        void Update()
        {
            var position = Temtem.Players.LocalPlayerAvatar.nkqrjhelndm.qqhqkomhdoq;
            if (Input.GetMouseButtonDown(1))
            {
                var obj = FindObjectsOfType<InGameMapUI>().FirstOrDefault();
                if (obj != null && obj.transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    var mapCenterOffset = -obj.GetField<Vector2>("hqkopqprqfl") / 10;
                    var mapZoom = obj.GetField<Single>("mjmcenrnfli");
                    var newX = (Input.mousePosition.x - Screen.width / 2) / 10 / mapZoom;
                    var newY = (Input.mousePosition.y - Screen.height / 2) / 10 / mapZoom;
                    var newTarget = new Vector3(position.x + newX + mapCenterOffset.x, position.y + 100, position.z + newY + mapCenterOffset.y);
                    UnityEngine.AI.NavMeshHit navHit;
                    UnityEngine.AI.NavMesh.SamplePosition(newTarget, out navHit, 200, UnityEngine.AI.NavMesh.AllAreas);
                    //Temtem.Network.NetworkLogic.nkqrjhelndm.elennjqknrp(Temtem.Network.NetworkLogic.nkqrjhelndm.npqcecmqpio, navHit.position);
                    typeof(LocalPlayerAvatar).GetField<LocalPlayerAvatar>().gameObject.GetComponent<PlayerMovement>().cimqnlfephf(navHit.position);
                }
                else
                {
                    var minimap = (MonoBehaviour)FindObjectsOfType<MinimapFogController>().FirstOrDefault();
                    if (minimap == null) minimap = (MonoBehaviour)FindObjectsOfType<GenericMinimap>().FirstOrDefault();
                    if (minimap != null && minimap.gameObject.activeInHierarchy)
                    {
                        var pos = Input.mousePosition;
                        pos.z = typeof(Temtem.Configuration.VisualSettings).GetField<Single>("cameraDistance");
                        var playerPlane = new Plane(Vector3.up, position);
                        var ray = Camera.allCameras[0].ScreenPointToRay(pos);
                        var hitdist = 0.0f;
                        if (playerPlane.Raycast(ray, out hitdist))
                        {
                            var clickPosition = ray.GetPoint(hitdist);
                            UnityEngine.AI.NavMeshHit navHit;
                            UnityEngine.AI.NavMesh.SamplePosition(clickPosition, out navHit, 200, UnityEngine.AI.NavMesh.AllAreas);
                            //Temtem.Network.NetworkLogic.nkqrjhelndm.elennjqknrp(Temtem.Network.NetworkLogic.nkqrjhelndm.npqcecmqpio, navHit.position);
                            typeof(LocalPlayerAvatar).GetField<LocalPlayerAvatar>().gameObject.GetComponent<PlayerMovement>().cimqnlfephf(navHit.position);
                        }
                    }
                }
            }
            var teleportDistance = 1;
            if (Input.GetKeyDown(KeyCode.Keypad4))
                typeof(LocalPlayerAvatar).GetField<LocalPlayerAvatar>().gameObject.GetComponent<PlayerMovement>().cimqnlfephf(new Vector3(position.x - teleportDistance, position.y, position.z));
            if (Input.GetKeyDown(KeyCode.Keypad6))
                typeof(LocalPlayerAvatar).GetField<LocalPlayerAvatar>().gameObject.GetComponent<PlayerMovement>().cimqnlfephf(new Vector3(position.x + teleportDistance, position.y, position.z));
            if (Input.GetKeyDown(KeyCode.Keypad8))
                typeof(LocalPlayerAvatar).GetField<LocalPlayerAvatar>().gameObject.GetComponent<PlayerMovement>().cimqnlfephf(new Vector3(position.x, position.y, position.z + teleportDistance));
            if (Input.GetKeyDown(KeyCode.Keypad2))
                typeof(LocalPlayerAvatar).GetField<LocalPlayerAvatar>().gameObject.GetComponent<PlayerMovement>().cimqnlfephf(new Vector3(position.x, position.y, position.z - teleportDistance));
            if (Input.GetKeyDown(KeyCode.Keypad7))
                typeof(LocalPlayerAvatar).GetField<LocalPlayerAvatar>().gameObject.GetComponent<PlayerMovement>().cimqnlfephf(new Vector3(position.x, position.y + teleportDistance, position.z));
            if (Input.GetKeyDown(KeyCode.Keypad1))
                typeof(LocalPlayerAvatar).GetField<LocalPlayerAvatar>().gameObject.GetComponent<PlayerMovement>().cimqnlfephf(new Vector3(position.x, position.y - teleportDistance, position.z));
        }
    }
}