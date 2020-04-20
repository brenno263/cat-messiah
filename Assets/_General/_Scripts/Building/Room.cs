using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _General._Scripts.Building
{
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

        public bool hasStairs;
        
        public Door leftDoor;

        public Door rightDoor;

        public int x;

        public int y;

        public SpriteRenderer background; //this is a temporary way to visualize fire

        public FireParticleSystem fireParticleSystem;

        [Header("Set Dynamically")]
        public float fireCounter;

        [Header("Fetched on Init")]
        public Building building;

        public int FireLevel
        {
            get => _fireLevel;
            set
            {
                if (value >= 4) gameObject.SetActive(false);
                _fireLevel = value;
                fireParticleSystem.FireLevel = _fireLevel;
            }
        }

        [SerializeField]
        // ReSharper disable once InconsistentNaming
        private int _fireLevel;

        
        //[Header("Fetched on Init")]

        #endregion
        #region monobehavior methods

        private void Start()
        {
            building = Building.Singleton;
            ResetFireCounter();
        }

        private void FixedUpdate()
        {
            UpdateFire();
        }
        #endregion

        #region private methods
        
        private void UpdateFire()
        {
            if (FireLevel > 3 || FireLevel <= 0) return;
            if (fireCounter <= 0)
            {
                ResetFireCounter();
                FireLevel++;
            }
            else
            {
                fireCounter -= Time.deltaTime;
            }
        }

        public void setRoomOnFire()
        {
            FireLevel += 1;
        }
        
        private void Extinguish()
        {
            FireLevel = Math.Max(FireLevel - 2, 0);
            ResetFireCounter();
        }

        private void ResetFireCounter()
        {
            fireCounter = Random.Range(building.fireLevelAdvanceTimerMin, building.fireLevelAdvanceTimerMax);
        }
        
        #region accessors
        
        private bool IsDestroyed()
        {
            return FireLevel >= 4;
        }
        
        public bool OnFire()
        {
            return FireLevel > 0 && FireLevel < 4;
        }
        
        #endregion

        #endregion
        
    }
}