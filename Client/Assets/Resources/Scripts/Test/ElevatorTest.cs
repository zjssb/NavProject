using UnityEngine;

public class ElevatorTest : MonoBehaviour{
    
    public Transform target1;
    
    public Transform target2;

    public GameObject go;
    
    public bool isToggle = false;

    // Update is called once per frame
    void Update()
    {
        if (isToggle){
            isToggle = false;
        }
    }
}
