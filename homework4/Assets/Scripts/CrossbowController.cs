using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowController : MonoBehaviour
{
    public Animator animator; // Animator组件
    private bool Charging = false; // 是否正在蓄力
    private float chargeTime = 0f; // 当前蓄力时间
    private float maxChargeTime = 1f; // 最大蓄力时间
    float progress = 0; //蓄力百分比
    private bool full = false; //是否蓄满
    private bool isPaused = false; //蓄力中途暂停

    public GameObject arrowPrefab; // 箭的Prefab
    public Transform firePoint;    // 发射点
    public float arrowSpeed = 100f; // 箭的发射速度
    public float shootForce = 50f; // 箭的发射力
    private int arrowNumber = 0;
    public AudioSource LoadSound;
    public AudioSource ShootSound;

    public PlayerController playerController;
    public GameManager gameManager;
    public bool isInShootArea = false;
    void ShootArrow(float percent)
    {
        arrowSpeed = arrowSpeed * percent;
        // 创建箭实例
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        // 设置箭的初速度
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
        animator.speed = isPaused ? 0f : 1f; // 动画暂停或恢复
        // 检测鼠标右键
        if (isInShootArea && arrowNumber > 0)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (!Charging)
                {
                    // 开始蓄力
                    isPaused = false;
                    Charging = true;
                    animator.SetBool("Charging", true);
                    animator.SetBool("IsPaused", false);
                    LoadSound.Play();
                }
                else if (Charging)
                {
                    // 停止蓄力，进入Hold状态
                    isPaused = true;
                    Charging = false;
                    animator.SetBool("Charging", false);
                    animator.SetBool("IsPaused", true);
                }
            }
            // 蓄力逻辑
            if (Charging)
            {
                chargeTime += Time.deltaTime;
                progress = chargeTime / maxChargeTime; // 计算蓄力百分比
                animator.SetFloat("ChargeProgress", progress);
                if (chargeTime >= maxChargeTime)
                {
                    // 到达最大蓄力时间
                    chargeTime = maxChargeTime;
                    Charging = false;
                    full = true;
                    animator.SetBool("Charging", false);
                    animator.SetBool("Full", true);
                }
            }
            // 检测鼠标左键
            if (Input.GetMouseButtonDown(0))
            {
                if (isPaused || full)
                {
                    // 发射
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
