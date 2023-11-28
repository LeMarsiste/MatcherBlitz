using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;   // The prefab that this pool will be responsible for
    public int initialSize;     // The number of objects to be created when the pool is initialized
    public bool ignoreOptimizations = false;

    protected readonly Queue<GameObject> instances = new Queue<GameObject>();   // The queue of objects in the pool
    protected readonly List<GameObject> allInstances = new List<GameObject>();

    private void Awake()
    {
        Assert.IsNotNull(prefab);
    }

    public void Initialize()
    {
        for (var i = 0; i < initialSize; i++)
        {
            var obj = CreateInstance();
            obj.SetActive(false);
            instances.Enqueue(obj); // Enqueue the new object into the queue
            allInstances.Add(obj);
        }
    }


    public GameObject GetObject()
    {
        var obj = instances.Count > 0 ? instances.Dequeue() : CreateInstance();  // Dequeue an object from the queue if there are any, otherwise create a new object
        obj.SetActive(true);
        obj.GetComponent<PooledObject>().isInsidePool = false;
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        var pooledObject = obj.GetComponent<PooledObject>();
        Assert.IsNotNull(pooledObject);
        Assert.IsTrue(pooledObject.pool == this);

        obj.SetActive(false);
        if (!instances.Contains(obj))
        {
            instances.Enqueue(obj); // Enqueue the returned object into the queue
            obj.GetComponent<PooledObject>().isInsidePool = true;
        }
    }

    public void ReturnObject(GameObject obj, float delay)
    {
        var pooledObject = obj.GetComponent<PooledObject>();
        Assert.IsNotNull(pooledObject);
        Assert.IsTrue(pooledObject.pool == this);
        StartCoroutine(ReturnWithDelay(obj, delay));

    }

    IEnumerator ReturnWithDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);

        if (!instances.Contains(obj))
        {
            instances.Enqueue(obj); // Enqueue the returned object into the queue
        }
    }

    public void Reset()
    {
        var objectsToReturn = new List<GameObject>();
        foreach (var instance in transform.GetComponentsInChildren<PooledObject>())
        {
            if (instance.gameObject.activeSelf)
            {
                objectsToReturn.Add(instance.gameObject);
            }
        }
        foreach (var instance in objectsToReturn)
        {
            ReturnObject(instance);
        }
    }

    protected GameObject CreateInstance()
    {
        var obj = Instantiate(prefab);
        var pooledObject = obj.AddComponent<PooledObject>();
        pooledObject.pool = this;
        obj.transform.SetParent(transform);
        return obj;
    }
}

public class PooledObject : MonoBehaviour
{
    public ObjectPool pool; // The pool that this object belongs to
    public bool isInsidePool;
}

