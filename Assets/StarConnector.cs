using UnityEngine;
using System.Collections.Generic;

public class StarConnector : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject linePrefab;  // 线条预制体（带 LineRenderer）
    public GameObject dotPrefab;   // 可选：动态生成点

    private Transform firstDot = null;  // 记录起点
    private LineRenderer tempLine = null; // 临时拖动的线
    private List<GameObject> lines = new List<GameObject>();

    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // 按下鼠标左键
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);
            if (hit != null && hit.CompareTag("Dot"))
            {
                firstDot = hit.transform;

                // 生成一条临时线条
                GameObject temp = Instantiate(linePrefab);
                tempLine = temp.GetComponent<LineRenderer>();
                tempLine.positionCount = 2;
                tempLine.SetPosition(0, firstDot.position);
                tempLine.SetPosition(1, firstDot.position);
            }
        }

        // 拖动时更新临时线条
        if (Input.GetMouseButton(0) && tempLine != null)
        {
            tempLine.SetPosition(1, mouseWorldPos);
        }

        // 松开鼠标
        if (Input.GetMouseButtonUp(0) && tempLine != null)
        {
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);
            if (hit != null && hit.CompareTag("Dot") && hit.transform != firstDot)
            {
                // 连接两点
                tempLine.SetPosition(0, firstDot.position);
                tempLine.SetPosition(1, hit.transform.position);
                lines.Add(tempLine.gameObject);
            }
            else
            {
                // 未连接成功则删除线
                Destroy(tempLine.gameObject);
            }

            tempLine = null;
            firstDot = null;
        }
    }

    /// <summary>
    /// 清除所有线
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

