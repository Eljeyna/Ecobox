using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    public enum Alignment
    {
        Horizontal,
        Vertical
    }
    public enum FitType
    {
        Uniform,
        Width,
        Height,
        FixedRows,
        FixedColumns,
        FixedBoth
    }

    public Alignment alignment;
    public FitType fitType;

    public int rows;
    public int columns;
    public Vector2 cellSize;
    public Vector2 spacing;

    public bool fitX;
    public bool fitY;

    public override void CalculateLayoutInputVertical()
    {
        base.CalculateLayoutInputHorizontal();

        if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
        {
            fitX = true;
            fitY = true;
            float sqrRt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt(sqrRt);
        }

        if (fitType == FitType.Width || fitType == FitType.FixedColumns || fitType == FitType.Uniform)
        {
            rows = Mathf.CeilToInt(transform.childCount / (float)columns);
        }
        
        if (fitType == FitType.Height || fitType == FitType.FixedRows || fitType == FitType.Uniform)
        {
            columns = Mathf.CeilToInt(transform.childCount / (float)rows);
        }

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;
        
        float cellWidth;
        float cellHeight;

        if (alignment == Alignment.Horizontal)
        {
            cellWidth = (parentWidth / columns) - ((spacing.x / columns) * (columns - 1)) - (padding.left / (float)columns) - (padding.right / (float)columns);
            cellHeight = (parentHeight / rows) - ((spacing.y / rows) * (rows - 1)) - (padding.top / (float)rows) - (padding.bottom / (float)rows);
        }
        else
        {
            cellHeight = (parentWidth / columns) - ((spacing.x / columns) * (columns - 1)) - (padding.left / (float)columns) - (padding.right / (float)columns);
            cellWidth = (parentHeight / rows) - ((spacing.y / rows) * (rows - 1)) - (padding.top / (float)rows) - (padding.bottom / (float)rows);
        }

        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHeight : cellSize.y;

        int columnCount;
        int rowCount;

        for (int i = 0; i < rectChildren.Count; i ++)
        {
            RectTransform item = rectChildren[i];

            if (alignment == Alignment.Horizontal)
            {
                rowCount = i / columns;
                columnCount = i % columns;
                float xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
                float yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);
            }
            else
            {
                rowCount = i / rows;
                columnCount = i % rows;
                float xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
                float yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

                SetChildAlongAxis(item, 0, yPos, cellSize.y);
                SetChildAlongAxis(item, 1, xPos, cellSize.x);
            }
        }
    }

    public override void SetLayoutHorizontal()
    {

    }

    public override void SetLayoutVertical()
    {

    }
}
