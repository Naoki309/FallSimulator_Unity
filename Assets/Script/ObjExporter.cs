using System.IO;
using UnityEngine;

public class ObjExporter : MonoBehaviour
{
    public string outputFolderPath = @"C:\SharedFolder";  // 出力フォルダパス

    void Start()
    {
        // GameObject.Findを使用して階層内のオブジェクトを正確に指定
        string objectPath = "Funiture/Sofa_17/mesh";
        GameObject objectToExport = GameObject.Find(objectPath);

        if (objectToExport != null)
        {
            // オブジェクトが見つかった場合、OBJファイルとしてエクスポート
            ExportObjectToOBJ(objectToExport, outputFolderPath);
        }
        else
        {
            Debug.LogError("Object not found at path: " + objectPath);
        }
    }

    public void ExportObjectToOBJ(GameObject obj, string folderPath)
    {
        string objFileName = obj.name + ".obj";
        string fullPath = Path.Combine(folderPath, objFileName);

        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("No MeshFilter found on object: " + obj.name);
            return;
        }

        Mesh mesh = meshFilter.mesh;
        using (StreamWriter writer = new StreamWriter(fullPath))
        {
            writer.Write(MeshToString(mesh, obj.transform));
            Debug.Log("OBJ file exported to: " + fullPath);
        }
    }

    private string MeshToString(Mesh mesh, Transform transform)
    {
        StringWriter sw = new StringWriter();
        sw.WriteLine("# Exported from Unity");
        sw.WriteLine("o " + transform.name);

        foreach (Vector3 v in mesh.vertices)
        {
            Vector3 worldPos = transform.TransformPoint(v);
            sw.WriteLine(string.Format("v {0} {1} {2}", worldPos.x, worldPos.y, worldPos.z));
        }

        foreach (Vector3 n in mesh.normals)
        {
            Vector3 worldNormal = transform.TransformDirection(n);
            sw.WriteLine(string.Format("vn {0} {1} {2}", worldNormal.x, worldNormal.y, worldNormal.z));
        }

        foreach (Vector2 uv in mesh.uv)
        {
            sw.WriteLine(string.Format("vt {0} {1}", uv.x, uv.y));
        }

        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            int idx0 = mesh.triangles[i] + 1;
            int idx1 = mesh.triangles[i + 1] + 1;
            int idx2 = mesh.triangles[i + 2] + 1;
            sw.WriteLine(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}", idx0, idx1, idx2));
        }

        return sw.ToString();
    }
}
