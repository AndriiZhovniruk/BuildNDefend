using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraHandler : MonoBehaviour
{
    public static CameraHandler Instance { get; private set; }
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private float ortoghraphicSize;
    private float targetOrtoghraphicSize;
    private bool edgeScrolling;

    private void Start()
    {
        cinemachineVirtualCamera.m_Lens.OrthographicSize = 15;
        ortoghraphicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        targetOrtoghraphicSize = ortoghraphicSize;
    }
    private void Awake()
    {
        Instance = this;
        edgeScrolling = PlayerPrefs.GetInt("edgeScrolling", 1) == 1;
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
       
    }

    private void HandleZoom()
    {
        float zoomAmount = 2f;

        targetOrtoghraphicSize += Input.mouseScrollDelta.y * zoomAmount * -1;

        float minOrtoghraphicSize = 5f;
        float maxOrtoghraphicSize = 50f;

        targetOrtoghraphicSize = Mathf.Clamp(targetOrtoghraphicSize, minOrtoghraphicSize, maxOrtoghraphicSize);
        float zoomSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            zoomSpeed = 15f;
        }
        else
        {
            zoomSpeed = 5f;
        }
        ortoghraphicSize = Mathf.Lerp(ortoghraphicSize, targetOrtoghraphicSize, Time.deltaTime * zoomSpeed);
        cinemachineVirtualCamera.m_Lens.OrthographicSize = ortoghraphicSize;
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        float moveSpeed;
        if (edgeScrolling)
        {
            float edgeScrollingSize = 30;
            if (Input.mousePosition.x > Screen.width - edgeScrollingSize)
            {
                x = 1f;
            }
            if (Input.mousePosition.x < edgeScrollingSize)
            {
                x = -1f;
            }
            if (Input.mousePosition.y > Screen.height - edgeScrollingSize)
            {
                y = 1f;
            }
            if (Input.mousePosition.y < edgeScrollingSize)
            {
                y = -1f;
            }
        }
       
        Vector3 moveDir = new Vector3(x, y).normalized;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 60f;
        }
        else
        {
            moveSpeed = 15f;
        }
       
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
    public void SetEdgeScrolling(bool edgeScrolling)
    {
        this.edgeScrolling = edgeScrolling;
        PlayerPrefs.SetInt("edgeScrolling", edgeScrolling ? 1 : 0);
    }
    public bool GetEdgeScrolling()
    {
        return edgeScrolling;
    }
        
    
}
