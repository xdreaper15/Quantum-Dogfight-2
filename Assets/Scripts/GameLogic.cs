using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameLogic : MonoBehaviour
{
    public NetworkManager nm;
    public float camSens;
    public static readonly Dictionary<uint, NetworkObject> _playerObjects = new Dictionary<uint, NetworkObject>();

    private void Awake()
    {
        Debug.Log("GL.Awake");

        Application.targetFrameRate = 60;

        //if (NetworkManager.Instance == null)
        //{
        //    nm = this.gameObject.GetComponent<NetworkManager>();
        //    Debug.Log("nm = this...");
        //}
    }
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
            _playerObjects.Add(obj.NetworkId, obj);
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
        }
    }

    private void Update()
    {
        Debug.Log("GL - Update");

    }
}
