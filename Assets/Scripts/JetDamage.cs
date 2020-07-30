using BeardedManStudios.Forge.Networking.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetDamage : MonoBehaviour
{
    public Rigidbody rb;
    public Transform tr;
    public PlayerFlightControl pfc;
    public SimpleHealthBar healthBar;
    public static int maxHealth = 100, maxAmmo = 200, damage = 1, reloadTime = 10;
    public int currentHealth, currentAmmo;
    public bool reloading;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        healthBar = new SimpleHealthBar();

    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
