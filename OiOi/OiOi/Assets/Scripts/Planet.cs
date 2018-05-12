﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.iOS
{
    public class Planet : MonoBehaviour
    {
        private GameObject gameManager;
        private GameObject spawn;

        private GameObject shielded;
        private GameObject shieldAbility;
        private GameObject bombAbility;
        private GameObject timeAbility;

        private int projectile;

        private int planetLoc;

        private Vector3 startPos;

        private Vector3 startPlanetPos;
        private Vector3 endPlanetPos;

        private float distance;

        private float speed;

		private void Awake()
		{
            gameManager = GameObject.FindWithTag("GameManager");
            spawn = GameObject.Find("SpawnPlatform");

            shielded = this.transform.Find("Shielded").gameObject;
            shieldAbility = this.transform.Find("ShieldAbility").gameObject;
            bombAbility = this.transform.Find("BombAbility").gameObject;
            timeAbility = this.transform.Find("TimeSlowAbility").gameObject;

            shielded.SetActive(false);
            shieldAbility.SetActive(false);
            bombAbility.SetActive(false);
            timeAbility.SetActive(false);
		}

        public void ShieldStatus(bool status)
        {
            shielded.SetActive(status);
        }

        public void ShieldAbilityStatus(bool status)
        {
            shieldAbility.SetActive(status);
        }

        public void BombAbilityStatus(bool status)
        {
            bombAbility.SetActive(status);
        }

        public void TimeAbilityStatus(bool status)
        {
            timeAbility.SetActive(status);
        }

        public void DeactivateAbilities()
        {
            shieldAbility.SetActive(false);
            bombAbility.SetActive(false);
            timeAbility.SetActive(false);
        }

        public void SetPlanetLoc(int i)
        {
            planetLoc = i;
            startPlanetPos = spawn.GetComponent<Spawn>().GetPlanetTransform(i);
        }

		IEnumerator Spread()
        {
            while (gameManager.GetComponent<GameManager>().NumberOfInfected() < 10)
            {
                yield return new WaitForSeconds(Random.Range(4.0f, 10.0f));

                if (this.gameObject.tag == "Infected")
                {
                    projectile = gameManager.GetComponent<GameManager>().GetProjectile();

                    gameManager.GetComponent<GameManager>().ProjectileSet(planetLoc, projectile);

                    endPlanetPos = gameManager.GetComponent<GameManager>().DestinationPlanetPos(projectile);

                    distance = gameManager.GetComponent<GameManager>().DistanceBetween(projectile);

                    speed = gameManager.GetComponent<GameManager>().GetSpeed();

                    for (int n = (projectile * 10); n < (projectile * 10) + 10; n++)
                    {
                        spawn.GetComponent<Spawn>().ActivatePath(n);
                        yield return new WaitForSeconds(0.5f);
                    }

                    yield return new WaitForSeconds(1.0f);

                    bool fly = spawn.GetComponent<Spawn>().GetProjectileStatus(projectile);

                    spawn.GetComponent<Spawn>().ProjectileSound(projectile);

                    startPos = startPlanetPos;

                    int nextInfection = gameManager.GetComponent<GameManager>().DestPlanet(projectile);

                    while (fly)
                    {
                        speed = gameManager.GetComponent<GameManager>().GetSpeed();

                        spawn.GetComponent<Spawn>().SetProjectileLocation(projectile, Vector3.Lerp(startPos, endPlanetPos, speed));

                        startPos = spawn.GetComponent<Spawn>().GetProjectileLocation(projectile);

                        if (Vector3.Distance(startPos, endPlanetPos) <= distance / 10)
                        {
                            if (spawn.GetComponent<Spawn>().GetPlanetHealth(nextInfection) == "Planet")
                            {
                                spawn.GetComponent<Spawn>().SetPlanetBad(nextInfection);
                                gameManager.GetComponent<GameManager>().ResetEndPlanet(projectile);
                            }
                            gameManager.GetComponent<GameManager>().InfectedProjectileDeactivate(projectile);
                        }

                        fly = spawn.GetComponent<Spawn>().GetProjectileStatus(projectile);

                        yield return new WaitForSeconds(0.05f);
                    }

                    gameManager.GetComponent<GameManager>().ResetEndPlanet(projectile);
                }

            }
        }

    }
}
