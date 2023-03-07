using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputFormatter;
using static InputMapper;

public class InputMapper : MonoBehaviour
{
    public enum Actions { TELEPORT, TOGGLE, PUSH_LEFT, PUSH_RIGHT, GRAB, RELEASE };
    public enum Modifier { ANY, LEFT, RIGHT };

    public delegate void VoidEventHandler();
    public delegate void DataEventHandler(object vals);

    private List<EventCombo> mapping = new List<EventCombo>();
    //private List<Tuple<InputFormatter[], Actions>> mapping = new List<Tuple<InputFormatter[], Actions>>();

    //private List<Tuple<string, Actions>> mapping = new List<Tuple<string, Actions>>();
    private List<Tuple<Actions, Delegate>> actions = new List<Tuple<Actions, Delegate>>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void checkForEvents(InputFormatter inputEvent, object state = null)
    {
        // Part 1: update all mappings.
        foreach (EventCombo combo in mapping)
        {
            for (int i = 0; i < combo.format.Length; i++)
            {
                InputFormatter c = combo.format[i];
                
                // Found a match.
                if ((inputEvent.name.ToLower() == c.name.ToLower() && inputEvent.modifier == c.modifier)
                || (inputEvent.name.ToLower() == c.name.ToLower() && c.modifier == Modifier.ANY))
                {
                    combo.met[i] = true;
                    combo.state[i] = state;
                }
            }
        }
        // Part 2: check if any mappings completed.
        foreach (EventCombo combo in mapping)
        {
            // Will be true only if all met valeus are true.
            bool complete = true;
            for (int i = 0; i < combo.met.Length; i++)
            {
                complete = complete && combo.met[i];
            }
            if (complete)
            {
                // Part 3: original check to find all objects that want to know about this event.
                foreach (Tuple<Actions, Delegate> actions in actions)
                {
                    if (combo.action == actions.Item1)
                    {
                        if (combo.sendLastAction)
                        {
                            object info = combo.state[combo.state.Length - 1];
                            actions.Item2.DynamicInvoke(info);
                        }
                        else
                        {
                            actions.Item2.DynamicInvoke();
                        }
                    }
                }
            }
        }
    }

    private void checkForEvents(string name, Modifier m)
    {
        checkForEvents(new InputFormatter(name, m));
    }

    public void OnDown(string buttonName, Modifier m = Modifier.ANY)
    {
        Debug.Log("down: " + buttonName);
        checkForEvents("down: " + buttonName, m);
    }
    public void OnUp(string buttonName, Modifier m = Modifier.ANY)
    {
        Debug.Log("up: " + buttonName);
        checkForEvents("up: " + buttonName, m);
    }
    public void isDown(string buttonName, Modifier m = Modifier.ANY)
    {
        Debug.Log("held: " + buttonName);
        checkForEvents("held: " + buttonName, m);
    }
    public void OnMove(string buttonName, Vector3 axis, Modifier m = Modifier.ANY)
    {
        Debug.Log("move: " + axis);
        checkForEvents(new InputFormatter("move: " + buttonName, m), axis);
    }

    public void register(Actions a, Delegate func)
    {
        actions.Add(new Tuple<Actions, Delegate>(a, func));
    }
    public void deregister(Delegate func)
    {
        Tuple<Actions, Delegate> t = null;
        foreach (Tuple<Actions, Delegate> map in actions)
        {
            if (map.Item2 == func)
                t = map;
        }
        if (t != null)
            actions.Remove(t);
    }
    public void registerMap(string inputName, Actions a)
    {
        EventCombo combo = new EventCombo(
        new InputFormatter[] { new InputFormatter(inputName) },
        a);
        mapping.Add(combo);
    }
    public void registerMap(InputFormatter[] formats, Actions a, bool sendLast = false)
    {
        EventCombo combo = new EventCombo(formats, a, sendLast);
        mapping.Add(combo);
    }

    public void startFrame()
    {
        foreach (EventCombo c in mapping)
        {
            for (int i = 0; i < c.met.Length; i++)
            {
                c.met[i] = false;
            }
        }
    }

}
