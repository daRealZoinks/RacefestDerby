using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}

public class CarController : MonoBehaviour
{
    [SerializeField] private List<AxleInfo> axleInfos;

    private float _motor;
    public float Motor { set => _motor = value; }

    private float _steering;
    public float Steering { set => _steering = value; }

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        Transform visualWheel = collider.transform.GetChild(0);

        collider.GetWorldPose(out var position, out var rotation);

        visualWheel.transform.SetPositionAndRotation(position, rotation);
    }

    public void FixedUpdate()
    {
        foreach (AxleInfo axleInfo in axleInfos)
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
        }
    }
}