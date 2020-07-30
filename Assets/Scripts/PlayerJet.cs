using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
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
    public JetDamage jetDamage;

    public int currentHealth, currentAmmo;
    public bool reloading;

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
            cam.GetComponent<AudioListener>().enabled = false;
        }
    }
    private void Update()
    {
        
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

    /// <summary>
	/// Used to move the cube that this script is attached to up
	/// </summary>
	/// <param name="args">null</param>
    public override void Fire(RpcArgs args)
    {
        // RPC calls are not made from the main thread for performance, since we
        // are interacting with Unity engine objects, we will need to make sure
        // to run the logic on the main thread
        MainThreadManager.Run(() =>
        {
            //we need to make the Jet Fire() here;
            jetDamage.Fire();
        });
    }
}
