using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

[CustomEditor(typeof(GOTable))]
public class ButtonSerializerEditor : Editor{
    public override void OnInspectorGUI(){
        // 绘制默认的Inspector界面
        DrawDefaultInspector();

        // 获取目标脚本
        GOTable serializer = (GOTable)target;

        // 添加一个按钮
        if (GUILayout.Button("初始化GoTable")){
            // 点击按钮时执行序列化操作
            serializer.SerializeList();
        }
    }
}

public class GOTable : MonoBehaviour{
    public List<Button> BtnList = new();

    public List<Toggle> ToggleList = new();

    public List<TextMeshProUGUI> TextList = new();

    public List<Image> ImageList = new();

    public List<GameObject> ObjList = new();
    
    public UIBaseView uiView;
    
    public event Action<GameObject,bool> BtnOnClick;
    
    // 序列化子物体中的所有按钮
    public void SerializeList(){
        // 清空之前的按钮信息
        BtnList.Clear();
        ToggleList.Clear();
        TextList.Clear();
        ImageList.Clear();
        ObjList.Clear();

        UpdateList(transform);
    }

    public Object GetListComponent(string name, EnumManager.TypeEnum enumType){
        Type type = EnumManager.TypeDict[enumType];
        if (type==typeof(Button)){
            foreach (var button in BtnList){
                if (button.name == name){
                    return button;
                }
            }
            Debug.LogError("按钮" + name + "不存在");
            return null;
        }
        else if(type==typeof(Toggle))
        {
            foreach (var toggle in ToggleList){
                if (toggle.name == name){
                    return toggle;
                }
            }
            Debug.LogError("触发器" + name + "不存在");
            return null;
        }
        else if (type == typeof(TextMeshProUGUI)){
            foreach (var text in TextList){
                if (text.name == name){
                    return text;
                }
            }
            Debug.LogError("文本" + name + "不存在");
            return null;
        }

        if (type == typeof(Image)){
            foreach (var image in ImageList){
                if (image.name == name){
                    return image;
                }
            }
            Debug.LogError("图片" + name + "不存在");
            return null;
        }

        if (type == typeof(GameObject)){
            foreach (var obj in ObjList){
                if (obj.name == name){
                    return obj;
                }
            }
            Debug.LogError("obj:" + name + "不存在");
            return null;
        }
        
        return null;
    }

    
    private void UpdateList(Transform t){
        // 遍历所有子物体
        foreach (Transform child in t){
            bool flag = !child.name[0].Equals('$');
            if (child.childCount > 0 && flag){
                UpdateList(child);
            }
            
            if (!child.name[0].Equals('@') && flag){
                continue;
            }

            var strs = child.gameObject.name.Split('_');
            switch (strs[1]){
                case "btn": AddListComponent(child,typeof(Button)); break;
                case "toggle": AddListComponent(child,typeof(Toggle)); break;
                case "text": AddListComponent(child,typeof(TextMeshProUGUI)); break;
                case "image": AddListComponent(child,typeof(Image)); break;
                case "obj": ObjList.Add(child.gameObject); break;
            }
        }
    }

    private void AddListComponent(Transform tran,Type type){
        var go = tran.gameObject;
        if (type == typeof(Button)){
            var btn = go.GetComponent<Button>();
            btn.onClick.AddListener(()=> {
                BtnOnClick?.Invoke(go,false);
            });
            BtnList.Add(btn);
            return;
        }

        if (type == typeof(Toggle)){
            var toggle = go.GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(isOn => {
                BtnOnClick?.Invoke(go,isOn);
            });
            ToggleList.Add(toggle);
            return;
        }

        if (type == typeof(TextMeshProUGUI)){
            TextList.Add(go.GetComponent<TextMeshProUGUI>());
            return;
        }

        if (type == typeof(Image)){
            ImageList.Add(go.GetComponent<Image>());
            return;
        }
    }


    private void OnValidate(){
        var view = gameObject.GetComponent<UIBaseView>();
        if (!view){
            view = this.gameObject.AddComponent<UIBaseView>();
        }
        uiView = view;
    }

}