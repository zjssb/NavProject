using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class NavAIMoveModel : MonoBehaviour{
    public static NavAIMoveModel Instance;

    private void Awake(){
        Instance = this;
    }


    public NavMeshAgent agent;

    [Tooltip("是否开启路径绘制")] public bool isDrawLine;

    [FormerlySerializedAs("LineRenderer")] 
    public LineRenderer lineRenderer;

    /// <summary>
    /// 是否正在电梯中
    /// </summary>
    public bool isElevator = false;
    
    private void Update(){
        if (agent&& agent.isOnOffMeshLink && !isElevator){
            isElevator = true;
            var data = agent.currentOffMeshLinkData;
            // agent.updateRotation = false;
            // agent.updatePosition = false;
            agent.velocity = Vector3.zero;
            ElevatorController.Instance.Action(data.startPos,data.endPos,agent.gameObject, () => {
                isElevator = false;
                agent.updateRotation = true;
                agent.updatePosition = true;
                agent.CompleteOffMeshLink();
            });
        }
    }

    public void NavMove(Transform target){
        NavMove(target.position);
    }

    public void NavMove(Vector3 target){
        agent.ResetPath();
        agent.SetDestination(target);
        if (isDrawLine){
            StartCoroutine(DrawLine());
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