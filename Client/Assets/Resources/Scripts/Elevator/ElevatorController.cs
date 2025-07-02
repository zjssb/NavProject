using System;
using UnityEngine;
using DG.Tweening;


[Serializable]
public class EleController{
    public GameObject elevator;
    public ElevatorNode[] elevatorNodes = Array.Empty<ElevatorNode>();
}

[Serializable]
public class EleDoor{
    public GameObject DoorL;
    public GameObject DoorR;
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
    
    public EleController[] eleControllers;

    public EleDoor F11;
    public EleDoor F12;
    public EleDoor F21;
    public EleDoor F22;
    public EleDoor F31;
    public EleDoor F32;
    
    [Tooltip("电梯的运行速度")] public float elevatorSpeed = 2f;

    [Tooltip("电梯启动和停下的总时间")] public float elevatorStartAndStopTime = 1f;

    private static ElevatorController _mInstance;

    private int PlayerFoolNum = 1;

    private void Awake(){
        _mInstance = this;
        DOTween.Init();
    }

    public void SetElevator(GameObject player){
        float minNum = Int16.MaxValue;
        float length;
        foreach (var controller in eleControllers){
            length = Vector3.Distance(player.transform.position, controller.elevator.transform.position);
            if ( length < minNum){
                minNum = length;
                elevatorGameObject = controller.elevator;
            }
        }
    }
    
    
    /// <summary>
    /// 使用电梯
    /// </summary>
    /// <param name="start">开始位置</param>
    /// <param name="end">结束位置</param>
    /// <param name="player">玩家对象</param>
    /// <param name="action">回调</param>
    public void Action(Vector3 start, Vector3 end, GameObject player, Action action){
        PlayerFoolNum = (int)(player.transform.position.y / 4 + 1);
        ElevatorMoveToPlayerFloor(start, player,() => { ElevatorMove(start, end, player, action); });
    }

    /// <summary>
    /// 移动到电梯门
    /// </summary>
    /// <param name="start"></param>
    /// <param name="action"></param>
    public void ElevatorMoveToPlayerFloor(Vector3 start,GameObject player, Action action){
        var sequence = DOTween.Sequence();
        sequence.Append(player.transform.DOMove(
            start + Vector3.up,
            (player.transform.position - start).magnitude / NavAIMoveModel.Instance.agent.speed));
        var look = elevatorGameObject.transform.position;
        look.y = player.transform.position.y;
        sequence.Append(player.transform.DOLookAt(look, 1f));
        
        float time = GetElevatorMovePlayerFoolTime(start);
        if (time != 0f){
            sequence.Append(elevatorGameObject.transform.DOMoveY(start.y, time));
        }
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
        string name = elevatorGameObject.gameObject.name;
        var index = name.Substring(name.Length - 1, 1);
        GameObject DL = null;
        GameObject DR = null;
        GetTargetFool(PlayerFoolNum,index, out DL, out DR);
        sequence.Append(DL.transform.DOMove(DL.transform.position - new Vector3(0,0,1),1f));
        sequence.Insert(0,DR.transform.DOMove(DR.transform.position + new Vector3(0,0,1),1f));
        sequence.Insert(0,elevatorGameObject.transform.GetChild(0).GetChild(0).DOMove(
            elevatorGameObject.transform.GetChild(0).GetChild(0).position - new Vector3(0,0,1),1f));
        sequence.Insert(0,elevatorGameObject.transform.GetChild(0).GetChild(1).DOMove(
            elevatorGameObject.transform.GetChild(0).GetChild(1).position + new Vector3(0,0,1),1f));
        // 进入电梯
        var v3 = elevatorGameObject.transform.position;
        v3.y = player.transform.position.y;
        sequence.Append(player.transform.DOMove(v3, 2f));
        sequence.Append(player.transform.DORotate(new Vector3(0,180,0), 1f,RotateMode.WorldAxisAdd));
        
        sequence.Append(DL.transform.DOMove(DL.transform.position,1f));
        sequence.Insert(2,DR.transform.DOMove(DR.transform.position,1f));
        sequence.Insert(2,elevatorGameObject.transform.GetChild(0).GetChild(0).DOMove(
            elevatorGameObject.transform.GetChild(0).GetChild(0).position,1f));
        sequence.Insert(2,elevatorGameObject.transform.GetChild(0).GetChild(1).DOMove(
            elevatorGameObject.transform.GetChild(0).GetChild(1).position,1f));
        
        var parent = player.transform.parent;
        //将玩家设置为电梯子物体
        sequence.AppendCallback(() => { player.transform.SetParent(elevatorGameObject.transform); });
        
        // 电梯启动
        var time = Mathf.Abs(elevatorGameObject.transform.position.y - end.y) / elevatorSpeed + elevatorStartAndStopTime;
        sequence.Append(elevatorGameObject.transform.DOMoveY(end.y, time));
        
        sequence.Append(elevatorGameObject.transform.GetChild(0).GetChild(0).DOMoveZ(
            elevatorGameObject.transform.GetChild(0).GetChild(0).position.z - 1,1f));
        sequence.Join(elevatorGameObject.transform.GetChild(0).GetChild(1).DOMoveZ(
            elevatorGameObject.transform.GetChild(0).GetChild(1).position.z + 1,1f));
        
        GetTargetFool((int)((end.y + 1) / 4 + 1),index, out DL, out DR);
        sequence.Join(DL.transform.DOMoveZ(DL.transform.position.z - 1,1f));
        sequence.Join(DR.transform.DOMoveZ(DR.transform.position.z + 1,1f));
        // 重置玩家父物体
        sequence.AppendCallback(() => { player.transform.SetParent(parent); });

        // 退出电梯
        end.y += 1; // player对象中心点位于中心，用于防止player模型移动到地下
        sequence.Append(player.transform.DOMove(end, 2f));
    
        sequence.Append(elevatorGameObject.transform.GetChild(0).GetChild(0).DOMoveZ(
            elevatorGameObject.transform.GetChild(0).GetChild(0).position.z,1f));
        sequence.Join(elevatorGameObject.transform.GetChild(0).GetChild(1).DOMoveZ(
            elevatorGameObject.transform.GetChild(0).GetChild(1).position.z,1f));
        sequence.Join(DL.transform.DOMoveZ(DL.transform.position.z ,1f));
        sequence.Join(DR.transform.DOMoveZ(DR.transform.position.z ,1f));
        // 执行回调
        sequence.AppendCallback(() => { backCall?.Invoke(); });
        sequence.Play();
    }

    private float GetElevatorMovePlayerFoolTime(Vector3 start){
        var length = Mathf.Abs(elevatorGameObject.transform.position.y - start.y);
        if (length < 2){
            return 0f;
        }
        return  length / elevatorSpeed + elevatorStartAndStopTime;
    }

    private void GetTargetFool(int fool, string index, out GameObject DL, out GameObject DR){
        DL = null;
        DR = null;
        switch (fool){
            case 1:
                if (index == "1"){
                    DL = F11.DoorL;
                    DR = F11.DoorR;
                }
                else{
                    DL = F12.DoorR;
                    DR = F12.DoorL;
                }
                break;
            case 2:
                if (index == "1"){
                    DL = F21.DoorL;
                    DR = F21.DoorR;
                }
                else{
                    DL = F22.DoorR;
                    DR = F22.DoorL;
                }
                break;
            case 3:
                if (index == "1"){
                    DL = F31.DoorL;
                    DR = F31.DoorR;
                }
                else{
                    DL = F32.DoorR;
                    DR = F32.DoorL;
                }
                break;
        }
    }
}