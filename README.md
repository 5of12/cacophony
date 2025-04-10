# Cacophony Gesture System

![Cacophony A gesture library build for action](/media/Cacophony_banner.png "Cacophony banner")

Cacophony is a **gesture detection system for Unity**. Designed for extensibility and speed when prototyping new ideas, and to reduce iteration time when building for reliability. 

Examples are included which demonstrate hand gesture detection via [Ultraleap Hand tracking](https://ultraleap.com), though the architecture is agnostic to the data source. 
_With a little imagination you can use it with almost anything..._

Cacophony breaks down the process of building gesture detection for applications into three main parts:
- Detecting a gesture to initiate interaction
- Processing the action performed by the user to derive intent
- Facilitating clear reactions to the user input by the application

To learn more about the thinking behind Cacophony you can read our short blog on the subject: [A Cacophony of Gestures](https://5of12.github.io/2025/04/07/a-cacophony-of-gestures.html)

## Requirements

Cacophony works as a drop in package for Unity and has been tested with Unity 6.0. 
Previous versions of Unity should work, but have not been thoroughly tested.

## Quick Start - Ultraleap Hand Tracking Example

### Project setup
- Open or create a new Unity Project
- Import Cacophony as a submodule or copy the files to a directory of your choice.
    - Navigate to `Assets/Plugins` in your project and run `git submodule add https://github.com/5of12/cacophony`
- Follow the instructions at: [github.com/ultraleap/UnityPlugin](https://github.com/ultraleap/UnityPlugin) to install the Ultraleap plugin
- **IMPORTANT: In `ProjectSettings/Player` add `ULTRALEAP` to the `Script Compilation > Scripting Define Symbols` section**. This enables the Ultraleap specific code required to process the data.

### Scene Setup
To start out, you can open one of the provided example scenes in: **cacophony/Examples/Scenes**

![Video of showing a virtual hand grabbing and releasing, then confetti is release](/media/Grab-Release.gif "Grab Release")

Alternatively, starting from a new scene:
- Add an Ultraleap hand tracking provider from the Scene hierarcy right-click menu: `Ultraleap > Tracking > Service Provider (*)`
- On the Service Provider GameObject, add the `LeapHandConnector` component. **This will take Ultraleap tracking data and convert into a usable form for cacophony.**
- Add a `GestureManager` prefab to the scene from `cacophony/Prefabs`
- Select the `GestureManger`. In the inspector, there are two components `Hand Gesture Manager` and `Hand Gesture Detector`.
    - On the `Hand Gesture Manager` observe the selected `Action Processor`, you can choose a different action asset here
    - On the `Hand Gesture Detector` observe the two options `Ready Gesture` and `Hand Gesture`, you can choose different gesture assets here
        - The `Ready Gesture` will be detected first, and until this is detected the action cannot trigger. It can be the same as the main Gesture you want to detect.
        - The `Hand Gesture` is the main focus of the interaction. When detected this will trigger action processing.
- Connect an Ultraleap tracking camera and press play. You should be able to observe the confidence changing on the gesture detector and the active state changing when the gesture is detected. 

To connect the gesture detection to application behaviour we need a `Consumer`, an object to receive the action events.
- Exit Play mode to return to Edit mode
- Create a new cube object in the scene and place it in front of the game camera.
- To add behaviour to your scene, create a new `GameObject` and add the `GestureConsumerUnityEvents` component. 
- Connect the `OnGestureStart` event to the new cube in the inspector. Select the `GameObject` `SetActive` option to the event and make sure the checkbox is checked.
- Connect the `OnGestureEnd` event to the new cube in the inspector. Select the `GameObject` `Set Active` option to the event and make sure the checkbox is unchecked.
- Press play. You should now observe the cube being enabled when the gesture is first detected and disabled when the action completes.

Check out our [Cacophony Playground](https://github.com/5of12/Cacophony-Playground) for more examples and see how you can set up cacophony for different scenarios.

## Components of the system

### Scriptable Assets
Cacophony is built around scriptable assets for defining the parts of the system you want to design specifically for your application. This makes iterating on the design of the interactions faster, with less time in code editors and waiting for recompilation. The assets can be created programaticaly or in the editor.

* `Poses` defined by values for a set of input data.
* `Gestures` define by a collection of positive and negative poses, and outputting a set of events.
* `Actions` are defined by constraints (eg. movements), outputting their own set of events.

## Game Components

`Gesture Managers` bring together an action and a gesture, provide them with data and control updates. These are the basic entry point to the system. The gesture manager provides the interface between Cacophony gestures and the application.

`Consumers` hook into the action events that are routed via the `Gesture Manager`. 

These are optional components that demonstrate how you to interface with gestures to get different effects. Examples of Consumers for Animation, Audio and Unity Events are found in `cacophony/GestureSystem/Consumers`:

* `GestureConsumerAnimator.cs`
* `GestureConsumerAudio.cs`
* `GestureConsumerUnityEvents.cs`

### Creating New Gestures - A Hand Gesture Example

### Creating Poses, Gesture Definitions, Actions
#### Hand Pose
* Open the `Cacophony` menu from the `Project View` context menu
* Create a new `Hand Pose` asset
* Configure the sliders for each fingers Bend (rotation from the knuckles), Curl (rotation at the finger tips) and Splay (the spread the hand) to define your pose
* If you want the pose to be detected only in a particular orientation, set the direction and normal vectors. 
* * Set them to zero if you want the pose to work in any orientation

#### Gesture Definition
* Open the `Cacophony` menu from the `Project View` context menu
* Create a new `Hand Gesture` asset
* Add the pose (or poses) that **you want to detect** to the list of **Positive Poses**
* Add a poses (or poses) you **definitely don't want to detect** to the list of **Negative Poses**
* Set the `Confidence Threshold` to an appropriate value.
* * If you have a single Positive Pose, a confidence value close to 1 will work
* * If you have Negative Poses, overall confidence will be much lower (close to zero), so reduce the confidence accordingly. (This might take some testing to find the right value)

#### Action Definition
* Open the `Cacophony` menu from the `Project View` context menu
* Create a new `HoldTimeAction`, `MovementAction` or `PassthroughAction` asset
* `HoldTimeAction` allows you to set the time a pose must be held and a distance. If the hand moves further than the distance the timer is cancelled.
* `MovementAction` allows you define a distance, direction and angle of movement. While the gesture is held, a hand movement aligned to this will trigger the action. Moving outside the angle will cancel the action.
* `PassthroughAction` relays the input events directly to matching output events, with an optional distance value for filtering very small hand movements.

### Setting up a gesture in the scene

* Add a new `GameObject`
* Add the `HandGestureManager` component
* Set the `Gesture` parameter to a `Hand Gesture` asset of your choice
* Set the `Action` parameter to an asset of your choice

The Gesture Manager will configure the gesture and action on start and triger updates every frame. It needs to be supplied with hand data, otherwise it will always update with default values.

## Action Output Events

Actions output the following events that can be subscribed to by a Consumer:

* `Start` to indicate the gesture has been detected and the action is starting. This is accompanied by the position at which the gesture started
* `Hold` to indicate the gesture is being held and the action is progressing, but has not yet completed. This is accompanied by the current source position
* `End` to indicate the action has been completed successfully. This is accompanied by the position at the time it completed
* `Cancel` to indicate that the gesture was ended before the action completed.

