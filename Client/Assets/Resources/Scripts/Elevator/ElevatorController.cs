using System;
using UnityEngine;
using DG.Tweening;


[Serializable]
public class EleController{
    public GameObject elevator;
    public ElevatorNode[] elevatorNodes = Array.Empty<ElevatorNode>();
}

/// <summary>
/// 电梯控制器
/// </summary>
public class ElevatorController : MonoBehaviour{
    public static ElevatorController Instance => _mInstance;

    /// <summary>
    /// 电梯对象
    /// </summary>
    public GameObject elevatorGameObject;

    public ElevatorNode[] elevatorNodes;
    
    public EleController[] eleControllers;
    
    [Tooltip("电梯的运行速度")] public float elevatorSpeed = 2f;

    [Tooltip("电梯启动和停下的总时间")] public float elevatorStartAndStopTime = 1f;

    private static ElevatorController _mInstance;

    private void Awake(){
        _mInstance = this;
        DOTween.Init();
    }

    /// <summary>
    /// 使用电梯
    /// </summary>
    /// <param name="start">开始位置</param>
    /// <param name="end">结束位置</param>
    /// <param name="player">玩家对象</param>
    /// <param name="action">回调</param>
    public void Action(Vector3 start, Vector3 end, GameObject player, Action action){
        ElevatorMoveToPlayerFloor(start, () => { ElevatorMove(start, end, player, action); });
    }

    public void ElevatorMoveToPlayerFloor(Vector3 start, Action action){
        var sequence = DOTween.Sequence();
        float time = Mathf.Abs(elevatorGameObject.transform.position.y - start.y) / elevatorSpeed +
                     elevatorStartAndStopTime;
        sequence.Append(elevatorGameObject.transform.DOMoveY(start.y, time));
        sequence.AppendCallback(() => { action?.Invoke(); });
        sequence.Play();
    }

    /// <summary>
    /// 电梯移动方法
    /// </summary>
    /// <param name="start">起始坐标</param>
    /// <param name="end">终点坐标</param>
    /// <param name="player">玩家对象</param>
    /// <param name="backCall">完成回调</param>
    private void ElevatorMove(Vector3 start, Vector3 end, GameObject player, Action backCall){
        var sequence = DOTween.Sequence();

        sequence.Append(player.transform.DOMove(
            start + Vector3.up,
            (player.transform.position - start).magnitude / NavAIMoveModel.Instance.agent.speed));

        sequence.Append(player.transform.DORotate(new Vector3(0, 90, 0), 1f));

        float time = 0f;
        // 电梯是否在当前楼层
        if (Mathf.Abs(start.y - elevatorGameObject.transform.position.y) >= 1){
            time = Mathf.Abs(elevatorGameObject.transform.position.y - start.y) / elevatorSpeed +
                   elevatorStartAndStopTime;
            sequence.Append(elevatorGameObject.transform.DOMoveY(start.y, time));
        }

        // 进入电梯
        var v3 = elevatorGameObject.transform.position;
        v3.y += 1;
        sequence.Append(player.transform.DOMove(v3, 2f));
        sequence.Append(player.transform.DORotate(new Vector3(0, -90, 0), 1f));
        var parent = player.transform.parent;
        //将玩家设置为电梯子物体
        sequence.AppendCallback(() => { player.transform.SetParent(elevatorGameObject.transform); });

        // 电梯启动
        time = Mathf.Abs(elevatorGameObject.transform.position.y - end.y) / elevatorSpeed + elevatorStartAndStopTime;
        sequence.Append(elevatorGameObject.transform.DOMoveY(end.y, time));

        // 重置玩家父物体
        sequence.AppendCallback(() => { player.transform.SetParent(parent); });

        // 退出电梯
        end.y += 1; // player对象中心点位于中心，用于防止player模型移动到地下
        sequence.Append(player.transform.DOMove(end, 2f));

        // 执行回调
        sequence.AppendCallback(() => { backCall?.Invoke(); });
        sequence.Play();
    }
}