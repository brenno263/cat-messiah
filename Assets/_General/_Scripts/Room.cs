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
        
        public int x;

        public int y;
        
        public RoomDirection roomOrientation;
        
        public int fireCounterResetMax;
        
        public int fireCounterResetMin;

        [Header("Set Dynamically")] 
        
        public Door rightDoor;

        public Door leftDoor;
        
        public int fireLevel = 0;

        public int fireCounter;
        
        //[Header("Fetched on Init")]

        #endregion
        #region monobehavior methods
        
        void Start()
        {
            fireCounter = Random.Range(fireCounterResetMin * 50, fireCounterResetMax * 50);
        }
        
        void FixedUpdate()
        {
            UpdateFire();
        }

        public void Extinguish()
        {
            fireLevel = Math.Max(fireLevel - 2, 0);
            fireCounter = Random.Range(fireCounterResetMin * 50, fireCounterResetMax * 50);
        }

        public bool isDestroyed()
        {
            return fireLevel >= 4;
        }

        public bool onFire()
        {
            return fireLevel > 0;
        }

        public void setRoomOnFire()
        {
            fireLevel = Math.Max(1, fireLevel);
        }

        #endregion

        #region private methods

        void UpdateFire()
        {
            if (onFire() && !isDestroyed())
            {
                if (fireCounter <= 0)
                {
                    fireCounter = Random.Range(fireCounterResetMin * 50, fireCounterResetMax * 50);
                    fireLevel++;
                }
                else
                {
                    fireCounter--;
                }
            }
        }

        #endregion
    }
