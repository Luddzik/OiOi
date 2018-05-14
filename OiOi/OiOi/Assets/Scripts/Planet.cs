using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.iOS
{
    public class Planet : MonoBehaviour
    {
        private GameObject gameManager;
        private GameObject spawn;

        [SerializeField] private Material[] planetMat;

        [SerializeField] private GameObject shielded;
        [SerializeField] private GameObject shieldAbility;
        [SerializeField] private GameObject bombAbility;
        [SerializeField] private GameObject timeAbility;

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

        public void SetPlanetLoc(int i, Vector3 pos)
        {
            planetLoc = i;
            startPlanetPos = pos;
        }

		IEnumerator Spread()
        {
            while (gameManager.GetComponent<GameManager>().NumberOfInfected() < 10)
            {
                yield return new WaitForSeconds(Random.Range(4.0f, 10.0f));

                if (this.gameObject.tag == "Infected")
                {
                    projectile = gameManager.GetComponent<GameManager>().GetProjectile();

                    if(projectile < 5)
                    {
                        gameManager.GetComponent<GameManager>().ProjectileSet(planetLoc, projectile);

                        endPlanetPos = gameManager.GetComponent<GameManager>().DestinationPlanetPos(projectile);

                        distance = gameManager.GetComponent<GameManager>().DistanceBetween(projectile);

                        speed = gameManager.GetComponent<GameManager>().GetSpeed();


                        //spawn.GetComponent<Spawn>().ActivatePath(projectile);

                        for (int i = (projectile * 10); i < (projectile * 10) + 10; i++)
                        {
                            spawn.GetComponent<Spawn>().ActivatePath(i);
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

                            //spawn.GetComponent<Spawn>().SetPlanetWarning(nextInfection);

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

		private void Update()
		{
            if(this.gameObject.tag == "Planet")
            {
                shielded.SetActive(false);
                // set healthy material
                this.gameObject.GetComponent<Renderer>().material = new Material(planetMat[0]);
                bool b = gameManager.GetComponent<GameManager>().InfectedPlanetListContain(planetLoc);
                if (b)
                {
                    gameManager.GetComponent<GameManager>().RemoveInfected(planetLoc);
                }
            }
            else if(this.gameObject.tag == "Infected")
            {
                // Set infected material
                this.gameObject.GetComponent<Renderer>().material = new Material(planetMat[1]);

                bool b = gameManager.GetComponent<GameManager>().InfectedPlanetListContain(planetLoc);
                if(!b)
                {
                    gameManager.GetComponent<GameManager>().AddInfected(planetLoc);
                }
            }
            else if(this.gameObject.tag == "Shielded")
            {
                // Set shield material
                this.gameObject.GetComponent<Renderer>().material = new Material(planetMat[0]);
                shielded.SetActive(true);
            }
		}
	}
}
