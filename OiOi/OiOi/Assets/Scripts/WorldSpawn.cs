using System;
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
		[SerializeField] private GameObject playerPref;
		[SerializeField] private GameObject connectionPrefab;

		[SerializeField] private UI.Image outline;
		[SerializeField] private UI.Image alert;

		[SerializeField] private GameObject gameManager;
		
		public Transform m_HitTransform;
		public float maxRayDistance = 30.0f;
		public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer

		private bool worldSpawn = false;

		private Vector3[] playerMovePositions;
		private GameObject[] planets;
		private GameObject player;
		private int playerPlanetPosition;
		private int startPlanet;

		private GameObject[] links;
		//private List<> planetConnections;
		private int linkCounter = 0;

		private List<int> infectedPlanets = new List<int>();

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

		void PlanetsSpawn () 
		{
			
			planets = new GameObject[10];
			playerMovePositions = new Vector3[10];
			links = new GameObject[9];

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
				i++;
			}

			PlayerSpawn();

			StartCoroutine("Virus");
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
					if(planet.tag == "Planet")
					{
						temp = Mathf.Abs(Mathf.Sqrt((Mathf.Pow((planets[infectedPlanets[i]].transform.position.x - planet.transform.position.x), 2.0f)) + (Mathf.Pow((planets[infectedPlanets[i]].transform.position.y - planet.transform.position.y), 2.0f)) + (Mathf.Pow((planets[infectedPlanets[i]].transform.position.z - planet.transform.position.z), 2.0f))));
						if (temp < planetDistance)
						{
							startPlanet = infectedPlanets[i];
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

			yield return new WaitForSeconds(2.0f);

			outline.color = new Color(255,255,255,0);
			alert.color = new Color(255,255,255,0);

			yield return new WaitForSeconds(8.0f);

			while(worldSpawn)
			{
				int i = SpreadPlanet();

				LinkSpread(i);

				planets[i].GetComponent<Renderer>().material = new Material(virusMaterial);
				planets[i].tag = "Infected";

				infectedPlanets.Add(i);

				if (playerPlanetPosition == i)
				{
					gameManager.GetComponent<Pause>().GameOverScreen();
				}

				yield return new WaitForSeconds(5.0f);
			}
		}

		void LinkSpread(int destinationPlanet)
		{
			links[linkCounter] = (GameObject) Instantiate(connectionPrefab);
			//link.transform.position = planets[startPlanet].transform.position;
			//link.transform.LookAt(planets[destinationPlanet].transform.position);

			Vector3 between = planets[startPlanet].transform.position - planets[destinationPlanet].transform.position;
			float distance = between.magnitude;

			float x = links[linkCounter].transform.localScale.x;
			float y = links[linkCounter].transform.localScale.y;
			float z = links[linkCounter].transform.localScale.z;

			links[linkCounter].transform.localScale = new Vector3(x, y, z * distance * 50.0f);

			links[linkCounter].transform.position = planets[startPlanet].transform.position - (between / 2.0f);
			links[linkCounter].transform.LookAt(planets[destinationPlanet].transform);


			linkCounter++;	
			//Vector3 direction = planets[startPlanet].transform.position - planets[destinationPlanet].transform.position;
			//direction.Normalize();

			//Transform pos = connections[linkCounter].transform.Find("EndPoint").GetComponent<Transform>();

			//link.transform.Translate(direction);

			//float scale = link.transform.localScale.y;
			//float newScale = scale + distance;

			//link.transform.localScale.Scale(new Vector3(1.0f, newScale, 1.0f));

		}

		int GetRandomPlanet()
		{
			int planet = Random.Range(0, planets.Length);

			if (planet == playerPlanetPosition)
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

		// Update is called once per frame
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

					RaycastHit hit;
							
					if (Physics.Raycast(ray, out hit, 50.0f)) 
					{
						if (hit.collider.tag == "Planet" || hit.collider.tag == "Infected")
						{

							Vector3 pos = new Vector3((float)hit.transform.position.x, (float)hit.transform.position.y, (float)hit.transform.position.z);

							int planet = ClosestPlanet(pos);
							playerPlanetPosition = planet;
							player.transform.position = playerMovePositions[planet];

						}
					}

					if (worldSpawn && planets[playerPlanetPosition].tag == "Infected")
					{
						gameManager.GetComponent<Pause>().GameOverScreen();

					}

					//GameObject.Find("Canvas").GetComponent<Pause>().GameOverScreen();

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
				}
			}

		}


	}
}

