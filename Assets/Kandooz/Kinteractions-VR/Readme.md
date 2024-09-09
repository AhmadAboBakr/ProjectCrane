# Getting Started Guide
## Project Setup
### Add OpenXR package
* Go to `Window-> Package manager`
* Install The OpenXR package by following the steps in the picture below
![InstallOpenXR.png](Documentation%2FInstallOpenXR.png) 
### Configure OpenXR package

* Go To `Edit-> Project Settings`
* Enable the `OpenXR` Plugin, or any other plugin if you prefer using other plugins
* Go the the settings for the openXR plugin
* Choose the controllers you want to support
![CongigureOpenXR.png](Documentation%2FCongigureOpenXR.png)

### Initializing the scene
From the menuBar goto `Kandooz->Initialize Scene`
you can instead drag the cameraRig prefab to the scene from the K-interactions folder the scene.
This will delete the Camera in the Scene and add an Camera-Rig Game object instead.

## Configuring the asset
### Main config files
the config file of the asset can be found in `kinteraction system->Data` folder
![config.png](Documentation%2Fconfig.png)
The config file contains the following properties

| Property                 | Description                                                                                                                                                                                     |
|--------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Hand Data**            | This is a reference to the scriptable object containing all the poses and the prefabs associated with the hand to be used                                                                       |
| **Left Hand Layer**      | The layer the camera rig should use to create the hand, </br> This should be generated automatically, only change if you want to use a custom layer for the left hand                           |
| **Right Hand Layer**     | The layer the camera rig should use to create the hand, </br> This should be generated automatically, only change if you want to use a custom layer for the right hand                          |
| **Interactables Layer**  | This layer will be the default layer when making an interactable object.</br>This should be generated automatically, only change if you want to use a custom layer for the interactable objects |
| **Player Layer**         | The layer the camera rig should use to create the player Gameobject.</br> This should be generated automatically, only change if you want to use a custom layer for the Player                  |
| **Input Method**         | It controls how input for the asset will be handled, can either be Input system, Input manager or Keyboard debug                                                                                |
| **Hand Mass**            | The default mass of the hand the camera rig will create                                                                                                                                         |
| **Linear Damping**       | the default drag of the hand the camera rig will create                                                                                                                                         |
| **angular Damping**      | The default angular drag of the hands the camera rig will create                                                                                                                                |

## Creating Interactable Objects
There are multiple types of Interactables you can use:
- **Grabable** : Grabable objects can be grabbed by the player but when the player stops grabbing they hover on space.
- **Throwable** : Throwables are just like grabables but can be tossed around with physics. 
- **Button** : can be pressed with finger tips, or any colliders in the hand.
- **Linerarly Constrained** :  Can be grabbed to only move in one direction between two points.
- **Lever** : Can only be rotated in one direction.
- **Joystick** : Can be rotated around two axis.
- **Wheel** : rotates indefenitly around one axis.
### Grabables
To make an object grabable you can either right click on the object then choose `Kandooz/Make Grabable` or you add the `Grabable` component to it
When you add the component it will also add another component called Interaction Pose Constrainter
#### Grabable component
this component inherts from `Interactable Base` which means that like every other interactable type it has the follwoing:
![Grabable.png](Documentation%2FGrabable.png)
- **Interaction Hand**: Defines which hand can interact with this object it can be Left, Right or Both
- **Selection Button**: Defines which button will grab the object, it can either be Trigger or Grip
- **Unity Events**: 
  - **On Selected**: is called when the object is grabbed 
  - **OnDeslected**: is called when the user leaves the object
  - **OnHoverStart**: is called when the hand starts hovering over the object
  - **OnHoverEnd**: is caleed when the hand stops hovering over the object
  - **On Activated**: when the player presses the other button while grabbing the object

#### Interactable Pose Constraier component
This component constraints the hand pose while it's being interacted with.
you can edit the shape of the hands while they interact with an object
![PoseConstrainter.gif](Documentation%2FPoseConstrainter.gif)

### Throwables
a throwable have the same components as the grabble with one extra component added to it called `throwable`
this component try to interpolate the velocity of the object based on the hand movement so that when it is left it will move in the direction the hand was moving
it have only one property which is `iterations`, the higher this value the better the interpolation of the throw but the more processing it requires.

### Buttons
to create a button you can either right click on the hierarchy then go to `Kandooz-> Make a button`.
or add the `Button` Component
![Button.png](Documentation%2FButton.png)
the button components requires that the game object also have a trigger collider that will trigger the button when anything enters it.
the Button have the following properties
- OnClick : a unity event that will get called when the button is pressed
- Button refference: a refference to the gameobject that will move when the button is pressed
- Normal Position: the local position of the button Object when not pressed
- Pressed Position: the local Posiion of the button object when pressed
- press speed: how fast the button will move when pressed or unpressed
### lever

### joystick
### Wheel
# Adding a new Hand
To add a hand you first need to find a rigged hand model with at least two poses ne where the hand is making a fist and the other is a relaxed pose as per the screen shot below.

![Poses.png](Documentation%2FPoses.png)


Create The Hand Pose Prefabs
- Create an empty Game Object
- Add the `HandPose Controller` Component and the `Hand` component to it
- Place the hand model inside of it
- Set it to either be left or right in the hand component
- Create a prefab for the Hand
- Flip the Hand Model and Create a prefab variant for the second Hand


Then you create a Hand Data asset:
 - Go to `Create Asset -> Kandooz -> Interaction System -> Hand Data`
 - Drag the left hand right hand prefabs you just create into the asset properties 
 - Create Avatar Masks for Each finger and add drag them to the correct fields
 - Add two poses representing the hand fully closed and fully open
![HandData.png](Documentation%2FHandData.png)

   