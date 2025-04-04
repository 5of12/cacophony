using NUnit.Framework;
using UnityEngine;
using Cacophony;

public class DoubleTriggerActionTest
{
    // TODO: Extend DoubleTrigger Tests
    public MockDetectionSource mockSource;
    public GameObject testObject;
    public DoubleTriggerAction testAction;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        testObject = new GameObject();
        mockSource = testObject.AddComponent<MockDetectionSource>();
        mockSource.Init();
        testAction = ScriptableObject.CreateInstance<DoubleTriggerAction>();
        testAction.Initialise(mockSource);
    }

    [TearDown]
    public void AfterEach()
    {
        testAction.OnStart.RemoveAllListeners();
        testAction.OnHold.RemoveAllListeners();
        testAction.OnEnd.RemoveAllListeners();
        testAction.OnCancel.RemoveAllListeners();
    }

    [Test]
    public void DoubleTriggerInitialisesWaitingForFirstTrigger()
    {
        Assert.IsTrue(testAction.WaitingForFirstTrigger);
    }
}
