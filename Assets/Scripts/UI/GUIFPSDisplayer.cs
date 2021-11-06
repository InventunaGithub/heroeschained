using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIFPSDisplayer : MonoBehaviour
{
    // fps display
    public Text Display;
    public bool Detailed = false;
    public float Frequency = 0.25f;
    public bool Enabled = false;

    // Start is called before the first frame update
    float totalFps = 0;
    long totalRead = 0;

    // Start is called before the first frame update
    void Start()
    {
        Display.gameObject.SetActive(Enabled);
        StartCoroutine(DisplayFPS());
    }

    IEnumerator DisplayFPS()
    {
        float fps;
        while (true)
        {
            yield return new WaitForSeconds(Frequency);

            fps = (1.0f / Time.deltaTime);
            totalFps += fps;
            totalRead += 1;

            if (Enabled)
            {
                Display.gameObject.SetActive(true);

                if (Detailed)
                {
                    Display.text = "FPS: " + fps.ToString("N0") + " (Avg: " + (totalFps / totalRead).ToString("N0") + ")";
                }
                else
                {
                    Display.text = "FPS: " + (totalFps / totalRead).ToString("N0");
                }
            } else
            {
                Display.gameObject.SetActive(false);
            }
        }
    }
}
