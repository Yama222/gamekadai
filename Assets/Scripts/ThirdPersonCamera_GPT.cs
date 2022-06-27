using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera_GPT : MonoBehaviour
{
    public Transform cameraTransform;
    private Transform _target;

// The distance in the x-z plane to the target

    public float distance = 0.0f; // カメラ距離
    public float height = 13.0f; // カメラ高さ

    public float angularSmoothLag = 0.3f;
    public float angularMaxSpeed = 15.0f;

    public float heightSmoothLag = 0.3f;

    public float snapSmoothLag = 0.2f;
    public float snapMaxSpeed = 720.0f;

    public float clampHeadPositionScreenSpace = 0.75f;

    public float lockCameraTimeout = 0.2f;

    private Vector3 headOffset = Vector3.zero;
    private Vector3 centerOffset = Vector3.zero;

    private float heightVelocity = 0.0f;
    private float angleVelocity = 0.0f;
    private bool snap = false;
    private PlatformInputController controller;
    private float targetHeight = 100000.0f;


    private void Awake()
    {
        if (!cameraTransform && Camera.main)
            cameraTransform = Camera.main.transform;
        if (!cameraTransform)
        {
            enabled = false;
        }

        _target = transform;
        if (_target)
        {
            controller = _target.GetComponent<PlatformInputController>();
        }

        if (controller)
        {
            CharacterController characterController = _target.GetComponent<CharacterController>();
            centerOffset = characterController.bounds.center - _target.position;
            headOffset = centerOffset;
            headOffset.y = characterController.bounds.max.y - _target.position.y;
        }

        Cut(_target, centerOffset);
    }

    void DebugDrawStuff()
    {
        Debug.DrawLine(_target.position, _target.position + headOffset);

    }

    float AngleDistance(float a, float b)
    {
        a = Mathf.Repeat(a, 360);
        b = Mathf.Repeat(b, 360);

        return Mathf.Abs(b - a);
    }

    void Apply(Transform dummyTarget, Vector3 dummyCenter)
    {
        // Early out if we don't have a target
        if (!controller)
            return;

        var targetCenter = _target.position + centerOffset;
        var targetHead = _target.position + headOffset;

        //	DebugDrawStuff();

        // Calculate the current & target rotation angles
        var originalTargetAngle = _target.eulerAngles.y;
        var currentAngle = cameraTransform.eulerAngles.y;

        // Adjust real target angle when camera is locked
        var targetAngle = originalTargetAngle;

        if (snap)
        {
            // We are close to the target, so we can stop snapping now!
            if (AngleDistance(currentAngle, originalTargetAngle) < 3.0)
                snap = false;

            currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref angleVelocity, snapSmoothLag, snapMaxSpeed);
        }
        // Normal camera motion
        else
        {
            currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref angleVelocity, angularSmoothLag, angularMaxSpeed);
        }

        // When walking always update the target height
        targetHeight = targetCenter.y + height;

        // Damp the height
        var currentHeight = cameraTransform.position.y;
        currentHeight = Mathf.SmoothDamp(currentHeight, targetHeight, ref heightVelocity, heightSmoothLag);

        // Convert the angle into a rotation, by which we then reposition the camera
        var currentRotation = Quaternion.Euler(60, 0, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        cameraTransform.position = targetCenter;
        cameraTransform.position += currentRotation * Vector3.back * distance;

        // Set the height of the camera
        cameraTransform.position = new Vector3(cameraTransform.position.x, currentHeight, cameraTransform.position.z);
    }

    void LateUpdate()
    {
        Apply(transform, Vector3.zero);
    }

    void Cut(Transform dummyTarget, Vector3 dummyCenter)
    {
        var oldHeightSmooth = heightSmoothLag;
        var oldSnapMaxSpeed = snapMaxSpeed;
        var oldSnapSmooth = snapSmoothLag;

        snapMaxSpeed = 10000;
        snapSmoothLag = 0.001f;
        heightSmoothLag = 0.001f;

        snap = true;
        Apply(transform, Vector3.zero);

        heightSmoothLag = oldHeightSmooth;
        snapMaxSpeed = oldSnapMaxSpeed;
        snapSmoothLag = oldSnapSmooth;
    }

    void SetUpRotation(Vector3 centerPos, Vector3 headPos)
    {
        // Now it's getting hairy. The devil is in the details here, the big issue is jumping of course.
        // * When jumping up and down we don't want to center the guy in screen space.
        //  This is important to give a feel for how high you jump and avoiding large camera movements.
        //   
        // * At the same time we dont want him to ever go out of screen and we want all rotations to be totally smooth.
        //
        // So here is what we will do:
        //
        // 1. We first find the rotation around the y axis. Thus he is always centered on the y-axis
        // 2. When grounded we make him be centered
        // 3. When jumping we keep the camera rotation but rotate the camera to get him back into view if his head is above some threshold
        // 4. When landing we smoothly interpolate towards centering him on screen
        Vector3 cameraPos = cameraTransform.position;
        Vector3 offsetToCenter = centerPos - cameraPos;

        // Calculate the projected center position and top position in world space
        var centerRay = cameraTransform.GetComponent<Camera> ().ViewportPointToRay(new Vector3(.5f, 0.5f, 1));
        var topRay = cameraTransform.GetComponent< Camera > ().ViewportPointToRay(new Vector3(.5f, clampHeadPositionScreenSpace, -1));

        var centerRayPos = centerRay.GetPoint(distance);
        var topRayPos = topRay.GetPoint(distance);

    }

    Vector3 GetCenterOffset()
    {
        return centerOffset;
    }
}
