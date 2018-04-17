using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.XR.iOS
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject spawn;
        [SerializeField] private GameObject ui;

        [SerializeField] private Text healthyPlanets;
        [SerializeField] private Text infPlanets;

        [SerializeField] private GameObject ability;

        private List<int> infectedPlanets = new List<int>();

        // Spread planet, able to have 5 shots at a time
        private bool[] availableProjectile = new bool[5];
        private float[] between = new float[5];
        private Vector3[] stPlanet = new Vector3[5];
        private Vector3[] edPlanet = new Vector3[5];

        private Vector3 spawnPos;

        //private int slowTimeAbility = 0;
        private bool slowActive = false;

        //private int bombAbility = 0;

        private int t; // planet variable
        //private int shieldAbility = 0;
        private bool waitForPress;
        private bool shieldActive = false;
        private bool shieldActived = false;

        private bool abilityActive = false;
        private int abilityInUse = 0;

        private bool tutorialClick = false;
        private int nextInfection = 0;

        IEnumerator GameSequence()
        {
            // Set spawn location of ability (0.1f above the spawn location)
            spawnPos = spawn.GetComponent<Spawn>().m_HitTransform.position;

            Vector3 tempPos = new Vector3(spawnPos.x, spawnPos.y + 0.1f, spawnPos.z);

            spawnPos = tempPos;

            SetUpProjectiles();

            // First planet introduction
            Vector3 planetLoc = spawn.GetComponent<Spawn>().m_HitTransform.position;
            Vector3 planetEndPos = spawn.GetComponent<Spawn>().GetPlanetTransform(0);

            float planetScale = spawn.GetComponent<Spawn>().GetPlanetScale(0);

            float newPlanetScale = planetScale / 10.0f;

            spawn.GetComponent<Spawn>().SetPlanetScale(0, new Vector3(newPlanetScale, newPlanetScale, newPlanetScale));

            spawn.GetComponent<Spawn>().SetPlanetPosition(0, planetLoc);

            spawn.GetComponent<Spawn>().SetPlanetStatus(0, true);

            Vector3 x = (planetLoc - planetEndPos);
            float s = x.magnitude;
            //spawn.GetComponent<Spawn>().SetPlanetPosition(0, planetEndPos);

            // Show intro text
            ui.GetComponent<Interface>().TutorialOne();

            while (Vector3.Distance(planetLoc, planetEndPos) > 0.05f)
            {
                spawn.GetComponent<Spawn>().SetPlanetPosition(0, Vector3.Lerp(planetLoc, planetEndPos, s * Time.deltaTime));

                if (newPlanetScale < planetScale)
                {
                    newPlanetScale += planetScale * Time.deltaTime;
                    spawn.GetComponent<Spawn>().SetPlanetScale(0, new Vector3(newPlanetScale, newPlanetScale, newPlanetScale));
                }
                else if (newPlanetScale > planetScale)
                {
                    newPlanetScale = planetScale;
                    spawn.GetComponent<Spawn>().SetPlanetScale(0, new Vector3(newPlanetScale, newPlanetScale, newPlanetScale));
                }

                planetLoc = spawn.GetComponent<Spawn>().GetPlanetTransform(0);

                yield return new WaitForSeconds(0.05f);
            }

            // Show next button
            //ui.GetComponent<Interface>().TutorialTextOn();


            spawn.GetComponent<Spawn>().SetPlanetPosition(0, planetEndPos);

            yield return new WaitForSeconds(1.0f);

            //yield return new WaitUntil(ButtonClicked);

            //tutorialClick = false;

            // Infection introduction

            planetLoc = spawn.GetComponent<Spawn>().m_HitTransform.position;
            planetEndPos = spawn.GetComponent<Spawn>().GetPlanetTransform(1);

            planetScale = spawn.GetComponent<Spawn>().GetPlanetScale(1);

            newPlanetScale = planetScale / 10.0f;

            spawn.GetComponent<Spawn>().SetPlanetScale(1, new Vector3(newPlanetScale, newPlanetScale, newPlanetScale));

            spawn.GetComponent<Spawn>().SetPlanetPosition(1, planetLoc);

            spawn.GetComponent<Spawn>().SetPlanetStatus(1, true);

            x = (planetLoc - planetEndPos);
            s = x.magnitude * 2.0f;

            ui.GetComponent<Interface>().TutorialTwo();

            while (Vector3.Distance(planetLoc, planetEndPos) > 0.05f)
            {
                spawn.GetComponent<Spawn>().SetPlanetPosition(1, Vector3.Lerp(planetLoc, planetEndPos, s * Time.deltaTime));

                if (newPlanetScale < planetScale)
                {
                    newPlanetScale += planetScale * Time.deltaTime;
                    spawn.GetComponent<Spawn>().SetPlanetScale(1, new Vector3(newPlanetScale, newPlanetScale, newPlanetScale));
                }
                else if (newPlanetScale > planetScale)
                {
                    newPlanetScale = planetScale;
                    spawn.GetComponent<Spawn>().SetPlanetScale(1, new Vector3(newPlanetScale, newPlanetScale, newPlanetScale));
                }

                planetLoc = spawn.GetComponent<Spawn>().GetPlanetTransform(1);

                yield return new WaitForSeconds(0.05f);
            }

            // Show infected text

            spawn.GetComponent<Spawn>().SetPlanetPosition(1, planetEndPos);

            yield return new WaitForSeconds(1.0f);

            spawn.GetComponent<Spawn>().SetPlanetBad(1);

            // Show next button
            ui.GetComponent<Interface>().TutorialTextOn();

            yield return new WaitUntil(ButtonClicked);

            tutorialClick = false;

            // Path Display
            // Path Text

            ui.GetComponent<Interface>().TutorialThree();

            spawn.GetComponent<Spawn>().CreatePath(0, 1, 0);

            for (int i = 0; i < 10; i++)
            {
                spawn.GetComponent<Spawn>().ActivatePath(i);
                yield return new WaitForSeconds(0.5f);
            }

            // Warning Planet - Change text to warning planet
            // Show button
            //ui.GetComponent<Interface>().TutorialTextOn();

            spawn.GetComponent<Spawn>().SetPlanetWarning(0);

            //yield return new WaitUntil(ButtonClicked);

            //tutorialClick = false;

            yield return new WaitForSeconds(1.0f);

            // Projectile Shoot - Change text to explain projectile and to shoot projectile

            Vector3 start = spawn.GetComponent<Spawn>().GetPlanetTransform(1);

            spawn.GetComponent<Spawn>().SetProjectileStatus(0, true);

            spawn.GetComponent<Spawn>().SetProjectileLocation(0, start);

            Vector3 destination = spawn.GetComponent<Spawn>().GetPlanetTransform(0);

            nextInfection = 0;

            float distance = Vector3.Distance(start, destination) / 2.0f;

            s = 2.0f * Time.deltaTime;

            spawn.GetComponent<Spawn>().ProjectileSound(0);

            while (!tutorialClick)
            {
                spawn.GetComponent<Spawn>().SetProjectileLocation(0, Vector3.Lerp(start, destination, s));

                start = spawn.GetComponent<Spawn>().GetProjectileLocation(0);

                if (Vector3.Distance(start, destination) <= distance && !tutorialClick)
                {
                    Time.timeScale = 0.1f;
                    // Click on the projectile to get off - Text
                }

                if (Vector3.Distance(start, destination) <= (distance / 3) && !tutorialClick)
                {
                    spawn.GetComponent<Spawn>().SetPlanetBad(0);
                    // Lost, Reset message

                    yield return new WaitForSeconds(2.0f);

                    start = spawn.GetComponent<Spawn>().GetPlanetTransform(1);
                    spawn.GetComponent<Spawn>().SetPlanetWarning(0);
                    Time.timeScale = 1.0f;
                }

                yield return new WaitForSeconds(0.1f);
            }

            spawn.GetComponent<Spawn>().SetPlanetGood(0);

            ui.GetComponent<Interface>().TutorialFour();

            tutorialClick = false;

            yield return new WaitForSeconds(1.0f);

            ui.GetComponent<Interface>().TutorialDeactivate();
            ui.GetComponent<Interface>().AbilitiesScreenON();

            spawn.GetComponent<Spawn>().RefreshAbilities();

            yield return new WaitForSeconds(1.0f);

            StartCoroutine("GameStart");

        }

        IEnumerator GameStart()
        {
            for (int i = 0; i < 10; i++)
            {
                spawn.GetComponent<Spawn>().SetPlanetStatus(i, true);
                if (spawn.GetComponent<Spawn>().GetPlanetHealth(i) == "Infected")
                {
                    if (!infectedPlanets.Contains(i))
                    {
                        infectedPlanets.Add(i);
                    }
                }

                yield return new WaitForSeconds(0.5f);
            }

            if (infectedPlanets.Count == 0)
            {
                int x = Random.Range(0, 10);
                spawn.GetComponent<Spawn>().SetPlanetBad(x);
                infectedPlanets.Add(x);
            }

            yield return new WaitForSeconds(2.0f);

            StartCoroutine("InfectionSpread");
        }

        IEnumerator InfectionSpread()
        {
            // Which planets are projectiles assigned to

            while (infectedPlanets.Count > 0)
            {
                for (int i = 0; i < availableProjectile.Length; i++)
                {
                    availableProjectile[i] = spawn.GetComponent<Spawn>().GetProjectileStatus(i);
                }

                yield return new WaitForSeconds(Random.Range(0.0f, 5.0f));

                // Projectile location in array
                int q = 0;
                int oneCount = 0;

                for (int i = 0; i < availableProjectile.Length; i++)
                {
                    bool b = GetRandomInfectedPlanet(i);

                    if (b == false && oneCount == 0)
                    {
                        q = i;
                        oneCount++;
                    }
                }

                if (oneCount > 0)
                {
                    int z = Random.Range(0, infectedPlanets.Count);
                    int startPlanet = infectedPlanets[z];

                    nextInfection = spawn.GetComponent<Spawn>().Spread(startPlanet);

                    spawn.GetComponent<Spawn>().CreatePath(q, startPlanet, nextInfection);

                    // Consider the projectile already in action (location stored in q)

                    spawn.GetComponent<Spawn>().SetPlanetWarning(nextInfection);

                    stPlanet[q] = spawn.GetComponent<Spawn>().GetPlanetTransform(startPlanet);

                    spawn.GetComponent<Spawn>().SetProjectileLocation(q, stPlanet[q]);

                    edPlanet[q] = spawn.GetComponent<Spawn>().GetPlanetTransform(nextInfection);

                    between[q] = Vector3.Distance(stPlanet[q], edPlanet[q]);

                    float s = 2.0f * Time.deltaTime;

                    spawn.GetComponent<Spawn>().SetProjectileStatus(q, true);

                    for (int n = (q * 10); n < (q * 10) + 10; n++)
                    {
                        spawn.GetComponent<Spawn>().ActivatePath(n);
                        yield return new WaitForSeconds(0.5f);
                    }

                    yield return new WaitForSeconds(1.0f);

                    bool fly = spawn.GetComponent<Spawn>().GetProjectileStatus(q);

                    spawn.GetComponent<Spawn>().ProjectileSound(q);

                    while (fly)
                    {
                        spawn.GetComponent<Spawn>().SetProjectileLocation(q, Vector3.Lerp(stPlanet[q], edPlanet[q], s));

                        stPlanet[q] = spawn.GetComponent<Spawn>().GetProjectileLocation(0);

                        if (Vector3.Distance(stPlanet[q], edPlanet[q]) <= (between[q] / 10))
                        {
                            if (spawn.GetComponent<Spawn>().GetPlanetHealth(nextInfection) == "Planet")
                            {
                                spawn.GetComponent<Spawn>().SetPlanetBad(nextInfection);
                            }
                            InfectedProjectileDeactivate(q);
                        }

                        fly = spawn.GetComponent<Spawn>().GetProjectileStatus(q);

                        yield return new WaitForSeconds(0.05f);
                    }
                }

            }

            yield return null;
        }

        public void AddInfected(int i)
        {
            infectedPlanets.Add(i);
        }

        public void RemoveInfected(int i)
        {
            infectedPlanets.Remove(i);
        }

        public void TutorialNext()
        {
            tutorialClick = true;
        }

        public void TutorialProjectile()
        {
            tutorialClick = true;

            Time.timeScale = 1.0f;

            spawn.GetComponent<Spawn>().DeactivatePath(0);

            spawn.GetComponent<Spawn>().SetProjectileStatus(0, false);

            spawn.GetComponent<Spawn>().SetPlanetGood(nextInfection);
        }

        public void ProjectileDeactivated(int i)
        {
            spawn.GetComponent<Spawn>().DeactivatePath(i);

            spawn.GetComponent<Spawn>().SetProjectileStatus(i, false);

            spawn.GetComponent<Spawn>().SetPlanetGood(nextInfection);
        }

        public bool InfectedPlanetListContain(int i)
        {
            return infectedPlanets.Contains(i);
        }

        public void TimeSlowAbility()
        {

            if (slowActive)
            {
                StopCoroutine("TimeSlow");
            }

            StartCoroutine("TimeSlow");

        }

        public void ShieldAbility()
        {
            if (shieldActive)
            {
                shieldActive = false;
                StopCoroutine("ShieldPlanet");

            }
            else
            {
                shieldActive = true;
                waitForPress = false;

                StartCoroutine("ShieldPlanet");
            }

        }

        public void ShieldPress(bool p, int i)
        {
            shieldActived = p;

            if (p)
            {
                t = i;
            }

            ButtonPressed();
        }

        public bool IsShieldActive()
        {
            return shieldActive;
        }

        public bool IsShielded()
        {
            return shieldActived;
        }

        public void BombAbilityUse()
        {

            ui.GetComponent<Interface>().ChangeBombAbilityState(true);

            spawn.GetComponent<Spawn>().BombAbility();

            StartCoroutine("BombCountdown");

        }

        public void UpdateUI()
        {
            int x = 10 - infectedPlanets.Count;
            healthyPlanets.text = x.ToString();
            infPlanets.text = infectedPlanets.Count.ToString();

        }

        public bool IsAbilityActive()
        {
            return abilityActive;
        }

        public void SetAbilityActive(bool state)
        {
            abilityActive = state;

            if(state)
            {
                abilityInUse = ability.GetComponent<Ability>().UsingAbility();
            }
        }

        IEnumerator BombCountdown()
        {
            yield return new WaitForSeconds(1.0f);
            ui.GetComponent<Interface>().ChangeBombAbilityState(false);
        }

        IEnumerator TimeSlow()
        {
            slowActive = true;
            //Time.timeScale = 0.2f;
            yield return new WaitForSeconds(5.0f);
            //Time.timeScale = 1.0f;
        }

        IEnumerator ShieldPlanet()
        {
            yield return new WaitForSeconds(0.5f);
 
            yield return new WaitUntil(ButtonPressed);
            if (shieldActived)
            {
                spawn.GetComponent<Spawn>().SetPlanetShield(t);
                yield return new WaitForSeconds(5.0f);
            }
            else 
            {
                ShieldAbility();
            }

            spawn.GetComponent<Spawn>().SetPlanetGood(t);
            shieldActived = false;

        }

        void SetUpProjectiles()
        {
            for (int i = 0; i < 5; i++)
            {
                availableProjectile[i] = false;
            }
        }

        bool ButtonPressed()
        {
            waitForPress = true;

            return waitForPress;
        }

        void InfectedProjectileDeactivate(int i)
        {
            spawn.GetComponent<Spawn>().DeactivatePath(i);

            spawn.GetComponent<Spawn>().SetProjectileStatus(i, false);
        }

        bool GetRandomInfectedPlanet(int x)
        {
            bool state = spawn.GetComponent<Spawn>().GetProjectileStatus(x);

            return state;
        }

        bool ButtonClicked()
        {
            return tutorialClick;
        }
    }
}
