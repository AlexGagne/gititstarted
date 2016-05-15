using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class EntrancePatientSpawner : BasePatientSpawner
    {
        public int MaxFramesBeforeSpawn = 600;
        public int MinFramesBeforeSpawn = 180;

        private int randomFramesBeforeSpawn;

        // Use this for initialization
        void Start()
        {
            randomFramesBeforeSpawn = (int)(Random.Range(MinFramesBeforeSpawn, MaxFramesBeforeSpawn));
        }

        // Update is called once per frame
        void Update()
        {
            if (canSpawnPatient())
            {
                var rand = (int)(Random.value * 5000) % 6;
                switch (rand)
                {
                    case 0:
                        Instantiate(patientBloodPrefab, transform.position, Quaternion.identity);
                        break;
                    case 1:
                        Instantiate(patientCrazyPrefab, transform.position, Quaternion.identity);
                        break;
                    case 2:
                        Instantiate(patientKnifePrefab, transform.position, Quaternion.identity);
                        break;
                    case 3:
                        Instantiate(patientPossessedPrefab, transform.position, Quaternion.identity);
                        break;
                    case 4:
                        Instantiate(patientVomitPrefab, transform.position, Quaternion.identity);
                        break;
                }
            }
        }

        protected override bool canSpawnPatient()
        {
            if(randomFramesBeforeSpawn > 0)
            {
                randomFramesBeforeSpawn--;
                return false;
            }
            else
            {
                randomFramesBeforeSpawn = (int)(Random.value * MaxFramesBeforeSpawn);
                return true;
            }
        }
    }
}
