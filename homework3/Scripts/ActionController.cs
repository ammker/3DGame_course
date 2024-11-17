using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    private static ActionController _instance;

    public DiskFactory diskFactory;
    public FirstController firstController;
    public int diskCountinRound;  //本轮飞碟的数量
    public float squareSize = 10.0f;      // 目标正方形的边长
    public static ActionController GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ActionController();
        }

        return _instance;
    }

    public void Initial()
    {
        diskFactory = DiskFactory.GetInstance();
        firstController = FirstController.GetInstance();
        if (firstController != null)
        {
            firstController.OnRoundStart += OnRoundStartHandler;
        }
    }

    public void OnRoundStartHandler(int roundnum)
    {
        diskCountinRound = GetDiskCountinRound(roundnum);
        diskFactory.FreeDisk();
        while(diskFactory.GetAvailableCount() < diskCountinRound)
        {
            diskFactory.AddDisk();
        }
        SetDisk();
    }
    public int GetDiskCountinRound(int Roundnum)      //获取本轮需要安排的飞碟数
    {
        int baseDiskCount = 5;
        int growthFactor = 2;
        int diskCount = baseDiskCount + Roundnum * growthFactor;
        return diskCount;
    }

    public void SetDisk()              //设置飞碟的位置和运动方向
    {
        List<GameObject> availableList = diskFactory.availableList;
        // 获取屏幕中心的世界坐标
        Vector3 screenCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        screenCenter.z = 0; // 确保 z 轴为 0（2D 游戏场景）

        // 屏幕四角的位置
        Vector3[] corners = new Vector3[4]
        {
            Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)),          // 左上角
            Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)), // 右上角
            Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)),                        // 左下角
            Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0))             // 右下角
        };

        // 确保 z 轴统一为 0
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i].z = 0;
        }

        // 正方形四个顶点的偏移量（相对于屏幕中心）
        Vector3[] squareVertices = new Vector3[4]
        {
            screenCenter + new Vector3(-squareSize / 2, squareSize / 2, 0), // 左上
            screenCenter + new Vector3(squareSize / 2, squareSize / 2, 0),  // 右上
            screenCenter + new Vector3(-squareSize / 2, -squareSize / 2, 0),// 左下
            screenCenter + new Vector3(squareSize / 2, -squareSize / 2, 0)  // 右下
        };

        // 随机将飞碟分布到四角并设置运动方向
        foreach (GameObject disk in availableList)
        {
            if (disk == null) continue;

            // 随机选择一个屏幕四角
            int cornerIndex = Random.Range(0, 4);
            disk.transform.position = corners[cornerIndex]; // 设置飞碟初始位置

            // 随机选择正方形的一个顶点作为目标方向
            int vertexIndex = Random.Range(0, 4);
            Vector3 targetVertex = squareVertices[vertexIndex];

            // 计算运动方向
            Vector3 direction = (targetVertex - disk.transform.position).normalized;

            // 设置飞碟运动方向和速度
            Action action = disk.GetComponent<Action>();
            action.direction = direction;
            action.startMoving = true;

            disk.SetActive(true);
        }
    }

    public void Reset()
    {
        
    }
}