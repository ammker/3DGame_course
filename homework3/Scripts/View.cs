using UnityEngine;

public class GameView : MonoBehaviour
{
    public int currentRound = 1; // ��ǰ����
    public int totalScore = 0;   // �ܷ���
    private GUIStyle guiStyle;   // �����Զ���������ʽ

    void Start()
    {
        guiStyle = new GUIStyle();
        guiStyle.fontSize = 20;                        // ���������С
        guiStyle.normal.textColor = Color.white;       // ����������ɫ
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 30), $"Round: {currentRound}", guiStyle);
        GUI.Label(new Rect(10, 40, 200, 30), $"Score: {totalScore}", guiStyle);

    }

    // ��������
    public void UpdateRound(int round)
    {
        currentRound = round;
    }

    // �����ܷ���
    public void UpdateScore(int score)
    {
        totalScore = score;
    }
}
