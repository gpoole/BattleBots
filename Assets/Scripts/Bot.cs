    using System;
    using System.Collections;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class Bot : MonoBehaviour
    {
        [SerializeField]
        private float _moveSpeed;

        [SerializeField] private float _turnSpeed;

        public IEnumerator MoveRoutine(float distance)
        {
            var t = transform;
            for (var moved = 0f; moved < distance;)
            {
                var step = _moveSpeed * Time.deltaTime;
                t.position += t.up * step;
                moved += step;
                yield return null;
            }
        }

        public IEnumerator TurnRoutine(float angle)
        {
            var t = transform;
            for (var turned = 0f; turned < angle;)
            {
                var step = _turnSpeed * Time.deltaTime;
                t.Rotate(Vector3.back, step);
                turned += step;
                yield return null;
            }
        }
    }