using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Cacophony;

public class PassthroughActionTest
{
    public MockDetectionSource mockSource;

    // A Test behaves as an ordinary method
    [Test]
    public void PassthroughActionTestSimplePasses()
    {
        GameObject testObject = new GameObject();
        mockSource = testObject.AddComponent<MockDetectionSource>();
        PassthroughAction testAction = ScriptableObject.CreateInstance<PassthroughAction>();
        testAction.Initialise(mockSource);

        bool started = false;
        testAction.OnStart.AddListener ( () => {started = true;} );
        mockSource.OnStart.Invoke();
        
        Assert.IsTrue(started);
    }

}
