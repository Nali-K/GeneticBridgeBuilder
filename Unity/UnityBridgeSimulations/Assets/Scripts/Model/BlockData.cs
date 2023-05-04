using UnityEngine;

namespace Simulation
{
    public class BlockData
    {
        private Vector3 _position;
        private Quaternion _rotation;
        private Vector3 _up;
        private Vector3 _velocity;
        private Vector3 _angularVelocity;
        private Vector3 _inertiaTensor;
        private Quaternion _inertiaTensorRotation;
        private Vector3 _worldCenterOfMass;

        public BlockData(Rigidbody rb)
        {
            GetFromRigidbody(rb);
        }

        public void GetFromRigidbody(Rigidbody rb)
        {
            var transform = rb.transform;
            _position = transform.position;
            _rotation = transform.rotation;
            _up = transform.up;
            _velocity = rb.velocity;
            _angularVelocity =rb.angularVelocity;
            _inertiaTensor=rb.inertiaTensor;
            _inertiaTensorRotation=rb.inertiaTensorRotation;

        }

        public void ApplyToRigidbody(Rigidbody rb)
        {
            var transform = rb.transform;
             transform.position =_position;
              transform.rotation =_rotation;
              rb.velocity =_velocity;
              rb.angularVelocity = _angularVelocity;
             rb.inertiaTensor =_inertiaTensor;
             rb.inertiaTensorRotation =_inertiaTensorRotation;

        }

        public float Compare(Rigidbody rb)
        {
            return Vector3.Distance(rb.transform.up/2f, _up/2f);
        }

    }
}