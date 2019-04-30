using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public void NewGame()
	{
		AudioManager.instance.PlaySound("Load Game");
		StartCoroutine(NewGameCoroutine());
	}

	public void LoadGame()
	{
		AudioManager.instance.PlaySound("Load Game");
		StartCoroutine(LoadGameCoroutine());
	}

	public void QuitGame()
	{
		AudioManager.instance.PlaySound("Back");
		StartCoroutine(QuitGameCoroutine());
	}

	private IEnumerator NewGameCoroutine()
	{
		yield return new WaitForSeconds(0.5f);
		PlayerPrefs.SetFloat("CheckpointPositionX", 0.0f);
		PlayerPrefs.SetFloat("CheckpointPositionY", 0.0f);
		SceneManager.LoadScene("Tutorial");
	}

	private IEnumerator LoadGameCoroutine()
	{
		yield return new WaitForSeconds(0.5f);
		SceneManager.LoadScene("Game");
	}

	private IEnumerator QuitGameCoroutine()
	{
		yield return new WaitForSeconds(0.3f);
		Application.Quit();
	}
}
