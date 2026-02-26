using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAndRenameLuaScript : MonoBehaviour
{
    // 默认文件名
    private static readonly string FILE_NAME = "NewLuaScript.lua";
    // 脚本默认内容
    private static readonly string DEFAULT_CONTENT = "-- Lua脚本的默认内容\nprint(\"Hello, World!\")";

    [MenuItem("Assets/Create/Lua Script", false, 80)]
    public static void CreateNewLuaScript()
    {
        // 获取当前选中的对象的路径
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (path == "")
        {
            path = "Assets";
        }
        // 如果选中的对象不是文件夹，则在其所在目录下创建Lua脚本
        else if (!AssetDatabase.IsValidFolder(path))
        {
            path = Path.GetDirectoryName(path);
        }

        // 设置新Lua脚本的完整路径和名称
        path = Path.Combine(path, FILE_NAME);
        string newFilePath = AssetDatabase.GenerateUniqueAssetPath(path);

        // 创建Asset
        ProjectWindowUtil.CreateAssetWithContent(newFilePath, DEFAULT_CONTENT);
    }

}
