using System;
using System.Collections.Generic;

namespace UnityEngine.XR.iOS
{
	public class WorldSpawn: MonoBehaviour
	{
		[SerializeField] private GameObject planet;
		[SerializeField] private Material[] material;
		[SerializeField] private GameObject playerPref;
		
		public Transform m_HitTransform;
		public float maxRayDistance = 30.0f;
		public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer

		private bool worldSpawn = false;

		private Vector3[] playerMovePositions;

		bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
		{
			List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
			if (hitResults.Count > 0) {
				foreach (var hitResult in hitResults) {
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
			GameObject[] planets = new GameObject[5];
			playerMovePositions = new Vector3[5];

			int i = 0;

			planets[0] = (GameObject) Instantiate(planet);
			planets[0].transform.position = new Vector3(m_HitTransform.position.x, m_HitTransform.position.y + 1.5f, m_HitTransform.position.z + 1.0f);
			planets[0].GetComponent<Renderer>().material = new Material(material[0]);

			planets[1] = (GameObject) Instantiate(planet);
			planets[1].transform.position = new Vector3(m_HitTransform.position.x, m_HitTransform.position.y + 1.2f, m_HitTransform.position.z - 1.0f);
			planets[1].GetComponent<Renderer>().material = new Material(material[1]);

			planets[2] = (GameObject) Instantiate(planet);
			planets[2].transform.position = new Vector3(m_HitTransform.position.x + 1.0f, m_HitTransform.position.y + 1f, m_HitTransform.position.z + 0.5f);
			planets[2].GetComponent<Renderer>().material = new Material(material[2]);

			planets[3] = (GameObject) Instantiate(planet);
			planets[3].transform.position = new Vector3(m_HitTransform.position.x - 1.0f, m_HitTransform.position.y + 0.9f, m_HitTransform.position.z - 0.2f);
			planets[3].GetComponent<Renderer>().material = new Material(material[3]);

			planets[4] = (GameObject) Instantiate(planet);
			planets[4].transform.position = new Vector3(m_HitTransform.position.x - 0.0f, m_HitTransform.position.y + 1.2f, m_HitTransform.position.z - 0.0f);
			planets[4].GetComponent<Renderer>().material = new Material(material[4]);

			foreach (GameObject planet in planets)
			{
				GameObject pos;
				pos = GameObject.FindGameObjectWithTag("Respawn");
				playerMovePositions[i] = pos.transform.position;
				i++;
			}

			PlayerSpawn();
		}

		void PlayerSpawn() 
		{
			GameObject player = (GameObject) Instantiate(playerPref);

			player.transform.position = playerMovePositions[0];
		}

		// Update is called once per frame
		void Update () 
		{
			if ((Input.touchCount > 0 && m_HitTransform != null) && !worldSpawn)
			{
				var touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
				{
					var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
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
				}
			}

		}


	}
}

