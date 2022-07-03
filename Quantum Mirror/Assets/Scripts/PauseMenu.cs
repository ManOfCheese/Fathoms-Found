using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{

	public int menuScene;
	public int gameScene;
	public BoolValue paused;
	public GameObject pauseMenu;
	public Animator pauseMenuAnimator;

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	public void OnPause( InputAction.CallbackContext value )
	{
		if ( value.performed )
		{
			paused.Value = !paused.Value;
			OnPause( paused.Value );
		}
	}

	public void OnPause( bool pause )
	{
		if ( pause != paused.Value )
			paused.Value = pause;
		if ( pause )
		{
			pauseMenuAnimator.SetBool( "Open", true );
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		else
		{
			pauseMenuAnimator.SetBool( "Open", false );
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	public void ExitToDesktop()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		SceneManager.LoadSceneAsync( menuScene, LoadSceneMode.Single );
	}

	public void Restart()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		SceneManager.LoadSceneAsync( gameScene, LoadSceneMode.Single );
	}

}
