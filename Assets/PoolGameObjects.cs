using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PooledItemID
{
    enemy1 = 0,
    enemy2 = 1,
    enemy3 = 2,
    enemy4 = 3
}
public class PoolGameObjects : MonoBehaviour
{
    public static PoolGameObjects poolGameObjects;
    public List<PooledItem> pooledGameobjects;
    private List<CompletePolledItem> completePolledItems;
    //private List<GameObject> poolObjects;

    
    
    private void Awake()
    {
        poolGameObjects = this;
        
    }

    void Start()
    {
        //poolObjects = new List<GameObject>();
        completePolledItems = new List<CompletePolledItem>();
        InitPoolObjects();
        //ReturnNonActiveObjectToPool(GetObjectInPool(PooledItemID.enemy1));

        
    }

    private void InitPoolObjects()
    {
        if (pooledGameobjects.Count == 0)
        {
            return;
        }
        for (int i = 0; i < pooledGameobjects.Count; i++)
        {
            for (int p = 0; p < pooledGameobjects[i].numInstance; p++)
            {
                GameObject newInstance = Instantiate(pooledGameobjects[i].itemPooled);
                newInstance.SetActive(false);
                CompletePolledItem completePolled = new CompletePolledItem();
                completePolled.linkToObject = newInstance;
                completePolled.pooledItemID = pooledGameobjects[i].enumID;
                completePolledItems.Add(completePolled);
            }
        }
    }

    public GameObject GetObjectInPool(PooledItemID pooledItemID)
    {
        GameObject gettedObject;
        int convertId = (int)pooledItemID;
        for (int i = 0; i < completePolledItems.Count; i++)
        {
            if (completePolledItems[i].pooledItemID == convertId)
            {
                if (completePolledItems[i].linkToObject.activeSelf == false)
                {
                    gettedObject = completePolledItems[i].linkToObject;
                    gettedObject.SetActive(true);
                    return gettedObject;
                }
            }
        }
        
        return null;
    }
   
    public void ReturnNonActiveObjectToPool(GameObject objectToReturn)
    {
        for (int i = 0; i < completePolledItems.Count; i++)
        {
            if (completePolledItems[i].linkToObject.activeSelf == true)
            {
                if (completePolledItems[i].linkToObject.GetHashCode() == objectToReturn.GetHashCode())
                {
                    completePolledItems[i].linkToObject.SetActive(false);
                    objectToReturn = null;
                }
            }
            
        }
    }
    
}

public class CompletePolledItem
{
    public GameObject linkToObject;
    public int pooledItemID;
}

[System.Serializable]
public class PooledItem
{
    public GameObject itemPooled;
    public int numInstance;
    public int enumID;
}




