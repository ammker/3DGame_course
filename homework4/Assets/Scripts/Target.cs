using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Vector3 initialPosition;  // ���ӵĳ�ʼλ��
    private Quaternion initialRotation; // ���ӵĳ�ʼ��ת
    private Vector3 initialScale; // ���ӵĳ�ʼ����

    void Start()
    {
        // ��¼���ӵĳ�ʼ״̬
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;
    }

    // ��λ���ӵķ���
    public void ResetTarget()
    {
        //ɾ���Ӷ����
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject); // ����ÿ���Ӷ���
        }
        // �ָ�λ�á���ת������
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
        //��λ����
        if (Input.GetKeyDown(KeyCode.T))
        {
            ResetTarget();
        }
    }
}

