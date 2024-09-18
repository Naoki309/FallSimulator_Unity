using UnityEngine;
using Renci.SshNet;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

[Serializable]
public class BoundingBox
{
    public int ID;
    public string className;
    public Center center;
    public Size size;
    public Rotation rotation;
}

[Serializable]
public class Center
{
    public float x, y, z;
}

[Serializable]
public class Size
{
    public float l, w, h;
}

[Serializable]
public class Rotation
{
    public float theta;
}

public class JsonTest : MonoBehaviour
{
    void Start()
    {
        string host = "172.27.83.240"; // UbuntuマシンのIPアドレス
        string username = "naoki";      // Ubuntuのユーザー名
        string password = "401402";      // Ubuntuのパスワード
        string pythonScriptPath = "/home/naoki/test/test_json.py";

        try
        {
            using (var client = new SshClient(host, username, password))
            {
                client.Connect();
                using (var cmd = client.CreateCommand($"python3 {pythonScriptPath}"))
                {
                    // Pythonスクリプトの実行と結果の取得
                    var result = cmd.Execute();
                    Debug.Log($"Python script output: {result}");

                    // JSONデータをパースしてバウンディングボックスの情報を取得
                    List<BoundingBox> boundingBoxes = JsonConvert.DeserializeObject<List<BoundingBox>>(result);

                    // 各バウンディングボックスのデータをログに表示
                    foreach (var box in boundingBoxes)
                    {
                        Debug.Log($"ID: {box.ID}, Class: {box.className}, Center: ({box.center.x}, {box.center.y}, {box.center.z}), " +
                                  $"Size: ({box.size.l}, {box.size.w}, {box.size.h}), Rotation: {box.rotation.theta}");
                    }
                }
                client.Disconnect();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"SSH connection or script execution failed: {e.ToString()}");
        }
    }
}
