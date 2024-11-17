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
        NotStarted, // 未开始
        InProgress, // 游戏进行中
        GameOver    // 游戏结束
    }

    public GameState currentState = GameState.NotStarted; // 当前游戏状态
    public int Round;
    public int Score;
    private bool isWaitingForNextRound = false; // 是否等待下一轮

    public delegate void RoundStartEvent(int roundNumber);  // 回合开始的事件
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
        //判断游戏是否结束
        if (IsGameOver())
        {
            EndGame();
        }
        // 进行中时
        if (currentState == GameState.InProgress)
        {
            // 检测飞碟是否移出场景
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

            //检测鼠标点击
            if (Input.GetMouseButtonDown(0))
            {
                DetectDiskClick();
            }

            //当前场景中飞碟被全部清空后开启下一轮
            if (diskFactory.availableList.Count == 0 && !isWaitingForNextRound)
            {
                StartCoroutine(WaitAndStartNextRound(2.0f)); // 等待 2 秒
            }

        }
    }

    //处理鼠标点击
    void DetectDiskClick()
    {
        // 获取鼠标位置，并生成射线
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 检测射线是否与飞碟的碰撞体相交
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            // 判断命中的是否是飞碟
            if (hitObject.CompareTag("Disk"))
            {
                DiskData diskData = hitObject.GetComponent<DiskData>();
                Debug.Log($"飞碟被点击！分数增加: {diskData.point}");

                // 执行飞碟点击后的逻辑，例如加分、销毁、回收等
                OnDiskClicked(hitObject);
            }
        }
    }

    //鼠标点击飞碟时
    void OnDiskClicked(GameObject disk)
    {
        DiskData diskData = disk.GetComponent<DiskData>();
        Score += diskData.point;
        diskFactory.KillDisk(disk);
    }

    private IEnumerator WaitAndStartNextRound(float delay)
    {
        isWaitingForNextRound = true; // 防止重复调用
        Debug.Log("All disks cleared. Waiting for the next round...");

        yield return new WaitForSeconds(delay); // 等待指定秒数

        StartNextRound(); // 开始下一轮
        isWaitingForNextRound = false; // 重置等待状态
    }

    //开始新回合
    public void StartNextRound()
    {
        Round++;
        Debug.Log($"回合 {Round} 开始!");
        OnRoundStart?.Invoke(Round);        // 触发回合开始事件
    }

    // 判断游戏是否结束
    private bool IsGameOver()
    {
        if (Round >= 10) return true;
        return false;
    }

    // 开始游戏逻辑
    public void StartGame()
    {
        Debug.Log("Game Started!");
        currentState = GameState.InProgress;
        StartNextRound();   // 开始第一轮
    }

    // 游戏结束逻辑
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


    // 使用 OnGUI 方法绘制按钮
    void OnGUI()
    {
        if (currentState == GameState.NotStarted)
        {
            // 绘制“开始游戏”按钮
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "Start Game"))
            {
                StartGame(); // 点击按钮后开始游戏
            }
        }
        else if (currentState == GameState.GameOver)
        {
            // 绘制“重新开始”按钮
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "Restart"))
            {
                Reset();
                StartGame(); // 重新开始游戏
            }
        }
    }
}
