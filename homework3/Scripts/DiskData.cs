using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public enum DiskColor
{
    red,
    green, 
    blue
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
    private Vector3 size;
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
        UnityEngine.Color color;
        //随机获取飞碟的颜色和大小
        diskColor = (DiskColor)Random.Range(0, System.Enum.GetValues(typeof(DiskColor)).Length);

        diskSize = (DiskSize)Random.Range(0, System.Enum.GetValues(typeof(DiskSize)).Length);

        if (diskColor == DiskColor.red) point = 1;
        else if (diskColor == DiskColor.green) point = 2;
        else if (diskColor == DiskColor.blue) point = 3;

        if (diskSize == DiskSize.Big) size = new Vector3(0.84f, 1.68f, 1.4f);
        else if (diskSize == DiskSize.Small) size = new Vector3(0.48f, 0.16f, 0.8f);
        else size = new Vector3(0.6f, 0.26f, 1f);

        if (ColorUtility.TryParseHtmlString(diskColor.ToString(), out color))
                GetComponent<MeshRenderer>().material.color = color;         //设置飞碟的颜色
        else Debug.Log("设置颜色失败");

        location = new Vector3 (0, 0, 0);
        // 设置位置
        transform.position = location;

        // 设置大小
        transform.localScale = size;

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
