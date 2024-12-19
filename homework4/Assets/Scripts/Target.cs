using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Vector3 initialPosition;  // 靶子的初始位置
    private Quaternion initialRotation; // 靶子的初始旋转
    private Vector3 initialScale; // 靶子的初始缩放

    void Start()
    {
        // 记录靶子的初始状态
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;
    }

    // 复位靶子的方法
    public void ResetTarget()
    {
        //删除子对象箭
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject); // 销毁每个子对象
        }
        // 恢复位置、旋转和缩放
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        transform.localScale = initialScale;
        Animator targetAnimator = GetComponent<Animator>();
        if (targetAnimator != null)
        {
            targetAnimator.enabled = true;
        }
        Rigidbody targetRb = GetComponent<Rigidbody>();

        if (targetRb != null)
        {
            targetRb.isKinematic = false; 
        }
        
    }

    void Update()
    {
        //复位功能
        if (Input.GetKeyDown(KeyCode.T))
        {
            ResetTarget();
        }
    }
}

