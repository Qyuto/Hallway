﻿using System.Collections;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] private Vector3 standalonePosition;
    [SerializeField] private Vector3 crounchPosition;
    [SerializeField] private PlayerMove playerMove;

    private void Awake()
    {
        playerMove.OnCrouching.AddListener(OnCrouching);
        transform.localPosition = standalonePosition;
    }

    private void OnCrouching(bool status)
    {
        Vector3 nextPosition;
        nextPosition = status ? crounchPosition : standalonePosition;

        if (_setCameraCoroutine == null)
            _setCameraCoroutine = StartCoroutine(SetCameraPosition(nextPosition, 0.05f));
        else
        {
            StopCoroutine(_setCameraCoroutine);
            _setCameraCoroutine = StartCoroutine(SetCameraPosition(nextPosition, 0.05f));
        }
    }

    private Coroutine _setCameraCoroutine;

    IEnumerator SetCameraPosition(Vector3 nextLocalPosition, float step = 1f)
    {
        while (!Mathf.Approximately((nextLocalPosition - transform.localPosition).magnitude, 0))
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, nextLocalPosition, step);
            yield return null;
        }

        _setCameraCoroutine = null;
    }
}