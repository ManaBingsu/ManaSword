using UnityEngine;

namespace ManaSword.Physics2D.GridWorld
{
    public abstract class MovementHandler
    {
        public abstract void HandleVelocity(ref Vector3 bodyVelocity, ref Vector3 nextFixedPosition);
    }
}
