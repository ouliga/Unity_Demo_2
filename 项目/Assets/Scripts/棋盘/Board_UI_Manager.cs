using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XLua;

public class Board_UI_Manager : Singleton<Board_UI_Manager>
{
    [SerializeField] private GameObject _board_Position;
    [SerializeField] private GameObject _board_Block;
    [SerializeField] private TextMeshProUGUI _score_Player_1;
    [SerializeField] private TextMeshProUGUI _score_Player_2;

    private GameObject[,] _board_Blocks;
    public override void Awake()
    {
        
    }
    [LuaCallCSharp]
    public void Reset_Board()
    {
        int width = _board_Blocks.GetLength(0);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject chess = _board_Blocks[i, j].transform.Find("Æå×Ó").gameObject;
                chess.SetActive(false);
            }
        }
    }
    [LuaCallCSharp]
    public void Set_Board_UI_Manager(int board_Width)
    {
        _board_Blocks = new GameObject[board_Width, board_Width];
        if(_board_Block != null && _board_Position!=null)
        {
            for(int i = 0;i< board_Width; i++)
            {
                for(int j = 0;j< board_Width; j++)
                {
                    _board_Blocks[i, j] = Instantiate(_board_Block, _board_Position.transform);
                    Board_Chess board_Chess = _board_Blocks[i,j].AddComponent<Board_Chess>();
                    board_Chess.Set_Board_Chess(i, j);
                }
            }
        }
    }
    [LuaCallCSharp]
    public void Show_Chess_UI(int player,int r,int c)
    {
        Debug.Log($"({r},{c})·ÅÖÃ{player}");
        GameObject chess = _board_Blocks[r, c].transform.Find("Æå×Ó").gameObject;

        if(chess != null)
        {
            Image image = chess.GetComponent<Image>();
            chess.SetActive(true);
            if(player == -1)
            {
                image.color = Color.black;
            }
            else if(player == 1)
            {
                image.color = Color.white;
            }

        }
    }
    [LuaCallCSharp]
    public void Update_Score_UI(int player,int score)
    {
        if(player == 1 && _score_Player_1!=null)
        {
            _score_Player_1.text = score+"";
        }
        else if(player == -1 && _score_Player_2 != null)
        {
            _score_Player_2.text = score + "";
        }
    }


}
