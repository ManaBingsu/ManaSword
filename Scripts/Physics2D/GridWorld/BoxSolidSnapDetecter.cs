using UnityEngine;

namespace ManaSword.Physics2D.GridWorld
{
    public class BoxSolidSnapDetecter
    {
        private BoxBody boxBody;

        private Vector3 boxSize;
        private Vector3 boxOffset;

        private Vector2[] points = new Vector2[]
{
        new Vector2(GridSnapping.GridSize, 0f),
        new Vector2(-GridSnapping.GridSize, 0f),
        new Vector2(0f, GridSnapping.GridSize),
        new Vector2(0f, -GridSnapping.GridSize)
};
        private Vector2[] directions = new Vector2[] { Vector2.right, Vector2.left, Vector2.up, Vector2.down };
        [SerializeField]
        private bool[] isSnapped = new bool[4] { false, false, false, false };
        public bool[] IsSnapped => isSnapped;

        public BoxSolidSnapDetecter(BoxBody boxBody)
        {
            this.boxBody = boxBody;
            boxSize = boxBody.BoxCollider2D.size;
            boxOffset = boxBody.BoxCollider2D.offset;
        }

        public void DetectSnap()
        {
            for (var i = 0; i < 4; i++)
            {
                isSnapped[i] = false;

                var direction = directions[i];

                var hits = GetRaycastHits(i);
                foreach (RaycastHit2D hit in hits)
                {
                    var collier = hit.collider;
                    if (collier.Equals(boxBody.BoxCollider2D))
                    {
                        continue;
                    }

                    float dotProduct = Vector2.Dot(direction, hit.normal);
                    if (collier.GetType() == typeof(BoxCollider2D))
                    {
                        if (Mathf.Approximately(dotProduct, -1f))
                        {
                            isSnapped[i] = true;
                        }
                    }
                }
            }
        }

        public RaycastHit2D[] GetRaycastHits(int index)
        {
            Vector2 rayPoint = points[index];
            return UnityEngine.Physics2D.BoxCastAll(new Vector3(boxBody.transform.position.x + boxOffset.x + rayPoint.x, boxBody.transform.position.y + boxOffset.y + rayPoint.y), boxSize, 0, Vector2.zero, 0);
        }
    }
}
