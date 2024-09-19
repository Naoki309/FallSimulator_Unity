using UnityEngine;
using System.Collections;
using System.IO;

public class PointCloudManager : MonoBehaviour
{
    // File path for the PLY file
    public string dataPath;
    public Material matVertex;

    // PointCloud
    private GameObject pointCloud;
    public float scale = 1;
    public bool invertYZ = false;

    private Vector3[] points;
    private Color[] colors;
    private int numPoints;

    void Start()
    {
        Debug.Log("Start method called.");
        string fullPath = Application.dataPath + "/funiture/402.ply"; // Adjust to your PLY file path
        Debug.Log("Loading PLY file from: " + fullPath);

        if (File.Exists(fullPath))
        {
            Debug.Log("PLY file found, loading...");
            StartCoroutine(LoadPLYFile(fullPath));
        }
        else
        {
            Debug.LogError("PLY file not found at: " + fullPath);
        }
    }

    IEnumerator LoadPLYFile(string plyFilePath)
    {
        Debug.Log("Simulating loading of PLY file...");
        byte[] fileBytes = File.ReadAllBytes(plyFilePath);
        int byteIndex = 0;

        // Read header
        string header = ReadHeader(fileBytes, ref byteIndex);
        Debug.Log("PLY Header: " + header);

        // Parse header to get number of points
        ParsePLYHeader(header, out numPoints);
        Debug.Log("Number of points: " + numPoints);

        // Initialize arrays for points and colors
        points = new Vector3[numPoints];
        colors = new Color[numPoints];

        // Read point data
        for (int i = 0; i < numPoints; i++)
        {
            points[i] = new Vector3(
                ReadFloat(fileBytes, ref byteIndex),
                ReadFloat(fileBytes, ref byteIndex),
                ReadFloat(fileBytes, ref byteIndex)
            );

            if (byteIndex + 3 <= fileBytes.Length)
            {
                byte r = fileBytes[byteIndex++];
                byte g = fileBytes[byteIndex++];
                byte b = fileBytes[byteIndex++];
                colors[i] = new Color(r / 255f, g / 255f, b / 255f);

				
            }
            else
            {
                colors[i] = Color.white;  // Default color
            }
        }

        // Create point cloud mesh
        CreatePointCloudMesh();

        yield return null;
    }

    void CreatePointCloudMesh()
    {
        pointCloud = new GameObject("PointCloud");
        Mesh mesh = new Mesh();

        int[] indices = new int[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            indices[i] = i;
        }

        mesh.vertices = points;
        mesh.colors = colors;
        mesh.SetIndices(indices, MeshTopology.Points, 0);

        MeshFilter meshFilter = pointCloud.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = pointCloud.AddComponent<MeshRenderer>();
        meshRenderer.material = matVertex;

        Debug.Log("Point cloud mesh created with " + numPoints + " points.");
    }

    // Helper methods to read PLY file
    string ReadHeader(byte[] fileBytes, ref int byteIndex)
    {
        System.Text.StringBuilder headerBuilder = new System.Text.StringBuilder();
        while (true)
        {
            string line = ReadLine(fileBytes, ref byteIndex);
            headerBuilder.AppendLine(line);
            if (line.StartsWith("end_header"))
                break;
        }
        return headerBuilder.ToString();
    }

    string ReadLine(byte[] fileBytes, ref int byteIndex)
    {
        System.Text.StringBuilder lineBuilder = new System.Text.StringBuilder();
        while (fileBytes[byteIndex] != '\n')
        {
            lineBuilder.Append((char)fileBytes[byteIndex]);
            byteIndex++;
        }
        byteIndex++; // Skip newline character
        return lineBuilder.ToString();
    }

    void ParsePLYHeader(string header, out int vertexCount)
    {
        vertexCount = 0;
        string[] lines = header.Split('\n');
        foreach (string line in lines)
        {
            if (line.StartsWith("element vertex"))
            {
                vertexCount = int.Parse(line.Split(' ')[2]);
                break;
            }
        }
    }

    float ReadFloat(byte[] fileBytes, ref int byteIndex)
    {
        float value = System.BitConverter.ToSingle(fileBytes, byteIndex);
        byteIndex += 4;
        return value;
    }
}
