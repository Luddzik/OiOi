﻿//using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UI;

namespace UnityEngine.XR.iOS
{
    public class Spawn : MonoBehaviour
    {
        [SerializeField] private GameObject[] planetPrefab;
        [SerializeField] private Material[] planetMaterial;

        [SerializeField] private GameObject pathPrefab;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private GameObject gameManager;

        private GameObject[] planet;
        private GameObject[] path;

        private int[] ability;

        private GameObject projectile;
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

        public void SetPlanetGood(int i)
        {
            planet[i].GetComponent<Renderer>().material = new Material(planetMaterial[0]);
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
        }

        public void SetProjectileStatus(bool status)
        {
            projectile.SetActive(status);
        }

        public void SetProjectileLocation(Vector3 pos)
        {
            projectile.transform.position = new Vector3(pos.x, pos.y, pos.z);
        }

        public Vector3 GetProjectileLocation()
        {
            return projectile.transform.position;
        }

        public void CreatePath(int start, int end)
        {
            Vector3 startLoc = planet[start].transform.position;
            Vector3 endLoc = planet[end].transform.position;

            Vector3 between = (startLoc - endLoc) / 10.0f;

            for (int i = 0; i < path.Length; i++)
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

        public void DeactivatePath()
        {
            for (int i = 0; i < path.Length; i++)
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
                    ability[i] = 0;
                }
            }
        }

        public void NewAbility(int i)
        {
            int x = Random.Range(0, 4);
            if (planet[i].tag == "Planet" || planet[i].tag == "Shielded")
            {
                ability[i] = x;
            }
            else
            {
                ability[i] = 0;
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
            path = new GameObject[10];
            ability = new int[10];

            projectile = (GameObject)Instantiate(projectilePrefab);
            projectile.transform.position = new Vector3(0, 0, 0);
            projectile.SetActive(false);

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

        int ClosestPlanet(Vector3 pos)
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
                        Vector3 pos = new Vector3((float)hit.transform.position.x, (float)hit.transform.position.y, (float)hit.transform.position.z);

                        int number = ClosestPlanet(pos);

                        if(hit.collider.tag == "Projectile")
                        {
                            gameManager.GetComponent<GameManager>().TutorialProjectile();
                        }
                    }
                }
            }

        }
    }
}