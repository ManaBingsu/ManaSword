using UnityEngine;

namespace ManaSword.Physics2D.GridWorld
{
    public abstract class SolidSnapDetecter
    {
        public abstract bool[] IsSnapped { get; }

        public abstract void DetectSnap();
    }
}
