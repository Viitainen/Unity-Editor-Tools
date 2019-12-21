using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickProfilerTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // QuickProfiler.StartTimer("Hello");

        // QuickProfiler.StartTimer("Testing");
    }
    float timer = 0;
    int frameNumber = 0;

    private void Update() {
        QuickProfiler.Log("Frame number", frameNumber.ToString());
        QuickProfiler.Log("Mouse delta", Input.mousePosition.ToString());
        frameNumber++;

        // timer += Time.deltaTime;

        // if (timer > 5) {
        //     QuickProfiler.StopTimer("Hello");
        //     QuickProfiler.StartTimer("Hello 2");
        // }

        // if (timer > 10) {
        //     QuickProfiler.StopTimer("Testing");
        //     QuickProfiler.StartTimer("Testing 2");
        // }
    }
}
