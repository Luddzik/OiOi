//using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UI;

namespace UnityEngine.XR.iOS
{
    public class Spawn : MonoBehaviour
    {
        [SerializeField] private GameObject[] planetPrefab;
        [SerializeField] private Material[] planetMaterial;

        [SerializeField] private GameObject gameManager;

        private GameObject[] planet;
        private bool worldSpawn = false;

        public Transform m_HitTransform;
        public float maxRayDistance = 30.0f;
        public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer

        public void SetPlanetStatus (int i, bool status)
        {
            planet[i].SetActive(status);
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

        void Update()
        {
            if ((Input.touchCount > 0 && m_HitTransform != null) /*&& !worldSpawn*/)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
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
                        ARHitTestResultType.ARHitTestResultTypeFeaturePoint
                    };

                    foreach (ARHitTestResultType resultType in resultTypes)
                    {
                        if (HitTestWithResultType(point, resultType))
                        {
                            return;
                        }
                    }


                }
            }

        }
    }
}