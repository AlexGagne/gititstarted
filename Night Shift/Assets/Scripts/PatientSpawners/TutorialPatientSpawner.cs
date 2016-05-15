using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class TutorialPatientSpawner : BasePatientSpawner
    {
        public bool canSpawnPatients = false;


        public GameObject patientTutorialBloodPrefab;
        public GameObject patientTutorialCrazyPrefab;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (canSpawnPatient())
            {
                switch (GameFlowManager.GamePhase)
                {
                    case GameFlowState.PhaseTutorial:
                        createEasyRandomPatient();
                        break;
                    case GameFlowState.PhaseEasy:
                        break;
                    case GameFlowState.PhaseMedium:
                        break;
                    case GameFlowState.PhaseHard:
                        break;
                    case GameFlowState.LastPhase:
                        break;
                    case GameFlowState.End:
                        break;
                }
            }
        }

        protected override bool canSpawnPatient()
        {
            return canSpawnPatients;
        }

        private void createEasyRandomPatient()
        {
            var rand = (int)(Random.Range(0, 2));
            Patient patient = null;
            switch (rand)
            {
                case 0:
                    patient = (Instantiate(patientTutorialBloodPrefab, transform.position, Quaternion.identity) as GameObject).transform.GetChild(0).gameObject.GetComponent<Patient>();
                    break;
                case 1:
                    patient = (Instantiate(patientTutorialCrazyPrefab, transform.position, Quaternion.identity) as GameObject).transform.GetChild(0).gameObject.GetComponent<Patient>();
                    break;
            }
            if (patient != null && gameManager != null)
            {
                patient.gameManager = gameManager;
            }
        }
    }
}
