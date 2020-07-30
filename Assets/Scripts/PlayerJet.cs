using BeardedManStudios.Forge.Networking.Generated;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJet : PlayerJetBehavior
{
    public PlayerFlightControl pfc;
    public PlayerInput pi;
    public InputControl controls;
    public CinemachineFreeLook c_FL;
    public GameObject cam;
    public PlayerJet Instance;
    public Rigidbody rb;

    private void Awake()
    {
        Debug.Log("PlayerJet Awake!");
        if (Instance = null)
            Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!networkObject.IsOwner)
        {
            pfc.controls.Disable();
            pfc.enabled = false;
            pi.enabled = false;
            c_FL.enabled = false;
            cam.SetActive(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!networkObject.IsOwner)
        {
            transform.position = networkObject.position;
            transform.rotation = networkObject.rotation;
        }
        if(networkObject.IsOwner)
        {
            networkObject.position = transform.position;
            networkObject.rotation = transform.rotation;
        }
    }
}
