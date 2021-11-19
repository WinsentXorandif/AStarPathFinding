using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeStateInfo : MonoBehaviour
{
    public int x;
    public int y;
    public string status;

    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable = true;
    public CubeStateInfo cameFromNode;

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void SetCubeXY(int varX, int varY) {
        x = varX;
        y = varY;
    }

    public void SetSTARTcube(string varName, Material mat) {
        status = varName;
        this.GetComponent<Renderer>().material = mat;
        isWalkable = true;
    }

    public void SetFINISHcube(string varName, Material mat)
    {
        status = varName;
        this.GetComponent<Renderer>().material = mat;
        isWalkable = true;
    }

    public void SetBLOCKcube(string varName, Material mat) {
        status = varName;
        this.GetComponent<Renderer>().material = mat;
        isWalkable = false;
        cameFromNode = null;
    }

    public void SetFREEcube(string varName, Material mat)
    {
        status = varName;
        this.GetComponent<Renderer>().material = mat;
        isWalkable = true;
    }

    public void SetPATHcube(string varName, Material mat)
    {
        status = varName;
        this.GetComponent<Renderer>().material = mat;
        isWalkable = true;
    }


}
