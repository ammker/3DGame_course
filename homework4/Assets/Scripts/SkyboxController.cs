using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    public Material[] skyboxes; // ��Ҫ�л�����պ�����
    private int currentSkyboxIndex = 0;

    void Update()
    {
        // �������ּ�������պ�
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeSkybox(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeSkybox(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { ChangeSkybox(2); }

        if (Input.GetKeyDown(KeyCode.Alpha4)) { ChangeSkybox(3); }
    }

    void ChangeSkybox(int index)
    {
        if (index >= 0 && index < skyboxes.Length)
        {
            RenderSettings.skybox = skyboxes[index];
            currentSkyboxIndex = index;
            Debug.Log($"Skybox changed to: {skyboxes[index].name}");
        }
        else
        {
            Debug.LogWarning("Invalid skybox index.");
        }
    }
}
