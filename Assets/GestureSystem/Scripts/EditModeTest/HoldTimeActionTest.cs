using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Cacophony;
using System.Collections;

public class HoldTimeActionTest
{
    public MockDetectionSource mockSource;
    public GameObject testObject;
    public HoldTimeAction testAction;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        testObject = new GameObject();
        mockSource = testObject.AddComponent<MockDetectionSource>();
        mockSource.Init();

        testAction = ScriptableObject.CreateInstance<HoldTimeAction>();
        testAction.Initialise(mockSource);
    }

    [TearDown]
    public void AfterEach()
    {
        testAction.OnStart.RemoveAllListeners();    
        testAction.OnHold.RemoveAllListeners();    
        testAction.OnEnd.RemoveAllListeners();    
        testAction.OnCancel.RemoveAllListeners();    
        testAction.maxDistanceM = 0f;
        testAction.holdTimeS = 0f;
    }

    [Test]
    public void ActionPassesThroughStartEvent()
    {
        bool started = false;
        testAction.OnStart.AddListener ( () => {started = true;} );
        mockSource.OnStart.Invoke();
        
        Assert.IsTrue(started);
    }

    [Test]
    public void GestureEndEventAfterStartCancelsDetection()
    {
        bool ended = false;
        testAction.OnCancel.AddListener ( () => {ended = true;} );

        mockSource.OnStart.Invoke();
        mockSource.OnEnd.Invoke();
        
        Assert.IsTrue(ended);
    }

    [Test]
    public void GestureEndEventBeforeStartSendsNothing()
    {
        bool ended = false;
        bool cancelled = false;
        testAction.OnCancel.AddListener ( () => {cancelled = true;} );
        testAction.OnEnd.AddListener ( () => {ended = true;} );
        
        mockSource.OnCancel.Invoke();
        Assert.IsTrue(cancelled);

        mockSource.OnEnd.Invoke();
        Assert.IsFalse(ended);
    }

    [Test]
    public void ActionPassesThroughCancelEvent()
    {
        bool cancelled = false;
        testAction.OnCancel.AddListener ( () => {cancelled = true;} );
        mockSource.OnCancel.Invoke();
        
        Assert.IsTrue(cancelled);
    }

    [Test]
    public void ActionCancelsAndStopsIfMovementTooGreat()
    {
        bool cancelled = false;
        int holdCalls = 0;

        Vector3 startPos = Vector3.one;

        testAction.OnCancel.AddListener ( () => { cancelled = true; } );
        testAction.OnHold.AddListener ( () => { holdCalls++; } );
        testAction.maxDistanceM = 0.1f;

        testAction.Evaluate(startPos);
        mockSource.OnStart.Invoke();
        testAction.Evaluate(startPos);
        mockSource.OnHold.Invoke();
        Assert.AreEqual(holdCalls, 1);

        testAction.Evaluate(startPos + new Vector3(0.2f, 0, 0));
        mockSource.OnHold.Invoke();
        Assert.IsTrue(cancelled);

        // Check it's not invoking hold after being cancelled
        mockSource.OnHold.Invoke();
        Assert.AreEqual(holdCalls, 1);
        
    }
}
