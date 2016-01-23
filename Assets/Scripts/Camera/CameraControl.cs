﻿using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f;                 
    public float m_ScreenEdgeBuffer = 4f;           
    public float m_MinSize = 6.5f;
    public Transform m_Target; 
                     
    private float m_ZoomSpeed;                      
    private Vector3 m_MoveVelocity;
    private Vector3 m_DesiredPosition;                       


    private void Awake()
    {
        m_DesiredPosition = Vector3.zero;
    }


    private void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
        var hit = new RaycastHit();
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);

        if (hit.transform != null)
            m_DesiredPosition = Vector3.Lerp(m_Target.position, hit.point, 0.5f);
        else
            m_DesiredPosition = m_Target.position;

        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }
}