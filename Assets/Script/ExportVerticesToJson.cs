using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class VertexData
{
    public List<Vector3Data> vertices;

    public VertexData(Mesh mesh)
    {
        vertices = new List<Vector3Data>();
        foreach (Vector3 vertex in mesh.vertices)
        {
            vertices.Add(new Vector3Data(vertex));
        }
    }
}

[System.Serializable]
public class Vector3Data
{
    public float x, y, z;

    public Vector3Data(Vector3 vector3)
    {
        x = vector3.x;
        y = vector3.y;
        z = vector3.z;
    }
}

public class ExportVerticesToJson : MonoBehaviour
{
    [SerializeField] private string outputPath = @"C:\SharedFolder\unity.json";  // 出力パスを指定

    void Start()
    {
        // Sofa_17/mesh オブジェクトを自動で見つけてエクスポート
        GameObject targetObject = GameObject.Find("Furniture/402/mesh");
        if (targetObject != null)
        {
            ExportMeshVertices(targetObject);
        }
        else
        {
            Debug.LogError("Furniture/Sofa_17/mesh not found in the scene!");
        }
    }

    public void ExportMeshVertices(GameObject objectToExport)
    {
        MeshFilter meshFilter = objectToExport.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("MeshFilter not found on the object!");
            return;
        }

        VertexData vertexData = new VertexData(meshFilter.mesh);
        string json = JsonUtility.ToJson(vertexData, true);
        File.WriteAllText(outputPath, json);
        Debug.Log("Vertices exported to JSON: " + outputPath);
    }
}
