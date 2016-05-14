using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class BasePatientSpawner : MonoBehaviour {
        
        public Patient patientKnifePrefab;
        public Patient patientCrazyPrefab;
        public Patient patientBloodPrefab;
        public Patient patientVomitPrefab;
        public Patient patientPossessedPrefab;

        // Use this for initialization
        void Start() {
        }

        // Update is called once per frame
        void Update() {
        }

        protected virtual bool canSpawnPatient()
        {
            return false;
        }
    }
}
