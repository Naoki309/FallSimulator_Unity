using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json;

public class BinaryPlyToJsonConverter : MonoBehaviour
{
    // plyファイルの絶対パスを指定（指定された絶対パス）
    private string plyFilePath = @"C:\Users\nozak\home_ver3_funiture_Englishver\Assets\funiture\402.ply";
    // 共有フォルダのパス
    private string jsonOutputPath = @"C:\SharedFolder\402.json";

    // 頂点数とプロパティを管理するための変数
    private int vertexCount = 0;
    private List<string> vertexProperties = new List<string>();

    void Start()
    {
        StartCoroutine(ConvertBinaryPlyToJson());
    }

    IEnumerator ConvertBinaryPlyToJson()
    {
        if (File.Exists(plyFilePath))
        {
            using (BinaryReader reader = new BinaryReader(File.Open(plyFilePath, FileMode.Open)))
            {
                // 頂点データを保持するリスト
                List<Dictionary<string, object>> verticesData = new List<Dictionary<string, object>>();
                string currentLine = "";

                // ヘッダー解析
                while (!currentLine.StartsWith("end_header"))
                {
                    currentLine = ReadAsciiLine(reader);
                    
                    if (currentLine.StartsWith("element vertex"))
                    {
                        vertexCount = int.Parse(currentLine.Split(' ')[2]);
                        Debug.Log("頂点の数: " + vertexCount);
                    }
                    
                    if (currentLine.StartsWith("property"))
                    {
                        string propertyType = currentLine.Split(' ')[1];
                        vertexProperties.Add(propertyType);
                    }
                }

                // 頂点データを読み込む
                for (int i = 0; i < vertexCount; i++)
                {
                    var vertex = new Dictionary<string, object>();

                    vertex["x"] = reader.ReadSingle();
                    vertex["y"] = reader.ReadSingle();
                    vertex["z"] = reader.ReadSingle();
                    vertex["red"] = reader.ReadByte();
                    vertex["green"] = reader.ReadByte();
                    vertex["blue"] = reader.ReadByte();

                    verticesData.Add(vertex);
                }

                // 頂点データを"vertices"キーにラップ
                var jsonData = new Dictionary<string, object>
                {
                    { "vertices", verticesData }
                };

                // JSON形式で保存
                string jsonContent = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
                File.WriteAllText(jsonOutputPath, jsonContent);
                Debug.Log("PLYファイルをJSONに変換して共有フォルダに保存しました。");
            }

            yield return null;
        }
        else
        {
            Debug.LogError("指定されたPLYファイルが見つかりません: " + plyFilePath);
        }
    }

    // バイナリファイルのASCII部分（ヘッダー）を読み取る関数
    private string ReadAsciiLine(BinaryReader reader)
    {
        List<byte> bytes = new List<byte>();
        byte b;
        while ((b = reader.ReadByte()) != 10)  // LF (Line Feed)で行を区切る
        {
            bytes.Add(b);
        }
        return Encoding.ASCII.GetString(bytes.ToArray());
    }
}
