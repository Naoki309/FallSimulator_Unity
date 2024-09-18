using UnityEngine;
using Renci.SshNet;
using System;

public class RunPythonTest : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Script started.");  // スクリプトが開始されたか確認
        
        string host = "172.27.83.240"; // UbuntuマシンのIPアドレス
        string username = "naoki";      // Ubuntuのユーザー名
        string password = "401402";      // Ubuntuのパスワード
        string pythonScriptPath = "/home/naoki/test/test_print.py"; // Ubuntu上のPythonスクリプトのパス
        try
        {
            using (var client = new SshClient(host, username, password))
            {
                client.Connect();
                using (var cmd = client.CreateCommand($"python3 {pythonScriptPath}"))
                {
                    var result = cmd.Execute();
                    Debug.Log($"Python script output: {result}");
                }
                client.Disconnect();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"SSH connection or script execution failed: {e.Message}");
        }
    }
}
