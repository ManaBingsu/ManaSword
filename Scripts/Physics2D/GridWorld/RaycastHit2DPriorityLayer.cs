using UnityEngine;

namespace ManaSword.Physics2D.GridWorld
{
    public class RaycastHit2DPriorityLayer : MonoBehaviour
    {
        [SerializeField]
        protected int priority;
        public int Priority => priority;
    }
}
