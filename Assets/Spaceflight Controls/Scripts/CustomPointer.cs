using UnityEngine;
using UnityEngine.InputSystem;

public class CustomPointer : MonoBehaviour
{

	[SerializeField]
	public PlayerFlightControl pfc;
	public Texture pointerTexture; //The image for the pointer, generally a crosshair or dot.
	public bool use_mouse_input;
	public bool use_gamepad_input;


	//public bool use_accelerometer_input = false;	//Pointer will be controlled by accelerometer
	public bool pointer_returns_to_center = false; //Pointer will drift to the center of the screen (Use this for joysticks)
	public bool instant_snapping = false; //If the pointer returns to the center, this will make it return to the center instantly when input is idle. Only works for joysticks
	public float center_speed = 5f; //How fast the pointer returns to the center.

	public bool center_lock = false; //Pointer graphic will be locked to the center. Also affects shooting raycast (always shoots to the center of the screen)

	public bool invert_yAxis = false; //Inverts the y axis.


	public float deadzone_radius = 0f; //Deadzone in the center of the screen where the pointer can move without affecting the ship's movement.

	public float thumbstick_speed_modifier = 1f; //Speed multiplier for joysticks.
	public float mouse_sensitivity_modifier = 15f; //Speed multiplier for the mouse.

	public static Vector2 pointerPosition; //Position of the pointer in screen coordinates.

	[HideInInspector]
	public Rect deadzone_rect; //Rect representation of the deadzone.
	//public static CustomPointer instance; //The instance of this class (Should only be one)
	float xAxis, yAxis;
    public CustomPointer instance;


    // Use this for initialization

    void Awake()
	{

		pointerPosition = new Vector2(Screen.width / 2, Screen.height / 2); //Set pointer position to center of screen
		

	}

	void Start()
	{
		instance = this;
		pfc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFlightControl>();


		//Uncomment for Unity 5 to get rid of the warnings.
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		//Screen.lockCursor = true;


		deadzone_rect = new Rect((Screen.width / 2) - (deadzone_radius), (Screen.height / 2) - (deadzone_radius), deadzone_radius * 2, deadzone_radius * 2);

		if (pointerTexture == null)
			Debug.LogWarning("(FlightControls) Warning: No texture set for the custom pointer!");

		if (use_gamepad_input == false && use_mouse_input == false)
			Debug.LogWarning("Select Input System");
	}


	// Update is called once per frame
	void Update()
	{
		if (use_mouse_input)
		{
            try
            {
				print(pfc._look.x);
            }
            catch 
            {
				
            }
			xAxis = pfc._look.x;
			yAxis = pfc._look.y;

			if (invert_yAxis)
				yAxis = -yAxis;




			else if (use_gamepad_input)
			{

				xAxis = pfc._look.x;
				yAxis = pfc._look.y;

				if (invert_yAxis)
				{
					yAxis = -yAxis;
				}
			}

			//Add the input to the pointer's position
			pointerPosition += new Vector2(xAxis * mouse_sensitivity_modifier,
										   yAxis * mouse_sensitivity_modifier);

			try
			{
				if (Gamepad.current.wasUpdatedThisFrame)
				{
					pointerPosition += new Vector2(xAxis * thumbstick_speed_modifier * Mathf.Pow(/*Input.GetAxis("Horizontal")*/ xAxis, 2),
													yAxis * thumbstick_speed_modifier * Mathf.Pow(/*Input.GetAxis("Vertical")*/ yAxis, 2));
				}
			}
			/* else if (use_accelerometer_input) {
			//WARNING: UNTESTED.
			//This /should/ be fairly close to working, though.
			//I would have tested this, but apparently Unity couldn't detect my Windows Phone 8 SDK.
			
			//Even though it's untested, the priciples of control are probably going to be very similar to the above two.
			
			float xAxis = Input.acceleration.x;
			float yAxis = -Input.acceleration.z;
		
			pointerPosition += new Vector2(xAxis * thumbstick_speed_modifier * Mathf.Pow(Input.GetAxis("Horizontal"), 2),
			                               yAxis * thumbstick_speed_modifier * Mathf.Pow(Input.GetAxis("Vertical"), 2));
			*/

			catch
			{

			};



			//If the pointer returns to the center of the screen and it's not in the deadzone...
			if (pointer_returns_to_center && !deadzone_rect.Contains(pointerPosition))
			{
				//If there's no input and instant snapping is on...
				if (/*Input.GetAxis("Horizontal)"*/ pfc._look.x == 0 && /*Input.GetAxis("Vertical")*/ pfc._look.y == 0 && instant_snapping)
				{
					pointerPosition = new Vector2(Screen.width / 2, Screen.height / 2); //Place pointer at the center.


				}
				else
				{
					//Move pointer to the center (Will stop when it hits the deadzone)
					pointerPosition.x = Mathf.Lerp(pointerPosition.x, Screen.width / 2, center_speed * Time.deltaTime);
					pointerPosition.y = Mathf.Lerp(pointerPosition.y, Screen.height / 2, center_speed * Time.deltaTime);
				}
			}

			//Keep the pointer within the bounds of the screen.
			pointerPosition.x = Mathf.Clamp(pointerPosition.x, 0, Screen.width);
			pointerPosition.y = Mathf.Clamp(pointerPosition.y, 0, Screen.height);


		}



	}

    private void OnGUI()
    {
		var test = pfc;
        //Draw the pointer texture.
        if (pointerTexture != null && !center_lock)
            GUI.DrawTexture(new Rect(pointerPosition.x - (pointerTexture.width / 2), Screen.height - pointerPosition.y - (pointerTexture.height / 2), pointerTexture.width, pointerTexture.height), pointerTexture);
        else
        {

            GUI.DrawTexture(new Rect((Screen.width / 2f) - (pointerTexture.width / 2), (Screen.height / 2f) - (pointerTexture.height / 2), pointerTexture.width, pointerTexture.height), pointerTexture);

        }

        //text.text = "\nLook: " + test._look +
        //           "\nRoll: " + test._roll +
        //            "\nThrust:  " + test._thrust +
        //            "\nFire: " + test._fire;
    }
}
