using UnityEngine;
public class GameModel : MonoBehaviour
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

    // 随机生成图案矩阵
    private void GenerateGrid()
    {
        System.Random rand = new System.Random();
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                Grid[i, j] = rand.Next(1, iconTypes + 1); // 图案从1到iconTypes
                IsCleared[i, j] = false; // 初始化时没有任何消除
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
}

public class GameView
{
    public delegate void IconClickHandler(int x, int y); // 点击图标事件
    public event IconClickHandler OnIconClicked;

    private int buttonSize = 50; // 图标按钮的大小

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
    }
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
        // 初始化MVC组件
        model = new GameModel();
        view = new GameView();
        controller = new GameController(model, view);
    }

    private void OnGUI()
    {
        // 每帧更新View
        controller.UpdateView();
    }
}
