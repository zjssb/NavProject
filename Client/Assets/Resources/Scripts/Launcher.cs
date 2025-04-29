using UnityEngine;

/*
 * 启动器，用于设置启动时的配置
 */
public class Launcher : MonoBehaviour{

    private static Launcher Instance;

    public static Launcher GetInstance(){
        if (Instance == null){
            Debug.LogError("Launcher is null!");
            return null;
        }
        return Instance;
    }
    
    [Header("第一人称控制器")]
    public FirstPersonController player;
    
    [Header("控制器相关")] 
    [Tooltip("是否开启移动")]
    public bool isMove;
    [Tooltip("是否开启跳跃")]
    public bool isJump;
    [Tooltip("是否开启疾跑")]
    public bool isRun;
    [Tooltip("是否开启下蹲")]
    public bool isSquat;
    [Tooltip("是否开启光标")]
    public bool isCursor;
    [Tooltip("是否开启右键缩放")]
    public bool isScale;
    [Tooltip("是否开启头模拟")]
    public bool isHeadBob;

    private void Awake(){
        Instance = this;
        player.enableJump = isJump;
        player.enableCrouch = isSquat;
        player.holdToCrouch = isSquat;
        player.enableSprint = isRun;
        player.enableZoom = isScale;
        player.playerCanMove = isMove;
        player.lockCursor = !isCursor;
        player.enableHeadBob = isHeadBob;
    }
}
