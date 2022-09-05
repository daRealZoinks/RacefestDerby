using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    [System.Serializable]
    public class AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public AnimationCurve wheelGripCurve;
        public bool motor;
        public bool steering;
    }

    public class CarController : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private float _motor;
        public float Motor { set => _motor = value; }

        private float _steering;
        public float Steering { set => _steering = value; }

        [SerializeField] private List<AxleInfo> axleInfos;

        public void FixedUpdate()
        {
            foreach (var axleInfo in axleInfos)
            {
                if (axleInfo.steering)
                {
                    axleInfo.leftWheel.steerAngle = _steering;
                    axleInfo.rightWheel.steerAngle = _steering;
                }
                if (axleInfo.motor)
                {
                    axleInfo.leftWheel.motorTorque = _motor;
                    axleInfo.rightWheel.motorTorque = _motor;
                }
                ApplyLocalPositionToVisuals(axleInfo.leftWheel);
                ApplyLocalPositionToVisuals(axleInfo.rightWheel);

                ApplyExtremumValueToWheel(axleInfo.leftWheel, axleInfo.wheelGripCurve.Evaluate(Mathf.Abs(_rigidbody.velocity.magnitude)));
                ApplyExtremumValueToWheel(axleInfo.rightWheel, axleInfo.wheelGripCurve.Evaluate(Mathf.Abs(_rigidbody.velocity.magnitude)));
            }
        }

        private void ApplyExtremumValueToWheel(WheelCollider wheel, float value)
        {
            var leftWheelTransform = wheel.transform;
            var leftWheelPosition = leftWheelTransform.position;

            var steeringVelocity = Vector3.Cross(leftWheelTransform.right, leftWheelPosition);
            var desiredVelocityChange = -steeringVelocity.magnitude * value;

            var leftCurve = wheel.sidewaysFriction;
            leftCurve.extremumSlip = desiredVelocityChange;
            wheel.sidewaysFriction = leftCurve;
        }

        private void ApplyLocalPositionToVisuals(WheelCollider wheelCollider)
        {
            var visualWheel = wheelCollider.transform.GetChild(0);

            wheelCollider.GetWorldPose(out var position, out var rotation);

            visualWheel.transform.SetPositionAndRotation(position, rotation);
        }
    }
}