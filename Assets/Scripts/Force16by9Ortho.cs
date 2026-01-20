using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Force16by9Ortho : MonoBehaviour
{
    public float targetAspect = 16f / 9f;
    public float orthoSize = 5f;

    void Update()
    {
        Camera cam = GetComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = orthoSize;

        float windowAspect = (float)Screen.width / Screen.height;
        float scale = windowAspect / targetAspect;

        if (scale < 1f)
        {
            cam.rect = new Rect(0, (1 - scale) / 2, 1, scale);
        }
        else
        {
            float w = 1f / scale;
            cam.rect = new Rect((1 - w) / 2, 0, w, 1);
        }
    }
}
