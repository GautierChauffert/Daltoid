using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TutorialManager : Singleton<TutorialManager>
{
	[Header("Events")]
	[SerializeField] private UnityEvent onStart = new UnityEvent();

	void Start()
	{
		this.onStart.Invoke();
	}

	void Update()
	{
		if(Input.GetButtonDown("Quit")) {
			Application.Quit();
		}
	}

	public void LoadGame()
	{
        AudioManager.instance.PlaySound("New Game");
        StartCoroutine(NewGameCoroutine());
	}

    private IEnumerator NewGameCoroutine()
    {
        yield return new WaitForSeconds(1.3f);
        LoadingManager.instance.Load("Game");
    }


}
