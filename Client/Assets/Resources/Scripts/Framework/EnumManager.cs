using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnumManager{
    #region 时间枚举
    
        public enum NavAction{
            NavStart,// 开始寻路
        }

    #endregion
    
    
    public enum TypeEnum{
        Btn,
        Toggle,
        Text,
        Image,
        GameObject,
    }

    public static Dictionary<TypeEnum, Type> TypeDict = new(){
        { TypeEnum.Btn, typeof(Button) },
        { TypeEnum.Toggle, typeof(Toggle) },
        { TypeEnum.Text, typeof(TextMeshProUGUI) },
        { TypeEnum.Image, typeof(Image) },
        { TypeEnum.GameObject, typeof(GameObject) }
    };


    
    
    
}