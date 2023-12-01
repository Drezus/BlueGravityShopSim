using UnityEngine;

namespace Behaviours.Player
{
    [RequireComponent(typeof(Animator))]
    public class DressableCharacterAnimationController : MonoBehaviour
    {
        public CharacterMovement characterMov;
        public Animator anim;
        
        [Header("Animation Parameter Labels")]
        public string horizontalLabel = "Horizontal";
        public string verticalLabel = "Vertical";
        public string walkLabel = "Walking";
        public string directionLabel = "Direction";

        [HideInInspector] public Directions lastDir = 0;

        // Update is called once per frame
        void Update()
        {
            anim.SetFloat(horizontalLabel, characterMov.Horizontal);
            anim.SetFloat(verticalLabel, characterMov.Vertical);
            
            anim.SetBool(walkLabel, characterMov.moving);

            if (!characterMov.moving) return;
            
            //Sets a last direction value so the Idle animation can use it regardless of input states.
            if (Mathf.Abs(characterMov.Horizontal) > Mathf.Abs(characterMov.Vertical))
            {
                lastDir = characterMov.Horizontal > 0 ? Directions.East : Directions.West;
            }
            else
            {
                lastDir = characterMov.Vertical > 0 ? Directions.North : Directions.South;
            }
            
            anim.SetFloat(directionLabel, (float)lastDir);
        }
    }
}
