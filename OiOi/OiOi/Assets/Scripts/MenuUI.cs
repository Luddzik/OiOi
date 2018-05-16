using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UnityEngine.XR.iOS
{
    public class MenuUI : MonoBehaviour
    {
        // UI Screens
        [SerializeField] private GameObject mainMenuScreen;
        [SerializeField] private GameObject gameScreen;
        [SerializeField] private GameObject settingScreen;


		// change spawn - menuON status based if player in menu or not.

		/* Tutorial pages
         * 1. Scan area to detect plane and show spawn area
         * 2. Planet introduction
         * 3. Infection Introduction
         * 4. Path/Projectile introduction
         * 5. Abilities intro
         * 6. Game screen UI description
         */

		private void Awake()
		{
            mainMenuScreen.SetActive(true);
            gameScreen.SetActive(false);
            settingScreen.SetActive(false);
		}

		public void MainMenu()
        {
            mainMenuScreen.SetActive(true);
            gameScreen.SetActive(false);
            settingScreen.SetActive(false);
        }

        public void GameStartChoice()
        {
            gameScreen.SetActive(true);
            mainMenuScreen.SetActive(false);
            settingScreen.SetActive(false);
        }

        public void TutorialGame()
        {
            SceneManager.UnloadSceneAsync("Menu");
            SceneManager.LoadScene("Tutorial");
        }

        public void NewGame()
        {
            SceneManager.UnloadSceneAsync("Menu");
            SceneManager.LoadScene("Game");
        }

        public void SettingScreen()
        {
            settingScreen.SetActive(true);
            mainMenuScreen.SetActive(false);
            gameScreen.SetActive(false);
        }

    }
}
