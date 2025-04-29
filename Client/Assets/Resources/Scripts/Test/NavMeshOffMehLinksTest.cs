using UnityEngine;
using UnityEngine.AI;

public class NavMeshOffMehLinksTest : MonoBehaviour{
    public GameObject target;
    
    public NavMeshAgent agent;
    
    public bool IsTogger = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsTogger){
            IsTogger = false;
            Move();
        }
    }

    void Move(){
        if (target){
            agent.SetDestination(target.transform.position);
        }
    }
    
}
