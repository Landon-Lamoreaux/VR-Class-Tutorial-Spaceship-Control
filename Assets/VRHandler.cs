using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using static InputMapper;

public class VRHandler : MonoBehaviour
{
    private InputMapper filter;
    private List<InputDevice> controllers = new List<InputDevice>();
    private Dictionary<string, bool> buttonStateRight = new Dictionary<string, bool>();
    private Dictionary<string, bool> buttonStateLeft = new Dictionary<string, bool>();

    // Start is called before the first frame update.
    void Start()
    {

    }

    private void Awake()
    {
        filter = GetComponent<InputMapper>();

        // Get all button codes, these are not in an enum, so they have to be done by hand.
        AddInputFeature(CommonUsages.grip.name);
        AddInputFeature(CommonUsages.gripButton.name);
        AddInputFeature(CommonUsages.menuButton.name);
        AddInputFeature(CommonUsages.primary2DAxis.name);
        AddInputFeature(CommonUsages.primary2DAxisClick.name);
        AddInputFeature(CommonUsages.primary2DAxisTouch.name);
        AddInputFeature(CommonUsages.primaryButton.name);
        AddInputFeature(CommonUsages.primaryTouch.name);
        AddInputFeature(CommonUsages.secondary2DAxis.name);
        AddInputFeature(CommonUsages.secondary2DAxisClick.name);
        AddInputFeature(CommonUsages.secondary2DAxisTouch.name);
        AddInputFeature(CommonUsages.secondaryButton.name);
        AddInputFeature(CommonUsages.secondaryTouch.name);
        AddInputFeature(CommonUsages.trigger.name);
        AddInputFeature(CommonUsages.triggerButton.name);

        // Register callback to update if controller lost\added.
        InputDevices.deviceConnected += deviceRecheck;
        InputDevices.deviceDisconnected += deviceRecheck;

        filter.registerMap("down: " + CommonUsages.primary2DAxisClick.name, Actions.TELEPORT);
        filter.registerMap("held: " + CommonUsages.triggerButton.name, Actions.TOGGLE);

    }
    void deviceRecheck(InputDevice device)
    {
        // Just refresh...slow but easy.
        InputDeviceCharacteristics characteristics = InputDeviceCharacteristics.HeldInHand |
        InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(characteristics, controllers);
    }
    void AddInputFeature(String usage)
    {
        buttonStateLeft.Add(usage, false);
        buttonStateRight.Add(usage, false);
    }

    private void checkButtons(Modifier v, InputDevice device)
    {
        // Find features for this device.
        List<InputFeatureUsage> supportedFeatures = new List<InputFeatureUsage>();
        device.TryGetFeatureUsages(supportedFeatures);

        // Switch to right hand if needed.
        Dictionary<string, bool> pastControllerState = buttonStateLeft;
        if (v == Modifier.RIGHT)
        {
            pastControllerState = buttonStateRight;
        }

        // Poll buttons.
        foreach (InputFeatureUsage feature in supportedFeatures)
        {
            // Check for normal keys-like events.
            if (feature.type == typeof(bool))
            {
                // Sheck state of button.
                bool state;
                bool success = device.TryGetFeatureValue(feature.As<bool>(), out state);
                if (success)
                {
                    // Safety check for unchecked items.
                    if (!pastControllerState.ContainsKey(feature.name))
                    {
                        continue;
                    }
                    // OnDown.
                    if (!pastControllerState[feature.name]) // Was the button up on the last frame?
                    {
                        if (state) // Is the button down on this frame?
                        {
                            filter.OnDown(feature.name, v);
                        }
                    }
                    // OnUp OR isDown.
                    else if (pastControllerState[feature.name]) // Was the button down on the last frame?
                    {
                        // OnUp.
                        if (!state) // Is the buttonup on this frame?
                        {
                            filter.OnUp(feature.name, v);
                        }
                        // IsDown.
                        else
                        {
                            filter.isDown(feature.name, v);
                        }
                    }
                    pastControllerState[feature.name] = state; // Update its state.
                }
            }

            //check for normal keys-like events
            if (feature.type == typeof(bool))
            {
                //...prior button code
            }
            else if (feature.type == typeof(Vector2))
            {
                //check state of button
                Vector2 state;
                bool success = device.TryGetFeatureValue(feature.As<Vector2>(), out state);
                if (success)
                {
                    filter.OnMove(feature.name, new Vector3(state.x, state.y, 0), v);
                }
            }
        }
    }


    // Update is called once per frame.
    void Update()
    {
        filter.startFrame();
        foreach (InputDevice device in controllers)
        {
            //check buttons on left or right controller
            if (device.characteristics.HasFlag(InputDeviceCharacteristics.Left))
            {
                checkButtons(Modifier.LEFT, device);
            }
            else
            {
                checkButtons(Modifier.RIGHT, device);
            }
        }
    }
}
