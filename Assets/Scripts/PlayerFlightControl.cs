using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public class PlayerFlightControl : MonoBehaviour
{
	public PlayerFlightControl instance;
	

	//"Objects", "For the main ship Game Object and weapons"));
	public GameObject actual_model; //"Ship GameObject", "Point this to the Game Object that actually contains the mesh for the ship. Generally, this is the first child of the empty container object this controller is placed in."
	public Transform weapon_hardpoint_1; //"Weapon Hardpoint 1", "Transform for the barrel of the weapon"
	public Transform weapon_hardpoint_2; //"Weapon Hardpoint 2", "Transform for the barrel of the weapon"
	public GameObject bullet; //"Projectile GameObject", "Projectile that will be fired from the weapon hardpoint."

	//"Core Movement", "Controls for the various speeds for different operations."
	public float speed = 20.0f; //"Base Speed", "Primary flight speed, without afterburners or brakes"
	public float afterburner_speed = 40f; //Afterburner Speed", "Speed when the button for positive thrust is being held down"
	public float slow_speed = 4f; //"Brake Speed", "Speed when the button for negative thrust is being held down"
	public float thrust_transition_speed = 5f; //Thrust Transition Speed", "How quickly afterburners/brakes will reach their maximum effect"
	public float turnspeed = 15.0f; //"Turn/Roll Speed", "How fast turns and rolls will be executed "
	public float rollSpeedModifier = 7; //"Roll Speed", "Multiplier for roll speed. Base roll is determined by turn speed"
	public float pitchYaw_strength = 0.5f; //"Pitch/Yaw Multiplier", "Controls the intensity of pitch and yaw inputs"


	[HideInInspector]
	public float roll, yaw, pitch; //Inputs for roll, yaw, and pitch, taken from Unity's input system.
	[HideInInspector]
	public bool afterburner_Active = false; //True if afterburners are on.
	[HideInInspector]
	public bool slow_Active = false; //True if brakes are on
	
	float currentMag = 0f; //Current speed/magnitude
	
	bool thrust_exists = true;
	bool roll_exists = true;


	//"Input Actions", New input actions to replace the oldy UnityEngine.Input
	public Controls controls;
	public InputAction fireAction, thrustAction, rollAction, yawAction, pitchAction;
	public float _fire, _thrust, _roll, _pitch, _yaw, deadzone_radius = 0f;
	public Vector2 _look;


	//---------------------------------------------------------------------------------
	private void Awake()
    {
		controls = new Controls();
		fireAction = controls.Game.Fire;
		thrustAction = controls.Game.Thrust;
		rollAction = controls.Game.Roll;
		yawAction = controls.Game.Yaw;
		pitchAction = controls.Game.Pitch;

	}
	void Start()
	{
		instance = this;

		roll = 0; //Setting this equal to 0 here as a failsafe in case the roll axis is not set up.

		//Error handling, in case one of the inputs aren't set up.
		try 
		{
			thrustAction.ReadValue<double>();
		}
		catch 
		{
			thrust_exists = false;
			Debug.LogError("(Flight Controls) Thrust input axis not set up! Go to Edit>Project Settings>Input to create a new axis called 'Thrust' so the ship can change speeds.");
		}
		try 
		{
			//Input.GetAxis("Roll");
			rollAction.ReadValue<double>();
		} 
		catch 
		{
			roll_exists = false;
			Debug.LogError("(Flight Controls) Roll input axis not set up! Go to Edit>Project Settings>Input to create a new axis called 'Roll' so the ship can roll.");
		}
    }

    private void OnEnable()
    {
        controls.Game.Enable();
		controls.Game.Thrust.performed += _ => OnThrust(_);
		controls.Game.Roll.performed += _ => OnRoll(_);
		controls.Game.Roll.canceled += _ => OnRoll(_);
		controls.Game.Fire.performed += _ => OnFire(_);
		controls.Game.Pitch.performed += _ => OnPitch(_);
		controls.Game.Pitch.canceled += _ => OnPitch(_);
		controls.Game.Yaw.performed += _ => OnYaw(_);
		controls.Game.Yaw.canceled += _ => OnYaw(_);

	}

	private void OnDisable()
	{
		controls.Game.Thrust.performed -= _ => OnThrust(_);
		controls.Game.Roll.performed -= _ => OnRoll(_);
		controls.Game.Roll.canceled -= _ => OnRoll(_);
		controls.Game.Fire.performed -= _ => OnFire(_);
		controls.Game.Pitch.performed -= _ => OnPitch(_);
		controls.Game.Pitch.canceled -= _ => OnPitch(_);
		controls.Game.Yaw.performed -= _ => OnYaw(_);
		controls.Game.Yaw.canceled -= _ => OnYaw(_);
		controls.Game.Disable();
	}
    public void OnThrust(InputAction.CallbackContext c)
	{
		_thrust = c.ReadValue<float>();
	}
	public void OnRoll(InputAction.CallbackContext c)
    {
		_roll = c.ReadValue<float>();
    }
	public void OnPitch(InputAction.CallbackContext c)
    {
		_pitch = c.ReadValue<float>();
	}
	public void OnYaw(InputAction.CallbackContext c)
    {
        _yaw = c.ReadValue<float>();
    }
	public void OnFire(InputAction.CallbackContext c)
	{
		Debug.DrawRay(weapon_hardpoint_1.position, weapon_hardpoint_1.forward * 100f, Color.red, 3);
		Debug.DrawRay(weapon_hardpoint_2.position, weapon_hardpoint_1.forward * 100f, Color.red, 3);

		//print("OnFire() - Fire Button was pressed: " + c.ReadValue<float>());
		_fire = c.ReadValue<float>();

		//don't fire if the button isn't pressed.
		if (_fire == 0) return; 

		if (weapon_hardpoint_1 == null || weapon_hardpoint_2 == null)
		{
			Debug.LogError("(FlightControls) Trying to fire weapon, but no hardpoint set up!");
			return;
		}
		if (bullet == null)
		{
			Debug.LogError("(FlightControls) Bullet GameObject is null!");
			return;
		}
		//Shoots it in the direction that the pointer is pointing. Might want to take note of this line for when you upgrade the shooting system.
		if (Camera.main == null)
		{
			Debug.LogError("(FlightControls) Main camera is null! Make sure the flight camera has the tag of MainCamera!");
			return;
		}

		GameObject shot1 = (GameObject)GameObject.Instantiate(bullet, weapon_hardpoint_1.position, Quaternion.identity);
		GameObject shot2 = (GameObject)GameObject.Instantiate(bullet, weapon_hardpoint_2.position, Quaternion.identity);

		Ray vRay1 = new Ray(weapon_hardpoint_1.position, weapon_hardpoint_1.forward);
		Ray vRay2 = new Ray(weapon_hardpoint_2.position, weapon_hardpoint_2.forward);		

		RaycastHit hit;

		//If we make contact with something in the world, we'll make the shot actually go to that point.
		if (Physics.Raycast(vRay1, out hit) || Physics.Raycast(vRay2, out hit))
		{
			shot1.transform.LookAt(hit.point);
			shot2.transform.LookAt(hit.point);
			shot1.GetComponent<Rigidbody>().AddForce(shot1.transform.forward * 9000f);
			shot2.GetComponent<Rigidbody>().AddForce(shot1.transform.forward * 9000f);

			//Otherwise, since the ray didn't hit anything, we're just going to guess and shoot the projectile in the general direction.
		}
		else
		{
			shot1.GetComponent<Rigidbody>().AddForce(vRay1.direction * 9000f);
			shot2.GetComponent<Rigidbody>().AddForce(vRay2.direction * 9000f);
		}
	}

	void FixedUpdate () 
	{
		if (actual_model == null)
		{
            try 
			{ 
				actual_model = gameObject.transform.GetChild(0).gameObject;
			}
			catch
			{
				Debug.LogError("(FlightControls) Ship GameObject is null.");
				return;
			}
		}

		if (!Application.isFocused)
		{
			if(controls.Game.enabled)
				controls.Game.Disable();
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		if (Application.isFocused)
		{
			if (!controls.Game.enabled)
				controls.Game.Enable();
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		if (Keyboard.current.escapeKey.isPressed)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		//pitch = Mathf.Clamp(distFromVertical, -screen_clamp - deadzone_radius, screen_clamp  + deadzone_radius) * pitchYaw_strength;
		pitch = _pitch * pitchYaw_strength;

		//yaw = Mathf.Clamp(distFromHorizontal, -screen_clamp - deadzone_radius, screen_clamp  + deadzone_radius) * pitchYaw_strength;
		yaw = _yaw * pitchYaw_strength;

		if (roll_exists)
			roll = (_roll * -rollSpeedModifier);

		//Getting the current speed.
		currentMag = GetComponent<Rigidbody>().velocity.magnitude;
		
		//If input on the thrust axis is positive, activate afterburners.
		if (thrust_exists) {
			if ( _thrust > 0) {
				afterburner_Active = true;
				slow_Active = false;
				currentMag = Mathf.Lerp(currentMag, afterburner_speed, thrust_transition_speed * Time.fixedDeltaTime);
				
			} else if ( _thrust < 0) { 	//If input on the thrust axis is negatve, activate brakes.
				slow_Active = true;
				afterburner_Active = false;
				currentMag = Mathf.Lerp(currentMag, slow_speed, thrust_transition_speed * Time.fixedDeltaTime);
				
			} else {
				slow_Active = false;
				afterburner_Active = false;
				currentMag = Mathf.Lerp(currentMag, speed, thrust_transition_speed * Time.fixedDeltaTime);
			}
		}
				
		//Apply all these values to the rigidbody on the container.
		GetComponent<Rigidbody>().AddRelativeTorque(
			(pitch * turnspeed * Time.fixedDeltaTime),
			(yaw * turnspeed * Time.fixedDeltaTime),
			(roll * turnspeed *  (rollSpeedModifier / 2) * Time.fixedDeltaTime));
		
		GetComponent<Rigidbody>().velocity = transform.forward * currentMag; //Apply speed
	}

	void Update() 
	{
		Camera.main.gameObject.GetComponentInChildren<Text>().text = "Thrust: " + _thrust.ToString() +
																	"\nRoll: " + _roll.ToString() +
																	"\nPitch: " + _pitch.ToString() +
																	"\nYaw: " + _yaw.ToString();
	}
}