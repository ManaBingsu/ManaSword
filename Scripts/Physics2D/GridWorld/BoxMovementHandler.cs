using System.Collections.Generic;
using System.Xml.Linq;

using ManaSword.Utility;

using UnityEngine;

namespace ManaSword.Physics2D.GridWorld
{
    public class BoxMovementHandler : MovementHandler
    {
        private BoxBody boxBody;

        private Vector3 boxSize;
        private Vector3 boxOffset;

        private float[] velocityDistances = new float[] { 0f, 0f };
        private Vector2[] velocityDirections = new Vector2[] { Vector2.zero, Vector2.zero };
        public Vector2[] VelocityDirections => velocityDirections;

        private HashSet<Collider2D> ignoreColliders = new HashSet<Collider2D>();
        public HashSet<Collider2D> IgnoreColliders => ignoreColliders;

        public BoxMovementHandler(BoxBody boxBody)
        {
            this.boxBody = boxBody;
            boxSize = boxBody.BoxCollider2D.size;
            boxOffset = boxBody.BoxCollider2D.offset;
        }

        public override void HandleVelocity(ref Vector3 bodyVelocity, ref Vector3 nextFixedPosition)
        {
            var velocity = bodyVelocity;
            for (var i = 0; i < 2; i++)
            {
                ignoreColliders.Clear();

                velocityDistances[i] = (i == 0) ? velocity.x : velocity.y;
                velocityDistances[i] = Mathf.Abs(velocityDistances[i]);
                velocityDirections[i] = (i == 0) ? new Vector2(velocity.normalized.x, 0f) : new Vector2(0f, velocity.normalized.y);

                var hits = GetRaycastHits(i);
                //foreach (var sortedRaycastHit in sortedRaycastHits)
                //var count = hits.Count;
                //UnityEngine.Debug.Log(hits.Count);
                while (!hits.IsEmpty())
                {
                    var hit = hits.Dequeue();
                    var collider = hit.collider;
                    if (collider.Equals(boxBody.BoxCollider2D))
                    {
                        continue;
                    }

                    if (collider.isTrigger)
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

                    float dotProduct = Vector2.Dot(velocityDirections[i], hit.normal);
                    if (collider.GetType() == typeof(BoxCollider2D))
                    {
                        var size = collider.bounds.extents;
                        var center = collider.bounds.center;
                        var bodyPosition = boxBody.transform.position;

                        if (i == 0)
                        {
                            if (!Mathf.Approximately(dotProduct, 0.0f))
                            {
                                if ((velocityDirections[i].x > 0f && center.x > boxBody.transform.position.x) || (velocityDirections[i].x < 0f && center.x < boxBody.transform.position.x))
                                {
                                    if (Mathf.Approximately(Mathf.Abs(boxBody.transform.position.y - center.y), (size.y + boxSize.y * 0.5f)))
                                        continue;
                                    velocity.x = 0f;
                                    bodyPosition.x = center.x - (size.x + boxSize.x * 0.5f) * (center.x > boxBody.transform.position.x ? 1 : -1);
                                }
                            }
                        }
                        else
                        {
                            if (!Mathf.Approximately(dotProduct, 0.0f))
                            {
                                if ((velocityDirections[i].y > 0f && center.y > boxBody.transform.position.y) || (velocityDirections[i].y < 0f && center.y < boxBody.transform.position.y))
                                {
                                    if (Mathf.Approximately(Mathf.Abs(boxBody.transform.position.x - center.x), (size.x + boxSize.x * 0.5f)))
                                        continue;
                                    velocity.y = 0f;
                                    bodyPosition.y = center.y - (size.y + boxSize.y * 0.5f) * (center.y > boxBody.transform.position.y ? 1 : -1);
                                }
                            }
                        }

                        boxBody.transform.position = bodyPosition;
                        nextFixedPosition = bodyPosition;
                    }
                }
            }

            nextFixedPosition += velocity;
            bodyVelocity = velocity;
        }

        public PriorityQueue<RaycastHit2D> GetRaycastHits(int isVertical)
        {
            var rayPoint = new Vector2(boxBody.transform.position.x + boxOffset.x, boxBody.transform.position.y + boxOffset.y);
            var hits = UnityEngine.Physics2D.BoxCastAll(rayPoint, boxSize, 0, velocityDirections[isVertical], velocityDistances[isVertical]);
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

    [System.Serializable]
    public class SortedRaycastHit2D
    {
        public int Priority = 0;
        private RaycastHit2D raycastHit2D;
        public RaycastHit2D RaycastHit2D => raycastHit2D;

        public SortedRaycastHit2D(RaycastHit2D raycastHit2D, int prioirty = 0)
        {
            this.raycastHit2D = raycastHit2D;
            Priority = prioirty;
        }
    }

    public class SortedRaycastHit2DCompare : IComparer<SortedRaycastHit2D>
    {
        public int Compare(SortedRaycastHit2D x, SortedRaycastHit2D y)
        {
            return Mathf.Max(x.Priority, y.Priority);
        }
    }
}
