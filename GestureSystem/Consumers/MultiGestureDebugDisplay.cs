using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiGestureDebugDisplay : MonoBehaviour
{
    public MultiGestureManager manager;
    public GameObject consumerPrefab;
    public Transform consumerParent;
    private List<GestureConsumerRowUI> consumers;

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
            var consumer = clone.GetComponent<GestureConsumerRowUI>();
            consumer.manager = man;
            consumer.Initialise(man.name);
            consumers.Add(consumer);
        }
    }

    public GestureConsumerRowUI GetConsumerByGestureName(string gestureName){
        try {
            GestureConsumerRowUI row = consumers.Where(obj => obj.gestureName == gestureName).First();
            return row;
        }
        catch {
            Debug.LogWarning("Unable to find GestureName row with name: " + gestureName + " Check the MultiGestureManger for valid names!");
            return null;
        }
    }
}
