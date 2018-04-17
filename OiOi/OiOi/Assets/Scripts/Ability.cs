using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.iOS
{
    public class Ability : MonoBehaviour
    {
        [SerializeField] private Material[] abilityMat;

        private int abilityInUse;

        Vector3 pos;

		private void Awake()
		{
            pos = transform.position;
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
            transform.position = new Vector3(pos.x, pos.y + newY, pos.z);
            transform.RotateAround(transform.position, Vector3.up, 40 * Time.deltaTime);
        }
    }
}
