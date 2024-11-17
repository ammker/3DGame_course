using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public enum DiskColor
{
    Red,
    Green, 
    Blue
}

public enum DiskSize
{
    Big,
    Middle,
    Small
}


public class DiskData : MonoBehaviour
{
    private DiskColor diskColor;
    private DiskSize diskSize;
    private Vector3 location;
    public int point;
    public bool isMoving;
    public bool isShow;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsInScene()) isShow = false;
        if (!isShow)
        {
            gameObject.SetActive(false);
        }
    }

    public void Initialize()
    {
        diskColor = (DiskColor)Random.Range(0, System.Enum.GetValues(typeof(DiskColor)).Length);

        diskSize = (DiskSize)Random.Range(0, System.Enum.GetValues(typeof(DiskSize)).Length);

        if (diskColor == DiskColor.Red) point = 1;
        else if (diskColor == DiskColor.Green) point = 2;
        else if (diskColor == DiskColor.Blue) point = 3;

        isShow = false;
    }

    public bool IsInScene()      //���ɵ��Ƿ�����Ļ��Χ��
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        // ���ɵ��Ƿ񳬳���Ļ��Χ
        if (screenPosition.x < 0 || screenPosition.x > Screen.width ||
            screenPosition.y < 0 || screenPosition.y > Screen.height)
        {
            return false; 
        }

        // ���ɵ��Ƿ�����Ļ�󷽣�z ����Ϊ����ʾ����Ļ��
        if (screenPosition.z < 0)
        {
            return false; // �ɵ�����Ļ��
        }

        return true;
    }
}
