//using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UI;
using UnityEngine.EventSystems;

namespace UnityEngine.XR.iOS
{
    public class Spawn : MonoBehaviour
    {
        [SerializeField] private GameObject[] planetPrefab;
        [SerializeField] private Material[] planetMaterial;

        [SerializeField] private GameObject bombPrefab;
        [SerializeField] private GameObject pathPrefab;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private GameObject gameManager;

        [SerializeField] private AudioClip goodSFX;
        [SerializeField] private AudioClip badSFX;

        private GameObject[] planet;
        private GameObject[] path;

        private int[] ability;
        private bool shieldButtonPress;
        private GameObject bomb;

        private GameObject[] projectile;
        private bool worldSpawn = false;

        private int oneOff = 0;

        public Transform m_HitTransform;
        public float maxRayDistance = 30.0f;
        public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer

		public void SetPlanetStatus (int i, bool status)
        {
            planet[i].SetActive(status);
        }

        public string GetPlanetHealth(int i)
        {
            return planet[i].transform.tag;
        }

        public Vector3 GetPlanetTransform (int i)
        {
            return planet[i].transform.position;
        }

        public void SetPlanetPosition (int i, Vector3 pos)
        {
            planet[i].transform.position = new Vector3(pos.x, pos.y, pos.z);
        }

        public float GetPlanetScale (int i)
        {
            return planet[i].transform.localScale.x;
        }

        public void SetPlanetScale (int i, Vector3 growth)
        {
            planet[i].transform.localScale = growth;
        }

        public void SetPlanetGood(int i)
        {
            planet[i].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            planet[i].transform.tag = "Planet";

            planet[i].GetComponent<AudioSource>().Stop();
            planet[i].GetComponent<AudioSource>().clip = goodSFX;
            planet[i].GetComponent<AudioSource>().Play();

            bool b = gameManager.GetComponent<GameManager>().InfectedPlanetListContain(i);
            if (b)
            {
                gameManager.GetComponent<GameManager>().RemoveInfected(i);
            }
        }

        public void SetPlanetShield(int i)
        {
            planet[i].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            planet[i].transform.tag = "Shielded";
        }

        public void SetPlanetWarning (int i)
        {
            //Test Out
            //planet[i].GetComponent<Renderer>().material.Lerp(planetMaterial[0], planetMaterial[1], 2.0f);
            planet[i].GetComponent<Renderer>().material = new Material(planetMaterial[1]);
        }

        public void SetPlanetBad(int i)
        {
            //Test Out
            //planet[i].GetComponent<Renderer>().material.Lerp(planetMaterial[1], planetMaterial[2], 2.0f);
            planet[i].GetComponent<Renderer>().material = new Material(planetMaterial[2]);
            planet[i].transform.tag = "Infected";

            planet[i].GetComponent<AudioSource>().Stop();
            planet[i].GetComponent<AudioSource>().clip = badSFX;
            planet[i].GetComponent<AudioSource>().Play();

            ability[i] = 0;

            gameManager.GetComponent<GameManager>().AddInfected(i);
            
        }

        public void SetProjectileStatus(int i, bool status)
        {
            projectile[i].SetActive(status);
        }

        public void ProjectileSound(int i)
        {
            projectile[i].GetComponent<AudioSource>().Play();
        }

        public bool GetProjectileStatus(int i)
        {
            return projectile[i].activeSelf;
        }

        public void SetProjectileLocation(int i, Vector3 pos)
        {
            projectile[i].transform.position = new Vector3(pos.x, pos.y, pos.z);
        }

        public Vector3 GetProjectileLocation(int i)
        {
            return projectile[i].transform.position;
        }

        public void CreatePath(int n, int start, int end)
        {
            Vector3 startLoc = planet[start].transform.position;
            Vector3 endLoc = planet[end].transform.position;

            Vector3 between = (startLoc - endLoc) / 10.0f;

            for (int i = (n * 10); i < (n * 10) + 10; i++)
            {
                Vector3 pos = startLoc - (between * (i+1));
                path[i].transform.position = new Vector3(pos.x, pos.y, pos.z);
                //path[i].SetActive(true);
            }
        }

        public void ActivatePath(int i)
        {
            path[i].SetActive(true);
        }

        public void DeactivatePath(int n)
        {
            for (int i = (n * 10); i < (n * 10) + 10; i++)
            {
                path[i].SetActive(false);
            }
        }

        public void RefreshAbilities()
        {
            for (int i = 0; i < ability.Length; i++)
            {
                int x = Random.Range(0, 4);

                if (planet[i].tag == "Planet" || planet[i].tag == "Shielded")
                {
                    ability[i] = x;
                }
                else
                {
                    ability[i] = 4;
                }
            }
        }

        public void NewAbility(int i)
        {
            int x = Random.Range(0, 4);
            if (planet[i].transform.tag == "Planet" || planet[i].transform.tag == "Shielded")
            {
                ability[i] = x;
            }
            else
            {
                ability[i] = 4;
            }
        }

        public void RemoveAbility(int i)
        {
            ability[i] = 0;
        }

        public int GetAbility(int i)
        {
            return ability[i];
        }

        public int Spread(int i)
        {
            int choice = 0;
            int[] ans = new int[3];
            int a = 0, b = 0, c = 0;
            float x = 100.0f;
            float y = 100.0f;
            float z = 100.0f;

            float planetDistance;

            int counter = 0;

            foreach (GameObject place in planet)
            {
                if (place.transform.tag == "Planet")
                {
                    planetDistance = Mathf.Abs(Mathf.Sqrt((Mathf.Pow((planet[i].transform.position.x - place.transform.position.x), 2.0f)) + (Mathf.Pow((planet[i].transform.position.y - place.transform.position.y), 2.0f)) + (Mathf.Pow((planet[i].transform.position.z - place.transform.position.z), 2.0f))));

                    if(planetDistance < x)
                    {
                        x = planetDistance;
                        a = counter;
                    }
                    else if(planetDistance < y)
                    {
                        y = planetDistance;
                        b = counter;
                    }
                    else if(planetDistance < z)
                    {
                        z = planetDistance;
                        c = counter;
                    }
                }
                counter++;
            }

            ans[0] = a;
            ans[1] = b;
            ans[2] = c;

            choice = ans[Random.Range(0, ans.Length)];

            return choice;
        }

        public void BombAbility()
        {
            bomb = (GameObject)Instantiate(bombPrefab);

            bomb.transform.position = Camera.current.transform.position;

            bomb.SetActive(true);

            Collider bombCollider = bomb.GetComponent<Collider>();

            int x = 0;

            foreach (GameObject pl in planet)
            {
                Collider planetCollider = pl.GetComponent<Collider>();
                if (bombCollider.bounds.Intersects(planetCollider.bounds))
                {
                    if (planet[x].transform.tag == "Infected")
                    {
                        gameManager.GetComponent<GameManager>().RemoveInfected(x);
                    }
                    SetPlanetGood(x);
                }
                x++;
            }

            bomb.SetActive(false);
        }

        bool HitTestWithResultType(ARPoint point, ARHitTestResultType resultTypes)
        {
            List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);
            if (hitResults.Count > 0 && !worldSpawn)
            {
                foreach (var hitResult in hitResults)
                {
                    worldSpawn = true;

                    //Debug.Log ("Got hit!");
                    m_HitTransform.position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                    m_HitTransform.rotation = UnityARMatrixOps.GetRotation(hitResult.worldTransform);

                    PlanetsSpawn();

                    gameManager.GetComponent<GameManager>().StartCoroutine("GameSequence");

                    //Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", m_HitTransform.position.x, m_HitTransform.position.y, m_HitTransform.position.z));
                    return true;
                }
            }

            return false;
        }

        void PlanetsSpawn()
        {
            int objectNumber = 10;
            planet = new GameObject[objectNumber];
            path = new GameObject[60];
            ability = new int[10];
            projectile = new GameObject[5];

            for (int i = 0; i < 5; i++)
            {
                projectile[i] = (GameObject)Instantiate(projectilePrefab);
                projectile[i].transform.position = new Vector3(0, 0, 0);
                projectile[i].SetActive(false);
            }


            for (int i = 0; i < path.Length; i++)
            {
                path[i] = (GameObject)Instantiate(pathPrefab);
                path[i].transform.position = new Vector3(0, 0, 0);
                path[i].SetActive(false);
            }

            int x;

            x = Random.Range(0, planetPrefab.Length);

            planet[0] = (GameObject)Instantiate(planetPrefab[x]);
            planet[0].transform.position = new Vector3(m_HitTransform.position.x, m_HitTransform.position.y + 1.5f, m_HitTransform.position.z + 0.1f);
            planet[0].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            planet[0].SetActive(false);

            x = Random.Range(0, planetPrefab.Length);

            planet[1] = (GameObject)Instantiate(planetPrefab[x]);
            planet[1].transform.position = new Vector3(m_HitTransform.position.x + 1.0f, m_HitTransform.position.y + 1.2f, m_HitTransform.position.z);
            planet[1].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            planet[1].SetActive(false);

            x = Random.Range(0, planetPrefab.Length);

            planet[2] = (GameObject)Instantiate(planetPrefab[x]);
            planet[2].transform.position = new Vector3(m_HitTransform.position.x + 0.5f, m_HitTransform.position.y + 1f, m_HitTransform.position.z + 0.5f);
            planet[2].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            planet[2].SetActive(false);

            x = Random.Range(0, planetPrefab.Length);

            planet[3] = (GameObject)Instantiate(planetPrefab[x]);
            planet[3].transform.position = new Vector3(m_HitTransform.position.x, m_HitTransform.position.y + 0.9f, m_HitTransform.position.z + 1.0f);
            planet[3].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            planet[3].SetActive(false);

            x = Random.Range(0, planetPrefab.Length);

            planet[4] = (GameObject)Instantiate(planetPrefab[x]);
            planet[4].transform.position = new Vector3(m_HitTransform.position.x - 0.5f, m_HitTransform.position.y + 1.2f, m_HitTransform.position.z - 0.5f);
            planet[4].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            planet[4].SetActive(false);

            x = Random.Range(0, planetPrefab.Length);

            planet[5] = (GameObject)Instantiate(planetPrefab[x]);
            planet[5].transform.position = new Vector3(m_HitTransform.position.x - 1.0f, m_HitTransform.position.y + 1.5f, m_HitTransform.position.z);
            planet[5].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            planet[5].SetActive(false);

            x = Random.Range(0, planetPrefab.Length);

            planet[6] = (GameObject)Instantiate(planetPrefab[x]);
            planet[6].transform.position = new Vector3(m_HitTransform.position.x - 0.5f, m_HitTransform.position.y + 1.2f, m_HitTransform.position.z + 0.5f);
            planet[6].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            planet[6].SetActive(false);

            x = Random.Range(0, planetPrefab.Length);

            planet[7] = (GameObject)Instantiate(planetPrefab[x]);
            planet[7].transform.position = new Vector3(m_HitTransform.position.x + 0.5f, m_HitTransform.position.y + 1f, m_HitTransform.position.z - 0.5f);
            planet[7].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            planet[7].SetActive(false);

            x = Random.Range(0, planetPrefab.Length);

            planet[8] = (GameObject)Instantiate(planetPrefab[x]);
            planet[8].transform.position = new Vector3(m_HitTransform.position.x, m_HitTransform.position.y + 0.9f, m_HitTransform.position.z - 1.0f);
            planet[8].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            planet[8].SetActive(false);

            x = Random.Range(0, planetPrefab.Length);

            planet[9] = (GameObject)Instantiate(planetPrefab[x]);
            planet[9].transform.position = new Vector3(m_HitTransform.position.x + 2.0f, m_HitTransform.position.y + 0.5f, m_HitTransform.position.z);
            planet[9].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            planet[9].SetActive(false);
        }

        int PlanetClicked(Vector3 pos)
        {
            int planetReturn = 0;
            float result = 100.0f;
            int counter = 0;
            float temp;

            foreach (GameObject pl in planet)
            {
                temp = Mathf.Abs(Mathf.Sqrt((Mathf.Pow((pos.x - pl.transform.position.x), 2.0f)) + (Mathf.Pow((pos.y - pl.transform.position.y), 2.0f)) + (Mathf.Pow((pos.z - pl.transform.position.z), 2.0f))));
                if (temp < result)
                {
                    result = temp;
                    planetReturn = counter;
                }
                counter++;
            }

            return planetReturn;
        }

        int ProjectileClicked(Vector3 pos)
        {
            int projReturn = 0;
            float result = 100.0f;
            int counter = 0;
            float temp;

            foreach (GameObject pl in projectile)
            {
                temp = Mathf.Abs(Mathf.Sqrt((Mathf.Pow((pos.x - pl.transform.position.x), 2.0f)) + (Mathf.Pow((pos.y - pl.transform.position.y), 2.0f)) + (Mathf.Pow((pos.z - pl.transform.position.z), 2.0f))));
                if (temp < result)
                {
                    result = temp;
                    projReturn = counter;
                }
                counter++;
            }

            return projReturn;
        }

        void Update()
        {
            if ((Input.touchCount > 0 && m_HitTransform != null) /*&& !worldSpawn*/)
            {
                var touch = Input.GetTouch(0);
                if ((touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved))
                {
                    var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);

                    Ray ray = Camera.main.ScreenPointToRay(touch.position);

                    ARPoint point = new ARPoint
                    {
                        x = screenPosition.x,
                        y = screenPosition.y
                    };

                    // prioritize reults types
                    ARHitTestResultType[] resultTypes = {
                        ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
                        // if you want to use infinite planes use this:
                        ARHitTestResultType.ARHitTestResultTypeExistingPlane,
                        ARHitTestResultType.ARHitTestResultTypeHorizontalPlane, 
                        //ARHitTestResultType.ARHitTestResultTypeFeaturePoint
                    };

                    foreach (ARHitTestResultType resultType in resultTypes)
                    {
                        if (HitTestWithResultType(point, resultType))
                        {
                            return;
                        }
                    }

                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 1.0f))
                    {
                        shieldButtonPress = gameManager.GetComponent<GameManager>().IsShieldActive();
                        Vector3 pos = new Vector3((float)hit.transform.position.x, (float)hit.transform.position.y, (float)hit.transform.position.z);

                        int number = PlanetClicked(pos);
                        int o = ProjectileClicked(pos);

                        if(shieldButtonPress)
                        {
                            if (hit.collider.tag == "Projectile")
                            {
                                gameManager.GetComponent<GameManager>().ShieldPress(false, 0);
                            }
                            if (hit.collider.tag == "Planet")
                            {
                                gameManager.GetComponent<GameManager>().ShieldPress(true, number);
                            }
                        }

                        if(hit.collider.tag == "Projectile")
                        {
                            if (oneOff == 0)
                            {
                                gameManager.GetComponent<GameManager>().TutorialProjectile();
                                oneOff++;
                            }
                            else
                            {
                                gameManager.GetComponent<GameManager>().ProjectileDeactivated(o);
                            }
                        }

                        if (hit.collider.tag == "Planet")
                        {
                            int z = ability[number];

                            if (z == 1)
                            {
                                ability[number] = 0;
                                gameManager.GetComponent<GameManager>().AddTimeAbility();
                            }
                            else if (z == 2)
                            {
                                ability[number] = 0;
                                gameManager.GetComponent<GameManager>().AddBombAbility();
                            }
                            else if (z == 3)
                            {
                                ability[number] = 0;
                                gameManager.GetComponent<GameManager>().AddShieldAbility();
                            }
                        }
                    }
                }
            }

        }
    }
}