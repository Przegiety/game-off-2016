using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Utils {
    /// <summary>
    /// Load text from file - main thread
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string LoadTextFromFile(string path) {
        WWW www = new WWW("file:///" + Application.streamingAssetsPath + "/" + path);
        while(!www.isDone) {
            // reading
        }
        if(!string.IsNullOrEmpty(www.error))
            Debug.LogError(www.error);
        return www.text;
    }
}