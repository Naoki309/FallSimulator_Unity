using Dummiesman;
using System.IO;
using UnityEngine;

public class ObjLoader : MonoBehaviour
{
    private string objFilePath = @"C:\SharedFolder\bbox.obj"; // bbox.obj のパス
    private GameObject loadedObject;

    void Start()
    {
        // メモリを解放してからロード
        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        LoadObjFile();
    }

    void LoadObjFile()
    {
        if (File.Exists(objFilePath))
        {
            if (loadedObject != null)
            {
                Destroy(loadedObject);
            }

            try
            {
                loadedObject = new OBJLoader().Load(objFilePath);
                Debug.Log("OBJ file loaded successfully from: " + objFilePath);

                loadedObject.transform.position = new Vector3(0, 0, 0);
                loadedObject.transform.localScale = new Vector3(1, 1, 1);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error loading OBJ file: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("File not found at path: " + objFilePath);
        }
    }
}
