using NUnit.Framework;
using UnityEngine;
using Cacophony;

public class DirectionMoveActionTest
{
    public MockDetectionSource mockSource;
    public GameObject testObject;
    public DirectionMoveAction testAction;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        testObject = new GameObject();
        mockSource = testObject.AddComponent<MockDetectionSource>();
        mockSource.Init();

        testAction = ScriptableObject.CreateInstance<DirectionMoveAction>();
        testAction.Initialise(mockSource);
    }

    [TearDown]
    public void AfterEach()
    {
        testAction.OnStart.RemoveAllListeners();    
        testAction.OnHold.RemoveAllListeners();    
        testAction.OnEnd.RemoveAllListeners();    
        testAction.OnCancel.RemoveAllListeners();    
        testAction.minDistanceM = 0f;
        testAction.directionTolerence = 0f;
        testAction.moveDirection = Vector3.zero;
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
        testAction.OnEnd.AddListener ( (ActionEventArgs pos) => {ended = true;} );
        
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
    public void EndTriggersWhenDistanceMovedAlongDirectionIsSufficient()
    {
        bool ended = false;
        int holdCalls = 0;

        Vector3 startPos = Vector3.zero;

        testAction.OnEnd.AddListener ( (ActionEventArgs pos) => { ended = true; } );
        testAction.OnHold.AddListener ( (ActionEventArgs pos) => { holdCalls++; } );
        testAction.minDistanceM = 0.1f;
        testAction.moveDirection = Vector3.right;
        testAction.directionTolerence = 45;

        testAction.Evaluate(startPos);
        mockSource.OnStart.Invoke();
        testAction.Evaluate(startPos);
        mockSource.OnHold.Invoke();
        Assert.AreEqual(holdCalls, 1);

        testAction.Evaluate(startPos + new Vector3(0.2f, 0, 0));
        mockSource.OnHold.Invoke();
        Assert.IsTrue(ended);

        // Check it's not invoking hold after being cancelled
        mockSource.OnHold.Invoke();
        Assert.AreEqual(holdCalls, 1);
        
    }

    [Test]
    public void HoldSentWhenMovementLessThanThreshold()
    {
        bool ended = false;
        int holdCalls = 0;

        Vector3 startPos = Vector3.zero;

        testAction.OnEnd.AddListener ( (ActionEventArgs pos) => { ended = true; } );
        testAction.OnHold.AddListener ( (ActionEventArgs pos) => { holdCalls++; } );
        testAction.minDistanceM = 0.3f;
        testAction.moveDirection = Vector3.right;
        testAction.directionTolerence = 45;

        testAction.Evaluate(startPos);
        mockSource.OnStart.Invoke();
        testAction.Evaluate(startPos);
        mockSource.OnHold.Invoke();
        Assert.AreEqual(holdCalls, 1);

        testAction.Evaluate(startPos + new Vector3(0.1f, 0, 0));
        mockSource.OnHold.Invoke();
        Assert.AreEqual(holdCalls, 2);
        Assert.IsFalse(ended);
    }

    [Test]
    public void EndNotSentWhenMovementAlongWrongDirection()
    {
        bool ended = false;

        Vector3 startPos = Vector3.zero;

        testAction.OnEnd.AddListener ( (ActionEventArgs pos) => { ended = true; } );
        testAction.minDistanceM = 0.1f;
        testAction.moveDirection = Vector3.right;
        testAction.directionTolerence = 45;

        testAction.Evaluate(startPos);
        mockSource.OnStart.Invoke();
        testAction.Evaluate(startPos);
        mockSource.OnHold.Invoke();

        // Test three directions that are orthogonol to the direction
        testAction.Evaluate(startPos + new Vector3(0, 1, 0));
        mockSource.OnHold.Invoke();
        testAction.Evaluate(startPos + new Vector3(-1f, 0, 0));
        mockSource.OnHold.Invoke();
        testAction.Evaluate(startPos + new Vector3(0, -1f, 0));
        mockSource.OnHold.Invoke();

        // And one that is close to 45
        testAction.Evaluate(startPos + new Vector3(0.9f, 1f, 0));
        mockSource.OnHold.Invoke();

        Assert.IsFalse(ended);
    }

    [Test]
    public void DirectionTolerenceCorrectlyApplied()
    {
        bool ended = false;

        Vector3 startPos = Vector3.zero;

        testAction.OnEnd.AddListener ( (ActionEventArgs pos) => { ended = true; } );
        testAction.minDistanceM = 0.1f;
        testAction.moveDirection = Vector3.right;
        testAction.directionTolerence = 30;

        testAction.Evaluate(startPos);
        mockSource.OnStart.Invoke();
        testAction.Evaluate(startPos);
        mockSource.OnHold.Invoke();

        // 45 degrees doesn't trigger
        testAction.Evaluate(startPos + new Vector3(0.5f, 0.5f, 0));
        mockSource.OnHold.Invoke();

        Assert.IsFalse(ended);

        // <30 degrees does trigger
        testAction.Evaluate(startPos + new Vector3(0.5f, 0.1f, 0));
        mockSource.OnHold.Invoke();

        Assert.IsTrue(ended);
    }
}
