using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static InputMapper;
public class KeyboardMouseHandler : MonoBehaviour
{
    private InputMapper filter;
    private List<KeyCode> held = new List<KeyCode>();
    private Dictionary<KeyCode, bool> keyState = new Dictionary<KeyCode, bool>();
    void Start()
    {
        filter = GetComponent<InputMapper>();

        // Get all key codes.
        KeyCode[] codes = (KeyCode[])System.Enum.GetValues(typeof(KeyCode));
        foreach (KeyCode code in codes)
        {
            if (!keyState.ContainsKey(code))
                keyState.Add(code, false);
        }

        filter.registerMap("down: " + KeyCode.Space, Actions.TELEPORT);
        filter.registerMap("held: " + KeyCode.Z, Actions.TOGGLE);

        InputFormatter[] formatter = new InputFormatter[] {
            new InputFormatter("held: Mouse0"),
            new InputFormatter("move: Mouse") };
        filter.registerMap(formatter, Actions.PUSH_LEFT, true);
        formatter = new InputFormatter[] {
            new InputFormatter("held: Mouse1"),
            new InputFormatter("move: Mouse") };
        filter.registerMap(formatter, Actions.PUSH_RIGHT, true);

        filter.registerMap("down: " + KeyCode.G, Actions.GRAB);
        filter.registerMap("up: " + KeyCode.G, Actions.RELEASE);

    }

    private void Update()
    {
        filter.startFrame();

        // Check keys.
        KeyCode[] codes = (KeyCode[])System.Enum.GetValues(typeof(KeyCode));
        foreach (KeyCode code in codes)
        {
            if (Input.GetKeyDown(code))
            {
                filter.OnDown(code.ToString());
                held.Add(code);
            }
            else if (Input.GetKeyUp(code))
            {
                filter.OnUp(code.ToString());
                held.Remove(code);
            }
            else
            {
                if (held.Contains(code))
                {
                    filter.isDown(code.ToString());
                }
            }
        }

        // Check mice.
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        Vector3 mouseMove = new Vector3(x, y, 0);

        // Mouse moved.
        if (mouseMove.magnitude > 0.1)
        {
            filter.OnMove("mouse", mouseMove);
        }

    }

    }