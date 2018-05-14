using System.Collections.Generic;

namespace UnityEngine.XR.iOS
{
    public class Spawn : MonoBehaviour
    {
        [SerializeField] private GameObject[] planetPrefab;
        //[SerializeField] private Material[] planetMaterial;
        //[SerializeField] private Material[] abilitiesMaterial;

        [SerializeField] private GameObject bombPrefab;
        [SerializeField] private GameObject pathPrefab;
        [SerializeField] private GameObject projectilePrefab;

        [SerializeField] private GameObject gameManager;
        [SerializeField] private GameObject abilitySpawn;

        [SerializeField] private AudioClip goodSFX;
        [SerializeField] private AudioClip badSFX;

        // Mesh deformation
        [SerializeField] private float force = 20.0f;
        [SerializeField] private float forceOffset = 0.1f;

        private GameObject[] planet;
        private GameObject[] path;

        private int[] ability;
        private bool abilityPress = false;
        private bool pulsingAbility = false;

        private GameObject bomb;

        private GameObject[] projectile;
        private bool worldSpawn = false;

        private int oneOff = 0;

        // is menu open?
        private bool menuON = false;

        // play tutorial?
        private bool tutorial = false;

        public Transform m_HitTransform;
        public float maxRayDistance = 30.0f;
        public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer

        public bool IsWorldSpawn()
        {
            return worldSpawn;
        }

        public void SetTutorialStatus(bool status)
        {
            tutorial = status;
        }

        public void SetMenuToggle(bool status)
        {
            menuON = status;
        }

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
            //planet[i].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            planet[i].transform.tag = "Planet";

            planet[i].GetComponent<AudioSource>().Stop();
            planet[i].GetComponent<AudioSource>().clip = goodSFX;
            planet[i].GetComponent<AudioSource>().Play();

            /*bool b = gameManager.GetComponent<GameManager>().InfectedPlanetListContain(i);
            if (b)
            {
                //NewAbility(i);
                gameManager.GetComponent<GameManager>().RemoveInfected(i);
            }*/
        }

        public void SetPlanetShield(int i)
        {
            planet[i].transform.tag = "Shielded";
            //planet[i].GetComponent<Planet>().ShieldStatus(state);
            //planet[i].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            //planet[i].transform.tag = "Shielded";
        }

        /*public void SetPlanetWarning (int i)
        {
            //Test Out
            //planet[i].GetComponent<Renderer>().material.Lerp(planetMaterial[0], planetMaterial[1], 2.0f);
            //planet[i].GetComponent<Renderer>().material = new Material(planetMaterial[1]);
        }*/

        public void SetPlanetBad(int i)
        {
            //Test Out
            //planet[i].GetComponent<Renderer>().material.Lerp(planetMaterial[1], planetMaterial[2], 2.0f);
            //planet[i].GetComponent<Renderer>().material = new Material(planetMaterial[2]);
            planet[i].transform.tag = "Infected";

            planet[i].GetComponent<AudioSource>().Stop();
            planet[i].GetComponent<AudioSource>().clip = badSFX;
            planet[i].GetComponent<AudioSource>().Play();

            RemoveAbility(i);

            //gameManager.GetComponent<GameManager>().AddInfected(i);
            
        }

        public void BeginPlanetSpread(int i)
        {
            planet[i].GetComponent<Planet>().SetPlanetLoc(i, planet[i].transform.position);
            planet[i].GetComponent<Planet>().StartCoroutine("Spread");
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

            int counter = 0;

            for (int i = (n * 10); i < (n * 10) + 10; i++)
            {
                Vector3 pos = startLoc - (between * (counter));
                path[i].transform.position = new Vector3(pos.x, pos.y, pos.z);
                //path[i].SetActive(true);
                counter++;
            }
        }

        public void ActivatePath(int projNumber)
        {
            /*for (int i = (projNumber * 10); i < (projNumber * 10) + 10; i++)
            {
                path[i].SetActive(true);
            }*/
            path[projNumber].SetActive(true);
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

                RemoveAbility(i);

                if (planet[i].tag == "Planet" || planet[i].tag == "Shielded")
                {
                    ability[i] = x;
                    if (x == 0)
                    {
                        planet[i].GetComponent<Planet>().DeactivateAbilities();
                        //planet[i].GetComponentInChildren<Transform>().Find("Ability").gameObject.SetActive(false);
                        //planet[i].GetComponentInChildren<AbilityBall>().UpdateStatus(false);
                        //planet[i].GetComponentInChildren<AbilityBall>().UpdateMaterial(abilitiesMaterial[x]);
                    }
                    else if(x == 1)
                    {
                        planet[i].GetComponent<Planet>().TimeAbilityStatus(true);
                        //planet[i].GetComponentInChildren<Transform>().Find("Ability").gameObject.SetActive(true);
                        //planet[i].GetComponentInChildren<AbilityBall>().UpdateStatus(true);
                        //planet[i].GetComponentInChildren<AbilityBall>().UpdateMaterial(abilitiesMaterial[(x - 1)]);
                    }
                    else if(x == 2)
                    {
                        planet[i].GetComponent<Planet>().BombAbilityStatus(true);
                        //planet[i].GetComponentInChildren<Transform>().Find("Ability").gameObject.SetActive(true);
                        //planet[i].GetComponentInChildren<AbilityBall>().UpdateStatus(true);
                        //planet[i].GetComponentInChildren<AbilityBall>().UpdateMaterial(abilitiesMaterial[(x - 1)]);
                    }
                    else if(x == 3)
                    {
                        planet[i].GetComponent<Planet>().ShieldAbilityStatus(true);
                        //planet[i].GetComponentInChildren<Transform>().Find("Ability").gameObject.SetActive(true);
                        //planet[i].GetComponentInChildren<AbilityBall>().UpdateStatus(true);
                        //planet[i].GetComponentInChildren<AbilityBall>().UpdateMaterial(abilitiesMaterial[(x - 1)]);
                    }

                }
                /*else
                {
                    ability[i] = 4;
                    planet[i].GetComponentInChildren<Transform>().Find("Ability").gameObject.SetActive(true);
                    planet[i].GetComponentInChildren<AbilityBall>().UpdateMaterial(abilitiesMaterial[3]);
                }*/
            }
        }

        public void NewAbility(int i)
        {
            RemoveAbility(i);

            int x = Random.Range(0, 4);
            if (planet[i].transform.tag == "Planet" || planet[i].transform.tag == "Shielded")
            {
                ability[i] = x;
                if (x == 0)
                {
                    planet[i].GetComponent<Planet>().DeactivateAbilities();
                    //planet[i].GetComponentInChildren<Transform>().Find("Ability").gameObject.SetActive(false);
                    //planet[i].GetComponentInChildren<AbilityBall>().UpdateStatus(false);
                    //planet[i].GetComponentInChildren<AbilityBall>().UpdateMaterial(abilitiesMaterial[x]);
                }
                else if (x == 1)
                {
                    planet[i].GetComponent<Planet>().TimeAbilityStatus(true);
                    //planet[i].GetComponentInChildren<Transform>().Find("Ability").gameObject.SetActive(true);
                    //planet[i].GetComponentInChildren<AbilityBall>().UpdateStatus(true);
                    //planet[i].GetComponentInChildren<AbilityBall>().UpdateMaterial(abilitiesMaterial[(x - 1)]);
                }
                else if (x == 2)
                {
                    planet[i].GetComponent<Planet>().BombAbilityStatus(true);
                    //planet[i].GetComponentInChildren<Transform>().Find("Ability").gameObject.SetActive(true);
                    //planet[i].GetComponentInChildren<AbilityBall>().UpdateStatus(true);
                    //planet[i].GetComponentInChildren<AbilityBall>().UpdateMaterial(abilitiesMaterial[(x - 1)]);
                }
                else if (x == 3)
                {
                    planet[i].GetComponent<Planet>().ShieldAbilityStatus(true);
                    //planet[i].GetComponentInChildren<Transform>().Find("Ability").gameObject.SetActive(true);
                    //planet[i].GetComponentInChildren<AbilityBall>().UpdateStatus(true);
                    //planet[i].GetComponentInChildren<AbilityBall>().UpdateMaterial(abilitiesMaterial[(x - 1)]);
                }

            }
            /*else
            {
                ability[i] = 4;
                planet[i].GetComponentInChildren<Transform>().Find("Ability").gameObject.SetActive(true);
                planet[i].GetComponentInChildren<AbilityBall>().UpdateMaterial(abilitiesMaterial[3]);
            }*/
        }

        public void RemoveAbility(int i)
        {
            ability[i] = 0;

            planet[i].GetComponent<Planet>().DeactivateAbilities();

            //planet[i].GetComponentInChildren<Transform>().Find("Ability").gameObject.SetActive(false);
            //planet[i].GetComponentInChildren<AbilityBall>().UpdateStatus(false);
            //planet[i].GetComponentInChildren<AbilityBall>().UpdateMaterial(abilitiesMaterial[0]);
        }

        public int GetAbility(int i)
        {
            return ability[i];
        }

        public void SetAbility(int pl, int i)
        {
            RemoveAbility(pl);

            ability[pl] = i;

            if (i == 0)
            {
                planet[pl].GetComponent<Planet>().DeactivateAbilities();
            }
            else if (i == 1)
            {
                planet[pl].GetComponent<Planet>().TimeAbilityStatus(true);
            }
            else if (i == 2)
            {
                planet[pl].GetComponent<Planet>().BombAbilityStatus(true);
            }
            else if (i == 3)
            {
                planet[pl].GetComponent<Planet>().ShieldAbilityStatus(true);
            }
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

        public void BombAbility(Vector3 pos)
        {
            bomb.transform.position = pos;

            bomb.SetActive(true);

            Collider bombCollider = bomb.GetComponent<Collider>();

            int x = 0;

            foreach (GameObject pr in projectile)
            {
                if (pr.activeSelf)
                {
                    Collider projCollider = pr.GetComponent<Collider>();

                    if (bombCollider.bounds.Intersects(projCollider.bounds))
                    {
                        gameManager.GetComponent<GameManager>().ProjectileDeactivated(x);
                    }
                }
                x++;
            }

            x = 0;

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

        public void SetPulsing(bool b)
        {
            pulsingAbility = b;
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

                    if(tutorial)
                    {
                        gameManager.GetComponent<GameManager>().StartCoroutine("GameSequence"); 
                    }
                    else
                    {
                        gameManager.GetComponent<GameManager>().StartCoroutine("GameStart");
                    }


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

            bomb = (GameObject)Instantiate(bombPrefab);
            bomb.SetActive(false);

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
            //planet[0].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            planet[0].SetActive(false);

            x = Random.Range(0, planetPrefab.Length);

            planet[1] = (GameObject)Instantiate(planetPrefab[x]);
            planet[1].transform.position = new Vector3(m_HitTransform.position.x + 1.0f, m_HitTransform.position.y + 1.2f, m_HitTransform.position.z);
            //planet[1].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            planet[1].SetActive(false);

            x = Random.Range(0, planetPrefab.Length);

            planet[2] = (GameObject)Instantiate(planetPrefab[x]);
            planet[2].transform.position = new Vector3(m_HitTransform.position.x + 0.5f, m_HitTransform.position.y + 1f, m_HitTransform.position.z + 0.5f);
            //planet[2].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            planet[2].SetActive(false);

            x = Random.Range(0, planetPrefab.Length);

            planet[3] = (GameObject)Instantiate(planetPrefab[x]);
            planet[3].transform.position = new Vector3(m_HitTransform.position.x, m_HitTransform.position.y + 0.9f, m_HitTransform.position.z + 1.0f);
            //planet[3].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            planet[3].SetActive(false);

            x = Random.Range(0, planetPrefab.Length);

            planet[4] = (GameObject)Instantiate(planetPrefab[x]);
            planet[4].transform.position = new Vector3(m_HitTransform.position.x - 0.5f, m_HitTransform.position.y + 1.2f, m_HitTransform.position.z - 0.5f);
            //planet[4].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            planet[4].SetActive(false);

            x = Random.Range(0, planetPrefab.Length);

            planet[5] = (GameObject)Instantiate(planetPrefab[x]);
            planet[5].transform.position = new Vector3(m_HitTransform.position.x - 1.0f, m_HitTransform.position.y + 1.5f, m_HitTransform.position.z);
            //[5].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            planet[5].SetActive(false);

            x = Random.Range(0, planetPrefab.Length);

            planet[6] = (GameObject)Instantiate(planetPrefab[x]);
            planet[6].transform.position = new Vector3(m_HitTransform.position.x - 0.5f, m_HitTransform.position.y + 1.2f, m_HitTransform.position.z + 0.5f);
            //planet[6].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            planet[6].SetActive(false);

            x = Random.Range(0, planetPrefab.Length);

            planet[7] = (GameObject)Instantiate(planetPrefab[x]);
            planet[7].transform.position = new Vector3(m_HitTransform.position.x + 0.5f, m_HitTransform.position.y + 1f, m_HitTransform.position.z - 0.5f);
            //planet[7].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            planet[7].SetActive(false);

            x = Random.Range(0, planetPrefab.Length);

            planet[8] = (GameObject)Instantiate(planetPrefab[x]);
            planet[8].transform.position = new Vector3(m_HitTransform.position.x, m_HitTransform.position.y + 0.9f, m_HitTransform.position.z - 1.0f);
            //planet[8].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            planet[8].SetActive(false);

            x = Random.Range(0, planetPrefab.Length);

            planet[9] = (GameObject)Instantiate(planetPrefab[x]);
            planet[9].transform.position = new Vector3(m_HitTransform.position.x + 2.0f, m_HitTransform.position.y + 0.5f, m_HitTransform.position.z);
            //planet[9].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
            planet[9].SetActive(false);
        }

        public int PlanetClicked(Vector3 pos)
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

        void HandleInput()
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(inputRay, out hit))
            {
                MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();

                if (deformer)
                {
                    Vector3 point = hit.point;
                    point += hit.normal * forceOffset;
                    deformer.AddDeformingForce(point, force);
                }
            }
        }

        void Update()
        {
            gameManager.GetComponent<GameManager>().UpdateUI();
            if ((Input.touchCount > 0 && m_HitTransform != null) && !menuON /*&& !worldSpawn*/)
            {
                var touch = Input.GetTouch(0);
                if ((touch.phase == TouchPhase.Began ))//|| touch.phase == TouchPhase.Moved))
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
                        abilityPress = abilitySpawn.GetComponent<Ability>().GetAbilityStatus();
                            
                        //shieldButtonPress = gameManager.GetComponent<GameManager>().IsShieldActive();
                        Vector3 pos = new Vector3((float)hit.transform.position.x, (float)hit.transform.position.y, (float)hit.transform.position.z);

                        int number = PlanetClicked(pos);
                        int o = ProjectileClicked(pos);

                        if (pulsingAbility)
                        {
                            int ab = abilitySpawn.GetComponent<Ability>().UsingAbility();

                            if (ab == 3 && hit.collider.tag == "Planet")
                            {
                                HandleInput();

                                pulsingAbility = false;
                                gameManager.GetComponent<GameManager>().ShieldPlanet(number);
                                gameManager.GetComponent<GameManager>().DoAbility(planet[number].transform.position);
                                abilitySpawn.GetComponent<Ability>().UpdateStatus(false);

                            }
                            else if (ab == 2 && (hit.collider.tag == "Shielded" || hit.collider.tag == "Planet"))
                            {
                                HandleInput();

                                pulsingAbility = false;

                                gameManager.GetComponent<GameManager>().DoAbility(planet[number].transform.position);

                                abilitySpawn.GetComponent<Ability>().UpdateStatus(false);
                            }
                        }
                        else if (hit.collider.tag == "Planet")
                        {
                            HandleInput();

                            int z = ability[number];
                            // using ability
                            int ab = abilitySpawn.GetComponent<Ability>().UsingAbility();

                            pulsingAbility = false;

                            if (z == 1 && z != ab)
                            {
                                RemoveAbility(number);

                                abilitySpawn.GetComponent<Ability>().SetPulsing(false);
                                abilitySpawn.GetComponent<Ability>().UpdateStatus(true);
                                abilitySpawn.GetComponent<Ability>().AbilityDisplay(z);
                            }
                            else if (z == 2 && z != ab)
                            {
                                RemoveAbility(number);

                                abilitySpawn.GetComponent<Ability>().SetPulsing(false);
                                abilitySpawn.GetComponent<Ability>().UpdateStatus(true);
                                abilitySpawn.GetComponent<Ability>().AbilityDisplay(z);
                            }
                            else if (z == 3 && z != ab)
                            {
                                RemoveAbility(number);

                                abilitySpawn.GetComponent<Ability>().SetPulsing(false);
                                abilitySpawn.GetComponent<Ability>().UpdateStatus(true);
                                abilitySpawn.GetComponent<Ability>().AbilityDisplay(z);
                            }
                        }

                        if (abilityPress && hit.collider.tag == "Ability")
                        {
                            // Set it to pulsing when ability is active... when activated again make it false.
                            pulsingAbility = true;

                            gameManager.GetComponent<GameManager>().SetAbilityActive(true);

                            abilitySpawn.GetComponent<Ability>().SetPulsing(true);
                        }

                        if (hit.collider.tag == "Projectile")
                        {
                            pulsingAbility = false;

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

                    }
                }
            }

        }
    }
}