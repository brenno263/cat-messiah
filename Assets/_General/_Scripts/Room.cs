using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum RoomDirection
    {
        Left,
        Both,
        Right,
    }
    
    public class Room : MonoBehaviour
    {
        #region variables

        [Header("Set in Inspector")]
        public RoomDirection roomOrientation;

        public int health;

        public int damageMultiplier;
        
        public int fireCounterResetMax;
        
        public int fireCounterResetMin;

        [Header("Set Dynamically")] 
        public int fireLevel = 0;

        public int fireCounter;
        
        //[Header("Fetched on Init")]

        #endregion
        #region monobehavior methods

        void Start()
        {
            fireCounter = Random.Range(fireCounterResetMin, fireCounterResetMax);
        }
        
        void FixedUpdate()
        {
            UpdateFire();
            UpdateDamage();
        }

        void Extinguish()
        {
            fireLevel = Math.Max(fireLevel - 2, 0);
            fireCounter = Random.Range(fireCounterResetMin, fireCounterResetMax);
        }

        bool isDestroyed()
        {
            return health <= 0;
        }

        #endregion

        #region private methods

        void UpdateFire()
        {
            if (fireLevel <= 3)
            {
                if (fireCounter == 0)
                {
                    fireCounter = Random.Range(fireCounterResetMin, fireCounterResetMax);
                    fireLevel++;
                }
                else
                {
                    fireCounter--;
                }
            }
        }

        void UpdateDamage()
        {
            health = Math.Max(health - (fireLevel * damageMultiplier), 0);
        }

        #endregion
    }
