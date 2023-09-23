using System;

using ManaSword.Physics.Time;

using UnityEngine;

namespace ManaSword.Physics2D.GridWorld
{
    [RequireComponent(typeof(Character), typeof(GridTransformHandler))]
    public abstract class Body : MonoBehaviour
    {
        protected Character character;
        public Character Character => character;

        protected GridTransformHandler gridTransform;
        public GridTransformHandler GridTransform => gridTransform;

        protected Vector3 velocity;
        public Vector3 Velocity => velocity;

        protected Vector3 lastFixedPosition;
        protected Vector3 nextFixedPosition;

        public Action FixedTranslateAdditionalEvent;

        //public Gravity gravity;

        protected void Awake()
        {
            //gravity = new Gravity(this);
            InitalizeEssential();
        }

        protected void InitalizeEssential()
        {
            lastFixedPosition = transform.position;
            nextFixedPosition = transform.position;

            character = GetComponent<Character>();
            gridTransform = GetComponent<GridTransformHandler>();
            InitializeAdditional();
        }

        protected virtual void InitializeAdditional() { }

        protected void Update()
        {
            var interpolationAlpha = (Time.time - Time.fixedTime) / TimeManager.LocalFixedDeltaTime(character.Timer);
            lastFixedPosition = GridSnapping.RoundToNearestGrid(lastFixedPosition);
            nextFixedPosition = GridSnapping.RoundToNearestGrid(nextFixedPosition);
            transform.Translate(Vector3.Lerp(lastFixedPosition, nextFixedPosition, interpolationAlpha) - transform.position);
        }

        protected void FixedUpdate()
        {
            FixedTranslateEssential();
            FixedTranslateAdditional();
            LateFixedTranslateAdditional();
        }

        protected void FixedTranslateEssential()
        {
            lastFixedPosition = nextFixedPosition;
        }

        protected virtual void FixedTranslateAdditional() 
        {
            FixedTranslateAdditionalEvent?.Invoke();
            //gravity.RunGravity();
        }
        protected virtual void LateFixedTranslateAdditional() { }

        public void SetVelocity(Vector3 velocity)
        {
            this.velocity = velocity;
        }
    }
}

