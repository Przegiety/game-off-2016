using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Utils {

    private static MonoBehaviour _coroutineHolder;
    private static MonoBehaviour CoroutineHolder
    {
        get {
            if(_coroutineHolder == null)
                _coroutineHolder = new GameObject("Utils").AddComponent<UtilsHolder>();
            return _coroutineHolder;
        }
    }
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

    public static void LoadTextFromFileNonBlocking(string path, Action<string> onEnd, Action<float> progressAction = null) {
        CoroutineHolder.StartCoroutine(LoadTextFromFileCoroutine(path, onEnd, progressAction));
    }

    private static IEnumerator LoadTextFromFileCoroutine(string path, Action<string> onEnd, Action<float> progressAction) {
        WWW www = new WWW("file:///" + Application.streamingAssetsPath + "/" + path);
        while(!www.isDone) {
            if(progressAction != null)
                progressAction(www.progress);
            yield return null;
        }
        if(!string.IsNullOrEmpty(www.error))
            Debug.LogError(www.error);
        if(onEnd != null)
            onEnd(www.text);
    }
    private class UtilsHolder : MonoBehaviour { }
}