using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public GameObject destination;
    public bool HasReachedGoal { get; private set; } = false;
    private NavMeshAgent playerNav;
    private bool isMoving = false;
    private float startTime;
    private TimeManager timeManager;
    private TrailRenderer trailRenderer;

    // 初期位置を保持するための変数
    private Vector3 initialPosition;
    private float currentSpeed;

    // プレイヤーを初期位置に戻すメソッド
    public void ResetPosition()
    {
        // NavMeshAgentのWarpメソッドを使用してプレイヤーを初期位置に戻す
        if (playerNav != null && initialPosition != null)
        {
            playerNav.Warp(initialPosition);
        }
        isMoving = false;
        HasReachedGoal = false;
        if (trailRenderer != null)
        {
            trailRenderer.Clear();
        }
    }

    void Start()
    {
        playerNav = GetComponent<NavMeshAgent>();
        timeManager = FindObjectOfType<TimeManager>();
        // スクリプトがアタッチされているGameObjectの現在の位置を初期位置として保存
        initialPosition = transform.position;
        trailRenderer = GetComponent<TrailRenderer>();
    }

    void Update()
    {
        if (isMoving)
        {
                // 移動中で、NavMeshAgentが経路を計算中でない（経路が完了した）ときにのみ、
                // 目的地に到着したかどうかをチェックする
                if (!playerNav.pathPending && playerNav.remainingDistance <= playerNav.stoppingDistance)
                {
                // プレイヤーの位置とゴールの位置の距離をXZ平面上で計算
                Vector3 playerPositionXZ = new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 destinationPositionXZ = new Vector3(destination.transform.position.x, 0, destination.transform.position.z);
                float distanceToGoalXZ = Vector3.Distance(playerPositionXZ, destinationPositionXZ);

                // 距離が0.5メートル以上の場合、「到達できません」と表示
                if (distanceToGoalXZ >= 0.2f)
                {
                    timeManager.ShowNoProgressMessage("Low likelihood of climbing");
                    StopMoving();
                }
                    StopMoving();
                }
                // NavMeshAgentの速度を常に更新
                if (playerNav != null)
                {
                    playerNav.speed = currentSpeed;
                }
        }
    }

    public void SetSpeed(float speed)
    {
        currentSpeed = speed; // 新しい速度を保持する変数に保存
        // ここで速度をNavMeshAgentに設定
        if (playerNav != null)
        {
            playerNav.speed = speed;
        }
    }

    public void StartMoving()
    {
        if (destination != null && !isMoving)
        {
            playerNav.SetDestination(destination.transform.position);
            isMoving = true;
            startTime = Time.time;

            // TrailRendererをオンにする
            if (trailRenderer != null)
            {
                trailRenderer.emitting = true;
            }

            if (timeManager != null)
            {
                timeManager.StartTimer();
            }
        }
    }

    private void StopMoving()
    {
        if (isMoving)
        {
            isMoving = false;
            HasReachedGoal = true;

            // TrailRendererをオフにする
            if (trailRenderer != null)
            {
                trailRenderer.emitting = false;
                //trailRenderer.Clear(); // オプションでトレイルをクリアする
            }

            if (timeManager != null)
            {
                timeManager.StopTimer();
            }
        }
    }

    public void PauseMovement()
    {
        if (isMoving)
        {
            playerNav.isStopped = true; // プレイヤーの動きを停止
            isMoving = false; // 移動状態のフラグを更新
        }
    }

    public void ResumeMovement()
    {
        if (!isMoving && destination != null)
        {
            playerNav.isStopped = false; // プレイヤーの動きを再開
            isMoving = true; // 移動状態のフラグを更新
        }
    }
}
