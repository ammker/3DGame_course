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

    public GameObject diskPrefab;// �ɵ�Ԥ����
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

    public void AddDisk()       //���б�������µķɵ�
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