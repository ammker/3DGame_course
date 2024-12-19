using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxTransition : MonoBehaviour
{
    public Material[] skyboxes; // ��պв�������
    public float transitionSpeed ; // �����ٶ�

    private Material currentSkybox; // ��ǰ��պ�
    private Material nextSkybox; // ��һ����պ�
    private float blendFactor = 0f; // �������
    private int currentSkyboxIndex = 0;
    int currentIndex;
    int maxIndex;

    void Start()
    {
        transitionSpeed = 1f;
        currentIndex = 0;
        maxIndex = 1;
        currentSkybox = skyboxes[0];
        nextSkybox = skyboxes[1];
        RenderSettings.skybox = currentSkybox;
    }

    void Update()
    {
        if (blendFactor < 1f)
        {
            blendFactor += Time.deltaTime * transitionSpeed;
            Debug.Log(blendFactor);
            RenderSettings.skybox.Lerp(currentSkybox, nextSkybox, blendFactor);
        }
        else
        {
            currentIndex = (currentIndex+1) % maxIndex;
            currentSkybox = skyboxes[currentIndex];
            nextSkybox = skyboxes[(currentIndex+1)%maxIndex];
            blendFactor = 0f; // ���û������
        }

    }
}
