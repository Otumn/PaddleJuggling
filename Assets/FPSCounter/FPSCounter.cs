using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Otumn.FPSCounter
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private Text fpsText;
        [SerializeField] private int frameSmoothCount = 20;
        private float fps;
        private float[] fpsArray;
        private int arrayIndex;

        private void Start()
        {
            Application.targetFrameRate = 60;
            fpsArray = new float[frameSmoothCount];
        }

        private void Update()
        {
            fpsArray[arrayIndex] = (1.0f / Time.deltaTime) * Time.timeScale;
            arrayIndex++;
            if (arrayIndex > fpsArray.Length - 1)
            {
                arrayIndex = 0;
            }

            for (int i = 0; i < fpsArray.Length; i++)
            {
                fps += fpsArray[i];
            }
            fps /= fpsArray.Length;
            fps = Mathf.Round(fps);
            fpsText.text = fps.ToString("F0") + " fps";
        }
    }
}
