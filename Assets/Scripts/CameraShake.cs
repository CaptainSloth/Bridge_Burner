using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public Camera mainCam;

    float shakeAmount = 0;

    void Awake()
    {
        if (mainCam == null)
            mainCam = Camera.main;
    }

    public void Shake(float amt, float length)
    {
        shakeAmount = amt;
        InvokeRepeating("StartShake", 0, 0.01f);
        Invoke("EndShake", length);
    }

    void StartShake()
    {
        if (shakeAmount > 0)
        {
            Vector3 camPos = mainCam.transform.position;


            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offestY = Random.value * shakeAmount * 2 - shakeAmount;

            camPos.x += offsetX;
            camPos.y += offestY;

            mainCam.transform.position = camPos;
        }
    }

    void EndShake()
    {
        CancelInvoke("StartShake");
        mainCam.transform.localPosition = Vector3.zero;
    }
}
