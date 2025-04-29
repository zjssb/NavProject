using System.IO;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Device.Application;

/// <summary>
/// 菜单栏扩展 Tool/xx
/// </summary>
public class Tool : MonoBehaviour{
    static string[] dataPathArr = Application.dataPath.Split('/');
    /// <summary>
    /// 获取项目名字（alpha） 区分 alpha/banshu 等的CSV路径
    /// </summary>
    static string ClientKey = dataPathArr[dataPathArr.Length - 3];

    public static string csvPathSaveKey = "CsvPathKey" + ClientKey;
    public static string XMLPath = Path.Combine(Application.dataPath, "Resources", "Config");

    [MenuItem("Tools/选择csv路径")]
    public static void ChooseCsvPath()
    {

        string p = UnityEngine.PlayerPrefs.GetString(csvPathSaveKey);

        string choosePath = EditorUtility.OpenFolderPanel("获取文件夹", p ?? Application.dataPath + "/..", "");
        if (!string.IsNullOrEmpty(choosePath))
        {
            UnityEngine.PlayerPrefs.SetString(csvPathSaveKey, choosePath);
        }

    }
    
    [MenuItem("Tools/csv转配置")]
    static void CSVtoXML(){
        var CSVPath = PlayerPrefs.GetString(csvPathSaveKey);

        if (Directory.Exists(XMLPath)){
            Directory.Delete(XMLPath, true);
        }

        Directory.CreateDirectory(XMLPath);


        DirectoryInfo di = new DirectoryInfo(CSVPath);
        foreach (var file in di.GetFiles()){
            ConvertCsvToXml(file.FullName, XMLPath);
        }

        AssetDatabase.Refresh();
    }

    static void ConvertCsvToXml(string csvFilePath, string xmlFilePath){
        // 读取CSV文件的所有行
        var lines = File.ReadAllLines(csvFilePath);

        // 解析CSV文件
        var columnNames = lines[0].Split(','); // 第一行：列名
        var columnTypes = lines[1].Split(','); // 第二行：数据类型
        var columnComments = lines[2].Split(','); // 第三行：注释

        // 创建XML文档
        XDocument xmlDoc = new XDocument(new XElement("Data"));

        // 从第四行开始读取数据
        for (int i = 3; i < lines.Length; i++){
            var fields = lines[i].Split(',');

            // 获取第一列的值作为索引
            string indexValue = fields[0];

            // 创建<Record>元素，并添加id属性
            XElement recordElement = new XElement("Record");
            recordElement.SetAttributeValue("id", indexValue);

            for (int j = 0; j < columnNames.Length; j++){
                // 创建列元素，并添加type和comment属性
                XElement fieldElement = new XElement(columnNames[j], fields[j]);
                fieldElement.SetAttributeValue("type", columnTypes[j]);
                fieldElement.SetAttributeValue("comment", columnComments[j]);

                // 将列元素添加到<Record>中
                recordElement.Add(fieldElement);
            }

            // 将<Record>添加到XML文档中
            xmlDoc.Root.Add(recordElement);
        }

        // 保存XML文件
        var name = Path.GetFileNameWithoutExtension(csvFilePath);
        xmlDoc.Save(Path.Combine(xmlFilePath, name + ".xml"));
        Debug.Log(name + "转配置");
    }
}
/*
    public string csvFileName = "data.csv"; // CSV文件名
    public string xmlFileName = "output.xml"; // 输出的XML文件名

    void Start()
    {
        string csvFilePath = Path.Combine(Application.dataPath, "Resources", csvFileName);
        string xmlFilePath = Path.Combine(Application.dataPath, xmlFileName);

        ConvertCsvToXml(csvFilePath, xmlFilePath);
        Debug.Log("CSV文件已成功转换为XML文件！");
    }

    void ConvertCsvToXml(string csvFilePath, string xmlFilePath)
    {
        // 读取CSV文件的所有行
        var lines = File.ReadAllLines(csvFilePath);

        // 解析CSV文件
        var columnNames = lines[0].Split(','); // 第一行：列名
        var columnTypes = lines[1].Split(','); // 第二行：数据类型
        var columnComments = lines[2].Split(','); // 第三行：注释

        // 创建XML文档
        XDocument xmlDoc = new XDocument(new XElement("Data"));

        // 从第四行开始读取数据
        for (int i = 3; i < lines.Length; i++)
        {
            var fields = lines[i].Split(',');

            // 获取第一列的值作为索引
            string indexValue = fields[0];

            // 创建<Record>元素，并添加id属性
            XElement recordElement = new XElement("Record");
            recordElement.SetAttributeValue("id", indexValue);

            for (int j = 0; j < columnNames.Length; j++)
            {
                // 创建列元素，并添加type和comment属性
                XElement fieldElement = new XElement(columnNames[j], fields[j]);
                fieldElement.SetAttributeValue("type", columnTypes[j]);
                fieldElement.SetAttributeValue("comment", columnComments[j]);

                // 将列元素添加到<Record>中
                recordElement.Add(fieldElement);
            }

            // 将<Record>添加到XML文档中
            xmlDoc.Root.Add(recordElement);
        }

        // 保存XML文件
        xmlDoc.Save(xmlFilePath);
    }
 */