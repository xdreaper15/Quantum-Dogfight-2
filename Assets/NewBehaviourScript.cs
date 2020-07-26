using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewBehaviourScript : MonoBehaviour
{
    public Camera camera;
    public Controls controls;
    public Rigidbody rb;
    public GameObject go;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    private void Awake()
    {
        controls = new Controls();
    }
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        camera = Camera.main.gameObject.GetComponent<Camera>();
        go = this.gameObject;
        text = camera.gameObject.GetComponent<TextMeshProUGUI>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnEnable() => controls.Game.Enable();
    private void OnDisable() => controls.Game.Disable();
    // Update is called once per frame
    void Update()
    {
        text.text = "Position: " + go.transform.position +
                    "\nSpeed: " + rb.velocity.magnitude +
                    "\nLook:  " + Mouse.current.delta.ReadValue() + " / " + controls.Game.Look.ReadValue<float>() +
                    "\nRoll: " + controls.Game.Roll.ReadValue<float>() +
                    "\nThrust:  " + Keyboard.current.wKey.isPressed + " / " + controls.Game.Thrust.ReadValue<float>() +
                    "\nFire: " + controls.Game.Fire.phase.ToString();
    }

    public void OnThrust()
    {

    }
    public void OnRoll()
    {

    }
    public void OnLook()
    {

    }
    public void OnFire()
    {

    }
}
