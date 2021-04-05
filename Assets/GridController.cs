using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridController : MonoBehaviour
{
    public Vector2Int gridSize;
    public float cellRadius = 0.5f;
    public FlowField curFlowField;
    public GameObject target;
    public Cell targetCell;
    private PlatformController platformController;
    //public GridDebug gridDebug;

    private void InitializeFlowField()
    {
        curFlowField = new FlowField(cellRadius, gridSize, GetOffsetToFlowField());
        curFlowField.CreateGrid();
        
    }

    private void OnEnable()
    {
        platformController = transform.parent.GetComponent<PlatformController>();
        InitializeFlowField();
        curFlowField.CreateCostField();
        Cell destinationCell = curFlowField.GetCellFromWorldPos(target.transform.position);
        curFlowField.CreateIntegrationField(destinationCell);
        targetCell = destinationCell;
        curFlowField.CreateFlowField();
        platformController.enemyController.currentFlowField = curFlowField;
    }


    /*private void OnDrawGizmos()
    {
        if (curFlowField == null) { return; }
        DrawGrid(gridSize, Color.green, cellRadius);

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleCenter;

        foreach (Cell cell in curFlowField.grid)
        {
            Handles.Label(cell.worldPos, cell.bestCost.ToString(), style);
        }
    }

    private void DrawGrid(Vector2Int drawGridSize, Color drawColor, float drawCellRadius)
    {
        Vector3 offset = GetOffsetToFlowField();
        Gizmos.color = drawColor;
        for (int x = 0; x < drawGridSize.x; x++)
        {
            for (int y = 0; y < drawGridSize.y; y++)
            {
                Vector3 center = new Vector3(offset.x + drawCellRadius * 2 * x + drawCellRadius, 0, offset.z + drawCellRadius * 2 * y + drawCellRadius);
                Vector3 size = Vector3.one * drawCellRadius * 2;
                Gizmos.DrawWireCube(center, size);
            }
        }
    }*/

    private Vector3 GetOffsetToFlowField()
    {
        Vector3 thisPos = transform.position;
        Vector3 offset = new Vector3(-gridSize.x*cellRadius+thisPos.x, 0f, -gridSize.y*cellRadius+thisPos.z);
        return offset;
    }
}
