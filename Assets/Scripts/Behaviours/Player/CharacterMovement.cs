using System;
using UnityEngine;
using UnityEngine.Android;

namespace Behaviours.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterMovement : MonoBehaviour
    {
        public bool isPlayer;
        public bool canMove = true;
        
        private const string horAxis = "Horizontal";
        private const string verAxis = "Vertical";
        
        private Rigidbody2D rbody;

        [SerializeField]
        private Animator anim;

        [Header("Analog Movement")]
        [SerializeField]
        private float speed = 5;
        
        public float Horizontal => isPlayer ? Input.GetAxis(horAxis) : 0; 
        public float Vertical => isPlayer ? Input.GetAxis(verAxis) : 0;
        public bool moving;

        private void Awake()
        {
            rbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            moving = canMove && (Mathf.Abs(Horizontal) > 0 || Mathf.Abs(Vertical) > 0);
        }
        
        private void FixedUpdate()
        {
            if (!canMove) return;
            
            if (isPlayer)
            {
                Vector2 dir = new(Horizontal, Vertical);

                Vector2 pos = transform.position;
                rbody.MovePosition(pos + dir * (Time.deltaTime * speed)); 
            }
            else
            {
                //TODO: Custom code for NPC walking.
            }
        }
    }
}