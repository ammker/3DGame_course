using UnityEngine;
using System.Collections.Generic;

public class GameModel
{
    public int[,] Grid { get; private set; } // 图案矩阵
    public bool[,] IsCleared { get; private set; } // 消除状态矩阵
    private int gridSize = 6; // 矩阵大小
    private int iconTypes = 5; // 图案类型数量

    public GameModel()
    {
        Grid = new int[gridSize, gridSize];
        IsCleared = new bool[gridSize, gridSize];
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        List<int> iconList = new List<int>();
        int totalCells = gridSize * gridSize;
        // 添加每种图案的成对元素
        for (int i = 1; i <= totalCells / 2; i++)
        {
            int icon = (i % iconTypes) + 1; // 确保图案编号在iconTypes范围内
            iconList.Add(icon); // 添加第一个图案
            iconList.Add(icon); // 添加第二个图案
        }

        // 打乱图案列表顺序
        System.Random rand = new System.Random();
        for (int i = iconList.Count - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            // 交换 iconList[i] 和 iconList[j]
            int temp = iconList[i];
            iconList[i] = iconList[j];
            iconList[j] = temp;
        }

        // 将打乱后的图案列表填充到矩阵中
        int index = 0;
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                Grid[i, j] = iconList[index];
                IsCleared[i, j] = false; 
                index++;
            }
        }
    }

    // 判断两个位置是否相同且未消除
    public bool AreMatching(int x1, int y1, int x2, int y2)
    {
        if (Grid[x1, y1] == Grid[x2, y2] && !IsCleared[x1, y1] && !IsCleared[x2, y2])
        {
            IsCleared[x1, y1] = true;
            IsCleared[x2, y2] = true;
            return true;
        }
        return false;
    }

    // 检查游戏是否结束
    public bool IsGameOver()
    {
        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                if (!IsCleared[i, j])
                    return false;
            }
        }
        return true;
    }
}

public class GameView
{
    public delegate void IconClickHandler(int x, int y); // 点击图标事件
    public event IconClickHandler OnIconClicked;

    private int buttonSize = 50; // 图标按钮的大小
    public bool GameOver { get; set; } // 游戏是否结束标志

    // 绘制整个图案矩阵
    public void DrawGrid(int[,] grid, bool[,] isCleared)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            GUILayout.BeginHorizontal(); // 绘制一行
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                // 如果图案未被消除，绘制按钮
                if (!isCleared[i, j])
                {
                    if (GUILayout.Button(grid[i, j].ToString(), GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
                    {
                        OnIconClicked?.Invoke(i, j); // 点击时触发事件
                    }
                }
                else
                {
                    // 图案消除后显示空白
                    GUILayout.Button("", GUILayout.Width(buttonSize), GUILayout.Height(buttonSize));
                }
            }
            GUILayout.EndHorizontal();
        }

        // 如果游戏结束，显示提示信息和重启按钮
        if (GameOver)
        {
            GUILayout.Label("Game Over!"); // 游戏结束提示
            if (GUILayout.Button("Restart"))
            {
                // 触发重启游戏的事件
                OnRestartRequested?.Invoke();
            }
        }
    }
    public event System.Action OnRestartRequested; // 重启游戏事件
}

public class GameController
{
    private GameModel model;
    private GameView view;

    private int? selectedX = null;
    private int? selectedY = null;

    public GameController(GameModel model, GameView view)
    {
        this.model = model;
        this.view = view;

        // 订阅View的点击事件
        this.view.OnIconClicked += HandleIconClick;
        this.view.OnRestartRequested += RestartGame; // 订阅重启事件
    }

    // 处理图案点击事件
    private void HandleIconClick(int x, int y)
    {
        if (selectedX == null && selectedY == null)
        {
            // 如果没有选择图案，记录第一次点击的位置
            selectedX = x;
            selectedY = y;
        }
        else
        {
            // 如果已经选择了第一个图案，处理第二次点击
            if (model.AreMatching(selectedX.Value, selectedY.Value, x, y))
            {
                Debug.Log("Match Found!");
            }
            else
            {
                Debug.Log("Not a Match!");
            }
            // 重置选择
            selectedX = null;
            selectedY = null;
        }

        // 检查游戏是否结束
        if (model.IsGameOver())
        {
            view.GameOver = true; // 设置游戏结束状态
        }
    }

    // 重启游戏
    private void RestartGame()
    {
        model = new GameModel(); // 重新初始化模型
        view.GameOver = false; // 重置游戏结束状态
    }

    // 更新View
    public void UpdateView()
    {
        view.DrawGrid(model.Grid, model.IsCleared);
    }
}

public class GameManager : MonoBehaviour
{
    private GameModel model;
    private GameView view;
    private GameController controller;

    private void Start()
    {
        model = new GameModel();
        view = new GameView();
        controller = new GameController(model, view);
    }

    private void OnGUI()
    {
        controller.UpdateView();
    }
}
