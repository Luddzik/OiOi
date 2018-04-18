using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.iOS
{
    public class Interface : MonoBehaviour
    {
        // Other Scripts
        [SerializeField] private GameObject gameManager;

        // UI Screens
        [SerializeField] private GameObject tutorialScreen;

        // Requried fields
        //[SerializeField] private GameObject tutorialText;
        [SerializeField] private GameObject tutorialNextButton;
        [SerializeField] private GameObject textBg;
        [SerializeField] private GameObject tutText1;
        [SerializeField] private GameObject tutText2;
        [SerializeField] private GameObject tutText3;
        [SerializeField] private GameObject tutText4;


		private void Awake()
		{
            tutorialScreen.SetActive(true);
            tutorialNextButton.SetActive(false);
		}

        public void TutorialOne()
        {
            textBg.SetActive(true);
            tutText1.SetActive(true);
        }

        public void TutorialTwo()
        {
            tutText1.SetActive(false);
            tutText2.SetActive(true);
        }

        public void TutorialThree()
        {
            textBg.SetActive(true);
            tutText3.SetActive(true);
        }

        public void TutorialFour()
        {
            tutText3.SetActive(false);
            tutText4.SetActive(true);
        }

        public void TutorialTextOn()
        {
            tutorialNextButton.SetActive(true);
        }

        public void TutorialButtonActive()
        {
            tutorialNextButton.SetActive(false);
            textBg.SetActive(false);
            tutText1.SetActive(false);
            tutText2.SetActive(false);
            tutText3.SetActive(false);
            tutText4.SetActive(false);

            gameManager.GetComponent<GameManager>().TutorialNext();
        }

        public void TutorialDeactivate()
        {
            tutorialScreen.SetActive(false);
        }


	}
}
