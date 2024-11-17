using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public Vector3 curruntPosition;
    public Vector3 targetPosition;

    public bool startMoving { get; set; }

    public Vector3 direction = new Vector3(1f, 0.5f, 0f); // �ƶ�����
    public float speed = 5f; // �ٶ�

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(startMoving == true)
        {
            transform.position += direction.normalized * speed * Time.deltaTime;
        }

    }
}
