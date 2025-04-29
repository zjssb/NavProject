using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 随机化导航权值模型
/// </summary>
public class NavRandomizedCostModel{
    public static NavRandomizedCostModel Instance => _instance ??= new NavRandomizedCostModel();
    
    private static NavRandomizedCostModel _instance;
    
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
    public float StreetPlateCostMax = 5f;
    /// <summary>
    /// 楼梯权值最小值
    /// </summary>
    public float StreetPlateCostMin = 1f;

    /// <summary>
    /// 电梯的offMeshLink组件数组
    /// </summary>
    public OffMeshLink[] ElevatorLinks;

    /// <summary>
    /// 随机化区域和电梯权值
    /// </summary>
    /// <param name="NavMeshAreas">导航网格区域名称</param>
    public void RandomizeCost(string[] NavMeshAreas){
        Random.InitState((int)System.DateTime.Now.Ticks);
        float cost;
        int areaIndex;
        for (int i = 0; i < NavMeshAreas.Length; i++){
            cost = Random.Range(StreetPlateCostMin, StreetPlateCostMax);
            areaIndex = NavMesh.GetAreaFromName(NavMeshAreas[i]);
            NavMesh.SetAreaCost(areaIndex,cost);
        }
        
    }
    
}