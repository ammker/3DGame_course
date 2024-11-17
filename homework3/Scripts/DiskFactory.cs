using System;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour
{
    private static DiskFactory _instance;

    public List<GameObject> availableList;
    public List<GameObject> usedList;
    public GameObject Disk;// �ɵ�Ԥ����
    public static DiskFactory GetInstance()
    {
        if (_instance == null)
        {
            _instance = new DiskFactory();
        }

        return _instance;
    }

    public void Initial()
    {
        Disk = Resources.Load<GameObject>("Assets/Resource/Disk");
        availableList = new List<GameObject>();
        usedList = new List<GameObject>();
    }

    public void AddDisk()       //���б�������µķɵ�
    {
        GameObject disk;
        disk = Instantiate(Disk);
        DiskData diskComponent = disk.GetComponent<DiskData>();
        if (diskComponent == null)
        {
            disk.AddComponent<DiskData>();
        }
        disk.SetActive(false);
        usedList.Add(disk);
    }

    // ���շɵ�
    public void FreeDisk()
    {
        foreach(GameObject disk in usedList)
        {
            ResetDisk(disk);
            availableList.Add(disk);
        }
        usedList.Clear();
    }

    private void ResetDisk(GameObject disk)

    {
        disk.GetComponent<DiskData>().Initialize();
        disk.SetActive(false);
    }

    public void KillDisk(GameObject disk)
    {
        if (availableList.Contains(disk))
        {
            availableList.Remove(disk);
            disk.SetActive(false);
            usedList.Add(disk);
        }
        else
        {
            Debug.Log("û����Ӧ�Ļ�Ծ�ɵ�");
        }
    }

    public int GetAvailableCount() { return availableList.Count; }

    public void Reset()
    {
        availableList.Clear();
        usedList.Clear();
    }
}