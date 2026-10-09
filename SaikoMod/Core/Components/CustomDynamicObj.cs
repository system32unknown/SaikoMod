using Action = System.Action;
using UnityEngine;

namespace SaikoMod.Core.Components {
    public class CustomDynamicObj : MonoBehaviour {
        public Action action;
        public void UseObject() => action();
    }
}