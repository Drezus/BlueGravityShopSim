using System;
using UnityEngine;

namespace Behaviours.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody2D rbody;

        [SerializeField]
        private Animator anim;

        [Header("Analog Movement")]
        [SerializeField]
        private float speed = 5;

        [Header("Axis Labels")]
        [SerializeField]
        private string horAxis = "Horizontal";

        [SerializeField]
        private string verAxis = "Vertical";
        
        public float Horizontal => Input.GetAxis(horAxis);
        public float Vertical => Input.GetAxis(verAxis);

        private void Awake()
        {
            rbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            Vector2 dir = new(Horizontal, Vertical);

            Vector2 pos = transform.position;
            rbody.MovePosition(pos + dir * (Time.deltaTime * speed));
        }
    }
}