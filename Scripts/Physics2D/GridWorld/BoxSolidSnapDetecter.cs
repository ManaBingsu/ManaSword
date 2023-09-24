
using ManaSword.Utility;

using UnityEngine;

namespace ManaSword.Physics2D.GridWorld
{
    public class BoxSolidSnapDetecter : SolidSnapDetecter
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
        public override bool[] IsSnapped => isSnapped;

        public BoxSolidSnapDetecter(BoxBody boxBody)
        {
            this.boxBody = boxBody;
            boxSize = boxBody.BoxCollider2D.size;
            boxOffset = boxBody.BoxCollider2D.offset;
        }

        public override void DetectSnap()
        {
            for (var i = 0; i < 4; i++)
            {
                isSnapped[i] = false;

                var direction = directions[i];

                var hits = GetRaycastHits(i);
                foreach (RaycastHit2D hit in hits)
                {
                    var collider = hit.collider;
                    var size = collider.bounds.extents;
                    var center = collider.bounds.center;

                    if (collider.Equals(boxBody.BoxCollider2D))
                    {
                        continue;
                    }

                    float dotProduct = Vector2.Dot(direction, hit.normal);
                    if (collider.GetType() == typeof(BoxCollider2D))
                    {
                        if (Mathf.Approximately(dotProduct, -1f))
                        {
                            if (i == 0)
                            {
                                if (Mathf.Approximately(Mathf.Abs(boxBody.transform.position.y - center.y), (size.y + boxSize.y * 0.5f)))
                                    continue;
                            }
                            else
                            {
                                if (Mathf.Approximately(Mathf.Abs(boxBody.transform.position.x - center.x), (size.x + boxSize.x * 0.5f)))
                                    continue;
                            }

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
