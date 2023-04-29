using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeWriter : MonoBehaviour
{
    [SerializeField]
    private float _speedInSec;
   
    public void Type(string text, Action<string> targetCallBack)
    {
        StartCoroutine(TypeText(text,targetCallBack));
    }

    private IEnumerator TypeText(string text, Action<string> targetCallback)
    {
        string typedText = "";

        foreach(char letter in text.ToCharArray())
        {
            typedText += letter;
            targetCallback(typedText);

            yield return new WaitForSeconds(_speedInSec);
        }

        yield return null;
    }
}
