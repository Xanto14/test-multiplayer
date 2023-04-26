using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointage : MonoBehaviour
{
    private int pointsAccumulés = 0;
    private float tempsParPoints = 0.5f;
    public int PointsAccumulés
    {
        get { return pointsAccumulés; }
        set { pointsAccumulés = value; }
    }

    public float getTempsÀSoustraire()
    {
        return PointsAccumulés * tempsParPoints;
    }
    public void IncrémenterPoints()
    {
        PointsAccumulés += 1;
        Debug.Log($"Nb de Points : {PointsAccumulés}");
    }

    public void DécrémenterPoints()
    {
        PointsAccumulés -= 1;
        Debug.Log($"Nb de Points : {PointsAccumulés}");
    }
}
