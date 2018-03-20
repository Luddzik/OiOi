using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.iOS
{
    public class AbilityBall : MonoBehaviour
    {
        public void UpdateMaterial(Material mat)
        {
            GetComponent<Renderer>().material = new Material(mat);
        }

        public void UpdateStatus(bool status)
        {
            this.gameObject.SetActive(status);
        }

        // Update is called once per frame
        void Update()
        {
            transform.RotateAround(transform.parent.position, Vector3.up, 90 * Time.deltaTime);
        }
    }
}
