using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerController playerController;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI AmmunitionText;
    public AudioSource HitSound;
    public AudioSource ExploreSound;
    public bool isInShootArea;
    public int score = 0;
    public int arrowNumber = 10;
    public RawImage RawImage;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddScore(int value)
    {
        score += value;
    }

    private void Update()
    {
        UpdateScoreUI();
        isInShootArea = playerController.isInShootArea;
        if (isInShootArea)
        {
            UpdateAmmunitionUI();
            if (Input.GetKeyDown(KeyCode.R)){
                arrowNumber = 10;
            }
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            RawImage.enabled = RawImage.enabled ? false : true;
        }
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }
    private void UpdateAmmunitionUI()
    {
        AmmunitionText.text = "Arrows: " + arrowNumber;

    }
}

