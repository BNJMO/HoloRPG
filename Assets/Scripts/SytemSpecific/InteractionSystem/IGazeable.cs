using UnityEngine;

public interface IGazeable
{
    void OnGazeEnter(RaycastHit hitinfo);
    void OnGazeStay(RaycastHit hitinfo);
    void OnGazeExit(RaycastHit hitinfo);
}