using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	void Update()
	{
		if(Input.GetButtonDown("Quit")) {
			Application.Quit();
		}
	}

	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void Quit()
	{
        AudioManager.instance.PlaySound("Quit");
        Application.Quit();
	}

	public void GoToMainMenu()
	{
        AudioManager.instance.PlaySound("Quit");
        Time.timeScale = 1;
		SceneManager.LoadScene("Menu");
	}

	public void ClosePauseMenu()
	{
		PlayerManager.instance.playerCharacter.inputs.cancelInput = false;
        AudioManager.instance.PlaySound("Confirm");
        HUDManager.instance.isPaused = false;
		Time.timeScale = 1;
	}
}
