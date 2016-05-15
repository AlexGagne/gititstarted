using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class EntrancePatientSpawner : BasePatientSpawner
    {
        public int EasyMinFramesBeforeSpawn = 240;
        public int EasyMaxFramesBeforeSpawn = 540;

        public int MediumMinFramesBeforeSpawn = 180;
        public int MediumMaxFramesBeforeSpawn = 400;

        public int HardMinFramesBeforeSpawn = 120;
        public int HardMaxFramesBeforeSpawn = 300;

        // Should not be different because of the Ambulance spawn
        public int LastMinFramesBeforeSpawn = 120;
        public int LastMaxFramesBeforeSpawn = 300;

        private int currentMinFramesBeforeSpawn;
        private int currentMaxFramesBeforeSpawn;

        private int randomFramesBeforeSpawn;

        // Use this for initialization
        void Start()
        {
            switch (GameFlowManager.GamePhase)
            {
                case GameFlowState.PhaseTutorial:
                    break;
                case GameFlowState.PhaseEasy:
                    currentMinFramesBeforeSpawn = EasyMinFramesBeforeSpawn;
                    currentMaxFramesBeforeSpawn = EasyMaxFramesBeforeSpawn;
                    break;
                case GameFlowState.PhaseMedium:
                    currentMinFramesBeforeSpawn = MediumMinFramesBeforeSpawn;
                    currentMaxFramesBeforeSpawn = MediumMaxFramesBeforeSpawn;
                    break;
                case GameFlowState.PhaseHard:
                    currentMinFramesBeforeSpawn = HardMinFramesBeforeSpawn;
                    currentMaxFramesBeforeSpawn = HardMaxFramesBeforeSpawn;
                    break;
                case GameFlowState.LastPhase:
                    currentMinFramesBeforeSpawn = LastMinFramesBeforeSpawn;
                    currentMaxFramesBeforeSpawn = LastMaxFramesBeforeSpawn;
                    break;
                case GameFlowState.End:
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (canSpawnPatient())
            {
                Patient patient = null;
                switch (GameFlowManager.GamePhase)
                {
                    case GameFlowState.PhaseTutorial:
                        break;
                    case GameFlowState.PhaseEasy:
                        var randEasy = (int)(Random.Range(0, 2));
                        switch (randEasy)
                        {
                            case 0:
                                Instantiate(patientBloodPrefab, transform.position, Quaternion.identity);
                                break;
                            case 1:
                                Instantiate(patientCrazyPrefab, transform.position, Quaternion.identity);
                                break;
                        }
                        break;
                    case GameFlowState.PhaseMedium:
                        var randMedium = (int)(Random.Range(0, 4));
                        switch (randMedium)
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
                                Instantiate(patientVomitPrefab, transform.position, Quaternion.identity);
                                break;
                        }
                        break;
                    case GameFlowState.PhaseHard:
                        var randHard = (int)(Random.Range(0,5));
                        switch (randHard)
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
                        break;
                    case GameFlowState.LastPhase:
                        var randLast = (int)(Random.Range(0, 5));
                        switch (randLast)
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
                        break;
                    case GameFlowState.End:
                        break;
                }
                if (patient != null && gameManager != null)
                {
                    patient.gameManager = gameManager;
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
                randomFramesBeforeSpawn = Mathf.Clamp((int)(Random.value * currentMaxFramesBeforeSpawn), currentMinFramesBeforeSpawn, currentMaxFramesBeforeSpawn);
                return true;
            }
        }
    }
}
