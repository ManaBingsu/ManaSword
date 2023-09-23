using UnityEngine;

namespace ManaSword.Physics.Time
{
    public static class TimeManager
    {
        public static float TimeScale = 1f;
        public static float GlobalDeltaTime => TimeScale * UnityEngine.Time.deltaTime;

        public static float FixedTimeScale = 1f;
        public static float GlobalFixedDeltaTime => FixedTimeScale * UnityEngine.Time.fixedDeltaTime;
        
        public static float LocalDeltaTime(Timer timer) => timer.TimeScale * GlobalDeltaTime;
        public static float LocalFixedDeltaTime(Timer timer) => timer.FixedTimeScale * GlobalFixedDeltaTime;
    }
}
