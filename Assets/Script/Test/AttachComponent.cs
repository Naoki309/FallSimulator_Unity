using UnityEngine;

public class AttachComponent : MonoBehaviour
{

    private void Awake()
    {
        foreach (Transform childTransform in gameObject.transform)
        {
            foreach (Transform grandChildTransform in childTransform)
            {               
                if (grandChildTransform.tag != "Untagged")
                {
                    grandChildTransform.gameObject.AddComponent<MeshCollider>();
                }

                foreach (Transform greatgrandChildTransform in grandChildTransform)
                {
                    // 全ひ孫オブジェクトにMeshColliderをAttach
                    greatgrandChildTransform.gameObject.AddComponent<MeshCollider>();
                }
            }
        }
    }

    //private void Awake()
    //{
    //    foreach (Transform childTransform in gameObject.transform)
    //    {
    //        foreach (Transform grandChildTransform in childTransform)
    //        {
    //            foreach (Transform greatgrandChildTransform in grandChildTransform)
    //            {
    //                string tagN = greatgrandChildTransform.tag;

    //                if (tagN != "Untagged" && tagN != "玄関" && tagN != "廊下" && tagN != "リビング" && tagN != "ダイニング"
    //                    && tagN != "キッチン" && tagN != "浴室" && tagN != "自室" && tagN != "寝室" && tagN != "ベランダ")
    //                {
    //                    // 認識させるためにコライダーをアタッチ
    //                    BoxCollider s = greatgrandChildTransform.gameObject.AddComponent<BoxCollider>();
    //                    s.isTrigger = true;
    //                    s.size = new Vector3(100.0f, 100.0f, 100.0f);
    //                }
    //                if (tagN != "Untagged")
    //                {
    //                    // 事故事例を表示するためのスクリプトをAttach
    //                    greatgrandChildTransform.gameObject.AddComponent<SituationSentence>();
    //                }
    //            }
    //        }
    //    }
    //}
    //private void Start()
    //{
    //    Invoke("AttachMesh", 1f);
    //}

    //void AttachMesh()
    //{
    //    foreach (Transform childTransform in gameObject.transform)
    //    {
    //        foreach (Transform grandChildTransform in childTransform)
    //        {
    //            foreach (Transform greatgrandChildTransform in grandChildTransform)
    //            {
    //                // 全ひ孫オブジェクトにMeshColliderをAttach
    //                greatgrandChildTransform.gameObject.AddComponent<MeshCollider>();
    //            }
    //        }
    //    }
    //}
}