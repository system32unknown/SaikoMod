namespace SaikoMod.Core.Components {
    public class CustomDynamicObj : UnityEngine.MonoBehaviour {
        public System.Action action;
        public void UseObject() => action();
    }
}