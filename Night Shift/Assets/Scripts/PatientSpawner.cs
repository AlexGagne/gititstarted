using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class PatientSpawner : MonoBehaviour {

        int framesUntilNextSpawn = (int)(Random.value * 900.0f);

        public Patient patientPrefab;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            /*if (framesUntilNextSpawn > 0)
            {
                framesUntilNextSpawn--;
                return;
            }

            var patient = (Patient) Instantiate(patientPrefab, transform.position, Quaternion.identity);
            framesUntilNextSpawn = (int)(Random.value * 900.0f);*/
        }
    }
}
