using UnityEngine;
using System.Collections.Generic;

public class StarConnector : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject linePrefab;  // ����Ԥ���壨�� LineRenderer��
    public GameObject dotPrefab;   // ��ѡ����̬���ɵ�

    private Transform firstDot = null;  // ��¼���
    private LineRenderer tempLine = null; // ��ʱ�϶�����
    private List<GameObject> lines = new List<GameObject>();

    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // ����������
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);
            if (hit != null && hit.CompareTag("Dot"))
            {
                firstDot = hit.transform;

                // ����һ����ʱ����
                GameObject temp = Instantiate(linePrefab);
                tempLine = temp.GetComponent<LineRenderer>();
                tempLine.positionCount = 2;
                tempLine.SetPosition(0, firstDot.position);
                tempLine.SetPosition(1, firstDot.position);
            }
        }

        // �϶�ʱ������ʱ����
        if (Input.GetMouseButton(0) && tempLine != null)
        {
            tempLine.SetPosition(1, mouseWorldPos);
        }

        // �ɿ����
        if (Input.GetMouseButtonUp(0) && tempLine != null)
        {
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);
            if (hit != null && hit.CompareTag("Dot") && hit.transform != firstDot)
            {
                // ��������
                tempLine.SetPosition(0, firstDot.position);
                tempLine.SetPosition(1, hit.transform.position);
                lines.Add(tempLine.gameObject);
            }
            else
            {
                // δ���ӳɹ���ɾ����
                Destroy(tempLine.gameObject);
            }

            tempLine = null;
            firstDot = null;
        }
    }

    /// <summary>
    /// ���������
    /// </summary>
    public void ClearAllLines()
    {
        foreach (GameObject line in lines)
        {
            Destroy(line);
        }
        lines.Clear();
        firstDot = null;
        Debug.Log("[DEBUG] All lines cleared");
    }
}

