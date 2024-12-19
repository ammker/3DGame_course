using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCameraFollow : MonoBehaviour
{
    public Transform targetPlayer;
    public float distanceUp = 1.3f;

    private Transform m_cameraTransform;

    void Awake()
    {
        m_cameraTransform = transform;
    }

    void LateUpdate()
    {
        if (null == targetPlayer) return;

        m_cameraTransform.position = targetPlayer.position + Vector3.up * distanceUp;
    }
}
