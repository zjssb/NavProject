using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;

/*
 * 用于读取配置表信息
 */
public class ConfigManager{
    public static ConfigManager Instance => mInstance ??= new ConfigManager();

    private static readonly string ConfigPath = Path.Combine(Application.dataPath, "Resources", "Config");

    private static ConfigManager mInstance;

    private XmlDocument _xmlDoc;

    /// <summary>
    /// 获取config
    /// </summary>
    /// <param name="key">名称</param>
    /// <returns>存在config，则返回；否则为空</returns>
    public XmlNode GetConfig(params string[] keys){
        _xmlDoc ??= new XmlDocument();
        _xmlDoc.Load(Path.Combine(ConfigPath, keys[0] + ".xml"));

        StringBuilder sb = new StringBuilder();
        sb.Append("/Data");
        if (keys.Length > 1){
            sb.Append($"/Record[@id='{keys[1]}']");
        }
        for (int i = 2; i < keys.Length; i++){
            sb.Append($"/{keys[i]}");
        }

        // 使用XPath查询id匹配的记录
        XmlNode node = _xmlDoc.SelectSingleNode(sb.ToString());

        if (node != null){
            return node; // 返回记录的XML内容
        }
        else{
            Debug.LogError($"未找到为'{String.Join("/", keys)}'的记录！");
            return null;
        }
    }

    #region 废弃优化代码

    private Dictionary<string, Dictionary<string, Dictionary<string, object>>> mConfigs;

    /// <summary>
    /// 添加config
    /// </summary>
    /// <param name="key"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public bool AddConfig(string key, Dictionary<string, Dictionary<string, object>> config){
        if (!mConfigs.ContainsKey(key)){
            mConfigs.Add(key, config);
            return true;
        }
        else{
            Debug.LogError("重复添加config" + key);
            return false;
        }
    }

    /// <summary>
    /// 查找指定config 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="config">查找的config</param>
    /// <returns>查找结果</returns>
    private bool FindConfig(string key, out Dictionary<string, Dictionary<string, object>> config){
        config = null;
        if (mConfigs.TryGetValue(key, out config)){
            Debug.Log("字典中已存在" + key);
            return true;
        }
        else{
            // 从外部文件中查找config，并读取存储   
        }

        return false;
    }

    #endregion
}