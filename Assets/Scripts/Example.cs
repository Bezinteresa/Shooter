using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class Example : MonoBehaviour
{

    [SerializeField] private string URL;

    public void StartRun(string url, Action<string> callback, Action<string> error = null) => StartCoroutine(Run(url,callback,error));

    private IEnumerator Run(string url,Action<string> callback, Action<string> error = null) {

        //�������� ���������� ��������� URL
      UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {
            
            error?.Invoke(www.error);

        } else {

         callback?.Invoke(www.downloadHandler.text);

        }


        //������� ��������� URL
        www.Dispose();
    }

    // ��� ��� Dispose ���� ������������ using

   //using (UnityWebRequest www = UnityWebRequest.Get(url)) {

   // yield return www.SendWebRequest();

   // if (www.result != UnityWebRequest.Result.Success) error?.Invoke(www.error);
   //  else callback?.Invoke(www.downloadHandler.text);

   // }



}
