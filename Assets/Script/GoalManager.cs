using UnityEngine;

public class GoalManager : MonoBehaviour
{
    // ゴールの座標を定義
    public Vector3 goalPositionBalcony;
    public Vector3 goalPositionBed;
    public Vector3 goalPositionSofa;
    public Vector3 goalPositionChair;


    // ゴールオブジェクトを参照
    public GameObject goalObject;

    public void SetGoal(Vector3 position)
    {
        // ゴールオブジェクトの位置を設定
        goalObject.transform.position = position;
    }
}
