using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowController : MonoBehaviour
{
    public Animator animator; // Animator���
    private bool Charging = false; // �Ƿ���������
    private float chargeTime = 0f; // ��ǰ����ʱ��
    private float maxChargeTime = 1f; // �������ʱ��
    float progress = 0; //�����ٷֱ�
    private bool full = false; //�Ƿ�����
    private bool isPaused = false; //������;��ͣ

    public GameObject arrowPrefab; // ����Prefab
    public Transform firePoint;    // �����
    public float arrowSpeed = 100f; // ���ķ����ٶ�
    public float shootForce = 50f; // ���ķ�����
    private int arrowNumber = 0;
    public AudioSource LoadSound;
    public AudioSource ShootSound;

    public PlayerController playerController;
    public GameManager gameManager;
    public bool isInShootArea = false;
    void ShootArrow(float percent)
    {
        arrowSpeed = arrowSpeed * percent;
        // ������ʵ��
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        // ���ü��ĳ��ٶ�
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * arrowSpeed;
        }
    }

        void Update()
    {
        isInShootArea = playerController.isInShootArea;
        arrowNumber = gameManager.arrowNumber;
        animator.speed = isPaused ? 0f : 1f; // ������ͣ��ָ�
        // �������Ҽ�
        if (isInShootArea && arrowNumber > 0)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (!Charging)
                {
                    // ��ʼ����
                    isPaused = false;
                    Charging = true;
                    animator.SetBool("Charging", true);
                    animator.SetBool("IsPaused", false);
                    LoadSound.Play();
                }
                else if (Charging)
                {
                    // ֹͣ����������Hold״̬
                    isPaused = true;
                    Charging = false;
                    animator.SetBool("Charging", false);
                    animator.SetBool("IsPaused", true);
                }
            }
            // �����߼�
            if (Charging)
            {
                chargeTime += Time.deltaTime;
                progress = chargeTime / maxChargeTime; // ���������ٷֱ�
                animator.SetFloat("ChargeProgress", progress);
                if (chargeTime >= maxChargeTime)
                {
                    // �����������ʱ��
                    chargeTime = maxChargeTime;
                    Charging = false;
                    full = true;
                    animator.SetBool("Charging", false);
                    animator.SetBool("Full", true);
                }
            }
            // ���������
            if (Input.GetMouseButtonDown(0))
            {
                if (isPaused || full)
                {
                    // ����
                    animator.SetTrigger("Fire");
                    float percent = progress;
                    isPaused = false;
                    animator.SetBool("IsPaused", false);
                    chargeTime = 0;
                    full = false;
                    ShootSound.Play();

                    //StartCoroutine(ShootAfterDelay(percent));
                    ShootArrow(percent);
                    gameManager.arrowNumber--;
                }
            }
        }
        
    }
}
