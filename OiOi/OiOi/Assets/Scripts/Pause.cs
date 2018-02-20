//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour {

	[SerializeField] private GameObject gameScreen;
	[SerializeField] private GameObject pauseScreen;
	[SerializeField] private GameObject gameOverScreen;

	void Awake()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		gameScreen.SetActive(true);
		pauseScreen.SetActive(false);
		gameOverScreen.SetActive(false);
	}

	public void GameScreen()
	{
		pauseScreen.SetActive(false);
		gameScreen.SetActive(true);
		Time.timeScale = 1.0f;
	}

	public void PauseScreen()
	{
		gameScreen.SetActive(false);
		pauseScreen.SetActive(true);
		Time.timeScale = 0.0f;
	}

	public void GameOverScreen()
	{
		gameScreen.SetActive(false);
		pauseScreen.SetActive(false);
		gameOverScreen.SetActive(true);
	}

	public void ReturnMainMenu()
	{
		//SceneManager.LoadScene("MainScreen", LoadSceneMode.Single);
	}

	public void Restart()
	{
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
	}

}
