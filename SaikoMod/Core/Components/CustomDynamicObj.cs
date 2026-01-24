using UnityEngine;
using System;

namespace SaikoMod.Core.Components {
    public class CustomDynamicObj : MonoBehaviour {
        public Action action;
        public void UseObject() => action();
    }
}