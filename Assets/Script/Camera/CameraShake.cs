using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public void Shake(float duration, float intensity)
    {
        StartCoroutine(ShakeCoroutine(duration, intensity));
    }

    IEnumerator ShakeCoroutine(float duration, float intensity)
    {
        Vector3 lastMovement = Vector3.zero;
        Vector3 newMovement = Vector3.zero;
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            lastMovement = newMovement;
            newMovement = new Vector3(Random.Range(-intensity * Time.deltaTime, intensity * Time.deltaTime), Random.Range(-intensity * Time.deltaTime, intensity * Time.deltaTime));
            transform.position -= lastMovement;
            transform.position += newMovement;
            yield return new WaitForEndOfFrame();
        }
        transform.position -= lastMovement;
    }
}
