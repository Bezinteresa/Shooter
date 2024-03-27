using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeadSHotText : MonoBehaviour
{
    
    [SerializeField] public Vector3 startPosition = new Vector3(1.5f, 1.5f, 1f);
    [SerializeField] public Vector3 endPosition = Vector3.one;
    [SerializeField] private float lerpTime = 1f;
    [SerializeField] private float currentTime = 0f;

    [SerializeField] private RectTransform _textTransform;
    [SerializeField] TextMeshProUGUI _textMesh;

    private bool _isCoroutineRunning = false;

    private void OnEnable() {
        Damage._OnHeadShot += SetTextHeadShot;
    }

    private void OnDisable() {
        Damage._OnHeadShot -= SetTextHeadShot;
    }

    private void SetTextHeadShot() {
        if (_isCoroutineRunning) {
            return; // Return if Coroutine is already running
        }
        currentTime = 0f;
        StartCoroutine(TextScale());
    }

    private IEnumerator TextScale() {
        _isCoroutineRunning=true;
        _textMesh.enabled = true;

        while (currentTime < lerpTime) {
            _textTransform.localScale = Vector3.Lerp(startPosition, endPosition, currentTime / lerpTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        _textTransform.localScale = endPosition;
        
        yield return new WaitForSecondsRealtime(1f);
        _textMesh.enabled = false;
        _isCoroutineRunning = false;
    }

}
