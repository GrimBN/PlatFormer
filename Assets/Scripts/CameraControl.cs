using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    TilePlacer tilePlacer;
    Touch touch1, touch2;
    float maxX, maxY, minX, minY, distance, maxSize, cameraSpeedZoomRatio, cameraInitialSize;
    [SerializeField] float cameraSpeed, zoomSpeed = 0.2f, minCameraSpeedRatio = 0.2f, minSize = 1;

    CinemachineVirtualCamera vCam;
    [SerializeField] CinemachineVirtualCamera playingCam;
    CinemachineConfiner confiner;
    //[SerializeField] PolygonCollider2D cameraConfiner;
    //[SerializeField] Text text;

    void Start()
    {
        tilePlacer = FindObjectOfType<TilePlacer>();
        vCam = GetComponent<CinemachineVirtualCamera>();        
        confiner = GetComponent<CinemachineConfiner>();
        //text.text = zoomSpeed.ToString();                
        maxSize = Mathf.Min(confiner.m_BoundingShape2D.bounds.extents.y , confiner.m_BoundingShape2D.bounds.extents.x * (1 / Camera.main.aspect)) * 0.99f;        
        if (vCam != null && confiner != null)
        {
            cameraInitialSize = vCam.m_Lens.OrthographicSize;
            CalcCameraLimits();
        }
    }

    private void CalcCameraLimits()
    {
        maxX = confiner.m_BoundingShape2D.bounds.max.x - vCam.m_Lens.OrthographicSize * Camera.main.aspect;
        minX = confiner.m_BoundingShape2D.bounds.min.x + vCam.m_Lens.OrthographicSize * Camera.main.aspect;
        maxY = confiner.m_BoundingShape2D.bounds.max.y - vCam.m_Lens.OrthographicSize;
        minY = confiner.m_BoundingShape2D.bounds.min.y + vCam.m_Lens.OrthographicSize;
        cameraSpeedZoomRatio = Mathf.Clamp(vCam.m_Lens.OrthographicSize / cameraInitialSize, minCameraSpeedRatio, 1f);
        if(vCam.transform.position.x > maxX || vCam.transform.position.x < minX || vCam.transform.position.y > maxY || vCam.transform.position.y < minY)
        {
            Vector3 correctedPos = new Vector3(Mathf.Clamp(vCam.transform.position.x, minX + 0.01f, maxX - 0.01f), Mathf.Clamp(vCam.transform.position.y, minY + 0.01f, maxY - 0.01f), vCam.transform.position.z);
            vCam.transform.position = correctedPos;
        }
    }

    void Update()
    {
        CameraChange();
    }

    private void CameraChange()
    {
        if (Input.touchCount == 1 && !tilePlacer.GetDrawing() && !tilePlacer.GetErasing())      // Move Camera
        {
            touch1 = Input.GetTouch(0);
            if (touch1.phase == TouchPhase.Moved)
            {
                Vector3 newPos = new Vector3(transform.position.x - touch1.deltaPosition.x * cameraSpeed * cameraSpeedZoomRatio, transform.position.y - touch1.deltaPosition.y * cameraSpeed * cameraSpeedZoomRatio, transform.position.z);
                if (vCam != null && confiner != null)
                {
                    if (newPos.x < maxX && newPos.x > minX && newPos.y < maxY && newPos.y > minY)
                    {
                        transform.position = newPos;
                    }
                }
            }
        }
        if (Input.touchCount >= 2 && !tilePlacer.GetDrawing() && !tilePlacer.GetErasing())      // Zoom in/out
        {
            touch1 = Input.GetTouch(0);
            touch2 = Input.GetTouch(1);
            if (touch2.phase == TouchPhase.Began)
            {
                distance = Vector2.Distance(touch1.position, touch2.position);
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                float newDistance = Vector2.Distance(touch1.position, touch2.position);
                if (newDistance > distance && vCam.m_Lens.OrthographicSize > minSize)
                {
                    vCam.m_Lens.OrthographicSize += zoomSpeed * (distance - newDistance);
                }
                else if (newDistance < distance && vCam.m_Lens.OrthographicSize < maxSize)
                {
                    vCam.m_Lens.OrthographicSize -= zoomSpeed * (newDistance - distance);
                }

                playingCam.m_Lens.OrthographicSize = vCam.m_Lens.OrthographicSize;
                CalcCameraLimits();
                distance = newDistance;
            }
        }

        //placeholder pc controls
        if (Input.GetButton("Vertical"))
        {
            Vector3 newPos = new Vector3(transform.position.x, cameraSpeed * Input.GetAxisRaw("Vertical") * 10 + transform.position.y, transform.position.z);
            if (vCam != null && confiner != null)
            {
                if (newPos.x < maxX && newPos.x > minX && newPos.y < maxY && newPos.y > minY)
                {
                    transform.position = newPos;
                }
            }
        }
        if (Input.GetButton("Horizontal"))
        {
            Vector3 newPos = new Vector3(cameraSpeed * Input.GetAxisRaw("Horizontal") * 10 + transform.position.x, transform.position.y, transform.position.z);
            if (vCam != null && confiner != null)
            {
                if (newPos.x < maxX && newPos.x > minX && newPos.y < maxY && newPos.y > minY)
                {
                    transform.position = newPos;
                }
            }
        }
        if(Input.mouseScrollDelta.y > 0 && vCam.m_Lens.OrthographicSize < maxSize)
        {
            vCam.m_Lens.OrthographicSize += 0.05f;
            playingCam.m_Lens.OrthographicSize = vCam.m_Lens.OrthographicSize;
            CalcCameraLimits();
        }
        if (Input.mouseScrollDelta.y < 0 && vCam.m_Lens.OrthographicSize > minSize)
        {
            vCam.m_Lens.OrthographicSize -= 0.05f;
            playingCam.m_Lens.OrthographicSize = vCam.m_Lens.OrthographicSize;
            CalcCameraLimits();
        }
    }

    public void SetZoomSpeed (float value)
    {
        zoomSpeed = value;
        //text.text = zoomSpeed.ToString();
    }
}
