using GreyFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace GreyFramework
{
    public class Httper : MonoBehaviour
    {
        public delegate void DownloadResultError(string error);
        public delegate void DownloadResultSucceed(string content);
        public bool isDone = false;
        public static Httper Create()
        {
            Httper httper = new GameObject("Httper").AddComponent<Httper>();
            //httper.gameObject.AddComponent<DontDestroyOnLoad>();
            //Managers.GetHttperManager().AddHttper(httper);
            return httper;
        }

        public void Dispose()
        {
            UnloadFunction();
        }

        /// <summary>
        /// get«Î«Û
        /// </summary>
        /// <param name="serverURL"></param>
        /// <param name="responseCallback"></param>
        /// <param name="error"></param>
        /// <param name="timeout"></param>
        public void RequestURLGET(string serverURL, DownloadResultSucceed responseCallback, DownloadResultError error, int timeout = 10)
        {
            isDone = false;
            Debug.Log("http«Î«Ûget:" + serverURL);
            StartCoroutine(RequestWWW(serverURL, responseCallback, error, timeout));
        }

        private IEnumerator RequestWWW(string serverURL, DownloadResultSucceed responseCallback, DownloadResultError error, int timeout)
        {
            Debug.Log("http«Î«Û:" + serverURL);
            UnityWebRequest webRequest = UnityWebRequest.Get(serverURL);
            webRequest.timeout = timeout;
            yield return webRequest.SendWebRequest();
            if (webRequest.isHttpError || webRequest.isNetworkError)
            {
                if (error != null)
                {
                    error(webRequest.error);
                }
            }
            else
            {
                if (responseCallback != null)
                {
                    responseCallback(webRequest.downloadHandler.text);
                }
            }
            isDone = true;
        }

        public void UnloadFunction()
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }
}

