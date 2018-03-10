//using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UI;

namespace UnityEngine.XR.iOS
{
	public class WorldSpawn: MonoBehaviour
	{
		[SerializeField] private GameObject planet;
		[SerializeField] private Material[] planetMaterial;
		[SerializeField] private Material virusMaterial;
        [SerializeField] private Material healthyPlanetMaterial; //Healthy planet material
		[SerializeField] private GameObject playerPref;
		[SerializeField] private GameObject connectionPrefab;
        [SerializeField] private GameObject healthyConnectionPrefab; //Healthy connection prefab
        [SerializeField] private GameObject bombPrefab; //Bomb prefab

		[SerializeField] private UI.Image outline;
		[SerializeField] private UI.Image alert;

		[SerializeField] private GameObject gameManager;

		[SerializeField] private GameObject planetMenuPrefab;

		private GameObject planetMenu;

        private GameObject bomb;
		
		public Transform m_HitTransform;
		public float maxRayDistance = 30.0f;
		public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer

		private bool worldSpawn = false;

		private Vector3[] playerMovePositions;
		private GameObject[] planets;
		private GameObject player;
		private int playerPlanetPosition;
		private int startInfectionPlanet;
        private int startHealthyPlanet;

        private int[] startPlanetPos;
        private int[] endPlanetPos;

		private int[] resources;
		private int winCondition = 0;
		private int bigBomb = 0;
		private int linkSolution = 0;

		private GameObject[] links;
        private GameObject[] healthLinks;

		private int linkCounter = 0;
        private int healthyLinkCounter = 0;

		private List<int> infectedPlanets = new List<int>();
        private List<int> healthyPlanets = new List<int>();

        private int linkCount = 0;

        private int menuMemory = 0;

		bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
		{
			List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
			if (hitResults.Count > 0 && !worldSpawn) 
			{
				foreach (var hitResult in hitResults) 
				{
					worldSpawn = true;

					//Debug.Log ("Got hit!");
					m_HitTransform.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
					m_HitTransform.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);

					PlanetsSpawn();

					//Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", m_HitTransform.position.x, m_HitTransform.position.y, m_HitTransform.position.z));
					return true;
				}
			}

			return false;
		}

		public int GetIngredientsNumber()
		{
			return winCondition;
		}

		public void SetIngredients(int ingredient)
		{
			winCondition -= ingredient;
		}

		public void AddBomb(int bomb)
		{
			bigBomb += bomb;
		}

		public int GetBomb()
		{
			return bigBomb;
		}

		public void AddLinkSolution(int link)
		{
			linkSolution += link;
		}

		public int GetLinkSolution()
		{
			return linkSolution;
		}

		void PlanetsSpawn () 
		{
			int objectNumber = 10;
			planets = new GameObject[objectNumber];
			playerMovePositions = new Vector3[objectNumber];

            startPlanetPos = new int[objectNumber];
            endPlanetPos = new int[objectNumber];

			links = new GameObject[objectNumber - 1];
            healthLinks = new GameObject[objectNumber - 1];

			resources = new int[objectNumber];

			int i = 0;

			i = Random.Range(0, planetMaterial.Length);

			planets[0] = (GameObject) Instantiate(planet);
			planets[0].transform.position = new Vector3(m_HitTransform.position.x, m_HitTransform.position.y + 1.5f, m_HitTransform.position.z + 1.0f);
			planets[0].GetComponent<Renderer>().material = new Material(planetMaterial[i]);

			i = Random.Range(0, planetMaterial.Length);

			planets[1] = (GameObject) Instantiate(planet);
			planets[1].transform.position = new Vector3(m_HitTransform.position.x, m_HitTransform.position.y + 1.2f, m_HitTransform.position.z - 1.0f);
			planets[1].GetComponent<Renderer>().material = new Material(planetMaterial[i]);

			i = Random.Range(0, planetMaterial.Length);

			planets[2] = (GameObject) Instantiate(planet);
			planets[2].transform.position = new Vector3(m_HitTransform.position.x + 1.0f, m_HitTransform.position.y + 1f, m_HitTransform.position.z + 0.5f);
			planets[2].GetComponent<Renderer>().material = new Material(planetMaterial[i]);

			i = Random.Range(0, planetMaterial.Length);

			planets[3] = (GameObject) Instantiate(planet);
			planets[3].transform.position = new Vector3(m_HitTransform.position.x - 1.0f, m_HitTransform.position.y + 0.9f, m_HitTransform.position.z - 0.2f);
			planets[3].GetComponent<Renderer>().material = new Material(planetMaterial[i]);

			i = Random.Range(0, planetMaterial.Length);

			planets[4] = (GameObject) Instantiate(planet);
			planets[4].transform.position = new Vector3(m_HitTransform.position.x + 0.1f, m_HitTransform.position.y + 1.2f, m_HitTransform.position.z);
			planets[4].GetComponent<Renderer>().material = new Material(planetMaterial[i]);

			i = Random.Range(0, planetMaterial.Length);

			planets[5] = (GameObject) Instantiate(planet);
			planets[5].transform.position = new Vector3(m_HitTransform.position.x + 0.5f, m_HitTransform.position.y + 1.5f, m_HitTransform.position.z - 1.0f);
			planets[5].GetComponent<Renderer>().material = new Material(planetMaterial[i]);

			i = Random.Range(0, planetMaterial.Length);

			planets[6] = (GameObject) Instantiate(planet);
			planets[6].transform.position = new Vector3(m_HitTransform.position.x - 0.5f, m_HitTransform.position.y + 1.2f, m_HitTransform.position.z + 1.0f);
			planets[6].GetComponent<Renderer>().material = new Material(planetMaterial[i]);

			i = Random.Range(0, planetMaterial.Length);

			planets[7] = (GameObject) Instantiate(planet);
			planets[7].transform.position = new Vector3(m_HitTransform.position.x + 0.5f, m_HitTransform.position.y + 1f, m_HitTransform.position.z + 1.5f);
			planets[7].GetComponent<Renderer>().material = new Material(planetMaterial[i]);

			i = Random.Range(0, planetMaterial.Length);

			planets[8] = (GameObject) Instantiate(planet);
			planets[8].transform.position = new Vector3(m_HitTransform.position.x + 1.0f, m_HitTransform.position.y + 0.9f, m_HitTransform.position.z + 0.8f);
			planets[8].GetComponent<Renderer>().material = new Material(planetMaterial[i]);

			i = Random.Range(0, planetMaterial.Length);

			planets[9] = (GameObject) Instantiate(planet);
			planets[9].transform.position = new Vector3(m_HitTransform.position.x + 0.2f, m_HitTransform.position.y + 0.5f, m_HitTransform.position.z);
			planets[9].GetComponent<Renderer>().material = new Material(planetMaterial[i]);

			i = 0;

			foreach (GameObject planet in planets)
			{
				Transform pos;
				pos = planet.transform.Find("PlayerMovementLocation").GetComponent<Transform>();
				playerMovePositions[i] = pos.position;

				resources[i] = Random.Range(0, 4);
				i++;
			}

			PlayerSpawn();

			MenuSpawn();

			StartCoroutine("Virus");
            StartCoroutine("PlanetRestore");
		}

        IEnumerator PlanetRestore()
        {
            while(worldSpawn)
            {
                yield return new WaitForSeconds(10.0f);

                for (int i = 0; i < resources.Length; i++)
                {
                    int temp = resources[i];
                    resources[i] = temp + 1;
                } 
            }

        }

		void MenuSpawn ()
		{
			planetMenu = (GameObject) Instantiate(planetMenuPrefab);
			planetMenu.SetActive(false);

		}

		void PlayerSpawn() 
		{
			player = (GameObject) Instantiate(playerPref);

			player.transform.position = playerMovePositions[0];

			playerPlanetPosition = 0;

		}

		int ClosestPlanet( Vector3 pos )
		{
			int planetReturn = 0;
			float result = 100.0f;
			int counter = 0;
			float temp;

			foreach (GameObject planet in planets)
			{
				temp = Mathf.Abs(Mathf.Sqrt((Mathf.Pow((pos.x - planet.transform.position.x), 2.0f)) + (Mathf.Pow((pos.y - planet.transform.position.y), 2.0f)) + (Mathf.Pow((pos.z - planet.transform.position.z), 2.0f))));
				if (temp < result)
				{
					result = temp;
					planetReturn = counter;
				}
				counter++;
			}

			return planetReturn;
		}

		int SpreadPlanet( )
		{
			int numberOfInfected = infectedPlanets.Count;
			int nextPlanet = 0;
			float planetDistance = 100.0f;

			float temp;
			int counter = 0;

			for (int i = 0; i < numberOfInfected; i++) 
			{
				foreach(GameObject planet in planets)
				{
                    if(planet.tag == "Planet" || planet.tag == "TempHealthyPlanet")
					{
						temp = Mathf.Abs(Mathf.Sqrt((Mathf.Pow((planets[infectedPlanets[i]].transform.position.x - planet.transform.position.x), 2.0f)) + (Mathf.Pow((planets[infectedPlanets[i]].transform.position.y - planet.transform.position.y), 2.0f)) + (Mathf.Pow((planets[infectedPlanets[i]].transform.position.z - planet.transform.position.z), 2.0f))));
						if (temp < planetDistance)
						{
                            startInfectionPlanet = infectedPlanets[i];
							planetDistance = temp;
							nextPlanet = counter;
						}
					}
					counter++;
				}
				counter = 0;
			}

            if(planets[nextPlanet].transform.tag != "Planet")
            {
                nextPlanet = SpreadPlanet();
            }

			return nextPlanet;
		}

		IEnumerator Virus()
		{

			yield return new WaitForSeconds(15.0f);

			outline.color = new Color(255,255,255,255);
			alert.color = new Color(255,255,255,255);

			yield return new WaitForSeconds(5.0f);

			int infPlanet = GetRandomPlanet();

			planets[infPlanet].GetComponent<Renderer>().material = new Material(virusMaterial);
			planets[infPlanet].tag = "Infected";

			infectedPlanets.Add(infPlanet);

			GameState(infPlanet);

			yield return new WaitForSeconds(2.0f);

			outline.color = new Color(255,255,255,0);
			alert.color = new Color(255,255,255,0);

			yield return new WaitForSeconds(8.0f);

			while(worldSpawn)
			{
                if (infectedPlanets.Count == 0)
                {
                    Win();
                }

				int i = SpreadPlanet();

				LinkSpread(i);

				planets[i].GetComponent<Renderer>().material = new Material(virusMaterial);
				planets[i].tag = "Infected";

				infectedPlanets.Add(i);

				GameState(i);

				yield return new WaitForSeconds(5.0f);
			}
		}

		void LinkSpread(int destinationPlanet)
		{
			links[linkCounter] = (GameObject) Instantiate(connectionPrefab);

            Vector3 between = planets[startInfectionPlanet].transform.position - planets[destinationPlanet].transform.position;
			float distance = between.magnitude;

			float x = links[linkCounter].transform.localScale.x;
			float y = links[linkCounter].transform.localScale.y;
			float z = links[linkCounter].transform.localScale.z;

			links[linkCounter].transform.localScale = new Vector3(x, y, z * distance * 50.0f);

            links[linkCounter].transform.position = planets[startInfectionPlanet].transform.position - (between / 2.0f);
			links[linkCounter].transform.LookAt(planets[destinationPlanet].transform);

            startPlanetPos[linkCounter] = startInfectionPlanet;
            endPlanetPos[linkCounter] = destinationPlanet;

			linkCounter++;	

		}

        IEnumerator Health()
        {
            yield return new WaitForSeconds(5.0f);

            while (worldSpawn)
            {
                int i = HealthySpreadPlanet();

                HealthyLinkSpread(i);

                if (planets[i].tag == "Infected")
                {
                    infectedPlanets.Remove(i);
                    RemoveInfectedLink(i);
                }

                planets[i].GetComponent<Renderer>().material = new Material(healthyPlanetMaterial);
                planets[i].tag = "HealthyPlanet";

                healthyPlanets.Add(i);

                linkCount++;

                if(linkCount >= 3)
                {
                    StopLink();
                }

                yield return new WaitForSeconds(5.0f);
            } 

        }

        void StopLink()
        {
            StopCoroutine("Health");
            linkCount = 0;
        }

        void RemoveInfectedLink(int i)
        {
            int counter = 0;

            foreach(int number in startPlanetPos)
            {
                if(number == i)
                {
                    links[counter].SetActive(false);
                }
                counter++;
            }

            counter = 0;

            foreach (int number in endPlanetPos)
            {
                if (number == i)
                {
                    links[counter].SetActive(false);
                }
                counter++;
            }
        }

        int HealthySpreadPlanet()
        {
            int numberOfHealth = healthyPlanets.Count;
            int nextPlanet = 0;
            float planetDistance = 100.0f;

            float temp;
            int counter = 0;

            for (int i = 0; i < numberOfHealth; i++)
            {
                foreach (GameObject planet in planets)
                {
                    if (planet.tag == "Planet" || planet.tag == "Infected" || planet.tag == "TempHealthyPlanet")
                    {
                        temp = Mathf.Abs(Mathf.Sqrt((Mathf.Pow((planets[healthyPlanets[i]].transform.position.x - planet.transform.position.x), 2.0f)) + (Mathf.Pow((planets[healthyPlanets[i]].transform.position.y - planet.transform.position.y), 2.0f)) + (Mathf.Pow((planets[healthyPlanets[i]].transform.position.z - planet.transform.position.z), 2.0f))));
                        if (temp < planetDistance)
                        {
                            startHealthyPlanet = healthyPlanets[i];
                            planetDistance = temp;
                            nextPlanet = counter;
                        }
                    }
                    counter++;
                }
                counter = 0;
            }

            return nextPlanet;
        }

        void HealthyLinkSpread(int destination)
        {
            healthLinks[healthyLinkCounter] = (GameObject)Instantiate(healthyConnectionPrefab);

            Vector3 between = planets[startHealthyPlanet].transform.position - planets[destination].transform.position;
            float distance = between.magnitude;

            float x = healthLinks[healthyLinkCounter].transform.localScale.x;
            float y = healthLinks[healthyLinkCounter].transform.localScale.y;
            float z = healthLinks[healthyLinkCounter].transform.localScale.z;

            healthLinks[healthyLinkCounter].transform.localScale = new Vector3(x, y, z * distance * 50.0f);

            healthLinks[healthyLinkCounter].transform.position = planets[startHealthyPlanet].transform.position - (between / 2.0f);
            healthLinks[healthyLinkCounter].transform.LookAt(planets[destination].transform);

            healthyLinkCounter++;
        }

		int GetRandomPlanet()
		{
			int planet = Random.Range(0, planets.Length);

            if (planet == playerPlanetPosition || planets[planet].transform.tag != "Planet")
			{
				GetRandomPlanet();
			}

			return planet;
		}

		int GetPlanet( Vector3 pos )
		{
			int returnPlanet = 0;
			int p = 0;
			foreach(GameObject planet in planets)
			{
				if (planet.transform.position.Equals(pos))
				{
					returnPlanet = p;
				}
				p++;
			}
			return returnPlanet;
		}

		void GameState (int planet)
		{
			if (planet == playerPlanetPosition)
			{
				gameManager.GetComponent<Pause>().GameOverScreen();
			}
		}

        void MenuSide()
        {
            Transform sidePos = planets[menuMemory].transform.Find("Left").GetComponent<Transform>();

            Transform cameraPos = Camera.current.transform;

            Transform left = planets[menuMemory].transform.Find("Left").GetComponent<Transform>();
            Transform right = planets[menuMemory].transform.Find("Right").GetComponent<Transform>();
            Transform front = planets[menuMemory].transform.Find("Front").GetComponent<Transform>();
            Transform back = planets[menuMemory].transform.Find("Back").GetComponent<Transform>();

            float leftDist = Mathf.Abs(Mathf.Sqrt((Mathf.Pow((cameraPos.position.x - left.position.x), 2.0f)) + (Mathf.Pow((cameraPos.position.y - left.position.y), 2.0f)) + (Mathf.Pow((cameraPos.position.z - left.position.z), 2.0f))));
            float rightDist = Mathf.Abs(Mathf.Sqrt((Mathf.Pow((cameraPos.position.x - right.position.x), 2.0f)) + (Mathf.Pow((cameraPos.position.y - right.position.y), 2.0f)) + (Mathf.Pow((cameraPos.position.z - right.position.z), 2.0f))));
            float frontDist = Mathf.Abs(Mathf.Sqrt((Mathf.Pow((cameraPos.position.x - front.position.x), 2.0f)) + (Mathf.Pow((cameraPos.position.y - front.position.y), 2.0f)) + (Mathf.Pow((cameraPos.position.z - front.position.z), 2.0f))));
            float backDist = Mathf.Abs(Mathf.Sqrt((Mathf.Pow((cameraPos.position.x - back.position.x), 2.0f)) + (Mathf.Pow((cameraPos.position.y - back.position.y), 2.0f)) + (Mathf.Pow((cameraPos.position.z - back.position.z), 2.0f))));

            if ((frontDist < backDist && frontDist < leftDist && frontDist < rightDist) || (backDist < frontDist && backDist < leftDist && backDist < rightDist))
            {
                if(leftDist < rightDist)
                {
                    sidePos = planets[menuMemory].transform.Find("Left").GetComponent<Transform>();
                }
                else
                {
                    sidePos = planets[menuMemory].transform.Find("Right").GetComponent<Transform>();
                }
            }
            else if ((leftDist < backDist && leftDist < frontDist && leftDist < rightDist) || (rightDist < backDist && rightDist < frontDist && rightDist < leftDist))
            {
                if (frontDist < backDist)
                {
                    sidePos = planets[menuMemory].transform.Find("Front").GetComponent<Transform>();
                }
                else
                {
                    sidePos = planets[menuMemory].transform.Find("Back").GetComponent<Transform>();
                }
            }

            planetMenu.transform.position = sidePos.position;
            planetMenu.transform.LookAt(cameraPos);

        }

		void PlanetMenuUpdate(int i)
		{
            //menuMemory = i;

			planetMenu.SetActive(true);

			if(i != playerPlanetPosition)
			{
				planetMenu.transform.Find("Move").gameObject.SetActive(true);
				planetMenu.transform.Find("Solution").gameObject.SetActive(false);
				planetMenu.transform.Find("Collected").gameObject.SetActive(false);
				planetMenu.transform.Find("Collect").gameObject.SetActive(false);
				planetMenu.transform.Find("CollectOrSolution").gameObject.SetActive(false);
				planetMenu.transform.Find("SolutionChoice").gameObject.SetActive(false);
			}
			if(i == playerPlanetPosition)
			{
				if((linkSolution >= 1 || bigBomb >= 1) && resources[i] > 0)
				{
					string resourceText = string.Concat("Collect: ", resources[i].ToString(), "\nIngredients");
					planetMenu.transform.Find("CollectOrSolution").Find("IngredientsText").GetComponent<TextMesh>().text = resourceText;
					planetMenu.transform.Find("CollectOrSolution").gameObject.SetActive(true);
					planetMenu.transform.Find("Solution").gameObject.SetActive(false);
					planetMenu.transform.Find("Move").gameObject.SetActive(false);
					planetMenu.transform.Find("Collected").gameObject.SetActive(false);
					planetMenu.transform.Find("Collect").gameObject.SetActive(false);
					planetMenu.transform.Find("SolutionChoice").gameObject.SetActive(false);
				}
				else if(linkSolution >= 1 || bigBomb >= 1)
				{
					planetMenu.transform.Find("Solution").gameObject.SetActive(true);
					planetMenu.transform.Find("CollectOrSolution").gameObject.SetActive(false);
					planetMenu.transform.Find("Move").gameObject.SetActive(false);
					planetMenu.transform.Find("Collected").gameObject.SetActive(false);
					planetMenu.transform.Find("Collect").gameObject.SetActive(false);
					planetMenu.transform.Find("SolutionChoice").gameObject.SetActive(false);
				}
				else if(resources[i] == 0)
				{
					planetMenu.transform.Find("Collected").gameObject.SetActive(true);
					planetMenu.transform.Find("CollectOrSolution").gameObject.SetActive(false);
					planetMenu.transform.Find("Move").gameObject.SetActive(false);
					planetMenu.transform.Find("Solution").gameObject.SetActive(false);
					planetMenu.transform.Find("Collect").gameObject.SetActive(false);
					planetMenu.transform.Find("SolutionChoice").gameObject.SetActive(false);
				}
				else if(resources[i] > 0)
				{
					string resourceText = string.Concat("Collect: ", resources[i].ToString(), "\nIngredients");
					planetMenu.transform.Find("Collect").Find("Text").GetComponent<TextMesh>().text = resourceText;
					planetMenu.transform.Find("Collect").gameObject.SetActive(true);
					planetMenu.transform.Find("CollectOrSolution").gameObject.SetActive(false);
					planetMenu.transform.Find("Move").gameObject.SetActive(false);
					planetMenu.transform.Find("Solution").gameObject.SetActive(false);
					planetMenu.transform.Find("Collected").gameObject.SetActive(false);
					planetMenu.transform.Find("SolutionChoice").gameObject.SetActive(false);
				}
			}

		}

		void MenuAction(int i)
		{
			if(planetMenu.transform.Find("Move").gameObject.activeSelf)
			{
				playerPlanetPosition = i;
				player.transform.position = playerMovePositions[i];

				planetMenu.transform.Find("Move").gameObject.SetActive(false);
				planetMenu.SetActive(false);
			}
			else if (planetMenu.transform.Find("Collect").gameObject.activeSelf)
			{
				CollectPoints();
			}
			else if (planetMenu.transform.Find("CollectOrSolution").gameObject.activeSelf)
			{
				CollectPoints();
			}
				
		}

		void ExitMenu()
		{
			planetMenu.SetActive(false);
		}

        void CollectPoints()
        {
            winCondition += resources[playerPlanetPosition];

            planetMenu.transform.Find("Collect").gameObject.SetActive(false);
            planetMenu.SetActive(false);
            resources[playerPlanetPosition] = 0;

            //MenuAction(playerPlanetPosition);
        }

		void Solution()
		{
			//gameManager.GetComponent<Pause>().WinScreen();
			planetMenu.transform.Find("SolutionChoice").gameObject.SetActive(true);
			planetMenu.transform.Find("Collect").gameObject.SetActive(false);
			planetMenu.transform.Find("CollectOrSolution").gameObject.SetActive(false);
			planetMenu.transform.Find("Move").gameObject.SetActive(false);
			planetMenu.transform.Find("Solution").gameObject.SetActive(false);
			planetMenu.transform.Find("Collected").gameObject.SetActive(false);

			planetMenu.transform.Find("SolutionChoice").Find("Bomb").gameObject.SetActive(false);
			planetMenu.transform.Find("SolutionChoice").Find("Link").gameObject.SetActive(false);

			if(bigBomb >= 1)
			{
				planetMenu.transform.Find("SolutionChoice").Find("Bomb").gameObject.SetActive(true);
			}
			if(linkSolution >= 1)
			{
				planetMenu.transform.Find("SolutionChoice").Find("Link").gameObject.SetActive(true);
			}
		}

        void HealthSpread()
        {
            planetMenu.SetActive(false);

            planets[playerPlanetPosition].GetComponent<Renderer>().material = new Material(healthyPlanetMaterial);
            planets[playerPlanetPosition].tag = "HealthyPlanet";

            healthyPlanets.Add(playerPlanetPosition);

            StartCoroutine("Health");


        }

        void HealthBomb()
        {
            planetMenu.SetActive(false);

            bomb = (GameObject)Instantiate(bombPrefab);

            bomb.transform.position = planets[playerPlanetPosition].transform.position;

            bomb.SetActive(true);

            Collider bombCollider = bomb.GetComponent<Collider>();

            int x = 0;

            foreach(GameObject planet in planets)
            {
                Collider planetCollider = planet.GetComponent<Collider>();
                if(bombCollider.bounds.Intersects(planetCollider.bounds))
                {
                    planets[x].GetComponent<Renderer>().material = new Material(healthyPlanetMaterial);
                    planets[x].transform.tag = "TempHealthyPlanet";
                }
                x++;
            }

            bomb.SetActive(false);
        }

		void Win()
		{
			gameManager.GetComponent<Pause>().WinScreen();
		}

		void Update () 
		{
            

			if ((Input.touchCount > 0 && m_HitTransform != null) /*&& !worldSpawn*/)
			{
				var touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
				{
					var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);

					Ray ray = Camera.main.ScreenPointToRay(touch.position);

					ARPoint point = new ARPoint {
						x = screenPosition.x,
						y = screenPosition.y
					};

					// prioritize reults types
					ARHitTestResultType[] resultTypes = {
						ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
						// if you want to use infinite planes use this:
						//ARHitTestResultType.ARHitTestResultTypeExistingPlane,
						//ARHitTestResultType.ARHitTestResultTypeHorizontalPlane, 
						//ARHitTestResultType.ARHitTestResultTypeFeaturePoint
					}; 

					foreach (ARHitTestResultType resultType in resultTypes)
					{
						if (HitTestWithResultType (point, resultType))
						{
							return;
						}
					}

					RaycastHit hit;

					if (Physics.Raycast(ray, out hit, 50.0f)) 
					{
						Vector3 pos = new Vector3((float)hit.transform.position.x, (float)hit.transform.position.y, (float)hit.transform.position.z);

						int number = ClosestPlanet(pos);

                        menuMemory = number;

                        MenuSide();

						if (hit.collider.tag == "Planet" || hit.collider.tag == "Infected")
						{
							if (touch.phase == TouchPhase.Began)
							{
								PlanetMenuUpdate(number);
							}

						}

						if (hit.collider.tag == "Action")
						{
							if (touch.phase == TouchPhase.Began)
							{
								MenuAction(number);
							}

						}

						if (hit.collider.tag == "Choice")
						{
							if (touch.phase == TouchPhase.Began)
							{
								Solution();
							}

						}

						if (hit.collider.tag == "Bomb")
						{
                            bigBomb -= 1;
                            HealthBomb();
						}

                        if (hit.collider.tag == "HealthyLink")
                        {
                            linkSolution -= 1;
                            HealthSpread();
                        }

						if (hit.collider.tag == "Exit")
						{
							if (touch.phase == TouchPhase.Began)
							{
								ExitMenu();
							}

						}
					}

					if (worldSpawn && planets[playerPlanetPosition].tag == "Infected")
					{
						gameManager.GetComponent<Pause>().GameOverScreen();

					}
				}
			}

		}


	}
}