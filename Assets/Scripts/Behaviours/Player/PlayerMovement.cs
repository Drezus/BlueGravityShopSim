using System;
using UnityEngine;

namespace Behaviours.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        private const string horAxis = "Horizontal";
        private const string verAxis = "Vertical";
        
        private Rigidbody2D rbody;

        [SerializeField]
        private Animator anim;

        [Header("Analog Movement")]
        [SerializeField]
        private float speed = 5;
        
        public float Horizontal => Input.GetAxis(horAxis); 
        public float Vertical => Input.GetAxis(verAxis);
        public bool moving;

        private void Awake()
        {
            rbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            moving = Mathf.Abs(Horizontal) > 0 || Mathf.Abs(Vertical) > 0;
        }
        
        private void FixedUpdate()
        {
            Vector2 dir = new(Horizontal, Vertical);

            Vector2 pos = transform.position;
            rbody.MovePosition(pos + dir * (Time.deltaTime * speed));
        }
    }
}