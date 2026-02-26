using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class Game_Manager : Singleton<Game_Manager>
{
    public async void Start()
    {
        if (Background_Manager.Instance != null)
        {
            await Background_Manager.Instance.Load_Background();
        }
    }
    public async void Update_Game()
    {
        if (Addressable_Manager.Instance != null)
        {
            await Addressable_Manager.Instance.Check_Addressable_Catalog_Update();
        }
        if (Background_Manager.Instance != null)
        {
            await Background_Manager.Instance.Load_Background();
        }
    }
    public async void Switch_Scene(string path)
    {
        var handle = Addressables.LoadSceneAsync(path, LoadSceneMode.Single);

        await handle.Task;
    }
}
