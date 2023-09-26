using System;

using UnityEngine;
using UnityEngine.Events;

namespace ManaSword.Physics2D.GridWorld
{
    [RequireComponent(typeof(BoxCollider2D), typeof(RaycastHit2DPriorityLayer))]
    public class InteractedBoxCollider2D : MonoBehaviour
    {
        [SerializeField]
        protected bool canPass;
        public bool CanPass => canPass;

        [SerializeField]
        protected BoxCollider2D boxCollider2D;

        protected RaycastHit2DPriorityLayer raycastHit2DPriorityLayer;

        protected void Awake()
        {
            boxCollider2D= GetComponent<BoxCollider2D>();
            raycastHit2DPriorityLayer = GetComponent<RaycastHit2DPriorityLayer>();
        }

        public virtual void RunBoxBodyInteractEvent(BoxBody boxBody)
        {
            BoxBodyInteractEvent?.Invoke(boxBody);
        }

        public Action<BoxBody> BoxBodyInteractEvent;
    }
}
