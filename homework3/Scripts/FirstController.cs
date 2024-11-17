using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class FirstController : MonoBehaviour
{
    private static FirstController _instance;
    public enum GameState
    {
        NotStarted, // δ��ʼ
        InProgress, // ��Ϸ������
        GameOver    // ��Ϸ����
    }

    public GameState currentState = GameState.NotStarted; // ��ǰ��Ϸ״̬
    public int Round;
    public int Score;
    private bool isWaitingForNextRound = false; // �Ƿ�ȴ���һ��

    public delegate void RoundStartEvent(int roundNumber);  // �غϿ�ʼ���¼�
    public event RoundStartEvent OnRoundStart;

    public ActionController actionController;
    public DiskFactory diskFactory;

    public static FirstController GetInstance()
    {
        if (_instance == null)
        {
            _instance = new FirstController();
        }

        return _instance;
    }

    void Start()
    {
        currentState = GameState.NotStarted;
        actionController = ActionController.GetInstance();
        diskFactory = DiskFactory.GetInstance();
        actionController.Initial();
        diskFactory.Initial();
        Round = 0;
        Score = 0;
        StartGame();
    }

    void Update()
    {
        //�ж���Ϸ�Ƿ����
        if (IsGameOver())
        {
            EndGame();
        }
        // ������ʱ
        if (currentState == GameState.InProgress)
        {
            // ���ɵ��Ƿ��Ƴ�����
            List<GameObject> activeList = diskFactory.availableList;
            for (int i = activeList.Count - 1; i >= 0; i--)
            {
                GameObject disk = activeList[i];
                DiskData diskData = disk.GetComponent<DiskData>();
                if (!diskData.IsInScene())
                {
                    diskFactory.KillDisk(disk);
                }
            }

            //��������
            if (Input.GetMouseButtonDown(0))
            {
                DetectDiskClick();
            }

            //��ǰ�����зɵ���ȫ����պ�����һ��
            if (diskFactory.availableList.Count == 0 && !isWaitingForNextRound)
            {
                StartCoroutine(WaitAndStartNextRound(2.0f)); // �ȴ� 2 ��
            }

        }
    }

    //���������
    void DetectDiskClick()
    {
        // ��ȡ���λ�ã�����������
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // ��������Ƿ���ɵ�����ײ���ཻ
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            // �ж����е��Ƿ��Ƿɵ�
            if (hitObject.CompareTag("Disk"))
            {
                DiskData diskData = hitObject.GetComponent<DiskData>();
                Debug.Log($"�ɵ����������������: {diskData.point}");

                // ִ�зɵ��������߼�������ӷ֡����١����յ�
                OnDiskClicked(hitObject);
            }
        }
    }

    //������ɵ�ʱ
    void OnDiskClicked(GameObject disk)
    {
        DiskData diskData = disk.GetComponent<DiskData>();
        Score += diskData.point;
        diskFactory.KillDisk(disk);
    }

    private IEnumerator WaitAndStartNextRound(float delay)
    {
        isWaitingForNextRound = true; // ��ֹ�ظ�����
        Debug.Log("All disks cleared. Waiting for the next round...");

        yield return new WaitForSeconds(delay); // �ȴ�ָ������

        StartNextRound(); // ��ʼ��һ��
        isWaitingForNextRound = false; // ���õȴ�״̬
    }

    //��ʼ�»غ�
    public void StartNextRound()
    {
        Round++;
        Debug.Log($"�غ� {Round} ��ʼ!");
        OnRoundStart?.Invoke(Round);        // �����غϿ�ʼ�¼�
    }

    // �ж���Ϸ�Ƿ����
    private bool IsGameOver()
    {
        if (Round >= 10) return true;
        return false;
    }

    // ��ʼ��Ϸ�߼�
    public void StartGame()
    {
        Debug.Log("Game Started!");
        currentState = GameState.InProgress;
        StartNextRound();   // ��ʼ��һ��
    }

    // ��Ϸ�����߼�
    public void EndGame()
    {
        Debug.Log("Game Over!");
        currentState = GameState.GameOver;
    }

    public void Reset()
    {
        Round = 0;
        Score = 0;
        actionController.Reset();
        diskFactory.Reset();
    }


    // ʹ�� OnGUI �������ư�ť
    void OnGUI()
    {
        if (currentState == GameState.NotStarted)
        {
            // ���ơ���ʼ��Ϸ����ť
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "Start Game"))
            {
                StartGame(); // �����ť��ʼ��Ϸ
            }
        }
        else if (currentState == GameState.GameOver)
        {
            // ���ơ����¿�ʼ����ť
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "Restart"))
            {
                Reset();
                StartGame(); // ���¿�ʼ��Ϸ
            }
        }
    }
}
