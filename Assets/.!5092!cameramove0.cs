using UnityEngine;
using System.Collections;

public class CameraController0 : MonoBehaviour
{
    [Header("Camera Transform")]
    public Vector3 startPosition;
    public Vector3 startRotation;
    public Vector3 targetPosition;
    public Vector3 targetRotation;

    [Header("Move Settings")]
    public float moveDuration = 2.0f;

    [Header("Start Delay")]
