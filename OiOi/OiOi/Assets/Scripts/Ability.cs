using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.iOS
{
    public class Ability : MonoBehaviour
    {
        [SerializeField] private Material[] abilityMat;

        private int abilityInUse;

        private bool pulsing = false;

        Vector3 pos;
        Vector3 scale;

		private void Awake()
		{
            pos = transform.position;
            scale = transform.localScale;
		}

		public void AbilityDisplay(int ability)
        {
            abilityInUse = ability;
            GetComponent<Renderer>().material = new Material(abilityMat[(ability - 1)]);
        }

        public int UsingAbility()
        {
            return abilityInUse;
        }

        public void SetPulsing(bool state)
        {
            pulsing = state;
        }

        public bool GetAbilityStatus()
        {
            return this.gameObject.activeSelf;
        }

        public void UpdateStatus(bool status)
        {
            this.gameObject.SetActive(status);
        }

        // Update is called once per frame
        void Update()
        {
            
            float newY = Mathf.Sin(Time.time) / 8.0f ;

            float newScale = Mathf.Sin(Time.time) / 2.0f;
                
            transform.position = new Vector3(pos.x, pos.y + newY, pos.z);
            transform.RotateAround(transform.position, Vector3.up, 40 * Time.deltaTime);

            if (pulsing)
            {
                transform.localScale = new Vector3(scale.x + newScale, scale.y + newScale, scale.z + newScale);
            }
            else
            {
                transform.localScale = new Vector3(scale.x + newY, scale.y + newY, scale.z + newY);
            }

            
        }
    }
}
