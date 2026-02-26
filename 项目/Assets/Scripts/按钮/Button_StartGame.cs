using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_StartGame : MonoBehaviour
{
    public void OnEnable()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
    }
    public void OnDisable()
    {
        gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
    }
    public void OnClick()
    {
        if(Board_Manager_Lua.Instance != null)
        {
            Board_Manager_Lua.Instance.Start_Game();
            gameObject.SetActive(false);
        }
    }
}
