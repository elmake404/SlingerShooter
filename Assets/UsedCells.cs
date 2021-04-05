using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsedCells
{
    public List<Cell> usedCells;

    public UsedCells()
    {
        usedCells = new List<Cell>();
    }
    public void AddCellToUsed(Cell cellToAdd)
    {
            usedCells.Add(cellToAdd);

        //Debug.Log(usedCells.Count);
    }

    public void RemoveCellFromUsed(Cell cellToRemove)
    {
        if (cellToRemove == null) { return; }
        int toRemoveHash = cellToRemove.GetHashCode();
        for (int i = 0; i < usedCells.Count; i++)
        {
            if (usedCells[i].GetHashCode() == toRemoveHash)
            {
                usedCells.RemoveAt(i);
            }
        }
        //Debug.Log(usedCells.Count);
    }

    
}


