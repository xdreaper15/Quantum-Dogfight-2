using BeardedManStudios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;
using UnityEngine.InputSystem.Controls;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    public Camera cam;
    public Controls controls;
    public Rigidbody rb;
    public GameObject go;
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    private void Awake()
    {
        Debug.Log("Awake");

        

        controls = new Controls();

    }


    void Start()
    {
        Debug.Log("Start");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable\n\n");

        controls.Game.Enable();
        controls.Game.Look.performed += context => OnLook(context);
    }
    private void OnDisable()
    {
        Debug.Log("OnDisable\n\n");

        controls.Game.Disable();
        controls.Game.Look.performed -= context => OnLook(context);
    }


    // Update is called once per frame
    void Update()
    {
        if(controls.Game.Look.triggered)
        {
            Debug.Log("Look Triggered");
        }

        //text.text = "\nLook: " + controls.Game.Look.ReadValue<Vector2>().ToString() +
        //            "\nRoll: " + controls.Game.Roll.ReadValue<float>() +
        //            "\nThrust:  " + controls.Game.Thrust.ReadValue<float>() +
        //            "\nFire: " + controls.Game.Fire.ReadValue<float>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        print("Look: " + (context.ReadValue<Vector2>()*Time.deltaTime).ToString());
    }
}
