using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : Singleton<LoadingManager>
{
	private bool loading = false;

	public void Load(string name)
	{
		if(this.loading) {
			return;
		}

		this.loading = true;

		// StartCoroutine(this.LoadCoroutine(name));

		SceneManager.LoadScene(name);
	}

	/*private IEnumerator LoadCoroutine(string name)
	{
		AsyncOperation asyncLoad;

		asyncLoad = SceneManager.LoadSceneAsync(name);
		asyncLoad.allowSceneActivation = false;

		// Wait until the asynchronous scene fully loads
		while(!asyncLoad.isDone)
		{
			// Debug.Log(asyncLoad.progress);

			if(asyncLoad.progress >= 0.9f)
			{
				if(Input.GetKeyDown(KeyCode.Space)) {
					asyncLoad.allowSceneActivation = true;
					break;
				}

				break;
			}

			yield return null;
		}

		// Debug.Log(asyncLoad.progress);
	}*/
}
