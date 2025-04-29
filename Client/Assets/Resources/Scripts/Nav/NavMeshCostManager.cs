using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


/// <summary>
/// 导航权值管理器
/// </summary>
public class NavMeshCostManager : MonoBehaviour{

    public static NavMeshCostManager Instance;

    public float ElevatorCost = 0f;
    
    /// <summary>
    /// 电梯权值最大值
    /// </summary>
    public float ElevatorCostMax = 2f;

    /// <summary>
    /// 电梯权值最小值
    /// </summary>
    public float ElevatorCostMin = 0.2f;

    /// <summary>
    /// 楼梯权值最大值
    /// </summary>
    public float StreetPlateCostMax;

    /// <summary>
    /// 楼梯权值最小值
    /// </summary>
    public float StreetPlateCostMin;

    /// <summary>
    /// 电梯的offMeshLink组件数组
    /// </summary>
    [SerializeField]
    public OffMeshLink[] ElevatorLinks;
    
    private void Awake(){
        Instance = this;
    }

    private void Start(){
        foreach (var node in ElevatorController.Instance.elevatorNodes){
            node.Init();
            ElevatorLinks =  ElevatorLinks.Concat(node.link.Values.ToArray()).ToArray();
        }
    }

    /// <summary>
    /// 随机化区域和电梯权值
    /// </summary>
    /// <param name="NavMeshAreaNames">导航网格区域名称</param>
    public void RandomizeCost(string[] NavMeshAreaNames){
        Random.InitState((int)System.DateTime.Now.Ticks);
        float cost;
        for (int i = 0; i < NavMeshAreaNames.Length; i++){
            cost = Random.Range(StreetPlateCostMin, StreetPlateCostMax);
            SetStreetPlateCost(NavMeshAreaNames[i], cost);
        }
        
        cost = Random.Range(ElevatorCostMin, ElevatorCostMax);
        SetElevatorCost(cost);
    }

    public float GetElevatorCost(){
        return ElevatorCost;
    }


    public float GetStreetPlateCost(string AreaName){
        var index = NavMesh.GetAreaFromName(AreaName);
        return NavMesh.GetAreaCost(index);
    }
    
    /// <summary>
    /// 设置楼梯区域的权值
    /// </summary>
    /// <param name="areaName">区域名称</param>
    /// <param name="cost">权值</param>
    public void SetStreetPlateCost(string areaName,float cost){
        var areaIndex = NavMesh.GetAreaFromName(areaName);
        NavMesh.SetAreaCost(areaIndex, cost);
    }
    
    /// <summary>
    /// 设置电梯区域的权值
    /// </summary>
    /// <param name="cost">权值</param>
    public void SetElevatorCost(float cost){
        if (ElevatorLinks != null){
            ElevatorCost = cost;
            for (int i = 0; i < ElevatorLinks.Length; i++){
                ElevatorLinks[i].costOverride = cost;
            }
        }
    }

    public void SetCost(){
        
    }


    
}