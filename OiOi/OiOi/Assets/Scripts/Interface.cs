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
        [SerializeField] private GameObject abilitiesScreen;

        // Requried fields
        //[SerializeField] private GameObject tutorialText;
        [SerializeField] private GameObject tutorialNextButton;

		private void Awake()
		{
            tutorialScreen.SetActive(true);
            tutorialNextButton.SetActive(false);

            abilitiesScreen.SetActive(false);
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

        public void TutorialDeactivate()
        {
            tutorialScreen.SetActive(false);
        }

        public void AbilitiesScreenON()
        {
            abilitiesScreen.SetActive(true);
        }

        public void SlowTimeAbility()
        {
            gameManager.GetComponent<GameManager>().TimeSlowAbility();
        }

        public void BombAbility()
        {
            gameManager.GetComponent<GameManager>().BombAbilityUse();
        }

        public void ShieldAbility()
        {
            if(!gameManager.GetComponent<GameManager>().IsShielded())
            {
                gameManager.GetComponent<GameManager>().ShieldAbility();
            }
        }
	}
}
