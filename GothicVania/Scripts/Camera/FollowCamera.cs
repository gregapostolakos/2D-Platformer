using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FollowCamera : MonoBehaviour
{
    [Header("Camera Target")]
    public GameObject target;
    public Vector3 targetOffset;
    public float cameraSpeed = 5f;

    [Header("Level Limits")]
    public bool levelLimits = true;
    public float levelTop;
    public float levelBottom;
    public float levelLeft;
    public float levelRight;

    [Header("Camera Shake")]
    public float intensity = 0.1f;
    public float duration = 0.1f;
    
    private Camera camera;
    private Vector3 currentPosition;
    private GameObject lockTarget = null;

    private static FollowCamera _instance;

    void Awake()
    {
        _instance = this;
        camera = GetComponent<Camera>();
    }

    void Start()
    {
        currentPosition = transform.position;
    }

    void LateUpdate()
    {
        GameObject cameraTarget = target;

        if (lockTarget != null)
            cameraTarget = lockTarget;

        if (cameraTarget != null)
        {
            //Find target
            Vector3 targetPosition = cameraTarget.transform.position + targetOffset;
            targetPosition = LimitPos(targetPosition);

            //Check if need to move
            Vector3 diff = targetPosition - transform.position;
            if (diff.magnitude > 0.1f)
            {
                //Move camera
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentPosition, 1f / cameraSpeed, Mathf.Infinity, Time.deltaTime);
            }
        }
    }

    private Vector3 LimitPos(Vector3 position)
    {
        //Set level limits
        if (levelLimits)
        {
            float frustrumHeight = GetFrustrumHeight() / 2f;
            float frustrumWidth = GetFrustrumWidth() / 2f;
            position.x = Mathf.Max(levelLeft + frustrumWidth, position.x);
            position.x = Mathf.Min(levelRight - frustrumWidth, position.x);
            position.y = Mathf.Max(levelBottom + frustrumHeight, position.y);
            position.y = Mathf.Min(levelTop - frustrumHeight, position.y);
        }
        return position;
    }

    public float GetFrustrumHeight()
    {
        if (camera.orthographic)
            return 2f * camera.orthographicSize;
        else
            return 2.0f * Mathf.Abs(transform.position.z) * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
    }

    public float GetFrustrumWidth()
    {
        return GetFrustrumHeight() * camera.aspect;
    }

    public void LockCameraOn(GameObject ltarget)
    {
        lockTarget = ltarget;
    }

    public void UnlockCamera()
    {
        lockTarget = null;
    }

    public void MoveTo(Vector3 targ_pos)
    {
        targ_pos = LimitPos(targ_pos);
        transform.position = targ_pos + targetOffset;
    }

    public void CameraShake()
    {
        StartCoroutine(DoShake());
    }

    private IEnumerator DoShake()
    {
        float elapsedTime = 0;
        Vector3 originalPosition = transform.position;

        while (elapsedTime < duration)
        {
            // Generate a random shake offset within the specified intensity range
            Vector3 randomOffset = new Vector3(
                Random.Range(-intensity, intensity),
                Random.Range(-intensity, intensity),
                0
            );

            // Apply the random offset to the original position
            transform.position = originalPosition + randomOffset;
            
            elapsedTime += Time.deltaTime;
            yield return null; // Use null to wait for the next frame
        }

        // Reset the camera position to the original position
        transform.position = originalPosition;
    }

    public static FollowCamera Get()
    {
        return _instance;
    }

    public static Camera GetCamera()
    {
        if (_instance)
            return _instance.camera;
        return null;
    }
}
