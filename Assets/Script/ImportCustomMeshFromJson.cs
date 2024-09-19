using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;  // Newtonsoft.Jsonライブラリを使用

[System.Serializable]
public class CustomVertexData  // 頂点データ
{
    public float x, y, z;
}

[System.Serializable]
public class CustomMeshData  // メッシュデータ
{
    public List<CustomVertexData> vertices;  // 頂点リスト
    public List<List<int>> faces;  // 面（インデックスのリスト）
}

public class ImportCustomMeshFromJson : MonoBehaviour
{
    [SerializeField] private string jsonFilePath = @"C:\SharedFolder\000000_pred_confident_nms_bbox.json";  // JSONファイルのパス
    [SerializeField] private Material meshMaterial;  // メッシュのマテリアル

    void Start()
    {
        // JSONファイルを読み込んでメッシュを生成
        if (File.Exists(jsonFilePath))
        {
            string jsonContent = File.ReadAllText(jsonFilePath);
            CustomMeshData meshData = JsonConvert.DeserializeObject<CustomMeshData>(jsonContent);  // Newtonsoft.Jsonを使用

            if (meshData == null)
            {
                Debug.LogError("meshDataがnullです。JSONの構造が正しいか確認してください。");
                return;
            }

            // verticesとfacesのチェック
            if (meshData.vertices == null || meshData.vertices.Count == 0)
            {
                Debug.LogError("meshData.verticesがnullか空です。JSONファイルを確認してください。");
                return;
            }

            if (meshData.faces == null || meshData.faces.Count == 0)
            {
                Debug.LogError("meshData.facesがnullか空です。JSONファイルを確認してください。");
                return;
            }

            // メッシュ生成を続行
            GenerateMesh(meshData);
        }
        else
        {
            Debug.LogError("JSONファイルが見つかりません: " + jsonFilePath);
        }
    }

    void GenerateMesh(CustomMeshData meshData)
    {
        // 新しいメッシュオブジェクトを作成
        Mesh mesh = new Mesh();

        // 頂点データをメッシュに適用
        Vector3[] vertices = new Vector3[meshData.vertices.Count];
        for (int i = 0; i < meshData.vertices.Count; i++)
        {
            vertices[i] = new Vector3(meshData.vertices[i].x, meshData.vertices[i].y, meshData.vertices[i].z);
        }
        mesh.vertices = vertices;

        // 面データをメッシュに適用
        List<int> indices = new List<int>();
        foreach (List<int> face in meshData.faces)
        {
            if (face.Count == 3)  // 三角形の面のみを処理
            {
                indices.AddRange(face);
            }
        }
        mesh.triangles = indices.ToArray();

        // 法線とバウンディングボックスを再計算
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // メッシュをアタッチ
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        // マテリアルがnullの場合のデフォルト設定
        Material transparentMaterial;
        if (meshMaterial != null)
        {
            transparentMaterial = new Material(meshMaterial);  // Inspectorで設定されたマテリアルをコピー
        }
        else
        {
            Debug.LogWarning("meshMaterialが設定されていないため、デフォルトのマテリアルを使用します。");
            transparentMaterial = new Material(Shader.Find("Standard"));  // デフォルトのStandardシェーダーマテリアルを使用
        }

        // マテリアルを透明に設定
        transparentMaterial.SetFloat("_Mode", 3);  // 透明モードに設定
        transparentMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        transparentMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        transparentMaterial.SetInt("_ZWrite", 0);
        transparentMaterial.DisableKeyword("_ALPHATEST_ON");
        transparentMaterial.EnableKeyword("_ALPHABLEND_ON");
        transparentMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        transparentMaterial.renderQueue = 3000;

        // 面の透明度を上げる (Alpha値を設定)
        Color transparentColor = Color.red;  // エッジは赤
        transparentColor.a = 0.1f;  // 透明度を設定
        transparentMaterial.color = transparentColor;

        // マテリアルを適用
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = transparentMaterial;

        Debug.Log("メッシュが生成されました");
    }
}
