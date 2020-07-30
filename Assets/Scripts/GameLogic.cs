using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public MainThreadManager main;
    public NetworkManager nm;
    public Camera cam;
    public float camSens;
    public static readonly Dictionary<uint, NetworkObject> _playerObjects = new Dictionary<uint, NetworkObject>();
    public static readonly Dictionary<uint, NetworkingPlayer> _players = new Dictionary<uint, NetworkingPlayer>();

    private void Awake()
    {
        Debug.Log("GL.Awake");
        nm = NetworkManager.Instance;
        Application.targetFrameRate = 60;


        // Start is called before the first frame update
        void Start()
        {
            if (nm.IsServer)
            {
                Debug.Log("NM Start - IS server");
            }
            if (!nm.IsServer)
            {
                gameObject.GetComponent<Camera>().enabled = false;

                Debug.Log("NM Start - NOT SERVER\nSpawning PlayerJet...");

                var obj = nm.InstantiatePlayerJet(0).networkObject;
                Debug.Log("Network Object (PlayerJet) Created for N(id)" + obj.Owner.NetworkId + "\n\tAdded it to the list...");
            }
        }
        // Update is called once per frame
        void FixedUpdate()
        {
            Debug.Log("GL - FixedUpdate");

            if (nm.IsServer)
            {
                var kb = Keyboard.current;
                if (kb.wKey.isPressed)
                    transform.position += transform.forward * Time.fixedDeltaTime * camSens;
                if (kb.sKey.isPressed)
                    transform.position -= transform.forward * Time.fixedDeltaTime * camSens;
                if (kb.aKey.isPressed)
                    transform.position -= transform.right * Time.fixedDeltaTime * camSens;
                if (kb.dKey.isPressed)
                    transform.position += transform.right * Time.fixedDeltaTime * camSens;
                if (kb.qKey.isPressed)
                    transform.position += transform.up * Time.fixedDeltaTime * camSens;
                if (kb.eKey.isPressed)
                    transform.position -= transform.up * Time.fixedDeltaTime * camSens;
                while (Mouse.current.rightButton.isPressed)
                {
                    gameObject.GetComponent<Camera>().transform.Rotate(new Vector3(
                                                                        Mathf.Clamp(Mouse.current.delta.ReadValue().x, -1f, 1f) * camSens,
                                                                        Mathf.Clamp(Mouse.current.delta.ReadValue().y, -1f, 1f) * camSens,
                                                                        0f), Space.Self);
                }
            }
        }

        void Update()
        {

            NetworkManager.Instance.Networker.playerDisconnected += (player, sender) =>
            {
                MainThreadManager.Run(() =>
                {
                    Debug.Log(player.NetworkId + " <= DISCONNECTED ~~~ DESTROYING => " + _playerObjects[player.NetworkId]);
                    _playerObjects[player.NetworkId].Destroy();
                    _players.Remove(player.NetworkId);
                }, MainThreadManager.UpdateType.Update);
            };

            NetworkManager.Instance.Networker.playerConnected += (player, sender) =>
            {
                MainThreadManager.Run(() =>
                {
                    Debug.Log(player.NetworkId + " <= CONNECTED ~~~ ADDINGTO DICT.");
                    _players.Add(player.NetworkId, player);
                });
            };

            NetworkManager.Instance.Networker.objectCreated += networkObject =>
            {
                MainThreadManager.Run(() =>
                {
                    Debug.Log(networkObject.ToString() + " <= CREATED FOR: " + networkObject.Owner.NetworkId + " ~~~ ADDING TO DICT.");
                    _playerObjects.Add(networkObject.Owner.NetworkId, networkObject);

                });
            };

        }
    }
}
