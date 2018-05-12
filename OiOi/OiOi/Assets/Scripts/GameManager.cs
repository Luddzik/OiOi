using System.Collections;
using System.Collections.Generic;
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

        private int sp; // planet variable shield planet
        //private int shieldAbility = 0;
        private bool waitForPress = false;
        private bool shieldActive = false;
        private bool shieldActived = false;

        private bool abilityActive = false;
        private int abilityInUse = 0;

        private bool tutorialClick = false;
        private int nextInfection = 0;
        private int[] nxtInf = new int[5];

        private float speed;

        private bool worldSpawn;

        IEnumerator GameSequence()
        {
            // Set spawn location of ability (0.1f above the spawn location)
            spawnPos = spawn.GetComponent<Spawn>().m_HitTransform.position;

            Vector3 tempPos = new Vector3(spawnPos.x, spawnPos.y + 0.1f, spawnPos.z);

            spawnPos = tempPos;

            SetUpProjectiles();

            /* Tutorial Sequence
             * 1. Spawning Location
             * 2. Planet Intro
             * 3. Infection Intro
             * 4. Spread & Projectile Shoot intro
             * 5. Abilities Intro
             * Game Start
             */

            // Scan area until you are able to place a spawning plate

            // 1. tutorial UI text
            ui.GetComponent<Interface>().TutorialOne();

            yield return new WaitUntil(() => IsWorldSpawn());

            ui.GetComponent<Interface>().TutorialButtonActive();

            // First planet introduction
            Vector3 planetLoc = spawn.GetComponent<Spawn>().m_HitTransform.position;
            Vector3 planetEndPos = spawn.GetComponent<Spawn>().GetPlanetTransform(0);

            float planetScale = spawn.GetComponent<Spawn>().GetPlanetScale(0);

            float newPlanetScale = planetScale / 10.0f;

            spawn.GetComponent<Spawn>().SetPlanetScale(0, new Vector3(newPlanetScale, newPlanetScale, newPlanetScale));

            spawn.GetComponent<Spawn>().SetPlanetPosition(0, planetLoc);

            spawn.GetComponent<Spawn>().SetPlanetStatus(0, true);

            Vector3 x = (planetLoc - planetEndPos);
            speed = x.magnitude;
            //spawn.GetComponent<Spawn>().SetPlanetPosition(0, planetEndPos);

            // 2. Show planet intro text
            ui.GetComponent<Interface>().TutorialTwo();

            while (Vector3.Distance(planetLoc, planetEndPos) > 0.05f)
            {
                spawn.GetComponent<Spawn>().SetPlanetPosition(0, Vector3.Lerp(planetLoc, planetEndPos, speed * Time.deltaTime));

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

            spawn.GetComponent<Spawn>().SetPlanetPosition(0, planetEndPos);

            yield return new WaitUntil(ButtonClicked);
            //yield return new WaitForSeconds(1.0f);

            tutorialClick = false;
            // 3. Infection introduction

            planetLoc = spawn.GetComponent<Spawn>().m_HitTransform.position;
            planetEndPos = spawn.GetComponent<Spawn>().GetPlanetTransform(1);

            planetScale = spawn.GetComponent<Spawn>().GetPlanetScale(1);

            newPlanetScale = planetScale / 10.0f;

            spawn.GetComponent<Spawn>().SetPlanetScale(1, new Vector3(newPlanetScale, newPlanetScale, newPlanetScale));

            spawn.GetComponent<Spawn>().SetPlanetPosition(1, planetLoc);

            spawn.GetComponent<Spawn>().SetPlanetStatus(1, true);

            x = (planetLoc - planetEndPos);
            speed = x.magnitude * 2.0f;

            ui.GetComponent<Interface>().TutorialTwo();

            while (Vector3.Distance(planetLoc, planetEndPos) > 0.05f)
            {
                spawn.GetComponent<Spawn>().SetPlanetPosition(1, Vector3.Lerp(planetLoc, planetEndPos, speed * Time.deltaTime));

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

            // 3.1. Show infected text
            ui.GetComponent<Interface>().TutorialThree();

            spawn.GetComponent<Spawn>().SetPlanetPosition(1, planetEndPos);

            yield return new WaitForSeconds(1.0f);

            spawn.GetComponent<Spawn>().SetPlanetBad(1);

            // 3.2. Show next button

            yield return new WaitUntil(ButtonClicked);

            tutorialClick = false;

            // Path Display
            // 4. Path Text

            ui.GetComponent<Interface>().TutorialFour();

            spawn.GetComponent<Spawn>().CreatePath(0, 1, 0);

            for (int i = 0; i < 10; i++)
            {
                spawn.GetComponent<Spawn>().ActivatePath(i);
                yield return new WaitForSeconds(0.5f);
            }

            // Warning Planet
            spawn.GetComponent<Spawn>().SetPlanetWarning(0);

            yield return new WaitForSeconds(1.0f);

            // 4.1. Projectile Shoot - Change text to explain projectile and to shoot projectile

            Vector3 start = spawn.GetComponent<Spawn>().GetPlanetTransform(1);

            spawn.GetComponent<Spawn>().SetProjectileStatus(0, true);

            spawn.GetComponent<Spawn>().SetProjectileLocation(0, start);

            Vector3 destination = spawn.GetComponent<Spawn>().GetPlanetTransform(0);

            nextInfection = 0;

            float distance = Vector3.Distance(start, destination) / 2.0f;

            speed = 1.0f * Time.deltaTime;

            spawn.GetComponent<Spawn>().ProjectileSound(0);

            while (!tutorialClick)
            {
                spawn.GetComponent<Spawn>().SetProjectileLocation(0, Vector3.Lerp(start, destination, speed));

                start = spawn.GetComponent<Spawn>().GetProjectileLocation(0);

                if (Vector3.Distance(start, destination) <= distance && !tutorialClick)
                {
                    speed = 0.2f * Time.deltaTime;
                    //Time.timeScale = 0.1f;
                    // Click on the projectile to get off - Text
                }

                if (Vector3.Distance(start, destination) <= (distance / 3) && !tutorialClick)
                {
                    spawn.GetComponent<Spawn>().SetPlanetBad(0);
                    // 4.1.1. Lost, Reset message

                    yield return new WaitForSeconds(2.0f);

                    start = spawn.GetComponent<Spawn>().GetPlanetTransform(1);
                    spawn.GetComponent<Spawn>().SetPlanetWarning(0);

                    speed = 1.0f * Time.deltaTime;
                    //Time.timeScale = 1.0f;
                }

                yield return new WaitForSeconds(0.1f);
            }

            speed = 1.0f * Time.deltaTime;

            spawn.GetComponent<Spawn>().SetPlanetGood(0);

            ui.GetComponent<Interface>().TutorialButtonActive();
            tutorialClick = false;

            yield return new WaitForSeconds(1.0f);

            // 5. Ability introduction. 3 planets each with ability. 

            // Spawn 3 planets
            // Get 3 different abilities on each planet
            // Have ability chosen at the base when selected

            // Time Ability
            ui.GetComponent<Interface>().TutorialFive();

            spawn.GetComponent<Spawn>().SetPlanetStatus(1, false);

            spawn.GetComponent<Spawn>().SetAbility(0, 1);

            yield return new WaitUntil(ButtonClicked);
            tutorialClick = false;

            // Bomb Ability
            spawn.GetComponent<Spawn>().SetAbility(0, 2);

            yield return new WaitUntil(ButtonClicked);
            tutorialClick = false;

            // Shield Ability
            spawn.GetComponent<Spawn>().SetAbility(0, 3);

            yield return new WaitUntil(ButtonClicked);
            tutorialClick = false;

            ui.GetComponent<Interface>().TutorialShield();

            spawn.GetComponent<Spawn>().SetAbility(0, 0);
            spawn.GetComponent<Spawn>().SetPlanetShield(0);

            yield return new WaitUntil(ButtonClicked);
            tutorialClick = false;

            ui.GetComponent<Interface>().TutorialEight();

            //ui.GetComponent<Interface>().TutorialDeactivate();

            //spawn.GetComponent<Spawn>().RefreshAbilities();

            yield return new WaitForSeconds(1.0f);

            //StartCoroutine("GameStart");

        }

        IEnumerator GameStart()
        {
            speed = 1.0f * Time.deltaTime; 

            // Set spawn location of ability (0.1f above the spawn location)
            spawnPos = spawn.GetComponent<Spawn>().m_HitTransform.position;

            Vector3 tempPos = new Vector3(spawnPos.x, spawnPos.y + 0.1f, spawnPos.z);

            spawnPos = tempPos;

            SetUpProjectiles();

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

            for (int i = 0; i < 10; i++)
            {
                spawn.GetComponent<Spawn>().BeginPlanetSpread(i);
            }

            while(infectedPlanets.Count < 10)
            {
                spawn.GetComponent<Spawn>().RefreshAbilities();

                yield return new WaitForSeconds(5.0f);
            }

            //StartCoroutine("InfectionSpread");
        }

        /*IEnumerator InfectionSpread()
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

                    speed = 1.0f * Time.deltaTime;

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
                        spawn.GetComponent<Spawn>().SetProjectileLocation(q, Vector3.Lerp(stPlanet[q], edPlanet[q], speed));

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
        }*/

        public void ProjectileSet(int startPlanet, int projectileLocation)
        {
            spawn.GetComponent<Spawn>().SetProjectileStatus(projectileLocation, true);

            nxtInf[projectileLocation] = spawn.GetComponent<Spawn>().Spread(startPlanet);

            spawn.GetComponent<Spawn>().CreatePath(projectileLocation, startPlanet, nxtInf[projectileLocation]);

            spawn.GetComponent<Spawn>().SetPlanetWarning(nxtInf[projectileLocation]);

            stPlanet[projectileLocation] = spawn.GetComponent<Spawn>().GetPlanetTransform(startPlanet);

            spawn.GetComponent<Spawn>().SetProjectileLocation(projectileLocation, stPlanet[projectileLocation]);

            edPlanet[projectileLocation] = spawn.GetComponent<Spawn>().GetPlanetTransform(nxtInf[projectileLocation]);

            between[projectileLocation] = Vector3.Distance(stPlanet[projectileLocation], edPlanet[projectileLocation]);
        }

        public float GetSpeed()
        {
            return speed;
        }

        public Vector3 DestinationPlanetPos(int projectileLocation)
        {
            return edPlanet[projectileLocation];
        }

        public void ResetEndPlanet(int projectileLocation)
        {
            edPlanet[projectileLocation] = new Vector3(0,0,0);
        }

        public float DistanceBetween(int projectileLoc)
        {
            return between[projectileLoc];
        }

        public int DestPlanet(int projectileLoc)
        {
            return nxtInf[projectileLoc];
        }

        public int GetProjectile()
        {
            int freeProj = availableProjectile.Length;
            for (int i = 0; i < availableProjectile.Length; i++)
            {
                availableProjectile[i] = spawn.GetComponent<Spawn>().GetProjectileStatus(i);
                if(!availableProjectile[i] && i < freeProj)
                {
                    freeProj = i;
                }
            }
            return freeProj;
        }

        public void AddInfected(int i)
        {
            infectedPlanets.Add(i);
        }

        public void RemoveInfected(int i)
        {
            infectedPlanets.Remove(i);
        }

        public int NumberOfInfected()
        {
            return infectedPlanets.Count;
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

            int x = nxtInf[i];
            //int x = spawn.GetComponent<Spawn>().PlanetClicked(edPlanet[i]);

            spawn.GetComponent<Spawn>().SetPlanetGood(x);
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
                spawn.GetComponent<Spawn>().SetPlanetGood(sp);
                spawn.GetComponent<Spawn>().SetPulsing(false);
                speed = 1.0f * Time.deltaTime;
                StopCoroutine("ShieldPlanet");
            }
            else
            {
                shieldActive = true;
                StartCoroutine("ShieldPlanet");
            }

        }

        public void BombAbilityUse()
        {
            spawn.GetComponent<Spawn>().BombAbility();
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

            if(abilityActive)
            {
                abilityInUse = ability.GetComponent<Ability>().UsingAbility();
                DoAbility();
            }
        }

        public void ShieldPlanet(int no)
        {
            sp = no;
        }

        IEnumerator TimeSlow()
        {
            slowActive = true;

            speed = 0.2f * Time.deltaTime;
            //Time.timeScale = 0.2f;
            yield return new WaitForSeconds(5.0f);

            speed = 1.0f * Time.deltaTime;
            //Time.timeScale = 1.0f;

            slowActive = false;

        }

        IEnumerator ShieldPlanet()
        {
            spawn.GetComponent<Spawn>().SetPlanetShield(sp);
            yield return new WaitForSeconds(5.0f);

            spawn.GetComponent<Spawn>().SetPlanetGood(sp);
            shieldActive = false;
        }

		public void DoAbility()
        {
            if(abilityInUse == 1)
            {
                // Slow ability
                spawn.GetComponent<Spawn>().SetPulsing(false);
                ability.GetComponent<Ability>().UpdateStatus(false);
                TimeSlowAbility();

            }
            else if(abilityInUse == 2)
            {
                // Bomb ability
                if (waitForPress)
                {
                    // Do Bomb Ability
                    BombAbilityUse();
                    spawn.GetComponent<Spawn>().SetPulsing(false);
                    waitForPress = false;
                }
                else
                {
                    waitForPress = true;
                }

            }
            else if(abilityInUse == 3)
            {
                // Shield ability
                if (waitForPress)
                {
                    // Do Shield Ability
                    ShieldAbility();
                    waitForPress = false;
                }
                else
                {
                    waitForPress = true;
                }
            }
        }

		void SetUpProjectiles()
        {
            for (int i = 0; i < 5; i++)
            {
                availableProjectile[i] = false;
            }
        }

        public void InfectedProjectileDeactivate(int i)
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

        bool IsWorldSpawn()
        {
            worldSpawn = spawn.GetComponent<Spawn>().IsWorldSpawn();
            return worldSpawn;
        }
    }
}
