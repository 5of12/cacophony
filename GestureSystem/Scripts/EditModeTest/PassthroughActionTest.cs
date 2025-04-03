using NUnit.Framework;
using UnityEngine;
using Cacophony;

public class PassthroughActionTest
{
    public MockDetectionSource mockSource;
    public GameObject testObject;
    public PassthroughAction testAction;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        testObject = new GameObject();
        mockSource = testObject.AddComponent<MockDetectionSource>();
        mockSource.Init();

        testAction = ScriptableObject.CreateInstance<PassthroughAction>();
        testAction.Initialise(mockSource);
    }

    [TearDown]
    public void AfterEach()
    {
        testAction.OnStart.RemoveAllListeners();    
        testAction.OnHold.RemoveAllListeners();    
        testAction.OnEnd.RemoveAllListeners();    
        testAction.OnCancel.RemoveAllListeners();    
        testAction.deadzoneDistance = 0.0f;

    }

    [Test]
    public void ActionPassesThroughStartEvent()
    {
        bool started = false;
        testAction.OnStart.AddListener ( (ActionEventArgs pos) => {started = true;} );
        mockSource.OnStart.Invoke();
        
        Assert.IsTrue(started);
    }

    [Test]
    public void ActionPassesThroughEndEvent()
    {
        bool ended = false;
        testAction.OnEnd.AddListener ( (ActionEventArgs pos) => {ended = true;} );
        mockSource.OnEnd.Invoke();
        
        Assert.IsTrue(ended);
    }

    [Test]
    public void ActionPassesThroughCancelEvent()
    {
        bool cancelled = false;
        testAction.OnCancel.AddListener ( (ActionEventArgs pos) => {cancelled = true;} );
        mockSource.OnCancel.Invoke();
        
        Assert.IsTrue(cancelled);
    }

    [Test]
    public void HoldTriggersWithZeroMovementAndNoDeadzone()
    {
        bool holding = false;
        Vector3 startPos = Vector3.one;
        testAction.OnHold.AddListener ( (ActionEventArgs pos) => {holding = true;} );

        testAction.Evaluate(startPos);
        mockSource.OnStart.Invoke();
        testAction.Evaluate(startPos);
        mockSource.OnHold.Invoke();
        
        Assert.IsTrue(holding);
    }

    [Test]
    public void HoldTriggersWithSomeMovementAndNoDeadzone()
    {
        bool holding = false;
        Vector3 startPos = Vector3.one;
        testAction.OnHold.AddListener ( (ActionEventArgs pos) => {holding = true;} );

        testAction.Evaluate(startPos);
        mockSource.OnStart.Invoke();
        testAction.Evaluate(startPos + new Vector3(0.1f, 0.2f, 0.3f));
        mockSource.OnHold.Invoke();
        
        Assert.IsTrue(holding);
    }

    [Test]
    public void HoldDoesntTriggerWithNoMovementAndADeadzone()
    {
        bool holding = false;
        Vector3 startPos = Vector3.one;
        testAction.OnHold.AddListener ( (ActionEventArgs pos) => {holding = true;} );
        testAction.deadzoneDistance = 0.1f;

        testAction.Evaluate(startPos);
        mockSource.OnStart.Invoke();
        testAction.Evaluate(startPos);
        mockSource.OnHold.Invoke();
        
        Assert.IsFalse(holding);
    }
}
