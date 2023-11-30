using System;
using UnityEngine;

namespace Behaviours.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody2D rbody;

        [Header("Analog Movement")]
        [SerializeField]
        private float speed = 5;

        [Header("Axis Labels")]
        [SerializeField]
        private string horAxis = "Horizontal";

        [SerializeField]
        private string verAxis = "Vertical";

        private void Awake()
        {
            rbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            float hor = Input.GetAxis(horAxis);
            float ver = Input.GetAxis(verAxis);

            Vector2 dir = new(hor, ver);

            if (dir.magnitude <= 0) return;

            Vector2 pos = transform.position;
            rbody.MovePosition(pos + dir * (Time.deltaTime * speed));
        }
    }
}