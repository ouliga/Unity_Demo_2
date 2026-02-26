using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board_Chess : MonoBehaviour
{
    private int _r = -1;
    private int _c = -1;
    public void OnEnable()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
    }
    public void OnDisable()
    {
        gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public void Set_Board_Chess(int r,int c)
    {
        _r = r;
        _c = c;
    }
    public void OnClick()
    {
        Board_Manager_Lua board_Manager_Lua = Board_Manager_Lua.Instance;
        if (board_Manager_Lua != null)
        {
            if (board_Manager_Lua.Check_Put_Chess(_r, _c))
            {
                board_Manager_Lua.Put_Chess( _r, _c );
                board_Manager_Lua.Show_Chess_UI(_r,_c );
                board_Manager_Lua.Change_Player();

                int winner = board_Manager_Lua.CheckWinner(_r, _c);
                if (winner != 0)
                {
                    board_Manager_Lua.Add_Score(winner);
                    board_Manager_Lua.Update_Score_UI(winner);
                    board_Manager_Lua.Reset_Board();
                    board_Manager_Lua.Reset_Board_UI();
                }
  
                
            }

        }
    }
}
