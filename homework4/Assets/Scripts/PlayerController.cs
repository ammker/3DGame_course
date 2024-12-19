using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private CharacterController cc;
    public GameManager gameManager;
    public float moveSpeed;
    public float sprintSpeed; // 疾跑速度
    public float jumpSpeed;

    private float horizontalMove, verticalMove;
    private Vector3 dir;

    public float gravity;
    private Vector3 velocity;

    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;
    public bool isGround;
    private GameObject waypoint1;
    private GameObject waypoint2;
    public bool isInShootArea = false; // 玩家是否在射击区域内

    private void Start () {
        cc = GetComponent<CharacterController> ();
        waypoint1 = GameObject.Find("Waypoint1");
        waypoint2 = GameObject.Find("Waypoint2");
    }

    private void Update () {
        //运动逻辑
        isGround = Physics.CheckSphere (groundCheck.position, checkRadius, groundLayer);

        if (isGround && velocity.y < 0) {
            velocity.y = -2f;
        }

        horizontalMove = Input.GetAxis ("Horizontal") * moveSpeed;
        verticalMove = Input.GetAxis ("Vertical") * moveSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            horizontalMove *= sprintSpeed / moveSpeed; // 疾跑时增加速度
            verticalMove *= sprintSpeed / moveSpeed;
        }

        dir = transform.forward * verticalMove + transform.right * horizontalMove;
        cc.Move (dir * Time.deltaTime);

        if (Input.GetButtonDown ("Jump") && isGround) {
            velocity.y = jumpSpeed;
        }

        velocity.y -= gravity * Time.deltaTime;
        cc.Move (velocity * Time.deltaTime);

        //传送逻辑
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            transform.position = waypoint1.transform.position;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            transform.position = waypoint2.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ShootArea"))
        {
            isInShootArea = true;
            Debug.Log("enter!");
        }
    }

    // 离开射击区域的检测
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ShootArea"))
        {
            isInShootArea = false;
            Debug.Log("quit!");
        }
    }
}