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
        //�����ȡ�ɵ�����ɫ�ʹ�С
        diskColor = (DiskColor)Random.Range(0, System.Enum.GetValues(typeof(DiskColor)).Length);

        diskSize = (DiskSize)Random.Range(0, System.Enum.GetValues(typeof(DiskSize)).Length);

        if (diskColor == DiskColor.red) point = 1;
        else if (diskColor == DiskColor.green) point = 2;
        else if (diskColor == DiskColor.blue) point = 3;

        if (diskSize == DiskSize.Big) size = new Vector3(0.84f, 1.68f, 1.4f);
        else if (diskSize == DiskSize.Small) size = new Vector3(0.48f, 0.16f, 0.8f);
        else size = new Vector3(0.6f, 0.26f, 1f);

        if (ColorUtility.TryParseHtmlString(diskColor.ToString(), out color))
                GetComponent<MeshRenderer>().material.color = color;         //���÷ɵ�����ɫ
        else Debug.Log("������ɫʧ��");

        location = new Vector3 (0, 0, 0);
        // ����λ��
        transform.position = location;

        // ���ô�С
        transform.localScale = size;

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
