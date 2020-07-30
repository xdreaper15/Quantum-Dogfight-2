using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEditor;
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
    public GameObject _bullet;

    private void Awake()
    {
        _bullet = pfc.bullet;
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

    //Fire a projectile from Hardopints 1 and 2
    public void Fire(Transform hp1= null, Transform hp2= null, GameObject bullet= null)
    {

        if((hp1 != null) && (hp1.transform.localPosition.x != -hp2.transform.localPosition.x))
        {
            print("(hp1 != null) && (hp1.transform.localPosition.x != -hp2.transform.localPosition.x)");
            hp1 = hp2;
            hp1.transform.localPosition = new Vector3(-hp2.transform.localPosition.x, hp1.transform.localPosition.y, hp1.transform.localPosition.z);
        }
        else if((hp2 != null) && (hp2.transform.localPosition.x != -hp1.transform.localPosition.x))
        {
            print("(hp2 != null) && (hp2.transform.localPosition.x != -hp1.transform.localPosition.x)");
            hp2 = hp1;
            hp2.transform.localPosition = new Vector3(-hp1.transform.localPosition.x, hp2.transform.localPosition.y, hp2.transform.localPosition.z);
        }
        else if(hp1 == null && hp2 == null)
        {
            print("hp1 == null && hp2 == null");
            hp1 = pfc.weapon_hardpoint_1;
            hp2 = pfc.weapon_hardpoint_2;
        }
        else if(bullet == null && _bullet == null)
        {
            print("bullet == null && _bullet == null");
            bullet = pfc.bullet;
        }
        else if(bullet == null && (_bullet != null))
        {
            print("bullet == null && (_bullet != null)");
            bullet = _bullet;
        }

        Debug.Log("JetDamage.Fire(): " + NetworkManager.Instance.Networker.Me.NetworkId.ToString());

        GameObject shot1 = (GameObject)GameObject.Instantiate(bullet, hp1.position, Quaternion.identity);
        GameObject shot2 = (GameObject)GameObject.Instantiate(bullet, hp2.position, Quaternion.identity);

        Ray vRay1 = new Ray(hp1.position, hp1.forward);
        Ray vRay2 = new Ray(hp2.position, hp2.forward);

        RaycastHit hit;

        //If we make contact with something in the world, we'll make the shot actually go to that point.
        if (Physics.Raycast(vRay1, out hit) || Physics.Raycast(vRay2, out hit))
        {
            shot1.transform.LookAt(hit.point);
            shot2.transform.LookAt(hit.point);
            shot1.GetComponent<Rigidbody>().AddForce(shot1.transform.forward * 9000f);
            shot2.GetComponent<Rigidbody>().AddForce(shot1.transform.forward * 9000f);
        }
        else //Otherwise, since the ray didn't hit anything, we're just going to guess and shoot the projectile in the general direction.
        {
            shot1.GetComponent<Rigidbody>().AddForce(vRay1.direction * 9000f);
            shot2.GetComponent<Rigidbody>().AddForce(vRay2.direction * 9000f);
        }
    }
}
