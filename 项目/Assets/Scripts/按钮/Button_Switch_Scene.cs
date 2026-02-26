using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Switch_Scene : MonoBehaviour
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
        if (Game_Manager.Instance != null)
        {
            Game_Manager.Instance.Switch_Scene("World");
        }
    }
}
