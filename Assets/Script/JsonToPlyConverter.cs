using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Text;

public class JsonToPlyConverter : MonoBehaviour
{
    private string jsonFilePath = @"C:\SharedFolder\000000_pred_confident_nms_bbox.json";  // 監視対象のJSONファイル
    private string plyFilePath = @"C:\Users\nozak\home_ver3_funiture_Englishver\Assets\funiture\000000_pred_confident_nms_bbox.ply";  // 出力先のPLYファイル

    private FileSystemWatcher watcher;
    private bool isProcessing = false;  // 処理中フラグ

    void Start()
    {
        watcher = new FileSystemWatcher();
        watcher.Path = Path.GetDirectoryName(jsonFilePath);
        watcher.Filter = Path.GetFileName(jsonFilePath);
        watcher.NotifyFilter = NotifyFilters.LastWrite;
        watcher.Changed += OnChanged;
        watcher.EnableRaisingEvents = true;

        Debug.Log("FileSystemWatcherが起動しました。");
        // 強制的にDelayedConvertJsonToPlyを実行してテスト
        StartCoroutine(DelayedConvertJsonToPly());


    }

    // ファイル変更時のイベント
    private void OnChanged(object source, FileSystemEventArgs e)
    {
        if (!isProcessing)
        {
            Debug.Log($"ファイルが変更されました: {e.FullPath}");

            // ここでコルーチンが呼ばれているか確認
            Debug.Log("StartCoroutineを呼び出します。");
            StartCoroutine(DelayedConvertJsonToPly());
            Debug.Log("StartCoroutineが呼び出されました。");
        }
    }

    // 少し遅延させて変換を開始
    IEnumerator DelayedConvertJsonToPly()
    {
        Debug.Log("DelayedConvertJsonToPlyが開始されました。");
        isProcessing = true;

        // 1秒待機（ファイル書き込み完了を待つ）
        Debug.Log("1秒待機中...");
        yield return new WaitForSeconds(1);

        // ファイルが使用中でないか確認して変換開始
        if (IsFileReady(jsonFilePath))
        {
            Debug.Log("ファイルが使用可能です。変換を開始します。");
            yield return ConvertJsonToPly();
        }
        else
        {
            Debug.LogError("ファイルが使用中のため、変換に失敗しました。");
        }

        isProcessing = false;
    }

    // ファイルが使用中でないかを確認する
    private bool IsFileReady(string filePath)
    {
        try
        {
            Debug.Log("ファイルの状態を確認しています...");
            using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                stream.Close();
            }
        }
        catch (IOException ex)
        {
            // ファイルが使用中であることを示す
            Debug.LogError($"ファイルが使用中です。詳細: {ex.Message}");
            return false;
        }

        // ファイルが使用されていない
        return true;
    }

   IEnumerator ConvertJsonToPly()
{
    if (File.Exists(jsonFilePath))
    {
        try
        {
            Debug.Log("JSONファイルを読み込んでいます...");
            // JSONファイルを読み込み
            string jsonContent = File.ReadAllText(jsonFilePath);
            var jsonData = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonContent);

            // 'vertices' を取得し、適切にキャスト
            if (!jsonData.ContainsKey("vertices") || jsonData["vertices"] == null)
            {
                Debug.LogError("'vertices'フィールドがJSONデータに存在しないか、nullです。");
                yield break;  // 処理を中断
            }

            var vertices = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonData["vertices"].ToString());
            if (vertices == null || vertices.Count == 0)
            {
                Debug.LogError("'vertices'リストがnull、または空です。");
                yield break;
            }

            Debug.Log($"verticesの数: {vertices.Count}");

            // 'faces' を安全に取得し、キャスト
            if (!jsonData.ContainsKey("faces"))
            {
                Debug.LogError("'faces'フィールドがJSONデータに存在しません。空のリストを使用します。");
                jsonData["faces"] = new List<List<int>>();  // 空のリストを作成して続行
            }

            var faces = JsonConvert.DeserializeObject<List<List<int>>>(jsonData["faces"].ToString());
            if (faces == null)
            {
                Debug.LogError("'faces'リストがnullです。空のリストとして扱います。");
                faces = new List<List<int>>();  // 空のリストを作成
            }

            Debug.Log($"facesの数: {faces.Count}");

            // PLYファイルの作成
            using (FileStream fs = new FileStream(plyFilePath, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                string header = "ply\n"
                              + "format binary_little_endian 1.0\n"
                              + $"element vertex {vertices.Count}\n"
                              + "property float x\n"
                              + "property float y\n"
                              + "property float z\n"
                              + "property uchar red\n"
                              + "property uchar green\n"
                              + "property uchar blue\n"
                              + $"element face {faces.Count}\n"
                              + "property list uchar int vertex_indices\n"
                              + "end_header\n";
                writer.Write(Encoding.ASCII.GetBytes(header));

                Debug.Log("ヘッダーを書き込みました。");

                // 頂点データを書き込み
                foreach (var vertex in vertices)
                {
                    if (vertex == null)
                    {
                        Debug.LogError("頂点データがnullです。スキップします。");
                        continue;
                    }

                    float x = Convert.ToSingle(vertex["x"]);
                    float y = Convert.ToSingle(vertex["y"]);
                    float z = Convert.ToSingle(vertex["z"]);

                    byte r = 255, g = 255, b = 255;
                    if (vertex.ContainsKey("red")) r = Convert.ToByte(vertex["red"]);
                    if (vertex.ContainsKey("green")) g = Convert.ToByte(vertex["green"]);
                    if (vertex.ContainsKey("blue")) b = Convert.ToByte(vertex["blue"]);

                    writer.Write(x);
                    writer.Write(y);
                    writer.Write(z);
                    writer.Write(r);
                    writer.Write(g);
                    writer.Write(b);
                }

                Debug.Log("頂点データの書き込みが完了しました。");

                // 面データを書き込み
                foreach (var face in faces)
                {
                    byte vertexCount = (byte)face.Count;
                    writer.Write(vertexCount);
                    foreach (var index in face)
                    {
                        writer.Write(index);
                    }
                }

                Debug.Log($"PLYファイルに変換して保存しました: {plyFilePath}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"エラーが発生しました: {e.Message}\n{e.StackTrace}");
        }

        yield return null;
    }
    else
    {
        Debug.LogError("指定されたJSONファイルが見つかりません: " + jsonFilePath);
    }
}





    void OnDestroy()
    {
        if (watcher != null)
        {
            watcher.EnableRaisingEvents = false;
            watcher.Dispose();
        }
    }
}
