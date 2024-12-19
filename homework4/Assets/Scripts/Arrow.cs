using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasHit = false;

    public float penetrationDepth = 2f; // ������Ŀ������
    public GameObject hitEffect; // ��ײ����Ч��

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        //if (hasHit) return;
        //hasHit = true;
        // ֹͣ������Ϊ
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        // �̶�����λ�úͷ���
        ContactPoint contact = collision.contacts[0];
        //transform.position = contact.point - transform.forward * penetrationDepth;
        transform.rotation = Quaternion.LookRotation(-contact.normal);
        // ����ΪĿ����Ӷ���
        transform.SetParent(collision.transform);

        if (collision.gameObject.CompareTag("StaticTarget"))
        {
            Debug.Log("Hit statictarget!");
            GameManager.Instance.AddScore(10);
        }
        else if (collision.gameObject.CompareTag("MoveTarget"))
        {
            Debug.Log("Hit movetarget!");
            GameManager.Instance.AddScore(20);
            //��������Ч��
            Instantiate(hitEffect, collision.contacts[0].point, Quaternion.identity);
            //ֹͣ����
            Animator targetAnimator = collision.gameObject.GetComponent<Animator>();
            if (targetAnimator != null)
            {
                targetAnimator.enabled = false; // ֹͣ����
            }
            Rigidbody targetRb = collision.gameObject.GetComponent<Rigidbody>();

            if (targetRb != null)
            {
                targetRb.isKinematic = false; 
            }
        }
    }
}
