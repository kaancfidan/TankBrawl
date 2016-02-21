using UnityEngine;

public class CameraControl : MonoBehaviour
{                     
    public Transform m_Target;
    public float m_LerpGain = 0.5f;
                     
    private Vector3 m_DesiredPosition;
                         


    private void Awake()
    {
        m_DesiredPosition = Vector3.zero;
    }


    private void Update()
    {
        Move();
    }

    private void Move()
    {
        var hit = new RaycastHit();
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);

        if (hit.transform != null)
            m_DesiredPosition = Vector3.Lerp(m_Target.position, hit.point, 0.4f);
        else
            m_DesiredPosition = m_Target.position;

        transform.position = Vector3.Lerp(transform.position, m_DesiredPosition, m_LerpGain * Time.deltaTime);
    }
}