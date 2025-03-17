# Cacophony Gesture System

A data agnostic gesture detection system for Unity. Designed for extensibility and speed when prototyping new ideas.

## Features

### Scriptable Assets
These assets can be created programaticaly or in the editor.

* `Poses` defined by values for a set of input data.
* `Gestures` define by a collection of positive and negative poses, and outputting a set of events.
* `Actions` are triggered by actions plus additional constraints or movements, outputting their own set of events.

## Getting Started

`Gesture Managers` bring together an action and a gesture, provide them with data and control updates. These are the basic entry point to the system.

`Consumers` hook into the action events that are routed via the `Gesture Manager`.

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
