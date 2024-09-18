using System.IO;
using UnityEngine;

public class ObjExporter : MonoBehaviour
{
    // 共有フォルダのパス（Windows側）
    public string sharedFolderPath = @"C:\SharedFolder\";
    
    // OBJファイル名
    public string objFileName = "test_room.obj";

    void Start()
    {
        // シーン開始時にOBJファイルを生成・保存
        GenerateAndExportOBJ();
    }

    /// <summary>
    /// OBJデータを共有フォルダに保存します。
    /// </summary>
    /// <param name="objData">保存するOBJデータ</param>
    public void ExportOBJ(string objData)
    {
        // 共有フォルダが存在しない場合は作成
        if (!Directory.Exists(sharedFolderPath))
        {
            Directory.CreateDirectory(sharedFolderPath);
            Debug.Log("共有フォルダを作成しました: " + sharedFolderPath);
        }

        // 一時ファイル名（競合防止）
        string tempFileName = objFileName + ".tmp";
        string tempFilePath = Path.Combine(sharedFolderPath, tempFileName);
        string finalFilePath = Path.Combine(sharedFolderPath, objFileName);

        try
        {
            // 一時ファイルにOBJデータを書き込み
            File.WriteAllText(tempFilePath, objData);
            Debug.Log("一時ファイルにOBJデータを書き込みました: " + tempFilePath);
            
            // 既に同名のファイルが存在する場合は削除
            if (File.Exists(finalFilePath))
            {
                File.Delete(finalFilePath);
                Debug.Log("既存のOBJファイルを削除しました: " + finalFilePath);
            }

            // 一時ファイルを最終ファイル名にリネーム
            File.Move(tempFilePath, finalFilePath);
            Debug.Log("OBJファイルを共有フォルダに保存しました: " + finalFilePath);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("OBJファイルの保存中にエラーが発生しました: " + ex.Message);
        }
    }

    /// <summary>
    /// 実際のエクスポート処理（テスト用）
    /// </summary>
    public void GenerateAndExportOBJ()
    {
        // テスト用に簡単なOBJデータを作成
        string objData = @"
v 0.000000 0.000000 0.000000
v 1.000000 0.000000 0.000000
v 1.000000 1.000000 0.000000
v 0.000000 1.000000 0.000000
f 1 2 3 4
";
        ExportOBJ(objData);
    }
}
