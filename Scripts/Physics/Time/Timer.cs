using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManaSword.Physics.Time
{
    [System.Serializable]
    public class Timer
    {
        [SerializeField]
        protected float timeScale = 1f;
        public float TimeScale => timeScale;

        [SerializeField]

        protected float fixedTimeScale = 1f;
        public float FixedTimeScale => fixedTimeScale;

        public Timer(float scale = 1f)
        {
            SetTimeScale(scale);
        }

        public void SetTimeScale(float timeScale) => this.timeScale = timeScale;
        public void SetFixedTimeScale(float fixedTimeScale) => this.timeScale = fixedTimeScale;
    }
}
