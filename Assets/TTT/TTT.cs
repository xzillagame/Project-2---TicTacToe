using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerOption
{
    NONE, //0
    X, // 1
    O // 2
}

public class TTT : MonoBehaviour
{
    public int Rows;
    public int Columns;
    [SerializeField] BoardView board;

    public PlayerOption currentPlayer = PlayerOption.X;
    Cell[,] cells;

    // Start is called before the first frame update
    void Start()
    {
        cells = new Cell[Columns, Rows];

        board.InitializeBoard(Columns, Rows);

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                cells[j, i] = new Cell();
                cells[j, i].current = PlayerOption.NONE;
            }
        }
    }

    public void ResetBoard()
    {
        board.ResetBoard(Columns,Rows);
        
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                cells[j, i].current = PlayerOption.NONE;
            }
        }

        currentPlayer = PlayerOption.X;
    }

    public void MakeOptimalMove()
    {
        CheckBoardStatePlacement();
    }

    public void ChooseSpace(int column, int row)
    {
        // can't choose space if game is over
        if (GetWinner() != PlayerOption.NONE)
            return;

        // can't choose a space that's already taken
        if (cells[column, row].current != PlayerOption.NONE)
            return;

        // set the cell to the player's mark
        cells[column, row].current = currentPlayer;

        // update the visual to display X or O
        board.UpdateCellVisual(column, row, currentPlayer);

        // if there's no winner, keep playing, otherwise end the game
        if(GetWinner() == PlayerOption.NONE)
            EndTurn();
        else
        {
            Debug.Log("GAME OVER!");
        }
    }


    private void CheckBoardStatePlacement()
    {

        //Board is clear at start. Set optimal space to a corner
        if (cells[0, 0].current == PlayerOption.NONE)
        {
            ChooseSpace(0, 0);
            return;
        }
        //If any other space is set, set optimal space to center
        else if (cells[1, 1].current == PlayerOption.NONE)
        {
            ChooseSpace(1, 1);
            return;
        }
        else if (cells[0,0].current == currentPlayer && cells[1,0].current == PlayerOption.NONE)
        {
            ChooseSpace(1, 0);
            return;
        }

        
        //if (cells[1,1].current == PlayerOption.NONE)

        for (int x = 0; x < Columns; x++)
        {
            for (int y = 0; y < Rows; y++)
            {




                //If detects winning space sets space
                if (cells[x, y].current == PlayerOption.NONE)
                {
                    cells[x, y].current = currentPlayer;

                    if (GetWinner() == currentPlayer)
                    {
                        cells[x, y].current = PlayerOption.NONE;
                        ChooseSpace(x, y);
                        return;
                    }

                    cells[x, y].current = PlayerOption.NONE;

                }

                //If detects opponent will win, block space
                if (cells[x, y].current == PlayerOption.NONE)
                {
                    cells[x, y].current = GetOtherPlayer();


                    if (GetWinner() == GetOtherPlayer())
                    {
                        cells[x, y].current = PlayerOption.NONE;
                        ChooseSpace(x, y);
                        return;
                    }

                    cells[x, y].current = PlayerOption.NONE;

                }





            }


            //End of last iteration. Winning or blocking descision was not made


        }



        for(int x = 0; x < Columns; x++)
        {
            for(int y = 0;y < Rows; y++)
            {
                if (cells[x,y].current == PlayerOption.NONE)
                {
                    ChooseSpace(x, y);
                    return;
                }
            }
        }


    }

    private PlayerOption GetOtherPlayer()
    {
        switch (currentPlayer)
        {
            case PlayerOption.X: return PlayerOption.O;
            case PlayerOption.O: return PlayerOption.X;
            default: return PlayerOption.NONE;
        }
    }

    public void EndTurn()
    {
        // increment player, if it goes over player 2, loop back to player 1
        currentPlayer += 1;
        if ((int)currentPlayer > 2)
            currentPlayer = PlayerOption.X;
    }

    public PlayerOption GetWinner()
    {
        // sum each row/column based on what's in each cell X = 1, O = -1, blank = 0
        // we have a winner if the sum = 3 (X) or -3 (O)
        int sum = 0;

        // check rows
        for (int i = 0; i < Rows; i++)
        {
            sum = 0;
            for (int j = 0; j < Columns; j++)
            {
                var value = 0;
                if (cells[j, i].current == PlayerOption.X)
                    value = 1;
                else if (cells[j, i].current == PlayerOption.O)
                    value = -1;

                sum += value;
            }

            if (sum == 3)
                return PlayerOption.X;
            else if (sum == -3)
                return PlayerOption.O;

        }

        // check columns
        for (int j = 0; j < Columns; j++)
        {
            sum = 0;
            for (int i = 0; i < Rows; i++)
            {
                var value = 0;
                if (cells[j, i].current == PlayerOption.X)
                    value = 1;
                else if (cells[j, i].current == PlayerOption.O)
                    value = -1;

                sum += value;
            }

            if (sum == 3)
                return PlayerOption.X;
            else if (sum == -3)
                return PlayerOption.O;

        }

        // check diagonals
        // top left to bottom right
        sum = 0;
        for(int i = 0; i < Rows; i++)
        {
            int value = 0;
            if (cells[i, i].current == PlayerOption.X)
                value = 1;
            else if (cells[i, i].current == PlayerOption.O)
                value = -1;

            sum += value;
        }

        if (sum == 3)
            return PlayerOption.X;
        else if (sum == -3)
            return PlayerOption.O;

        // top right to bottom left
        sum = 0;
        for (int i = 0; i < Rows; i++)
        {
            int value = 0;

            if (cells[Columns - 1 - i, i].current == PlayerOption.X)
                value = 1;
            else if (cells[Columns - 1 - i, i].current == PlayerOption.O)
                value = -1;

            sum += value;
        }

        if (sum == 3)
            return PlayerOption.X;
        else if (sum == -3)
            return PlayerOption.O;

        return PlayerOption.NONE;
    }
}
