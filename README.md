# Cacophony Gesture System

![Cacophony A gesture library build for action](/media/Cacophony_banner.png "Cacophony banner")

Cacophony is a gesture detection system for Unity. Designed for extensibility and speed when prototyping new ideas. Included are examples for how to use gesture detection with Ultraleap Hand tracking, though the architecture is agnostic to the data source. With a little imagination you can use it with almost anything.

Cacophony breaks down the process of building gesture detection for applications into three main parts:
- Detecting a gesture to initiate interaction
- Processing the action performed by the user to derive intent
- Facilitating clear reactions to the user input by the application

To learn more about the thinking behind Cacophony you can read our short blog on the subject: [A Cacophony of Gestures](https://5of12.github.io/2025/04/07/a-cacophony-of-gestures.html)


## Requirements

Cacophony is build to work as a drop in package for Unity and has been tested with Unity 6.0. 
Previous versions of Unity should work, but have not been thoroughly tested.

## Features

### Scriptable Assets
Cacophony is built around scriptable assets for defining the parts of the system you want to design specifically for your application. This makes iterating on the design of the interactions faster, with less time in code editors and waiting for recompilation. The assets can be created programaticaly or in the editor.

* `Poses` defined by values for a set of input data.
* `Gestures` define by a collection of positive and negative poses, and outputting a set of events.
* `Actions` are defined by constraints (eg. movements), outputting their own set of events.

## Getting Started

`Gesture Managers` bring together an action and a gesture, provide them with data and control updates. These are the basic entry point to the system. The gesture manager provides the interface between Cacophony gestures and the application.

`Consumers` hook into the action events that are routed via the `Gesture Manager`. These are optional components that demonstrate how you might interface with the gestures to get different effects.

### Hand Gesture Example

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
* Add the pose (or poses) that you want to detect to the list of positive poses
* Add an poses you definitely don't want to detect to the list of negative poses
* Set the `Confidence Threshold` to an appropriate value.
* * If you have a single positive pose, a confidence value close to 1 will work
* * If you have negative poses, overall confidence will be much lower (close to zero), so reduce the confidence accordingly. This might take some testing to find the right value

#### Action Definition
* Open the `Cacophony` menu from the `Project View` context menu
* Create a new `HoldTimeAction`, `MovementAction` or `PassthroughAction` asset
* `HoldTimeAction` allow you to set the time a pose must be held and a distance. If the hand moves further than the distance the timer is cancelled.
* `MovementAction` allows you define a distance, direction and angle of movement. While the gesture is held a hand movement aligned to this will trigger the action. Moving outside the angle will cancel the action.
* `PassthroughAction` relays the input events directly to matching output events, with an optional distance value for filtering very small hand movements.

### Setting up a gesture in the scene

* Add a new `GameObject`
* Add the `HandGestureManager` component
* Set the `Gesture` parameter to a `Hand Gesture` asset of your choice
* Set the `Action` parameter to an asset of your choice

The gesture manager will configure the gesture and action on start and triger updates every frame. It needs to be spplied with hand data however as otherwise it will always update with default values.

#### Ultraleap Hand Data example

* Add the `LeapHandConnector` component to your game object with the `HandGesturemanager` attached.
* Follow the instructions at: [github.com/ultraleap/UnityPlugin](https://github.com/ultraleap/UnityPlugin) to install the Ultraleap plugin
* IMPORTANT: In `ProjectSettings/Player` add `ULTRALEAP` to the `Script Compilation > Scripting Define Symbols` section, to enable the connector.
* Add a `LeapServiceProvider` to the scene and assign it to the `LeapHandConnector` parameter.

The connector will now send hand data in the correct format to the `Gesture Manager`

## Output Events

Actions output the following events that can be subscribed to by a consumer:

* `Start` to indicate the gesture has been detected and the action is starting. This is accompanied by the position at which the gesture started
* `Hold` to indicate the gesture is being held and the action is progressing, but has not yet completed. This is accompanied by the current source position
* `End` to indicate the action has been completed successfully. This is accompanied by the position at the time it completed
* `Cancel` to indicate that the gesture was ended before the action completed.
