using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Component for generating flexibile grid.
/// 
/// Author - GoGs
/// </summary>

public class FlexibileGridLayout : LayoutGroup
{
  [SerializeField] public int cellWidth = 523;
  [SerializeField] public int spacingX = 0;
  [SerializeField] public int spacingY = 0;

  private float parentWidth = 0;
  private float parentHeight = 0;
  private float offsetYtop = 0;

  private Dictionary<int, LayoutItem> columnDictionary = new Dictionary<int, LayoutItem>();

  public override void CalculateLayoutInputHorizontal()
  {
    base.CalculateLayoutInputHorizontal();

    parentWidth = rectTransform.rect.width;
    parentHeight = rectTransform.rect.height;

    Debug.Log("CalculateLayoutInputHorizontal");

    int rowIndex = 0;

    float xPos = 0;
    float yPos = 0;
    float prevYPos = 0;

    offsetYtop = 0;

    GetColumns();

    //Set parent width
    rectTransform.sizeDelta = new Vector2(((cellWidth + spacingX) * columnDictionary.Count) + (padding.left + padding.right - spacingX), rectTransform.sizeDelta.y);

    for (int i = 0; i < columnDictionary.Count; i++)
    {
      var layoutItem = columnDictionary[i];
 
      SetYOffeset(layoutItem);

      for (int c = 0; c < layoutItem.items.Count; c++)
      {
        var item = layoutItem.items[c];

        RectTransform itemBefore = layoutItem.items[c];

        if (c > 0)
        {
          itemBefore = layoutItem.items[c - 1];
        }

        //First column
        if (i == 0)
        {
          xPos = padding.left;
        } else
        {
          xPos = (cellWidth + spacingX) * i;
          xPos += padding.left;
        }

        if (rowIndex != 0)
        {
          prevYPos += itemBefore.sizeDelta.y + spacingY;
          yPos = prevYPos;
        }
        else
        {
          prevYPos = offsetYtop;
          yPos = offsetYtop;
        }

        rowIndex++;

        SetChildAlongAxis(item, 0, xPos);
        SetChildAlongAxis(item, 1, yPos);
      }

      yPos = 0;
      prevYPos = 0;
      rowIndex = 0;
    }  
  }

  private void SetYOffeset(LayoutItem layoutItem)
  {
    switch (childAlignment)
    {
      case TextAnchor.UpperLeft:
        break;
      case TextAnchor.UpperCenter:
        break;
      case TextAnchor.UpperRight:
        break;
      case TextAnchor.MiddleLeft:
        offsetYtop = (rectTransform.sizeDelta.y - layoutItem.totalHeight) / 2;
        break;
      case TextAnchor.MiddleCenter:
        break;
      case TextAnchor.MiddleRight:
        break;
      case TextAnchor.LowerLeft:
        break;
      case TextAnchor.LowerCenter:
        break;
      case TextAnchor.LowerRight:
        break;
      default:
        break;
    }
  }

  private void GetColumns()
  {
    columnDictionary.Clear();
    int columnIndex = 0;
    float columnHeight = 0;
    bool genreateColumn = false;

    List<RectTransform> itemsInColumn = new List <RectTransform>();

    for (int i = 0; i < rectChildren.Count; i++)
    {
      var item = rectChildren[i];

      float possibleHeight = item.sizeDelta.y + spacingY + columnHeight;

      if (possibleHeight <= parentHeight)
      {
        columnHeight += item.sizeDelta.y + spacingY;
        itemsInColumn.Add(item);

        if (i == rectChildren.Count - 1)
        {
          columnHeight -= spacingY;
          genreateColumn = true;
        }
      } else
      {
        if (item.sizeDelta.y + spacingY > parentHeight)
        {
          continue;
        }
        columnHeight -= spacingY;
        genreateColumn = true;
        i--;
      }

      if (genreateColumn)
      {
        List<RectTransform> tempList = new List<RectTransform>();
        tempList.AddRange(itemsInColumn);

        LayoutItem layoutItem = new LayoutItem();
        layoutItem.items = tempList;
        layoutItem.totalHeight = columnHeight;

        columnDictionary.Add(columnIndex, layoutItem);

        itemsInColumn.Clear();
        columnIndex++;
        columnHeight = 0;
        genreateColumn = false;
      }   
    }
  }

  public override void CalculateLayoutInputVertical()
  {

  }

  public override void SetLayoutHorizontal()
  {

  }

  public override void SetLayoutVertical()
  {

  }

  private class LayoutItem
  {
    public List<RectTransform> items;
    public float totalHeight;
  }
}


