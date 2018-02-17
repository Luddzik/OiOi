using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.XR.iOS
{
	public class WorldSpawn: MonoBehaviour
	{
		[SerializeField] private GameObject planet;
		[SerializeField] private Material[] planetMaterial;
		[SerializeField] private Material virusMaterial;
		[SerializeField] private GameObject playerPref;
		
		public Transform m_HitTransform;
		public float maxRayDistance = 30.0f;
		public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer

		private bool worldSpawn = false;

		private Vector3[] playerMovePositions;
		private GameObject[] planets;
		private GameObject player;
		private int playerPlanetPosition;

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
			Material planetMat;
			planets = new GameObject[5];
			playerMovePositions = new Vector3[5];

			int i = 0;

			planets[0] = (GameObject) Instantiate(planet);
			planets[0].transform.position = new Vector3(m_HitTransform.position.x, m_HitTransform.position.y + 1.5f, m_HitTransform.position.z + 1.0f);
			planets[0].GetComponent<Renderer>().material = new Material(planetMaterial[0]);

			planets[1] = (GameObject) Instantiate(planet);
			planets[1].transform.position = new Vector3(m_HitTransform.position.x, m_HitTransform.position.y + 1.2f, m_HitTransform.position.z - 1.0f);
			planets[1].GetComponent<Renderer>().material = new Material(planetMaterial[1]);

			planets[2] = (GameObject) Instantiate(planet);
			planets[2].transform.position = new Vector3(m_HitTransform.position.x + 1.0f, m_HitTransform.position.y + 1f, m_HitTransform.position.z + 0.5f);
			planets[2].GetComponent<Renderer>().material = new Material(planetMaterial[2]);

			planets[3] = (GameObject) Instantiate(planet);
			planets[3].transform.position = new Vector3(m_HitTransform.position.x - 1.0f, m_HitTransform.position.y + 0.9f, m_HitTransform.position.z - 0.2f);
			planets[3].GetComponent<Renderer>().material = new Material(planetMaterial[3]);

			planets[4] = (GameObject) Instantiate(planet);
			planets[4].transform.position = new Vector3(m_HitTransform.position.x + 0.1f, m_HitTransform.position.y + 1.2f, m_HitTransform.position.z);
			planets[4].GetComponent<Renderer>().material = new Material(planetMaterial[4]);

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

		}

		int ClosestPlanet( Vector3 pos )
		{
			int planetReturn = 0;
			float result = 100.0f;
			int counter = 0;
			float temp;

			foreach (GameObject planet in planets)
			{
				temp = Mathf.Abs(Mathf.Sqrt((Mathf.Pow((pos.x - planet.transform.position.x), 2.0f)) + (Mathf.Pow((pos.y - planet.transform.position.y), 2.0f)) + (Mathf.Pow((pos.y - planet.transform.position.y), 2.0f))));
				if (temp < result)
				{
					result = temp;
					planetReturn = counter;
				}
				counter++;
			}

			return planetReturn;
		}

		IEnumerator Virus()
		{

			yield return new WaitForSeconds(90.0f);

			int playerPos = GetPlanet();

			planets[playerPos].GetComponent<Renderer>().material = new Material(virusMaterial);

			yield return new WaitForSeconds(90.0f);

			while(worldSpawn)
			{
				
			}
		}

		int GetPlanet()
		{
			int planet = Random.Range(0, planets.Length);

			if (planet == playerPlanetPosition)
			{
				GetPlanet();
			}

			return planet;
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
							
					if (Physics.Raycast(ray, out hit, 30.0f)) 
					{
						if (hit.collider.tag == "Planet")
						{

							Vector3 pos = new Vector3((float)hit.transform.position.x, (float)hit.transform.position.y, (float)hit.transform.position.z);

							int planet = ClosestPlanet(pos);
							playerPlanetPosition = planet;
							player.transform.position = playerMovePositions[planet];

						}
					}

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

