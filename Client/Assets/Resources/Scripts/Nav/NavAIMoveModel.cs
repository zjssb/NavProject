using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class NavAIMoveModel : MonoBehaviour{
    public static NavAIMoveModel Instance;

    /// <summary>
    /// 是否正在导航
    /// </summary>
    public bool isMoveing = false;
    
    private void Awake(){
        Instance = this;
    }


    public NavMeshAgent agent;

    [Tooltip("是否开启路径绘制")] public bool isDrawLine;

    [FormerlySerializedAs("LineRenderer")] 
    public LineRenderer lineRenderer;

    public GameObject startSteert;
    
    /// <summary>
    /// 是否正在电梯中
    /// </summary>
    public bool isElevator = false;
    
    private void Update(){
        if (agent&& agent.isOnOffMeshLink && !isElevator){
            isElevator = true;
            var data = agent.currentOffMeshLinkData;
            agent.velocity = Vector3.zero;
            FirstPersonController.Instance.cameraCanMove = false;
            ElevatorController.Instance.SetElevator(agent.gameObject);
            ElevatorController.Instance.Action(data.startPos,data.endPos,agent.gameObject, () => {
                FirstPersonController.Instance.cameraCanMove = true;
                isElevator = false;
                agent.updateRotation = true;
                agent.updatePosition = true;
                agent.CompleteOffMeshLink();
            });
        }

        if (agent && agent.hasPath && agent.remainingDistance <= agent.stoppingDistance){
            isMoveing = false;
            agent.ResetPath();
        }
    }

    public void NavMove(Transform target){
        NavMove(target.position);
    }

    public void NavMove(Vector3 target){
        agent.ResetPath();
        agent.SetDestination(target);
        isMoveing = true;
    }

    public void RePos(){
        agent.ResetPath();
        if (startSteert){
            agent.gameObject.transform.position = startSteert.transform.position;
        }
        else{
            agent.gameObject.transform.position = new Vector3(2, 1, -2);
            agent.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }
    
    IEnumerator DrawLine(){
        if (agent is null){
            yield break;
        }

        yield return new WaitUntil(() =>
            (!agent.pathPending && agent.pathStatus == NavMeshPathStatus.PathComplete));
            
        // 绘制路线
        while (true){
            var path = agent.path.corners;
            lineRenderer.positionCount = path.Length;
            for (int i = 0; i < path.Length; i++){
                var p = path[i];
                lineRenderer.SetPosition(i, p);
            }

            yield return new WaitForSeconds(0.1f);
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.ResetPath();
                yield break;
            }
            
        }
    }
    
}