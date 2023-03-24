using UnityEngine;

public class CarController : MonoBehaviour
{
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    public float maxMotorTorque = 100f;
    public float maxSteeringAngle = 30f;
    public float maxBrakeTorque = 100f;
    public float maxHandbrakeTorque = 200f;
    public float maxDriftAngle = 45f;
    public float driftStiffness = 1.5f;
    public float antiRollForce = 500f;

    private float motor;
    private float steering;
    private bool isBraking;
    private bool isHandbraking;
    private bool isDrifting;
    private float driftDirection;

    private void FixedUpdate()
    {
        // Get input from the player
        motor = maxMotorTorque * Input.GetAxis("Vertical");
        steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        isBraking = Input.GetButton("Brake");
        isHandbraking = Input.GetButton("Handbrake");

        // Steer the front wheels
        frontLeftWheel.steerAngle = steering;
        frontRightWheel.steerAngle = steering;

        // Apply motor torque to the rear wheels
        rearLeftWheel.motorTorque = motor;
        rearRightWheel.motorTorque = motor;

        // Apply braking torque to all wheels
        float brakeTorque = isBraking ? maxBrakeTorque : 0f;
        frontLeftWheel.brakeTorque = brakeTorque;
        frontRightWheel.brakeTorque = brakeTorque;
        rearLeftWheel.brakeTorque = brakeTorque;
        rearRightWheel.brakeTorque = brakeTorque;

        // Apply handbrake torque to rear wheels
        float handbrakeTorque = isHandbraking ? maxHandbrakeTorque : 0f;
        rearLeftWheel.brakeTorque += handbrakeTorque;
        rearRightWheel.brakeTorque += handbrakeTorque;

        // Check if the car is drifting
        if (Mathf.Abs(steering) > 0.1f && Mathf.Abs(frontLeftWheel.rpm) > 100f)
        {
            isDrifting = true;
            driftDirection = Mathf.Sign(steering);
        }
        else
        {
            isDrifting = false;
        }
        
if (isDrifting)
{
    float driftAngle = maxDriftAngle * steering / maxSteeringAngle;
    Vector3 driftForce = -transform.right * driftAngle * driftStiffness * rigidbody.velocity.magnitude;
    rigidbody.AddForce(driftForce, ForceMode.Force);

    float antiRollForceFront = antiRollForce * (frontLeftWheel.suspensionDistance - frontRightWheel.suspensionDistance);
    float antiRollForceRear = antiRollForce * (rearLeftWheel.suspensionDistance - rearRightWheel.suspensionDistance);
    frontLeftWheel.suspensionSpring += new JointSpring { spring = -antiRollForceFront };
    frontRightWheel.suspensionSpring += new JointSpring { spring = antiRollForceFront };
    rearLeftWheel.suspensionSpring += new JointSpring { spring = -antiRollForceRear };
    rearRightWheel.suspensionSpring += new JointSpring { spring = antiRollForceRear };
}
else
{
    // Remove drift forces from the car
    frontLeftWheel.suspensionSpring += new JointSpring { spring = antiRollForce };
    frontRightWheel.suspensionSpring += new JointSpring { spring = -antiRollForce };
    rearLeftWheel.suspensionSpring += new JointSpring { spring = antiRollForce };
    rearRightWheel.suspensionSpring += new JointSpring { spring = -antiRollForce };

    // Apply counter-steering to the car
    if (Mathf.Abs(steering) > 0.1f)
    {
        float counterSteering = -driftDirection * maxDriftAngle * Mathf.Clamp01(rigidbody.velocity.magnitude / 50f);
        frontLeftWheel.steerAngle += counterSteering;
        frontRightWheel.steerAngle += counterSteering;
    }
}
}
