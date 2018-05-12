using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.XR.iOS
{
    public class Interface : MonoBehaviour
    {
        // Other Scripts
        [SerializeField] private GameObject gameManager;
        [SerializeField] private GameObject spawn;

        // UI Screens
        [SerializeField] private GameObject tutorialScreen;
        [SerializeField] private GameObject gameScreen;

        // Requried fields
        //[SerializeField] private GameObject tutorialText;
        [SerializeField] private GameObject tutorialNextButton;
        [SerializeField] private GameObject tutorialComplete;
        [SerializeField] private GameObject textBg;
        [SerializeField] private Text tutText;

        // UI text description
        [SerializeField] private GameObject helpDescription;
        [SerializeField] private GameObject pauseDescription;

        // Start game UI
        [SerializeField] private GameObject startGame;


        // change spawn - menuON status based if player in menu or not.

        /* Tutorial pages
         * 1. Scan area to detect plane and show spawn area
         * 2. Planet introduction
         * 3. Infection Introduction
         * 4. Path/Projectile introduction
         * 5. Abilities intro
         * 6. Game screen UI description
         */

        public void HelpScreen()
        {
            
        }

        public void PauseScreen()
        {
            
        }

        public void TutorialScreen()
        {
            spawn.GetComponent<Spawn>().SetTutorialStatus(true);
            tutorialScreen.SetActive(true);
            tutorialNextButton.SetActive(false);

            startGame.SetActive(false);

            gameScreen.SetActive(false);
        }

        public void TutorialOne()
        {
            string text = new string("Scan area and tap screen to place spawning pod.".ToCharArray());
            textBg.SetActive(true);
            //tutText1.SetActive(true);

            //tutorialNextButton.SetActive(true);

            tutText.text = text;
        }

        public void TutorialTwo()
        {
            //tutText1.SetActive(false);
            //tutText2.SetActive(true);

            string text = new string("This world is made up of PLANETS, protect healthy ones.".ToCharArray());

            spawn.GetComponent<Spawn>().SetMenuToggle(true);

            textBg.SetActive(true);

            tutorialNextButton.SetActive(true);

            tutText.text = text;
        }

        public void TutorialThree()
        {
            spawn.GetComponent<Spawn>().SetMenuToggle(true);

            textBg.SetActive(true);
            //tutText3.SetActive(true);

            string text = new string("Planets get INFECTED, having different shade.".ToCharArray());

            tutorialNextButton.SetActive(true);

            tutText.text = text;
        }

        public void TutorialFour()
        {
            //tutText3.SetActive(false);
            //tutText4.SetActive(true);

            string text = new string("Infection spreads by shooting PROJECTILES, PATH shown beforehand.".ToCharArray());

            textBg.SetActive(true);

            //tutorialNextButton.SetActive(true);

            tutText.text = text;
        }

        public void TutorialFive()
        {
            string text = new string("TIME is one of three abilities in the game. It slows down projectiles. Once ability been picked up, it shows up on spawn location. Tap to activate.".ToCharArray());

            spawn.GetComponent<Spawn>().SetMenuToggle(true);

            textBg.SetActive(true);

            tutorialNextButton.SetActive(true);

            tutText.text = text;
        }

        public void TutorialSix()
        {
            string text = new string("BOMB is another ability. Upon activation, click another planet to initialize bomb location, destroying infection at given location with radius.".ToCharArray());

            spawn.GetComponent<Spawn>().SetMenuToggle(true);

            textBg.SetActive(true);

            tutorialNextButton.SetActive(true);

            tutText.text = text;
        }

        public void TutorialSeven()
        {
            string text = new string("SHIELD protects planet from infection for short time. Upon activation tap planet which will be protected.".ToCharArray());

            spawn.GetComponent<Spawn>().SetMenuToggle(true);

            textBg.SetActive(true);

            tutorialNextButton.SetActive(true);

            tutText.text = text;
        }

        public void TutorialEight()
        {
            spawn.GetComponent<Spawn>().SetMenuToggle(true);
            helpDescription.SetActive(true);
            pauseDescription.SetActive(true);

            tutorialComplete.SetActive(true);
        }

        public void TutorialShield()
        {
            string text = new string("SHIELDED planet looks like this.".ToCharArray());

            spawn.GetComponent<Spawn>().SetMenuToggle(true);

            textBg.SetActive(true);

            tutorialNextButton.SetActive(true);

            tutText.text = text;
        }

        public void TutorialButtonActive()
        {
            spawn.GetComponent<Spawn>().SetMenuToggle(false);

            textBg.SetActive(false);

            tutText.text = "";
            //tutText1.SetActive(false);
            //tutText2.SetActive(false);
            //tutText3.SetActive(false);
            //tutText4.SetActive(false);

            helpDescription.SetActive(false);
            pauseDescription.SetActive(false);

            tutorialNextButton.SetActive(false);
            gameManager.GetComponent<GameManager>().TutorialNext();
        }

        public void TutorialDeactivate()
        {
            spawn.GetComponent<Spawn>().SetMenuToggle(true);
            spawn.GetComponent<Spawn>().SetTutorialStatus(false);
            tutorialScreen.SetActive(false);
            startGame.SetActive(true);
        }

        public void StartGame()
        {
            spawn.GetComponent<Spawn>().SetMenuToggle(false);

            startGame.SetActive(false);
            gameScreen.SetActive(true);

            gameManager.GetComponent<GameManager>().StartCoroutine("GameStart");
        }


	}
}
