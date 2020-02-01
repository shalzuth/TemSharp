﻿using System;
using System.Linq;
using UnityEngine;
namespace TemSharp
{
    public class Teleporter : MonoBehaviour
    {
        void Update()
        {
            var position = Temtem.Players.LocalPlayerAvatar.nkqrjhelndm.qqhqkomhdoq;
            if (Input.GetMouseButtonDown(1))
            {
                var obj = FindObjectsOfType<Temtem.UI.InGameMapUI>().FirstOrDefault();
                if (obj != null && obj.transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    var mapCenterOffset = -obj.GetField<Vector2>("hqkopqprqfl") / 10;
                    var mapZoom = obj.GetField<Single>("mjmcenrnfli");
                    var newX = (Input.mousePosition.x - Screen.width / 2) / 10 / mapZoom;
                    var newY = (Input.mousePosition.y - Screen.height / 2) / 10 / mapZoom;
                    var newTarget = new Vector3(position.x + newX + mapCenterOffset.x, position.y + 100, position.z + newY + mapCenterOffset.y);
                    UnityEngine.AI.NavMeshHit navHit;
                    UnityEngine.AI.NavMesh.SamplePosition(newTarget, out navHit, 200, UnityEngine.AI.NavMesh.AllAreas);
                    Temtem.Network.NetworkLogic.nkqrjhelndm.elennjqknrp(Temtem.Network.NetworkLogic.nkqrjhelndm.npqcecmqpio, navHit.position);
                }
            }
            var teleportDistance = 5;
            if (Input.GetKeyDown(KeyCode.Keypad4))
                Temtem.Network.NetworkLogic.nkqrjhelndm.elennjqknrp(Temtem.Network.NetworkLogic.nkqrjhelndm.npqcecmqpio, new Vector3(position.x - teleportDistance, position.y, position.z));
            if (Input.GetKeyDown(KeyCode.Keypad6))
                Temtem.Network.NetworkLogic.nkqrjhelndm.elennjqknrp(Temtem.Network.NetworkLogic.nkqrjhelndm.npqcecmqpio, new Vector3(position.x + teleportDistance, position.y, position.z));
            if (Input.GetKeyDown(KeyCode.Keypad8))
                Temtem.Network.NetworkLogic.nkqrjhelndm.elennjqknrp(Temtem.Network.NetworkLogic.nkqrjhelndm.npqcecmqpio, new Vector3(position.x, position.y, position.z + teleportDistance));
            if (Input.GetKeyDown(KeyCode.Keypad2))
                Temtem.Network.NetworkLogic.nkqrjhelndm.elennjqknrp(Temtem.Network.NetworkLogic.nkqrjhelndm.npqcecmqpio, new Vector3(position.x, position.y, position.z - teleportDistance));
            if (Input.GetKeyDown(KeyCode.Keypad7))
                Temtem.Network.NetworkLogic.nkqrjhelndm.elennjqknrp(Temtem.Network.NetworkLogic.nkqrjhelndm.npqcecmqpio, new Vector3(position.x, position.y + teleportDistance, position.z));
            if (Input.GetKeyDown(KeyCode.Keypad1))
                Temtem.Network.NetworkLogic.nkqrjhelndm.elennjqknrp(Temtem.Network.NetworkLogic.nkqrjhelndm.npqcecmqpio, new Vector3(position.x, position.y - teleportDistance, position.z));
        }
    }
}