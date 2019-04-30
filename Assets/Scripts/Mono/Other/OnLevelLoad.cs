using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnLevelLoad : MonoBehaviour
{
	void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		float positionX = PlayerPrefs.GetFloat("CheckpointPositionX", 0.0f);
		float positionY = PlayerPrefs.GetFloat("CheckpointPositionY", 0.0f);

		if (positionX != 0 && positionY != 0)
		{
			PlayerManager.instance.playerObject.transform.position = new Vector2(positionX, positionY);
			CameraController.instance.gameObject.transform.position = new Vector2(positionX, positionY);
		}
	}

	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}
