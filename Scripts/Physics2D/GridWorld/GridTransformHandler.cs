
using ManaSword.Utility;

using UnityEngine;

namespace ManaSword.Physics2D.GridWorld
{
    public sealed class GridTransformHandler : MonoBehaviour
    {
        public Vector3 Position => transform.position;

        private void Awake()
        {
            transform.position = GridSnapping.RoundToNearestGrid(transform.position);
            transform.localScale = GridSnapping.RoundToNearestGrid(transform.localScale);
        }
    }
}
