using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform player;
    public Vector3 positionOffset; // 位置のオフセット
    public GameObject panelStart; // PanelStartへの参照
    public Vector3 fixedDirection = Vector3.forward; // カメラが向く固定方向
    public Vector3 rotationOffset; // 角度のオフセット

    private Quaternion initialRotation; // 初期の回転
    private Vector3 initialPosition; // 初期の位置

    void Start()
    {
        // 初期位置と回転を設定
        initialPosition = player.position + positionOffset;
        initialRotation = Quaternion.LookRotation(fixedDirection) * Quaternion.Euler(rotationOffset);
    }

    void Update()
    {
        if (!panelStart.activeSelf)
        {
            // スタート時の位置と向きを保持
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
    }
}



// using UnityEngine;

// public class CameraManager : MonoBehaviour
// {
//     public Transform player;
//     public Vector3 offset;
//     public GameObject panelStart; // PanelStartへの参照

//     void Update()
//     {
//         // PanelStartが非アクティブな場合のみカメラを更新
//         if (!panelStart.activeSelf)
//         {
//             // カメラの位置をプレイヤーの位置にオフセットを加えて設定
//             transform.position = player.position + offset;

//             // カメラがプレイヤーの方を向くようにする
//             transform.LookAt(player.position + new Vector3(0, -1, 0));
//         }
//     }
// }