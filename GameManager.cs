using UnityEngine;
public class GameModel : MonoBehaviour
{
    public int[,] Grid { get; private set; } // ͼ������
    public bool[,] IsCleared { get; private set; } // ����״̬����
    private int gridSize = 6; // �����С
    private int iconTypes = 5; // ͼ����������

    public GameModel()
    {
        Grid = new int[gridSize, gridSize];
        IsCleared = new bool[gridSize, gridSize];
        GenerateGrid();
    }

    // �������ͼ������
    private void GenerateGrid()
    {
        System.Random rand = new System.Random();
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                Grid[i, j] = rand.Next(1, iconTypes + 1); // ͼ����1��iconTypes
                IsCleared[i, j] = false; // ��ʼ��ʱû���κ�����
            }
        }
    }

    // �ж�����λ���Ƿ���ͬ��δ����
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
    public delegate void IconClickHandler(int x, int y); // ���ͼ���¼�
    public event IconClickHandler OnIconClicked;

    private int buttonSize = 50; // ͼ�갴ť�Ĵ�С

    // ��������ͼ������
    public void DrawGrid(int[,] grid, bool[,] isCleared)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            GUILayout.BeginHorizontal(); // ����һ��
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                // ���ͼ��δ�����������ư�ť
                if (!isCleared[i, j])
                {
                    if (GUILayout.Button(grid[i, j].ToString(), GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
                    {
                        OnIconClicked?.Invoke(i, j); // ���ʱ�����¼�
                    }
                }
                else
                {
                    // ͼ����������ʾ�հ�
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

        // ����View�ĵ���¼�
        this.view.OnIconClicked += HandleIconClick;
    }

    // ����ͼ������¼�
    private void HandleIconClick(int x, int y)
    {
        if (selectedX == null && selectedY == null)
        {
            // ���û��ѡ��ͼ������¼��һ�ε����λ��
            selectedX = x;
            selectedY = y;
        }
        else
        {
            // ����Ѿ�ѡ���˵�һ��ͼ��������ڶ��ε��
            if (model.AreMatching(selectedX.Value, selectedY.Value, x, y))
            {
                Debug.Log("Match Found!");
            }
            else
            {
                Debug.Log("Not a Match!");
            }
            // ����ѡ��
            selectedX = null;
            selectedY = null;
        }
    }

    // ����View
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
        // ��ʼ��MVC���
        model = new GameModel();
        view = new GameView();
        controller = new GameController(model, view);
    }

    private void OnGUI()
    {
        // ÿ֡����View
        controller.UpdateView();
    }
}
