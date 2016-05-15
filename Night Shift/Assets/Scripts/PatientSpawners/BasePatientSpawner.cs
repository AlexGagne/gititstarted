using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class BasePatientSpawner : MonoBehaviour {
        
        public GameObject patientKnifePrefab;
        public GameObject patientCrazyPrefab;
        public GameObject patientBloodPrefab;
        public GameObject patientVomitPrefab;
        public GameObject patientPossessedPrefab;

        public GameManager gameManager;

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
