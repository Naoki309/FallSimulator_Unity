    using UnityEngine;
    using Renci.SshNet;
    using System.IO;

    public class BboxManager : MonoBehaviour
    {
        // Ubuntuサーバーの情報
        private string host = "172.27.83.240";  // UbuntuマシンのIPアドレス
        private string username = "naoki";      // Ubuntuのユーザー名
        private string password = "401402";     // Ubuntuのパスワード
        private string pythonScriptPath = "/home/naoki/votenet/demo.py";  // Ubuntu上のPythonスクリプトパス
        private string pythonVirtualEnvironmentPath = "/home/naoki/votenet_env/bin/python";  // Ubuntu上のPython仮想環境パス
        void Start()
        {

            // VoteNetを実行してBBox JSONファイルを生成
            ExecuteVoteNetScript();
        } 

        // VoteNetのPythonスクリプトをリモートで実行するメソッド
        private void ExecuteVoteNetScript()
        {
            try
            {
                using (var client = new SshClient(host, username, password))
                {
                    client.Connect();
                    Debug.Log("Connected to Ubuntu server.");

                   using (var cmd = client.CreateCommand($"source /home/naoki/votenet_env/bin/activate && python3 {pythonScriptPath}"))
                    {
                        var result = cmd.Execute();
                        var error = cmd.Error;

                        if (!string.IsNullOrEmpty(error))
                        {
                            Debug.LogError("Error in VoteNet Python script: " + error);
                        }
                        else
                        {
                            Debug.Log("VoteNet Python script executed successfully: " + result);
                        }
                    }
                    client.Disconnect();
                    Debug.Log("Disconnected from Ubuntu server.");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to run VoteNet Python script via SSH: {e.Message}");
            }
        }
    }
