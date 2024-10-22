using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiGestureDebugDisplay : MonoBehaviour
{
    public MultiGestureManager manager;
    public GameObject consumerPrefab;
    public Transform consumerParent;
    private List<PrototypeConsumer> consumers;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("InitialiseLate");
    }

    private IEnumerator InitialiseLate()
    {
        yield return new WaitForEndOfFrame();
        InitialiseConsumers();
    }
    private void InitialiseConsumers()
    {
        consumers = new();
        foreach(var man in manager.GetManagers())
        {
            var clone = Instantiate(consumerPrefab, consumerParent);
            var consumer = clone.GetComponent<PrototypeConsumer>();
            consumer.manager = man;
            consumer.Initialise(man.name);
            consumers.Add(consumer);
        }
    }
}
