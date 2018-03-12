//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UnityEngine.XR.iOS
{
	public class Pause : MonoBehaviour 
    {

        [SerializeField] private GameObject instructionsScreen;
		[SerializeField] private GameObject gameScreen;
		[SerializeField] private GameObject pauseScreen;
		[SerializeField] private GameObject gameOverScreen;
		[SerializeField] private GameObject winScreen;

        [SerializeField] private GameObject pageOne;
        [SerializeField] private GameObject pageTwo;

		[SerializeField] private GameObject world;
		[SerializeField] private Text ingredientsText;
		[SerializeField] private Text solutionOneText;
		[SerializeField] private Text solutionTwoText;

		private int ingredientsNumber = 0;
		private int solutionOneNumber = 0;
		private int solutionTwoNumber = 0;

		void Awake()
		{
			Screen.sleepTimeout = SleepTimeout.NeverSleep;

            Time.timeScale = 0.0f;

            instructionsScreen.SetActive(true);
			gameScreen.SetActive(false);
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

        public void ChangePage()
        {
            pageOne.SetActive(false);
            pageTwo.SetActive(true);
        }

        public void StartGame()
        {
            pageOne.SetActive(true);
            pageTwo.SetActive(false);

            instructionsScreen.SetActive(false);
            gameScreen.SetActive(true);
            pauseScreen.SetActive(false);
            winScreen.SetActive(false);
            gameOverScreen.SetActive(false);

            Time.timeScale = 1.0f;
        }

		public void PauseScreen()
		{
			ingredientsNumber = world.GetComponent<WorldSpawn>().GetIngredientsNumber();
			solutionOneNumber = world.GetComponent<WorldSpawn>().GetBomb();
			solutionTwoNumber = world.GetComponent<WorldSpawn>().GetLinkSolution();
			ingredientsText.text = ingredientsNumber.ToString();
			solutionOneText.text = solutionOneNumber.ToString();
			solutionTwoText.text = solutionTwoNumber.ToString();
			gameScreen.SetActive(false);
			pauseScreen.SetActive(true);
			Time.timeScale = 0.0f;
		}

		public void GameOverScreen()
		{
            instructionsScreen.SetActive(false);
			gameScreen.SetActive(false);
			pauseScreen.SetActive(false);
			winScreen.SetActive(false);
			gameOverScreen.SetActive(true);
		}

		public void WinScreen()
		{
            instructionsScreen.SetActive(false);
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

		public void AddSolutionOne()
		{
			if(ingredientsNumber >= 5)
			{
				world.GetComponent<WorldSpawn>().AddBomb(1);
				//ingredientsNumber -= 5;
				world.GetComponent<WorldSpawn>().SetIngredients(5);

				ingredientsNumber = world.GetComponent<WorldSpawn>().GetIngredientsNumber();
				solutionOneNumber = world.GetComponent<WorldSpawn>().GetBomb();
				solutionTwoNumber = world.GetComponent<WorldSpawn>().GetLinkSolution();
				ingredientsText.text = ingredientsNumber.ToString();
				solutionOneText.text = solutionOneNumber.ToString();
				solutionTwoText.text = solutionTwoNumber.ToString();
			}
		}

		public void AddSolutionTwo()
		{
			if(ingredientsNumber >= 10)
			{
				world.GetComponent<WorldSpawn>().AddLinkSolution(1);
				//ingredientsNumber -= 5;
				world.GetComponent<WorldSpawn>().SetIngredients(5);

				ingredientsNumber = world.GetComponent<WorldSpawn>().GetIngredientsNumber();
				solutionOneNumber = world.GetComponent<WorldSpawn>().GetBomb();
				solutionTwoNumber = world.GetComponent<WorldSpawn>().GetLinkSolution();
				ingredientsText.text = ingredientsNumber.ToString();
				solutionOneText.text = solutionOneNumber.ToString();
				solutionTwoText.text = solutionTwoNumber.ToString();
			}
		}

	}
}