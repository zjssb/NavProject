using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[Serializable]
public class Elevator{
    public float cost = -1;
    public GameObject elevator;
    public OffMeshLink[] ElevatorLinks = Array.Empty<OffMeshLink>();
}

/// <summary>
/// 导航权值管理器
/// </summary>
public class NavMeshCostManager : MonoBehaviour{
    public static NavMeshCostManager Instance;

    [Tooltip("自动随机权值")] public bool autoRandomCost = true;

    [Tooltip("自动随机电梯权值（在autoRandomCost开启的情况下）")]
    public bool autoElevatorCost = false;
    
    [Tooltip("随机化间隔时间")]
    public float randomTime = 5f;

    /// <summary>
    /// 电梯权值最大值
    /// </summary>
    public float ElevatorCostMax = 0f;

    /// <summary>
    /// 电梯权值最小值
    /// </summary>
    public float ElevatorCostMin = 0f;

    /// <summary>
    /// 楼梯权值最大值
    /// </summary>
    public float StreetPlateCostMax = 2f;

    /// <summary>
    /// 楼梯权值最小值
    /// </summary>
    public float StreetPlateCostMin = 1f;

    /// <summary>
    /// 电梯的offMeshLink组件数组
    /// </summary>
    public Elevator[] Elevators = Array.Empty<Elevator>();

    private Dictionary<string, float> navMeshArea = new();

    private void Awake(){
        Instance = this;
        navMeshArea = new Dictionary<string, float>{
            { "F11", GetStreetPlateCost("F11") },
            { "F12", GetStreetPlateCost("F12") },
            { "F21", GetStreetPlateCost("F21") },
            { "F22", GetStreetPlateCost("F22") },
            { "F31", GetStreetPlateCost("F31") },
            { "F32", GetStreetPlateCost("F32") }
        };
    }

    private void Start(){
        Elevators = new Elevator[ElevatorController.Instance.eleControllers.Length];
        for (int i = 0; i < ElevatorController.Instance.eleControllers.Length; i++){
            Elevators[i] = new Elevator();
            var con = ElevatorController.Instance.eleControllers[i];
            Elevators[i].elevator = con.elevator;
            foreach (var node in con.elevatorNodes){
                node.Init();
                Elevators[i].ElevatorLinks = Elevators[i].ElevatorLinks.Concat(node.link.Values.ToArray()).ToArray();
            }
        }

        StartCoroutine(SetElevatorCostCoroutine());
    }

    /// <summary>
    /// 随机化区域和电梯权值
    /// </summary>
    public void RandomizeCost(){
        Random.InitState((int)System.DateTime.Now.Ticks);
        float cost;
        var names = navMeshArea.Keys.ToArray();
        for (int i = 0; i < names.Length; i++){
            cost = Random.Range(StreetPlateCostMin, StreetPlateCostMax);
            SetStreetPlateCost(names[i], cost);
        }

        if (autoElevatorCost){
            for (int i = 0; i < Elevators.Length; i++){
                cost = Random.Range(ElevatorCostMin, ElevatorCostMax);
                SetElevatorCost(i, cost);
            }
        }
    }

    public float GetElevatorCost(int index){
        return Elevators[index].cost;
    }


    public float GetStreetPlateCost(string AreaName){
        var index = NavMesh.GetAreaFromName(AreaName);
        return NavMesh.GetAreaCost(index);
    }

    public float GetStreetPlateCostByDict(string areaName){
        if (navMeshArea == null){
            return 0f;
        }

        return navMeshArea.GetValueOrDefault(areaName, 0f);
    }

    /// <summary>
    /// 设置楼梯区域的权值
    /// </summary>
    /// <param name="areaName">区域名称</param>
    /// <param name="cost">权值</param>
    public void SetStreetPlateCost(string areaName, float cost){
        var areaIndex = NavMesh.GetAreaFromName(areaName);
        if (areaIndex <= 0){
            return;
        }

        NavMesh.SetAreaCost(areaIndex, cost);
        navMeshArea[areaName] = cost;
    }

    /// <summary>
    /// 设置电梯区域的权值
    /// </summary>
    /// <param name="cost">权值</param>
    public void SetElevatorCost(int index, float cost){
        Elevators[index].cost = cost;
        var elevatorLink = Elevators[index].ElevatorLinks;
        if (elevatorLink != null){
            for (int i = 0; i < elevatorLink.Length; i++){
                elevatorLink[i].costOverride = cost;
            }
        }
    }

    public IEnumerator SetElevatorCostCoroutine(){
        while (true){
            yield return new WaitUntil(() => autoRandomCost);
            yield return new WaitForSeconds(randomTime);
            RandomizeCost();
        }
    }
}