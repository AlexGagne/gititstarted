using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class AmbulancePatientSpawner : BasePatientSpawner
    {
        public int LastMinFramesBeforeSpawn = 120;
        public int LastMaxFramesBeforeSpawn = 300;

        private int randomFramesBeforeSpawn;

        private bool firstExplosionOfPatients = true;

        // Use this for initialization
        void Start()
        {
            randomFramesBeforeSpawn = (int)Random.Range(LastMinFramesBeforeSpawn, LastMaxFramesBeforeSpawn);
        }

        // Update is called once per frame
        void Update()
        {
            if (canSpawnPatient())
            {
                switch (GameFlowManager.GamePhase)
                {
                    case GameFlowState.PhaseTutorial:
                        break;
                    case GameFlowState.PhaseEasy:
                        break;
                    case GameFlowState.PhaseMedium:
                        break;
                    case GameFlowState.PhaseHard:
                        break;
                    case GameFlowState.LastPhase:
                        if (firstExplosionOfPatients)
                        {
                            for(int i = 0; i < 10; i++)
                            {
                                createRandomPatient();
                            }
                            firstExplosionOfPatients = false;
                        }
                        createRandomPatient();
                        
                        break;
                    case GameFlowState.End:
                        break;
                }
            }
        }

        protected override bool canSpawnPatient()
        {
            if (randomFramesBeforeSpawn > 0)
            {
                randomFramesBeforeSpawn--;
                return false;
            }
            else
            {
                randomFramesBeforeSpawn = Mathf.Clamp((int)(Random.value * LastMaxFramesBeforeSpawn), LastMinFramesBeforeSpawn, LastMaxFramesBeforeSpawn);
                return true;
            }
        }

        private void createRandomPatient()
        {
            var rand = (int)(Random.Range(0, 5));
            Patient patient = null;
            switch (rand)
            {
                case 0:
                    patient = (Instantiate(patientBloodPrefab, transform.position, Quaternion.identity) as GameObject).transform.GetChild(0).gameObject.GetComponent<Patient>();
                    break;
                case 1:
                    patient = (Instantiate(patientCrazyPrefab, transform.position, Quaternion.identity) as GameObject).transform.GetChild(0).gameObject.GetComponent<Patient>();
                    break;
                case 2:
                    patient = (Instantiate(patientKnifePrefab, transform.position, Quaternion.identity) as GameObject).transform.GetChild(0).gameObject.GetComponent<Patient>();
                    break;
                case 3:
                    patient = (Instantiate(patientPossessedPrefab, transform.position, Quaternion.identity) as GameObject).transform.GetChild(0).gameObject.GetComponent<Patient>();
                    break;
                case 4:
                    patient = (Instantiate(patientVomitPrefab, transform.position, Quaternion.identity) as GameObject).transform.GetChild(0).gameObject.GetComponent<Patient>();
                    break;
            }
            if(patient != null && gameManager!=null)
            {
                patient.gameManager = gameManager;
            }
        }
    }
}
