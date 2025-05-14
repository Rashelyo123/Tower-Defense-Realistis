using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiSwipe : MonoBehaviour
{
    public GameObject scrollbar;
    float scroll_pos = 0;
    float[] pos;
    int currentIndex = 0;
    float distance;

    void Start()
    {
        // Inisialisasi posisi sekali di awal
        int count = transform.childCount;
        pos = new float[count];
        distance = 1f / (count - 1f);

        for (int i = 0; i < count; i++)
        {
            pos[i] = distance * i;
        }
    }

    void Update()
    {
        // Input panah kiri
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex = Mathf.Max(0, currentIndex - 1);
        }

        // Input panah kanan
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex = Mathf.Min(pos.Length - 1, currentIndex + 1);
        }

        // Update posisi scroll ke target index jika tidak drag
        if (!Input.GetMouseButton(0))
        {
            scroll_pos = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[currentIndex], 0.1f);
            scrollbar.GetComponent<Scrollbar>().value = scroll_pos;
        }
        else
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
            // Update currentIndex berdasarkan scroll pos saat drag
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    currentIndex = i;
                }
            }
        }

        // Scaling efek
        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1.46f, 1.46f), 0.1f);
                for (int a = 0; a < pos.Length; a++)
                {
                    if (a != i)
                    {
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }
            }
        }
    }
}
