using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A basic singleton class that gives the current FPS.
/// </summary>
public class FPSCounter : Singleton<FPSCounter>
{
    private int myFps = 0;
    public int FPS { get { return myFps; }}

    [SerializeField] Text text;

    private float lastRegistredFPS;

    private float fpsOffset = 2;
    public float FPSOffset { set { fpsOffset = value; }}

    private int counter;
    private int iterationLimit = 50;

    private new void Awake()
    {
        base.Awake();
        counter = 0;
    }

    private void Update()
    {
        counter++;

        if (counter > iterationLimit)
        {
            int newFps = (int)(1 / Time.deltaTime);
            myFps = newFps;
            counter = 0;
        }
        if (text != null)
        {
            text.text = "fps : " + myFps;
        }

       /* int newFps = (int) (1 / Time.deltaTime);
        if (Mathf.Abs (newFps - lastRegistredFPS) > fpsOffset)
        {
            myFps = newFps;
        }*/
    }
    
}
