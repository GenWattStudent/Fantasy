using System;
using UnityEngine;

public class Grid<T>
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    [SerializeField] private Vector3 originPosition;

    [SerializeField] private float gridGap;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
        public T value;
    }

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;

    private T[,] gridArray;
    private TextMesh[,] debugTextArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new T[width, height];
        debugTextArray = new TextMesh[width, height];

        CreateGrid();
    }

    private void CreateGrid()
    {
       for (int x = 0; x < gridArray.GetLength(0); x++)
       {
           for (int y = 0; y < gridArray.GetLength(1); y++)
           {
                DrawText(x, y, gridArray[x, y].ToString());
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
           }
       }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    private void DrawText(int x, int y, string text)
    {
        GameObject textObject = new GameObject("Text", typeof(TextMesh));
        textObject.transform.position = GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f;
        textObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        TextMesh textMesh = textObject.GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.fontSize = 20;
        textMesh.color = Color.white;
        debugTextArray[x, y] = textMesh;
    }

    public Vector3 GetWorldPosition(float x, float z)
    {
        return new Vector3(x, 0, z) * cellSize + originPosition;
    }

    public void SetValue(int x, int y, T value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = value.ToString();
            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y, value = value });
        }
    }

    public void SetValue(Vector3 worldPosition, T value)
    {
        GetXZ(worldPosition, out int x, out int z);
        SetValue(x, z, value);
    }

    public void GetXZ(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
    }

    public T GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        } else
        {
            return default(T);
        }
    }

    public T GetValue(Vector3 worldPosition)
    {
        GetXZ(worldPosition, out int x, out int z);
        return GetValue(x, z);
    }
}
