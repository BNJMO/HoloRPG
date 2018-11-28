using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager for automatic waypoint tracing in edit mode.
/// </summary>
public class WaypointTracingManager : Singleton<WaypointTracingManager>//, IKeywordCommandProvider
{
	/*private AnchorManager anchorManager;

	private ApplicationStateManager appStateManager;

	public bool IsRecording {
		get {
			return isTracing;
		}
	}

	[SerializeField]
	private float samplingDistance = 0.125f;

	[SerializeField]
	private float smoothingThreshold = 0.25f;

	private Transform traceable;
	private bool isTracing = false;
	private Vector3 lastRecordedPosition;
	private List<Vector3> rawSamples = new List<Vector3>();
	private Vector3 recordingStartPoint;

	new void Awake()
	{
		base.Awake();
	}
	
	void Start ()
	{
		traceable = Camera.main.transform;
		anchorManager = AnchorManager.Instance;
		appStateManager = ApplicationStateManager.Instance;
		KeywordCommandManager.Instance.AddKeywordCommandProvider (this);
	}

	void FixedUpdate ()
	{
		if (isTracing) {
			Vector3 currentPosition = traceable.position;
			if (Vector3.Distance (lastRecordedPosition, currentPosition) > samplingDistance) {
				SampleCurrentPosition ();
			}
		}
	}
	
	private void SampleCurrentPosition()
	{
        Vector3 currentPosition = CameraHelper.Stats.groundPos;
		rawSamples.Add (currentPosition);
		lastRecordedPosition = currentPosition;
	}

	private void OnStartStopTracing(bool startTracing)
	{
		if (isTracing && startTracing || !isTracing && !startTracing) {
			return;
		}
		if (startTracing) {
			Notify.Show ("Waypoint Recording Started.");
			StartTracing(true);
		} else {
			Notify.Show ("Recording Stopped.");
			StopTracing ();
		}
	}

	/// <summary>
	/// Starts the tracing.
	/// </summary>
	/// <param name="initialSample">If set to <c>true</c>, an initial sample'is taken at start.</param>
	private void StartTracing(bool initialSample)
	{
		rawSamples.Clear ();
		if (initialSample) {
			SampleCurrentPosition ();
		}
		recordingStartPoint = traceable.position;
		isTracing = true;
	}
		
	private void StopTracing()
	{
        SampleCurrentPosition();
        CreateAnchorsForSamples (ReduceSamples ());
		rawSamples.Clear();
		isTracing = false;
	}
		
	private void CreateAnchorsForSamples(List<Vector3> samples)
	{
		AnchorManager anchorManager = AnchorManager.Instance;
        foreach (Vector3 position in samples) {
			anchorManager.AddWaypoint (position);
		}
        AnchorEditorUi.Instance.NotifyModelChanged();
		Notify.Show ("There were " + samples.Count + " new waypoints \n created");
	}

	/// <summary>
	/// Reduces all collected sample points to the minimal necessary set of points
	/// using the Douglas Peuker algorithm.
	/// </summary>
	/// <returns>The samples.</returns>
    private List<Vector3> ReduceSamples()
    {
        return SplitDouglasPeucker(rawSamples);
    }
		
	/// <summary>
	/// Implementation if the Douglas Peucker Algorithm.
	/// For more info see: https://de.wikipedia.org/wiki/Douglas-Peucker-Algorithmus
	/// </summary>
	/// <returns>A reduced list of points</returns>
	/// <param name="points">The raw list of points.</param>
    private List<Vector3> SplitDouglasPeucker(List<Vector3> points)
    {

        if (points.Count <= 2)
        {
            return new List<Vector3>(points);
        }

        float distanceOfMostDistantPoint = 0;
        int indexOfMostDistantPoint = FindMostDistantPointIndex(points, out distanceOfMostDistantPoint);

        if (distanceOfMostDistantPoint > smoothingThreshold)
        {
            List<Vector3> firstSegment = SplitDouglasPeucker(points.GetRange(0, indexOfMostDistantPoint + 1));
            List<Vector3> secondSegment = SplitDouglasPeucker(points.GetRange(indexOfMostDistantPoint, points.Count - indexOfMostDistantPoint));

            secondSegment.RemoveAt(0);
            List<Vector3> result = new List<Vector3>();
            result.AddRange(firstSegment);
            result.AddRange(secondSegment);
            return result;

        } else
        {
            return new List<Vector3> { points[0], points[points.Count - 1] };
        }
    }

	/// <summary>
	/// Finds the index of the most distant point.
	/// </summary>
	/// <returns>The most distant point index.</returns>
	/// <param name="points">Points.</param>
	/// <param name="distanceOfMostDistantPoint">Distance of most distant point.</param>
    private int FindMostDistantPointIndex(List<Vector3> points, out float distanceOfMostDistantPoint)
    {
		
        if (points.Count < 3)
        {
            throw new System.ArgumentException("Points count must be >= 3");
        }

        Vector3 firstPoint = points[0];
        Vector3 lastPoint = points[points.Count - 1];

        float largestDistanceFound = 0f;
        int  indexOfMostDistantPoint = 1;

        for(int i = 1; i < points.Count - 1; i++)
        {
            Vector3 currentPoint = points[i];
            float currentDistance = VectorUtils.CalcDistanceOfRay(firstPoint, lastPoint, currentPoint);

            if (currentDistance > largestDistanceFound)
            {
                largestDistanceFound = currentDistance;
                indexOfMostDistantPoint = i;
            } 
        }
        distanceOfMostDistantPoint = largestDistanceFound;
        return indexOfMostDistantPoint;
    }

	public List<KeywordCommand> GetSpeechCommands() {
		List<KeywordCommand> result = new List<KeywordCommand> ();

        Condition condEditModeActive = Condition.New(() => ApplicationStateManager.IsEditMode && AnchorEditorUi.Instance.isActiveAndEnabled);
        Condition condRecording = Condition.New(() => IsRecording);

        result.Add(new KeywordCommand(() => { OnStartStopTracing(false); }, condEditModeActive.And(condRecording), "Stop Recording", KeyCode.X, KeywordCommandManager.EXPERT_MODE));
        result.Add(new KeywordCommand(() => { OnStartStopTracing(true); }, condEditModeActive.And(condRecording.Not()), "Record Me", KeyCode.Y, KeywordCommandManager.EXPERT_MODE));

		return result;
	}*/
}
