using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxTransition : MonoBehaviour
{
    public Material[] skyboxes; // 天空盒材质数组
    public float transitionSpeed ; // 过渡速度

    private Material currentSkybox; // 当前天空盒
    private Material nextSkybox; // 下一个天空盒
    private float blendFactor = 0f; // 混合因子
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
            blendFactor = 0f; // 重置混合因子
        }

    }
}
