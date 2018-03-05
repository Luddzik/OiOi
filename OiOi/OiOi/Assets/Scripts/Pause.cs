//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UnityEngine.XR.iOS
{
	public class Pause : MonoBehaviour {

		[SerializeField] private GameObject gameScreen;
		[SerializeField] private GameObject pauseScreen;
		[SerializeField] private GameObject gameOverScreen;
		[SerializeField] private GameObject winScreen;

		[SerializeField] private GameObject world;
		[SerializeField] private Text ingredientsText;
		[SerializeField] private Text solutionOneText;
		[SerializeField] private Text solutionTwoText;

		void Awake()
		{
			Screen.sleepTimeout = SleepTimeout.NeverSleep;

			gameScreen.SetActive(true);
			pauseScreen.SetActive(false);
			gameOverScreen.SetActive(false);
			winScreen.SetActive(false);
		}

		public void GameScreen()
		{
			pauseScreen.SetActive(false);
			gameScreen.SetActive(true);
			Time.timeScale = 1.0f;
		}

		public void PauseScreen()
		{
			int ingredientsNumber = world.GetComponent<WorldSpawn>().GetIngredientsNumber();
			ingredientsText.text = ingredientsNumber.ToString();
			gameScreen.SetActive(false);
			pauseScreen.SetActive(true);
			Time.timeScale = 0.0f;
		}

		public void GameOverScreen()
		{
			gameScreen.SetActive(false);
			pauseScreen.SetActive(false);
			winScreen.SetActive(false);
			gameOverScreen.SetActive(true);
		}

		public void WinScreen()
		{
			gameScreen.SetActive(false);
			pauseScreen.SetActive(false);
			winScreen.SetActive(true);
			gameOverScreen.SetActive(false);
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
}