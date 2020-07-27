using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DemoUI : MonoBehaviour {

	bool cursorlock = true;
	public PlayerFlightControl pfc;
	public CustomPointer cp;

	// Use this for initialization
	void Start () 
	{
		cp = this.gameObject.GetComponent<CustomPointer>();
		pfc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFlightControl>();
	
	}
	
	// Update is called once per frame
	void LateUpdate () {

		/*
		//Uncomment for Unity 5 to get rid of the warnings.
		if (cursorlock)
			Cursor.lockState = CursorLockMode.Locked;
		else
			Cursor.lockState = CursorLockMode.None;
		*/

		//Delete this statement for Unity 5.
		if (cursorlock)
		{
			//Screen.lockCursor = true;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else
		{
			//Screen.lockCursor = false;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		
		if (/*Input.GetKeyDown(KeyCode.Escape)*/ Keyboard.current.escapeKey.isPressed)
			cursorlock = !cursorlock;
		
		if (/*Input.GetKeyDown(KeyCode.C)*/ Keyboard.current.cKey.isPressed) {
			cp.pointer_returns_to_center =  !cp.pointer_returns_to_center;
			
		}
		
		if (/*Input.GetKeyDown(KeyCode.L)*/ Keyboard.current.lKey.isPressed) {
			cp.center_lock =  !cp.center_lock;
			
		}		
		
		if (/*Application.loadedLevel != 3*/ SceneManager.GetActiveScene().buildIndex != 3) {
			if (/*Input.GetKeyDown(KeyCode.Equals*/ Keyboard.current.equalsKey.isPressed) {
				CameraFlightFollow.instance.follow_distance++;
			}
			
			if (/*Input.GetKeyDown\(KeyCode\.*/ Keyboard.current.minusKey.isPressed) {
				CameraFlightFollow.instance.follow_distance--;
			}
			
			if (/*Input.GetKeyDown\(KeyCode\.*/ Keyboard.current.commaKey.isPressed) {
				CameraFlightFollow.instance.follow_tightness--;
			}
			if (/*Input.GetKeyDown\(KeyCode\.*/ Keyboard.current.periodKey.isPressed) {
				CameraFlightFollow.instance.follow_tightness++;
			}
		}
		
		if (/*Input.GetKeyDown\(KeyCode\.*/ Keyboard.current.digit1Key.isPressed) {
			/*Application.LoadLevel(0)*/ SceneManager.LoadScene(0) ;
		}
		if (/*Input.GetKeyDown\(KeyCode\.*/ Keyboard.current.digit2Key.isPressed) {
			/*Application.LoadLevel(1)*/ SceneManager.LoadScene(1) ;
		}
		if (/*Input.GetKeyDown\(KeyCode\.*/ Keyboard.current.digit3Key.isPressed) {
			/*Application.LoadLevel(2);*/ SceneManager.LoadScene(2);
		}
		if (/*Input.GetKeyDown\(KeyCode\.*/ Keyboard.current.digit4Key.isPressed) {
			/*Application.LoadLevel(3);*/ SceneManager.LoadScene(3);
		}
	}
	
	

	void OnGUI() {
	
		if (/*Application.loadedLevel != 3*/ SceneManager.GetActiveScene().buildIndex != 3)
		GUI.Label(new Rect(10,10, 650,250), "Controls: W/S for thrust, A/D for roll, mouse for pitch/yaw." +
			          "\n-/+ to increase or decrease camera follow distance. </> to increase or decrease follow tightness.\nC to enable or disable pointer centering.\nL to toggle center lock\n1-4 to change scenes\nESC to unlock cursor");
		
		else
		GUI.Label(new Rect(10,10, 650,250), "Controls: W/S for thrust, A/D for roll, mouse for pitch/yaw." +
			          "\nC to toggle pointer centering.\nL to toggle center lock\n1-4 to change scenes\nESC to unlock cursor");

		print("wrote GUI values");
		GUI.Label(new Rect(10, 150, 650, 250), "fireAction: " + pfc.fireVal +
											  "\nrollAction: " + pfc.rollVal +
											  "\nlookAction: " + pfc.lookVal +
											  "\nthrustAction: " + pfc.thrustVal);

		
	
	}
	

	
}
