using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static InputMapper;

public struct InputFormatter
{
    public struct EventCombo
    {
        public InputFormatter[] format;
        public bool[] met;
        public Actions action;
        public bool sendLastAction;
        public object[] state;

        public EventCombo(InputFormatter[] format, Actions action, bool sendLast = false)
        {
            this.format = format;
            this.action = action;
            this.met = new bool[format.Length];
            sendLastAction = sendLast;
            this.state = new object[format.Length];
        }
    }

    public string name;
    public InputMapper.Modifier modifier;
    public InputFormatter(string name)
    {
        this.name = name;
        modifier = InputMapper.Modifier.ANY;
    }
    public InputFormatter(string name, InputMapper.Modifier m)
    {
        this.name = name;
        modifier = m;
    }
}