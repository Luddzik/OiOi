using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.iOS
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject spawn;

        IEnumerator GameSequence()
        {
            Transform spawnLoc = spawn.GetComponent<Spawn>().m_HitTransform;

            Vector3 planetLoc = spawn.GetComponent<Spawn>().m_HitTransform.position;
            Vector3 planetEndPos = spawn.GetComponent<Spawn>().GetPlanetTransform(0);

            float planetScale = spawn.GetComponent<Spawn>().GetPlanetScale(0);

            float newPlanetScale = planetScale / 10.0f;

            float temp = newPlanetScale;

            spawn.GetComponent<Spawn>().SetPlanetScale(0, new Vector3(newPlanetScale, newPlanetScale, newPlanetScale));

            spawn.GetComponent<Spawn>().SetPlanetPosition(0, planetLoc);

            spawn.GetComponent<Spawn>().SetPlanetStatus(0, true);

            Vector3 x = (planetLoc - planetEndPos);
            float s = x.magnitude;
            //spawn.GetComponent<Spawn>().SetPlanetPosition(0, planetEndPos);


            while (Vector3.Distance(planetLoc, planetEndPos) > 0.01f)
            {
                spawn.GetComponent<Spawn>().SetPlanetPosition(0, Vector3.Lerp(planetLoc, planetEndPos, s * Time.deltaTime));

                if (newPlanetScale < planetScale)
                {
                    newPlanetScale += planetScale * Time.deltaTime;
                    spawn.GetComponent<Spawn>().SetPlanetScale(0, new Vector3(newPlanetScale, newPlanetScale, newPlanetScale));
                }
                else if (newPlanetScale > planetScale)
                {
                    newPlanetScale = planetScale;
                    spawn.GetComponent<Spawn>().SetPlanetScale(0, new Vector3(newPlanetScale, newPlanetScale, newPlanetScale));
                }

                planetLoc = spawn.GetComponent<Spawn>().GetPlanetTransform(0);

                yield return new WaitForSeconds(0.1f);
            }

            //yield return new WaitForSeconds(0.1f);
        }

    }
}
