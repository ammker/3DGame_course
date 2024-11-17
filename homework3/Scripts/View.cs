using UnityEngine;

public class GameView : MonoBehaviour
{
    public int currentRound = 1; // 当前轮数
    public int totalScore = 0;   // 总分数
    private GUIStyle guiStyle;   // 用于自定义字体样式

    void Start()
    {
        guiStyle = new GUIStyle();
        guiStyle.fontSize = 20;                        // 设置字体大小
        guiStyle.normal.textColor = Color.white;       // 设置字体颜色
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 30), $"Round: {currentRound}", guiStyle);
        GUI.Label(new Rect(10, 40, 200, 30), $"Score: {totalScore}", guiStyle);

    }

    // 更新轮数
    public void UpdateRound(int round)
    {
        currentRound = round;
    }

    // 更新总分数
    public void UpdateScore(int score)
    {
        totalScore = score;
    }
}
