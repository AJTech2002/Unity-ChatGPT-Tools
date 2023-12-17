using UnityEngine;
using UnityEngine.Networking;

public class API
{
    public const string Url = "https://api.openai.com/v1/chat/completions";

    public static void SendRequest(Request request, System.Action<string> callback)
    {
        string json = JsonUtility.ToJson(request);
        
        var post = new UnityWebRequest(Url, "POST");
        post.SetRequestHeader("Authorization", "Bearer " + Secrets.secretKey);
        post.SetRequestHeader("Content-Type", "application/json");
        
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        post.uploadHandler = new UploadHandlerRaw(jsonToSend);
        post.downloadHandler = new DownloadHandlerBuffer();

        var req = post.SendWebRequest();

        req.completed += (r) =>
        {
            var outputJson = post.downloadHandler.text;
            var parsedData = JsonUtility.FromJson<Response>(outputJson);

            callback.Invoke(parsedData.choices[0].message.content);
        };
    }
}



