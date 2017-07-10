using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraHelper : HoloToolkit.Unity.Singleton<CameraHelper>
{

    public struct UserStats
    {
        public Vector3 camPos;
        public Vector3 camLookDir;
        public Vector3 camLookDirInPlane;
        public float eyeHeight;
        public Vector3 groundPos;
        
        public UserStats(Vector3 camPos, Vector3 camLookDir, float eyeHeight)
        {
            this.camPos = camPos;
            this.camLookDir = camLookDir;
            this.camLookDirInPlane = Vector3.Scale(camLookDir, new Vector3(1, 0, 1)).normalized;
            this.eyeHeight = eyeHeight;
            this.groundPos = new Vector3(camPos.x, camPos.y - eyeHeight, camPos.z);
        }
    }

    private class VectorSampler
    {
        private Vector3 estimatedVector = Vector3.zero;
        private float sampleInfluence = 0.1f;
        private float sampleOutlierThreshold = 0.5f;
        private int outlierCount = 0;

        public Vector3 Vector3
        {
            get { return estimatedVector; }
        }

        public VectorSampler(Vector3 value, float influence = 0.1f, float outlierThreshold = 0.5f)
        {
            estimatedVector = value;
            sampleInfluence = influence;
            sampleOutlierThreshold = outlierThreshold;
        }

        public void Sample(Vector3 newSample)
        {
         
            if (Vector3.Distance(estimatedVector, newSample) < sampleOutlierThreshold)
            {
                estimatedVector = estimatedVector * (1 - sampleInfluence) + newSample * sampleInfluence;
            } else if (outlierCount > 5)
            {
                estimatedVector = newSample;
                outlierCount = 0;
            }
            else
            {
                outlierCount++;
            }
        }
    }

    public static float EyeHeight
    {
        get {
            return Instance.eyeHeight;
        }
    }

    public static UserStats Stats
    {
        get
        {
            return new UserStats(Instance.camPos.Vector3, Instance.camLookDir.Vector3, Instance.eyeHeight);
        }
    }


    private Transform camera;
    private VectorSampler camPos;
    private VectorSampler camLookDir;
    private float eyeSamplingInfluence = 0.1f;
    private float eyeHeight = 1.7f; // educated guess, see wikipedia

    private Material cameraFadeOverlayMaterial;
    private float cameraFadeOverlayAlpha;

    void Start () {
        camera = Camera.main.transform;
        camPos = new VectorSampler(camera.position);
        camLookDir = new VectorSampler(camera.forward, 0.5f, 10);

        Shader shader = Shader.Find("Hidden/Internal-Colored");
        cameraFadeOverlayMaterial = new Material(shader);
        cameraFadeOverlayMaterial.hideFlags = HideFlags.HideAndDontSave;
    }
	
	void Update ()
    {
        camPos.Sample(camera.position);
        camLookDir.Sample(camera.forward);
        SampleDistanceToGround();
    }

    void OnPostRender()
    {
        if (cameraFadeOverlayAlpha > 0)
        {
            cameraFadeOverlayMaterial.color = new Color(0, 0, 0, cameraFadeOverlayAlpha);
            GL.PushMatrix();

            GL.LoadOrtho();
            cameraFadeOverlayMaterial.SetPass(0);

            GL.Begin(GL.QUADS);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(1, 0, 0);
            GL.Vertex3(1, 1, 0);
            GL.Vertex3(0, 1, 0);
            GL.End();
            GL.PopMatrix();
        }
    }

    public void FadeIn(Action onFadeInComplete)
    {
        AnimateThis.With(this).CancelAll()
            .Do(t => cameraFadeOverlayAlpha = t)
            .From(1)
            .To(0)
            .Duration(1)
            .OnEnd(onFadeInComplete)
            .Start();
    }

    public void FadeOut(Action onFadeOutComplete)
    {
        AnimateThis.With(this).CancelAll()
            .Do(t => cameraFadeOverlayAlpha = t)
            .From(0)
            .To(1)
            .Duration(1)
            .OnEnd(onFadeOutComplete)
            .Start();
    }

    private void SampleDistanceToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(camPos.Vector3, Vector3.down, out hit, Mathf.Infinity, SpatialMapping.PhysicsRaycastMask))
        {
            float newEyeHeight = camPos.Vector3.y - hit.point.y;
            eyeHeight = Mathf.Max(1.5f, Math.Min(2.0f, eyeHeight * (1 - eyeSamplingInfluence) + newEyeHeight * eyeSamplingInfluence));
        }
    }
}
