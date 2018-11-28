using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorUtils {

	public static float CalcDistanceOfRay(Vector3 start, Vector3 end, Vector3 point)
    {
        Vector3 ab = (end - start).normalized;
        Vector3 ap = point - start;
        Vector3 projectedConPoint = Vector3.Dot(ap, ab) * ab;
        float currentDistance = Vector3.Distance(projectedConPoint, ap);
        return currentDistance;
    }

    public static bool IsPointBetweenStartAndEnd(Vector3 start, Vector3 end, Vector3 point)
    {
        Vector3 ab = (end - start).normalized;
        Vector3 ap = point - start;
        Vector3 projectedConPoint = Vector3.Dot(ap, ab) * ab + start;
        float len = Vector3.Distance(start,  end);
        return Vector3.Distance(end, projectedConPoint) < len && Vector3.Distance(start, projectedConPoint) < len;
    }

    /// <summary>
    /// Gives a the needed rotation for an object 1 to face an object 2
    /// </summary>
    /// <param name="pos1">position of object 1</param>
    /// <param name="pos2">position of object 2</param>
    /// <returns>The rotation in a Quaternion</returns>
    public static Quaternion LookAt2D(Vector3 pos1, Vector3 pos2)
    {
        Vector3 dir = pos2 - pos1;
        dir.y = 0;
        return Quaternion.LookRotation(dir, Vector3.up);
    }

}
