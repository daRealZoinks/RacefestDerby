using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;

    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;

    private float _currentMotorForce;
    public float CurrentMotorForce
    {
        set
        {
            _currentMotorForce = value;
        }
    }

    private float _currentBrakeForce;
    public float CurentBreakForce
    {
        set
        {
            _currentBrakeForce = value;
        }
    }

    private float _currentSteerAngle;
    public float CurrentSteerAngle
    {
        set
        {
            _currentSteerAngle = value;
        }
    }

    private void FixedUpdate()
    {
        HandleMotor(_currentMotorForce);
        HandleBrake(_currentBrakeForce);
        HandleSteering(_currentSteerAngle);
    }

    private void Update()
    {
        UpdateWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateWheel(rearRightWheelCollider, rearRightWheelTransform);
    }


    void HandleMotor(float motorTorque)
    {
        frontRightWheelCollider.motorTorque = motorTorque;
        frontLeftWheelCollider.motorTorque = motorTorque;
    }

    void HandleBrake(float brakeTorque)
    {
        frontRightWheelCollider.brakeTorque = brakeTorque;
        frontLeftWheelCollider.brakeTorque = brakeTorque;
        rearRightWheelCollider.brakeTorque = brakeTorque;
        rearLeftWheelCollider.brakeTorque = brakeTorque;
    }

    void HandleSteering(float steerAngle)
    {
        frontRightWheelCollider.steerAngle = steerAngle;
        frontLeftWheelCollider.steerAngle = steerAngle;
    }

    void UpdateWheel(WheelCollider wheelCollider, Transform wheelTrasform)
    {
        wheelCollider.GetWorldPose(out var position, out var rotation);
        wheelTrasform.SetPositionAndRotation(position, rotation);
    }
}
