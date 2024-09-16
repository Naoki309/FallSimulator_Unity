using UnityEngine;

public class StartSetting : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject player;
    public GameObject panelStart;

    public float cameraRotateSpeed;
    public float cameraZoomSpeed;
    public float cameraPanSpeed;
    public float playerDragSpeed; // プレイヤーのドラッグ移動速度
    private bool isDraggingPlayer = false;

    void Update()
    {
        if (!panelStart.activeSelf) return;

        // プレイヤーのドラッグ移動（左ドラッグ）
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == player)
            {
                isDraggingPlayer = true;
                Rigidbody rb = player.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true; // 物理演算を無効化
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // ドラッグ終了時
            isDraggingPlayer = false;
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // 物理演算を有効化
            }
        }

        if (isDraggingPlayer)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetPosition = new Vector3(hit.point.x, player.transform.position.y, hit.point.z);
                player.transform.position = Vector3.Lerp(player.transform.position, targetPosition, playerDragSpeed);
            }
        }

        // カメラのズーム（ホイール）
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        mainCamera.transform.Translate(0, 0, scroll * cameraZoomSpeed * Time.deltaTime, Space.Self);

        // カメラのパン（ホイールドラッグ）
        if (Input.GetMouseButton(2))
        {
            float panX = Input.GetAxis("Mouse X") * cameraPanSpeed * Time.deltaTime;
            float panY = Input.GetAxis("Mouse Y") * cameraPanSpeed * Time.deltaTime;
            mainCamera.transform.Translate(-panX, -panY, 0, Space.World);
        }

        // カメラの回転（右ドラッグ）
        if (Input.GetMouseButton(1))
        {
            float rotationX = Input.GetAxis("Mouse X") * cameraRotateSpeed * Time.deltaTime;
            float rotationY = Input.GetAxis("Mouse Y") * cameraRotateSpeed * Time.deltaTime;
            mainCamera.transform.eulerAngles += new Vector3(-rotationY, rotationX, 0);
        }
    }
}
