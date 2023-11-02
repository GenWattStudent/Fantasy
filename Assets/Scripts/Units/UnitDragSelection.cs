using UnityEngine;

public class UnitDragSelection : MonoBehaviour
{
    Camera mainCamera;
    [SerializeField] private RectTransform selectionBoxVisual = null;
    private Rect selectionBox;
    private Vector2 startPosition = Vector2.zero;
    private Vector2 endPosition = Vector2.zero;

    private void DrawVisual()
    {
        Vector2 boxStart = startPosition;
        Vector2 boxEnd = endPosition;

        Vector2 boxCenter = (boxStart + boxEnd) / 2f;
        selectionBoxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));
        selectionBoxVisual.sizeDelta = boxSize;
    }

    private void DrawSelectionBox()
    {
        if (Input.mousePosition.x < startPosition.x)
        {
            selectionBox.xMin = Input.mousePosition.x;
            selectionBox.xMax = startPosition.x;
        }
        else
        {
            selectionBox.xMin = startPosition.x;
            selectionBox.xMax = Input.mousePosition.x;
        }

        if (Input.mousePosition.y < startPosition.y)
        {
            selectionBox.yMin = Input.mousePosition.y;
            selectionBox.yMax = startPosition.y;
        }
        else
        {
            selectionBox.yMin = startPosition.y;
            selectionBox.yMax = Input.mousePosition.y;
        }
    }

    private void SelectUnits()
    {
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            if (selectionBox.Contains(mainCamera.WorldToScreenPoint(unit.transform.position)))
            {
                UnitSelection.Instance.Select(unit);
            }
        }
    }

    void Start()
    {
        mainCamera = Camera.main;
        DrawVisual();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
            selectionBox = new Rect();
        }

        if (Input.GetMouseButton(0))
        {
           endPosition = Input.mousePosition;
           DrawVisual();
           DrawSelectionBox();
        }

        if (Input.GetMouseButtonUp(0))
        {
            SelectUnits();
            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            selectionBoxVisual.sizeDelta = Vector2.zero;
        }
    }
}
