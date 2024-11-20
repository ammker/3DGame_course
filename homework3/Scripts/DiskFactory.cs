using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DiskFactory : MonoBehaviour
{
    private static DiskFactory _instance;

    public List<GameObject> availableList;
    public List<GameObject> usedList;
    public int nameIndex;

    public GameObject diskPrefab;// 飞碟预制体
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
        GameObject diskfrab = GameObject.Find("Disk");
        //diskPrefab = Instantiate(Resources.Load<GameObject>("Assets/Resource/Disk"));
        //diskPrefab.name = "prefab";
        //diskPrefab.AddComponent<DiskData>();
        //diskPrefab.SetActive(false);
        availableList = new List<GameObject>();
        usedList = new List<GameObject>();
        nameIndex = 0;
    }

    public void AddDisk()       //往列表中添加新的飞碟
    {
        GameObject disk;
        disk = Instantiate(diskPrefab);
        disk.name = nameIndex.ToString();
        nameIndex++;
        disk.AddComponent<DiskData>();
        disk.GetComponent<DiskData>().Initialize();
        disk.SetActive(false);
        usedList.Add(disk);
    }

    public void  PrepareDisk(int diskCountinRound)
    {
        if (usedList.Count > 0)
            FreeDisk();
        while (GetAvailableCount() < diskCountinRound)
        {
            AddDisk();
        }
    }

    // 回收飞碟
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
            Debug.Log("没有相应的活跃飞碟");
        }
    }

    public int GetAvailableCount() { return availableList.Count; }

    public void Reset()
    {
        availableList.Clear();
        usedList.Clear();
    }
}