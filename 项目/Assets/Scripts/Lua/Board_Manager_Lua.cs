using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using XLua;

//[CSharpCallLua]
//public delegate LuaTable HotReloadDelegate(string moduleName);
[CSharpCallLua]
public delegate void Set_Board_Manager();
[CSharpCallLua]
public delegate void Reset_Board();
[CSharpCallLua]
public delegate void Reset_Board_UI();
[CSharpCallLua]
public delegate bool Check_Put_Chess(int r, int c);
[CSharpCallLua]
public delegate void Put_Chess(int r, int c);
[CSharpCallLua]
public delegate void Change_Player();
[CSharpCallLua]
public delegate void Show_Chess_UI(int r, int c);
[CSharpCallLua]
public delegate int CheckWinner(int lastR, int lastC);
[CSharpCallLua]
public delegate int Get_Score(int player);
[CSharpCallLua]
public delegate void Add_Score(int player);
[CSharpCallLua]
public delegate void Update_Score_UI(int player);

public class Board_Manager_Lua : Singleton<Board_Manager_Lua>
{
    private LuaEnv _luaEnv;
    private List<AsyncOperationHandle> _loadedHandles = new List<AsyncOperationHandle>();

    //private HotReloadDelegate hotReload;
    private Set_Board_Manager set_Board_Manager;
    private Reset_Board reset_Board;
    private Reset_Board_UI reset_Board_UI;
    private Check_Put_Chess check_Put_Chess;
    private Put_Chess put_Chess;
    private Change_Player change_Player;
    private Show_Chess_UI show_Chess_UI;
    private CheckWinner checkWinner;
    private Get_Score get_Score;
    private Add_Score add_Score;
    private Update_Score_UI update_Score_UI;
    

    private void Start()
    {
        //Check_Addressable_Catalog_Update();
        //Addressable_Manager.Instance.Check_Addressable_Catalog_Update();
    }
    private void OnDestroy()
    {
        //清空所有委托
        //hotReload = null;
        set_Board_Manager = null;
        reset_Board = null;
        reset_Board_UI = null;
        check_Put_Chess = null;
        put_Chess = null;
        change_Player = null;
        show_Chess_UI = null;
        checkWinner = null;
        get_Score = null;
        add_Score = null;
        update_Score_UI = null;
        // 先销毁 Lua 虚拟机
        // 必须先停掉 Lua，防止 Lua 在销毁过程中还在访问资源
        if (_luaEnv != null)
        {
            _luaEnv.Dispose();
            _luaEnv = null;
        }
        Release_Addressable();
        // 3. 清空列表
        _loadedHandles.Clear();

        Debug.Log("所有 Lua 脚本资源释放完毕");
    }
    private void Release_Addressable()
    {
        foreach (var handle in _loadedHandles)
        {
            // 检查句柄是否有效（防止重复释放）
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }
    }

    private byte[] LuaLoader(ref string filepath)
    {
        // 1. 处理路径：把 Lua 的 require 路径转为 Addressable Key
        // 比如 require 'Game.Main' -> filepath = "Game.Main"
        // 假设你的 Addressable Key 就是 "Game.Main" (或者是 "Assets/Lua/Game/Main.lua")
        string key = filepath;

        // 2. 加载资源 (注意：xLua Loader 必须同步返回)
        // 检查资源是否存在 (Addressables 1.17+ 支持)
        var op = Addressables.LoadAssetAsync<TextAsset>(key);

        // 3. 强制同步等待加载完成
        var asset = op.WaitForCompletion();

        if (asset != null)
        {
            // 4. 返回 Lua 字节流
            byte[] content = asset.bytes;
            // 释放引用，否则内存会泄漏！但在 Loader 里释放有点麻烦，
            // 通常做法是缓存这个 handle 或者不做释放(如果 Lua 代码常驻)
            // 严谨做法是：把 handle 存到一个 List 里，游戏结束统一释放
            // Addressables.Release(op); 
            _loadedHandles.Add(op);
            return content;
        }

        return null; // 返回 null 表示没找到，xLua 会尝试下一个 Loader
    }
    //public void ReLoad_Lua()
    //{
    //    if (hotReload != null)
    //    {
    //        Debug.Log("开始热更新");
    //        LuaTable results = hotReload("Board_Manager");
    //        if(results != null)
    //        {
    //            Set_Lua_Function(results);
    //        }
    //        else
    //        {
    //            Debug.LogError("热更新失败");
    //        }
            
    //    }
    //}
    private void Set_Lua_Function(LuaTable luaTable)
    {
        if (luaTable != null)
        {
            //获取事件
            //hotReload = _luaEnv.Global.Get<HotReloadDelegate>("HotReload");
            set_Board_Manager = luaTable.Get<Set_Board_Manager>("Set_Board_Manager");
            reset_Board = luaTable.Get<Reset_Board>("Reset_Board");
            reset_Board_UI = luaTable.Get<Reset_Board_UI>("Reset_Board_UI");
            check_Put_Chess = luaTable.Get<Check_Put_Chess>("Check_Put_Chess");
            put_Chess = luaTable.Get<Put_Chess>("Put_Chess");
            change_Player = luaTable.Get<Change_Player>("Change_Player");
            show_Chess_UI = luaTable.Get<Show_Chess_UI>("Show_Chess_UI");
            checkWinner = luaTable.Get<CheckWinner>("CheckWinner");
            get_Score = luaTable.Get<Get_Score>("Get_Score");
            add_Score = luaTable.Get<Add_Score>("Add_Score");
            update_Score_UI = luaTable.Get<Update_Score_UI>("Update_Score_UI");
        }
        else
        {
            Debug.LogError("读取lua文件失败");
        }
    }
    public void Start_Game()
    {
        _luaEnv = new LuaEnv();
        _luaEnv.AddLoader(LuaLoader);

        object[] results = _luaEnv.DoString("return require 'Board_Manager'");
        // 取出第一个返回值，这就是 Lua 里的 M 表
        LuaTable luaTable = results[0] as LuaTable;
        Set_Lua_Function(luaTable);
        Set_Board_Manager();
    }

    public void Set_Board_Manager()
    {
        if (set_Board_Manager != null)
        {
            set_Board_Manager();
        }
    }
    public void Reset_Board()
    {
        if(reset_Board != null)
        {
            reset_Board();
        }
    }
    public void Reset_Board_UI()
    {
        if(reset_Board_UI != null)
        {
            reset_Board_UI();
        }
    }
    public bool Check_Put_Chess(int r,int c)
    {
        if (check_Put_Chess == null) return false;
        return check_Put_Chess(r,c);
    }
    public void Put_Chess(int r,int c)
    {
        if (put_Chess != null)
        {
            put_Chess(r, c);
        }
    }
    public void Change_Player()
    {
        if (change_Player != null)
        {
            change_Player();
        }
    }
    public void Show_Chess_UI(int r,int c)
    {
        if (show_Chess_UI != null)
        {
            show_Chess_UI(r, c);
        }
    }
    public int CheckWinner(int lastR,int lastC)
    {
        if(checkWinner == null) return 0;
        return checkWinner(lastR,lastC);
    }
    public int Get_Score(int player)
    {
        if (get_Score == null) return 0; 
        return get_Score(player);
    }
    public void Add_Score(int player)
    {
        if (add_Score != null)
        {
            add_Score(player);
        }
    }
    public void Update_Score_UI(int player)
    {
        if(update_Score_UI != null)
        {
            update_Score_UI(player);
        }
    }

}
