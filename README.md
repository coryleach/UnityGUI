<h1 align="center">Welcome to com.gameframe.gui 👋</h1>
<p>
  <img alt="Version" src="https://img.shields.io/badge/version-1.0.21-blue.svg?cacheSeconds=2592000" />
  <a href="https://twitter.com/coryleach">
    <img alt="Twitter: coryleach" src="https://img.shields.io/twitter/follow/coryleach.svg?style=social" target="_blank" />
  </a>
</p>

> This is a library of GUI helpers for UGUI </br>
> Includes a panel system that implements a navigation stack. </br>
> Includes a scene transition system. </br>
> Includes a shader for blurring the background of UI panels. </br>

## Quick Package Install

#### Using UnityPackageManager (for Unity 2019.1 or later)

Find the manifest.json file in the Packages folder of your project and edit it to look like this:
```js
{
  "dependencies": {
    "com.gameframe.gui": "https://github.com/coryleach/UnityGUI.git#1.0.21",
    ...
  },
}
```

## Usage

### PanelView
PanelView which provides Show & Hide behavior which can be instant or async and awaitable.

### AnimatedPanelView
References one or more IPanelAnimator components to control animate the Show & Hide of a panel. If you await the Show & Hide async methods

### PanelViewController
Controller for the display of a PanelView.

### PanelViewControllerBehaviour
A version of PanelViewController that is also a MonoBehaviour component that can be added to a game object.
Often you may want to set up & configure PanelViewControllers with data other than the panel type and using this class will allow you to do that in editor.

### PanelStackSystem
A scriptable object representation of a panel stack. Main purpose is in maintaining the navigation history of a menu. Push and Pop panel controllers onto this stack. If any PanelViewStackController component that subscribes to the PanelStackSystem will respond to the push and show/hide the corresponding panels. Generally the top PanelViewController on the stack is always visible.

### PanelViewStackController
A MonoBehaviour that references a PanelStackSystem. It responds to changes in the panel stack by getting the current showing and hiding controllers. It will then perform the necessary transitions.

### PanelViewControllerProvider
A scriptable object which services is a collection of PanelViewControllers. Generally you register PanelViewControllerBehaviour with a provider so that you can push them using the PanelPusher component. It also controls the location that views are parented to in the hierarchy.

### PanelViewControllerRegisterer
Add this component to a PanelViewControllerBehaviour to register the controller with a PanelViewControllerProvider instance.

### PanelType
Contains information about a panel and is used by the PanelViewController to locate the prefab and instantiate the PanelView.

## Author

👤 **Cory Leach**

* Twitter: [@coryleach](https://twitter.com/coryleach)
* Github: [@coryleach](https://github.com/coryleach)

## Show your support

Give a ⭐️ if this project helped you!

***
_This README was generated with ❤️ by [readme-md-generator](https://github.com/kefranabg/readme-md-generator)_
