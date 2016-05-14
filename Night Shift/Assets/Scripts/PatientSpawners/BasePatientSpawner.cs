﻿using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class BasePatientSpawner : MonoBehaviour {
        
        public Patient patientPrefab;

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
