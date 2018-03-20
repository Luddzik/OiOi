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
        [SerializeField] private GameObject textBg;
        [SerializeField] private GameObject tutText1;
        [SerializeField] private GameObject tutText2;
        [SerializeField] private GameObject tutText3;
        [SerializeField] private GameObject tutText4;

        // Ability buttons
        [SerializeField] private GameObject TimeAbilityActive;
        [SerializeField] private GameObject TimeAbilityDeactivated;

        [SerializeField] private GameObject BombAbilityActive;
        [SerializeField] private GameObject BombAbilityDeactivated;

        private bool timeActive = false;
        private bool bombActive = false;

		private void Awake()
		{
            tutorialScreen.SetActive(true);
            tutorialNextButton.SetActive(false);

            abilitiesScreen.SetActive(false);
		}

        public void ChangeTimeAbilityState(bool state)
        {
            timeActive = state;

            if(!timeActive)
            {
                TimeAbilityDeactivated.SetActive(false);
                TimeAbilityActive.SetActive(true);
            }
            else if (timeActive)
            {
                TimeAbilityActive.SetActive(false);
                TimeAbilityDeactivated.SetActive(true);
            }
        }

        public void ChangeBombAbilityState(bool state)
        {
            bombActive = state;

            if (!bombActive)
            {
                BombAbilityDeactivated.SetActive(false);
                BombAbilityActive.SetActive(true);
            }
            else if (bombActive)
            {
                BombAbilityActive.SetActive(false);
                BombAbilityDeactivated.SetActive(true);
            }
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

        public void AbilitiesScreenON()
        {
            abilitiesScreen.SetActive(true);
        }

        public void SlowTimeAbility()
        {
            if(!timeActive)
            {
                gameManager.GetComponent<GameManager>().TimeSlowAbility();
            }
        }

        public void BombAbility()
        {
            if(!bombActive)
            {
                gameManager.GetComponent<GameManager>().BombAbilityUse(); 
            }
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
