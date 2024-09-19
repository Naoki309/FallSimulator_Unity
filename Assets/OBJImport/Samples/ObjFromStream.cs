using Dummiesman;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ObjFromStream : MonoBehaviour
{
    void Start()
    {
        // 非同期処理を実行
        StartCoroutine(LoadObjFromURL("https://people.sc.fsu.edu/~jburkardt/data/obj/lamp.obj"));
    }

    IEnumerator LoadObjFromURL(string url)
    {
        // UnityWebRequestでファイルをダウンロード
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error downloading OBJ file: " + www.error);
        }
        else
        {
            // ダウンロードしたOBJデータをメモリストリームに変換してロード
            var textStream = new MemoryStream(Encoding.UTF8.GetBytes(www.downloadHandler.text));
            var loadedObj = new OBJLoader().Load(textStream);
        }
    }
}
