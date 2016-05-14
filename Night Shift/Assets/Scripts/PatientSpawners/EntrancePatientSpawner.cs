using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class EntrancePatientSpawner : BasePatientSpawner
    {
        public int MaxFramesBeforeSpawn = 600;

        private int randomFramesBeforeSpawn;

        // Use this for initialization
        void Start()
        {
            randomFramesBeforeSpawn = (int) (Random.value * MaxFramesBeforeSpawn);
        }

        // Update is called once per frame
        void Update()
        {
            if (canSpawnPatient())
            {
                Instantiate(patientPrefab, transform.position, Quaternion.identity);
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
