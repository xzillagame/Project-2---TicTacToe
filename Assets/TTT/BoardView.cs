
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    CellView[,] cells;

    [SerializeField] GameObject rowPrefab;
    [SerializeField] GameObject cellPrefab;



    public void InitializeBoard(int cols, int rows)
    {
        cells = new CellView[cols, rows];

        for (int i = 0; i < rows; i++)
        {
            GameObject row = Instantiate(rowPrefab, transform);
            for (int j = 0; j < cols; j++)
            {
                GameObject cell = Instantiate(cellPrefab, row.transform);

                cells[j, i] = cell.GetComponent<CellView>();
                cells[j, i].SetColumnAndRow(j, i);
            }
        }
    }


    public void ResetBoard(int cols, int rows)
    {
        for(int x = 0; x < cols; x++)
        {
            for(int y = 0; y < rows; y++)
            {
                UpdateCellVisual(x, y, PlayerOption.NONE);
            }
        }
    }

    public void UpdateCellVisual(int c, int r, PlayerOption player)
    {
        string symbol = "";
        if (player == PlayerOption.X)
            symbol = "X";
        else if (player == PlayerOption.O)
            symbol = "O";
        cells[c, r].SetText(symbol);
    }
}
