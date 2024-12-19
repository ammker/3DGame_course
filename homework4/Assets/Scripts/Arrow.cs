using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasHit = false;

    public float penetrationDepth = 2f; // 箭插入目标的深度
    public GameObject hitEffect; // 碰撞粒子效果

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        //if (hasHit) return;
        //hasHit = true;
        // 停止物理行为
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        // 固定箭的位置和方向
        ContactPoint contact = collision.contacts[0];
        //transform.position = contact.point - transform.forward * penetrationDepth;
        transform.rotation = Quaternion.LookRotation(-contact.normal);
        // 设置为目标的子对象
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
            //生成粒子效果
            Instantiate(hitEffect, collision.contacts[0].point, Quaternion.identity);
            //停止动画
            Animator targetAnimator = collision.gameObject.GetComponent<Animator>();
            if (targetAnimator != null)
            {
                targetAnimator.enabled = false; // 停止动画
            }
            Rigidbody targetRb = collision.gameObject.GetComponent<Rigidbody>();

            if (targetRb != null)
            {
                targetRb.isKinematic = false; 
            }
        }
    }
}
