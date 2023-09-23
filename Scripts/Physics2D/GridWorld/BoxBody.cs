using UnityEngine;

namespace ManaSword.Physics2D.GridWorld
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BoxBody : Body
    {
        protected BoxCollider2D boxCollider2D;
        public BoxCollider2D BoxCollider2D => boxCollider2D;

        protected BoxMovementHandler boxMovementHandler;
        protected BoxSolidSnapDetecter boxSolidSnapDetecter;

        protected override void InitializeAdditional()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
            boxMovementHandler = new BoxMovementHandler(this);
            boxSolidSnapDetecter = new BoxSolidSnapDetecter(this);
        }

        protected override void LateFixedTranslateAdditional()
        {
            boxMovementHandler.HandleVelocity(ref velocity, ref nextFixedPosition);
            boxSolidSnapDetecter.DetectSnap();
        }
    }
}

