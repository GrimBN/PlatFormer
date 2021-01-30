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
    [SerializeField] float cameraSpeed, zoomSpeed = 0.2f, minCameraSpeedRatio = 0.2f, minSize = 1, mouseZoomSpeed = 0.5f, keyboardZoomSpeed = 0.1f, pcCameraSpeed = 0.2f;

    CinemachineVirtualCamera vCam;
    [SerializeField] CinemachineVirtualCamera playingCam;
    CinemachineConfiner confiner;
    //[SerializeField] PolygonCollider2D cameraConfiner;
    //[SerializeField] Text text;

    void Start()
    {
        Input.simulateMouseWithTouches = false;
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
                float xPosDelta = touch1.deltaPosition.x * cameraSpeed * cameraSpeedZoomRatio;
                float yPosDelta = touch1.deltaPosition.y * cameraSpeed * cameraSpeedZoomRatio;
                Vector3 newPos = new Vector3(Mathf.Clamp(transform.position.x - xPosDelta ,minX,maxX), Mathf.Clamp(transform.position.y - yPosDelta,minY,maxY), transform.position.z);
                if (vCam != null && confiner != null)
                {
                    transform.position = newPos;
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

        if(Input.GetMouseButton(1))
        {
            float xPosDelta = Input.GetAxis("Mouse X") * vCam.m_Lens.OrthographicSize / maxSize;
            float yPosDelta = Input.GetAxis("Mouse Y") * vCam.m_Lens.OrthographicSize / maxSize;
            Vector3 newPos = new Vector3(Mathf.Clamp(transform.position.x - xPosDelta,minX,maxX), Mathf.Clamp(transform.position.y - yPosDelta,minY,maxY), transform.position.z);
            if (vCam != null && confiner != null)
            {
                transform.position = new Vector3(newPos.x, newPos.y, -10);
            }
        }        

        if (Input.GetButton("Vertical"))
        {
            Vector3 newPos = new Vector3(transform.position.x, pcCameraSpeed * Input.GetAxisRaw("Vertical") + transform.position.y, transform.position.z);
            if (vCam != null && confiner != null)
            {
                if (newPos.x <= maxX && newPos.x >= minX && newPos.y <= maxY && newPos.y >= minY)
                {
                    transform.position = newPos;
                }
            }
        }

        if (Input.GetButton("Horizontal"))
        {
            Vector3 newPos = new Vector3(pcCameraSpeed * Input.GetAxisRaw("Horizontal") + transform.position.x, transform.position.y, transform.position.z);
            if (vCam != null && confiner != null)
            {
                if (newPos.x <= maxX && newPos.x >= minX && newPos.y <= maxY && newPos.y >= minY)
                {
                    transform.position = newPos;
                }
            }
        }
    
        if(Mathf.Abs(Input.mouseScrollDelta.y) > 0)// && vCam.m_Lens.OrthographicSize < maxSize && vCam.m_Lens.OrthographicSize > minSize)
        {
            float newCameraSize = vCam.m_Lens.OrthographicSize - (Input.mouseScrollDelta.y * mouseZoomSpeed);
            if (newCameraSize < maxSize && newCameraSize > minSize)
            {
                vCam.m_Lens.OrthographicSize = newCameraSize;
                playingCam.m_Lens.OrthographicSize = vCam.m_Lens.OrthographicSize;
                CalcCameraLimits();
            }
        }
        
        if(Input.GetAxis("ZoomIn") > 0)
        {
            float newCameraSize = vCam.m_Lens.OrthographicSize - keyboardZoomSpeed;
            if (newCameraSize < maxSize && newCameraSize > minSize)
            {
                vCam.m_Lens.OrthographicSize = newCameraSize;
                playingCam.m_Lens.OrthographicSize = vCam.m_Lens.OrthographicSize;
                CalcCameraLimits();
            }
        }
        else if(Input.GetAxis("ZoomOut") > 0)
        {
            float newCameraSize = vCam.m_Lens.OrthographicSize + keyboardZoomSpeed;
            if (newCameraSize < maxSize && newCameraSize > minSize)
            {
                vCam.m_Lens.OrthographicSize = newCameraSize;
                playingCam.m_Lens.OrthographicSize = vCam.m_Lens.OrthographicSize;
                CalcCameraLimits();
            }
        }
    }

    public void SetZoomSpeed (float value)
    {
        zoomSpeed = value;
        //text.text = zoomSpeed.ToString();
    }
}
