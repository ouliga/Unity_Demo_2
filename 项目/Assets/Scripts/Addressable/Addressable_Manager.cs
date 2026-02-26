using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Addressable_Manager : Singleton<Addressable_Manager>
{
    public async Task Check_Addressable_Catalog_Update()
    {
        //初始化 Addressables
        var handle_initial = Addressables.InitializeAsync(false);
        await handle_initial.Task;
        Addressables.Release(handle_initial);

        //检查catalog
        var handle_checkCatalog = Addressables.CheckForCatalogUpdates(false);
        await handle_checkCatalog.Task;
        List<string> catalogs = handle_checkCatalog.Result;
        Addressables.Release(handle_checkCatalog);

        if (catalogs != null && catalogs.Count > 0)
        {
            Debug.Log("发现新版本，开始更新目录...");
            //下载并应用新的 Catalog
            var handle_updateCatalog = Addressables.UpdateCatalogs(catalogs,false);
            await handle_updateCatalog.Task;
            if(handle_updateCatalog.Status == AsyncOperationStatus.Succeeded)
            {
                // result 是 IResourceLocator 列表，包含了所有新版本的资源定位信息
                List<IResourceLocator> locators = handle_updateCatalog.Result;

                // 将所有 locator 里的 Keys 收集起来
                List<object> keysToDownload = new List<object>();
                foreach (var locator in locators)
                {
                    // locator.Keys 包含了这次更新涉及的所有资源 Key
                    keysToDownload.AddRange(locator.Keys);
                }

                var handle_download = Addressables.DownloadDependenciesAsync(keysToDownload, false);

                //while (!handle_download.IsDone)
                //{
                //    float progress = handle_download.PercentComplete;
                //    Debug.Log($"下载进度: {(progress * 100):F1}%");
                //    await System.Threading.Tasks.Task.Delay(100);
                //}

                if (handle_download.Status == AsyncOperationStatus.Succeeded)
                {
                    Debug.Log("所有资源下载完毕！现在加载的就是最新资源了。");
                }
                else
                {
                    Debug.LogError("资源下载失败: " + handle_download.OperationException);
                }

                // 释放下载 Handle
                Addressables.Release(handle_download);
            }
            Addressables.Release(handle_updateCatalog);
        }
        else
        {
            Debug.Log("当前已是最新版本");
        }
        
    }
}
