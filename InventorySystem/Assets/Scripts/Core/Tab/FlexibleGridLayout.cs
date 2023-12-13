using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Ryu
{

    public class FlexibleGridLayout : LayoutGroup
    {
        public enum FitType
        {
            Uniform,        // 자동
            Width,          // 폭이 더 길게 배치
            Height,         // 높이가 더 길게 배치
            FixedRows,      // 폭 자유 또는 자동 (fix 유무) , 높이 고정
            FixedColums,    // 폭 자동 , 높이 자유 또는 자동(fix 유무)
        }

        public FitType fitType;
        public int rows;// 행 가로 집합   -- 개수
        public int columns;// 열 세로 집합 | 개수
        public Vector2 cellSize;
        public Vector2 spacing;
        public bool fixedSize;
        public bool fitX;
        public bool fitY;
        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            // 너비와 높이를 구하기
            float parentWidth = rectTransform.rect.width;
            float parentHeight = rectTransform.rect.height;
            if (rows <= 0)
            {
                rows = 1;
            }
            if (columns <= 0)
            {
                columns = 1;
            }

            if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
            {
                fitX = true;
                fitY = true;
                // 개수에 따른 비율 행과 열수 구하기
                float sqrRt = Mathf.Sqrt(transform.childCount);
                rows = Mathf.CeilToInt(sqrRt);// 반올림
                columns = Mathf.CeilToInt(sqrRt);// 반올림
            }
            if (fitType == FitType.Width || fitType == FitType.FixedColums)
            {
                rows = Mathf.CeilToInt(transform.childCount / (float)columns);
                fitX = true;
            }
            if (fitType == FitType.Height || fitType == FitType.FixedRows)
            {
                columns = Mathf.CeilToInt(transform.childCount / (float)rows);
                fitY = true;
            }
            if (fixedSize)
            {
                fitX = false;
                fitY = false;

                spacing.x = (parentWidth - (cellSize.x * columns) - padding.left - padding.right) / (float)(columns - 1);
                if(spacing.x <= 0 || float.IsInfinity(spacing.x))
                {
                    spacing.x = 0;
                }
                if(childAlignment == TextAnchor.UpperLeft)
                {
                    spacing.y = spacing.x;
                }
                else
                {
                    spacing.y = (parentHeight - (cellSize.y * rows) - padding.top - padding.bottom) / (float)(rows - 1);
                    if (spacing.y <= 0 || float.IsInfinity(spacing.y))
                    {
                        spacing.y = 0;
                    }
                }
            }
            
            // 슬롯들의 크기를 정의
            float cellWidth = (parentWidth - padding.left - padding.right - (spacing.x * (columns - 1))) / (float)columns;
            float cellHeight = (parentHeight - padding.top - padding.bottom - (spacing.y * (rows - 1))) / (float)rows;

            cellSize.x = fitX ? cellWidth : cellSize.x;
            cellSize.y = fitY ? cellHeight : cellSize.y;

            int columnCount = 0;
            int rowCount = 0;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                rowCount = i / columns;     //가로줄 최대치까지 증가하면 1 증가 (높이)
                columnCount = i % columns;  // 가로줄 최대치 까지 증가하다 다시 0으로(가로)

                var item = rectChildren[i];

                var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
                var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);
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
    }

}