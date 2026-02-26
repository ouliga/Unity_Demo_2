using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Update : MonoBehaviour
{
    // Start is called before the first frame update
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
        if(Game_Manager.Instance != null)
        {
            Game_Manager.Instance.Update_Game();
        }
    }
}
