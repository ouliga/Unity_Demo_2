using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class Background_Manager : Singleton<Background_Manager>
{
    public Image _background;
    private string _path = "Background";
    public override void Awake()
    {
    }

    public async Task Load_Background()
    {
        var handle = Addressables.LoadAssetAsync<Sprite>(_path);

        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("加载成功，换图！");
            _background.sprite = handle.Result;
        }
        else
        {
            Debug.LogError("加载出错了！");
        }
    }
}
