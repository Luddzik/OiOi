using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.iOS
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject spawn;

        private int slowTimeAbility = 0;
        private int bombAbility = 0;
        private int shieldAbility = 0;

        private bool tutorialClick = false;
        private int nextInfection = 0;

        IEnumerator GameSequence()
        {
            // First planet introduction

            Transform spawnLoc = spawn.GetComponent<Spawn>().m_HitTransform;

            Vector3 planetLoc = spawn.GetComponent<Spawn>().m_HitTransform.position;
            Vector3 planetEndPos = spawn.GetComponent<Spawn>().GetPlanetTransform(0);

            float planetScale = spawn.GetComponent<Spawn>().GetPlanetScale(0);

            float newPlanetScale = planetScale / 10.0f;

            float temp = newPlanetScale;

            spawn.GetComponent<Spawn>().SetPlanetScale(0, new Vector3(newPlanetScale, newPlanetScale, newPlanetScale));

            spawn.GetComponent<Spawn>().SetPlanetPosition(0, planetLoc);

            spawn.GetComponent<Spawn>().SetPlanetStatus(0, true);

            Vector3 x = (planetLoc - planetEndPos);
            float s = x.magnitude;
            //spawn.GetComponent<Spawn>().SetPlanetPosition(0, planetEndPos);


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

                yield return new WaitForSeconds(0.1f);
            }

            spawn.GetComponent<Spawn>().SetPlanetPosition(0, planetEndPos);

            yield return new WaitForSeconds(10.0f);

            // Infection introduction

            planetLoc = spawn.GetComponent<Spawn>().m_HitTransform.position;
            planetEndPos = spawn.GetComponent<Spawn>().GetPlanetTransform(1);

            planetScale = spawn.GetComponent<Spawn>().GetPlanetScale(1);

            newPlanetScale = planetScale / 10.0f;

            temp = newPlanetScale;

            spawn.GetComponent<Spawn>().SetPlanetScale(1, new Vector3(newPlanetScale, newPlanetScale, newPlanetScale));

            spawn.GetComponent<Spawn>().SetPlanetPosition(1, planetLoc);

            spawn.GetComponent<Spawn>().SetPlanetStatus(1, true);

            x = (planetLoc - planetEndPos);
            s = x.magnitude;

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

                yield return new WaitForSeconds(0.1f);
            }

            spawn.GetComponent<Spawn>().SetPlanetPosition(1, planetEndPos);

            yield return new WaitForSeconds(2.0f);

            spawn.GetComponent<Spawn>().SetPlanetBad(1);

            yield return new WaitForSeconds(2.0f);

            // Path Display

            spawn.GetComponent<Spawn>().CreatePath(1, 0);

            for (int i = 0; i < 10; i++)
            {
                spawn.GetComponent<Spawn>().ActivatePath(i);
                yield return new WaitForSeconds(0.5f);
            }

            // Warning Planet

            spawn.GetComponent<Spawn>().SetPlanetWarning(0);

            yield return new WaitForSeconds(2.0f);

            // Projectile Shoot

            Vector3 start = spawn.GetComponent<Spawn>().GetPlanetTransform(1);

            spawn.GetComponent<Spawn>().SetProjectileStatus(true);

            spawn.GetComponent<Spawn>().SetProjectileLocation(start);

            Vector3 destination = spawn.GetComponent<Spawn>().GetPlanetTransform(0);

            nextInfection = 0;

            float distance = Vector3.Distance(start, destination) / 2.0f;

            s = 1.0f * Time.deltaTime;

            while (!tutorialClick)
            {
                spawn.GetComponent<Spawn>().SetProjectileLocation(Vector3.Lerp(start, destination, s));

                start = spawn.GetComponent<Spawn>().GetProjectileLocation();

                if (Vector3.Distance(start, destination) <= distance && !tutorialClick)
                {
                    Time.timeScale = 0.1f;
                    // Click on the projectile to get off
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

            yield return new WaitForSeconds(5.0f);

            // Introduce to Ingredients




            // Projectile from infected planet...
            // Have projectile shoot and player has to interfere...
            // Introduction to Ingredients...
            // On-Screen buttons to create ingredients...
            //          Shield...
            //          Bomb...
            //          Slow Time...
            // Add sound effects/music...
            // Win/Lose conditions...

            for (int i = 0; i < 10; i++)
            {
                spawn.GetComponent<Spawn>().SetPlanetStatus(i, true);
                yield return new WaitForSeconds(0.5f);
            }

        }

        public void TutorialProjectile()
        {
            tutorialClick = true;

            Time.timeScale = 1.0f;

            spawn.GetComponent<Spawn>().DeactivatePath();

            spawn.GetComponent<Spawn>().SetProjectileStatus(false);

            spawn.GetComponent<Spawn>().SetPlanetGood(nextInfection);
        }

    }
}
