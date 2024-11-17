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

    public bool IsInScene()      //检测飞碟是否在屏幕范围内
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        // 检测飞碟是否超出屏幕范围
        if (screenPosition.x < 0 || screenPosition.x > Screen.width ||
            screenPosition.y < 0 || screenPosition.y > Screen.height)
        {
            return false; 
        }

        // 检测飞碟是否在屏幕后方（z 坐标为负表示在屏幕后）
        if (screenPosition.z < 0)
        {
            return false; // 飞碟在屏幕后
        }

        return true;
    }
}
