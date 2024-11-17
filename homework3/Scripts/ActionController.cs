using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    private static ActionController _instance;

    public DiskFactory diskFactory;
    public FirstController firstController;
    public int diskCountinRound;  //���ַɵ�������
    public float squareSize = 10.0f;      // Ŀ�������εı߳�
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
    public int GetDiskCountinRound(int Roundnum)      //��ȡ������Ҫ���ŵķɵ���
    {
        int baseDiskCount = 5;
        int growthFactor = 2;
        int diskCount = baseDiskCount + Roundnum * growthFactor;
        return diskCount;
    }

    public void SetDisk()              //���÷ɵ���λ�ú��˶�����
    {
        List<GameObject> availableList = diskFactory.availableList;
        // ��ȡ��Ļ���ĵ���������
        Vector3 screenCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        screenCenter.z = 0; // ȷ�� z ��Ϊ 0��2D ��Ϸ������

        // ��Ļ�Ľǵ�λ��
        Vector3[] corners = new Vector3[4]
        {
            Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)),          // ���Ͻ�
            Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)), // ���Ͻ�
            Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)),                        // ���½�
            Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0))             // ���½�
        };

        // ȷ�� z ��ͳһΪ 0
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i].z = 0;
        }

        // �������ĸ������ƫ�������������Ļ���ģ�
        Vector3[] squareVertices = new Vector3[4]
        {
            screenCenter + new Vector3(-squareSize / 2, squareSize / 2, 0), // ����
            screenCenter + new Vector3(squareSize / 2, squareSize / 2, 0),  // ����
            screenCenter + new Vector3(-squareSize / 2, -squareSize / 2, 0),// ����
            screenCenter + new Vector3(squareSize / 2, -squareSize / 2, 0)  // ����
        };

        // ������ɵ��ֲ����Ľǲ������˶�����
        foreach (GameObject disk in availableList)
        {
            if (disk == null) continue;

            // ���ѡ��һ����Ļ�Ľ�
            int cornerIndex = Random.Range(0, 4);
            disk.transform.position = corners[cornerIndex]; // ���÷ɵ���ʼλ��

            // ���ѡ�������ε�һ��������ΪĿ�귽��
            int vertexIndex = Random.Range(0, 4);
            Vector3 targetVertex = squareVertices[vertexIndex];

            // �����˶�����
            Vector3 direction = (targetVertex - disk.transform.position).normalized;

            // ���÷ɵ��˶�������ٶ�
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