using UnityEngine;

namespace Behaviours.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationController : MonoBehaviour
    {
        public PlayerMovement playerMov;
        public Animator anim;
        
        [Header("Animation Parameter Labels")]
        public string horizontalLabel = "Horizontal";
        public string verticalLabel = "Vertical";
        public string walkLabel = "Walking";
        public string directionLabel = "Direction";

        private Directions lastDir = 0;

        // Update is called once per frame
        void Update()
        {
            anim.SetFloat(horizontalLabel, playerMov.Horizontal);
            anim.SetFloat(verticalLabel, playerMov.Vertical);

            bool moving = Mathf.Abs(playerMov.Horizontal) > 0 || Mathf.Abs(playerMov.Vertical) > 0;
            
            anim.SetBool(walkLabel, moving);

            if (!moving) return;
            
            //Sets a last direction value so the Idle animation can use it regardless of input states.
            if (Mathf.Abs(playerMov.Horizontal) > Mathf.Abs(playerMov.Vertical))
            {
                lastDir = playerMov.Horizontal > 0 ? Directions.East : Directions.West;
            }
            else
            {
                lastDir = playerMov.Vertical > 0 ? Directions.North : Directions.South;
            }
            
            anim.SetFloat(directionLabel, (float)lastDir);
        }
    }
}
