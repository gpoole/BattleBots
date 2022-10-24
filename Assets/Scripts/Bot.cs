    using System;
    using UnityEngine;

    public class Bot : MonoBehaviour
    {
        public void Move(float distance)
        {
            var t = transform;
            t.position += t.up * (distance * Time.deltaTime);
        }

        public void Turn(float angle)
        {
            transform.Rotate(Vector3.back, angle * Time.deltaTime);
        }
    }