using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class Board_Manager : Singleton<Board_Manager>
{
    private int _player;
    private int _board_Width;
    private int[,] _board;

    private int _score_Player_1;
    private int _score_Player_2;

    private void Start()
    {
        //棋手1代表白，-1代表黑
        _player = 1;
        _board_Width = 15;
        _board = new int[_board_Width, _board_Width];

        _score_Player_1 = 0;
        _score_Player_2 = 0;

        Board_UI_Manager.Instance.Set_Board_UI_Manager(_board_Width);
    }
    public void Reset_Board()
    {
        _player = 1;
        for(int i = 0; i < _board_Width; i++)
        {
            for(int j=0; j < _board_Width; j++)
            {
                _board[i, j] = 0;
            }
        }
    }
    public void Reset_Board_UI()
    {
        if(Board_UI_Manager.Instance != null)
        {
            Board_UI_Manager.Instance.Reset_Board();
        }
    }
    public bool Check_Put_Chess(int r,int c)
    {
        if(r<0||r>=_board_Width||c<0||c>= _board_Width) return false;
        if (_board[r,c] != 0)
        {
            return false;
        }
        return true;
    }
    public void Put_Chess(int r,int c)
    {
        _board[r, c] = _player;
    }
    public void Show_Chess_UI(int r,int c)
    {
        if(Board_UI_Manager.Instance != null)
        {
            Board_UI_Manager.Instance.Show_Chess_UI(_player, r, c);
        }
    }
    public void Change_Player()
    {
        _player = -_player;
    }
    public int CheckWinner(int lastR, int lastC)
    {
        int player = _board[lastR, lastC];
        if (player == 0) return 0;

        // 检查四个轴向：横、竖、主对角、副对角
        if (CountContinuous(_board, lastR, lastC, 1, 0) >= 5 || // 横向
            CountContinuous(_board, lastR, lastC, 0, 1) >= 5 || // 纵向
            CountContinuous(_board, lastR, lastC, 1, 1) >= 5 || // 左上到右下
            CountContinuous(_board, lastR, lastC, 1, -1) >= 5)  // 左下到右上
        {
            return player;
        }
        return 0;
    }
    private int CountContinuous(int[,] board, int r, int c, int dr, int dc)
    {
        int player = board[r, c];
        int count = 1; // 包含自己

        int width = board.GetLength(0);

        // 向正方向找
        for (int i = 1; i < 5; i++)
        {
            int nr = r + i * dr;
            int nc = c + i * dc;
            if (nr >= 0 && nr < width && nc >= 0 && nc < width && board[nr, nc] == player)
                count++;
            else
                break;
        }

        // 向反方向找
        for (int i = 1; i < 5; i++)
        {
            int nr = r - i * dr;
            int nc = c - i * dc;
            if (nr >= 0 && nr < width && nc >= 0 && nc < width && board[nr, nc] == player)
                count++;
            else
                break;
        }

        return count;
    }

    //玩家分数//
    [LuaCallCSharp]
    public int Get_Score(int player)
    {
        if (player == 1) return _score_Player_1;
        if(player == -1) return _score_Player_2;
        return 0;
    }

    public void Update_Score(int player,int score)
    {
        if(player == 1)
        {
            _score_Player_1 = score;
        }
        else if(player == -1)
        {
            _score_Player_2 = score;
        }
    }
    public void Update_Score_UI(int player)
    {
        if(Board_UI_Manager.Instance != null)
        {
            if(player == 1)
            {
                Board_UI_Manager.Instance.Update_Score_UI(player, _score_Player_1);
            }
            else if(player == -1)
            {
                Board_UI_Manager.Instance.Update_Score_UI(player, _score_Player_2);
            }
        }
    }
}
