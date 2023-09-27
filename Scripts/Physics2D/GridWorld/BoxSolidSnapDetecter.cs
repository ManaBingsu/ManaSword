using System.Collections.Generic;
using ManaSword.Utility;
using UnityEngine;

namespace ManaSword.Physics2D.GridWorld
{
    [System.Serializable]
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

        private HashSet<Collider2D> ignoreColliders = new HashSet<Collider2D>();
        public HashSet<Collider2D> IgnoreColliders => ignoreColliders;

        public BoxSolidSnapDetecter(BoxBody boxBody)
        {
            this.boxBody = boxBody;
            boxSize = boxBody.BoxCollider2D.size;
            boxOffset = boxBody.BoxCollider2D.offset;
        }

        public override void DetectSnap()
        {
            ignoreColliders.Clear();
            for (var i = 0; i < 4; i++)
            {
                isSnapped[i] = false;

                var direction = directions[i];

                var hits = GetRaycastHits(i);
                while (!hits.IsEmpty())
                {
                    var hit = hits.Dequeue();
                    var collider = hit.collider;
                    var size = collider.bounds.extents;
                    var center = collider.bounds.center;

                    if (collider.Equals(boxBody.BoxCollider2D))
                    {
                        continue;
                    }

                    if (collider.TryGetComponent(out InteractedBoxCollider2D interactedBoxCollider2D))
                    {
                        interactedBoxCollider2D.RunBoxBodyInteractEvent(boxBody);
                        if (interactedBoxCollider2D.CanPass)
                            continue;
                    }

                    if (ignoreColliders.Contains(collider))
                    {
                        continue;
                    }

                    if (collider.isTrigger)
                    {
                        continue;
                    }

                    float dotProduct = Vector2.Dot(direction, hit.normal);
                    if (collider.GetType() == typeof(BoxCollider2D))
                    {
                        if (Mathf.Approximately(dotProduct, -1f))
                        {
                            if (i < 2)
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

        public PriorityQueue<RaycastHit2D> GetRaycastHits(int index)
        {
            Vector2 rayPoint = points[index];
            var hits = UnityEngine.Physics2D.BoxCastAll(new Vector3(boxBody.transform.position.x + boxOffset.x + rayPoint.x, boxBody.transform.position.y + boxOffset.y + rayPoint.y), boxSize, 0, Vector2.zero, 0);
            var priorityQueue = new PriorityQueue<RaycastHit2D>();
            foreach (var hit in hits)
            {
                var priority = 0;
                //SortedRaycastHit2D sortedRaycastHit2D = new SortedRaycastHit2D(hit);
                if (hit.collider.TryGetComponent(out RaycastHit2DPriorityLayer raycastHit2DPriorityLayer))
                {
                    priority = raycastHit2DPriorityLayer.Priority;
                }
                priorityQueue.Enqueue(hit, priority);
            }

            //foreach (var element in priorityQueue)
            //{
            //    UnityEngine.Debug.Log(element.Priority);
            //}

            return priorityQueue;
        }
    }
}
