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
        //[SerializeField] private GameObject gameScreen;

        // Requried fields
        //[SerializeField] private GameObject tutorialText;
        [SerializeField] private GameObject tutorialNextButton;

		private void Awake()
		{
            tutorialScreen.SetActive(true);
            tutorialNextButton.SetActive(false);
		}

        public void TutorialTextOn()
        {
            tutorialNextButton.SetActive(true);
        }

        public void TutorialButtonActive()
        {
            tutorialNextButton.SetActive(false);
            gameManager.GetComponent<GameManager>().TutorialNext();
        }
	}
}
