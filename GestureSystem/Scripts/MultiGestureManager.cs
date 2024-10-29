using System.Collections.Generic;
using UnityEngine;
using Cacophony;

[System.Serializable]
public struct GestureAction
{
    public string name;
    [SerializeField]
    public HandGestureDefinition readyGesture;
    [SerializeField]
    public HandGestureDefinition handGesture;
    [SerializeField]
    public IActionProcessor actionProcessor;
}

public class MultiGestureManager : MonoBehaviour
{
    public List<GestureAction> gestureActions;
    private SimpleHandPose currentHandPose;
    private Vector3 currentPosition;
    private List<HandGestureManager> managers;

    // Start is called before the first frame update
    void Start()
    {
        managers = new();
        currentHandPose = new();
        InstatiateDetectors();
    }

    private void InstatiateDetectors()
    {
        foreach (var ga in gestureActions)
        {
            GameObject clone = new GameObject();
            clone.transform.parent = transform;
            var manager = clone.AddComponent<HandGestureManager>();
            var detector = clone.AddComponent<HandGestureDetector>();

            detector.readyGesture = ga.readyGesture;
            detector.handGesture = ga.handGesture;
            manager.gestureDetector = detector;

            var ap = Instantiate(ga.actionProcessor);
            manager.actionProcessor = ap;
            manager.actionProcessor.Initialise(manager.gestureDetector);
            manager.name = ga.name;
            managers.Add(manager);
        }
    }

    public void SetHandPose(SimpleHandPose pose)
    {
        currentHandPose = pose;
    }

    public void SetHandPosition(Vector3 position)
    {
        currentPosition = position;
    }

    public void EnableGesture()
    {
        foreach(var manager in managers)
        { 
            manager.gestureDetector.EnableDetector();
        }
    }

    public void DisableGesture()
    {
        foreach(var manager in managers)
        { 
            manager.gestureDetector.DisableDetector();
        }
    }

    public List<HandGestureManager> GetManagers()
    {
        return managers;
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var manager in managers)
        {
            manager.SetHandData(currentHandPose, currentPosition);
        }
    }
}
