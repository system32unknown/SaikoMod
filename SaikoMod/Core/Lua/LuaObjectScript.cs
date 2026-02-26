using System;
using System.IO;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using UnityEngine;

namespace SaikoMod.Core.Lua {
    /// <summary>
    /// Attachable component: loads and runs a Lua file for a spawned object and calls hooks.
    /// Lua hooks (optional):
    ///   function onSpawn(self) end
    ///   function onUpdate(self, dt) end
    ///   function onAction(self) end
    ///   function onDestroy(self) end
    /// </summary>
    public sealed class LuaObjectScript : MonoBehaviour {
        [Serializable]
        public sealed class LuaSelf {
            public GameObject gameObject;
            public Transform transform;

            public void Log(string msg) => Debug.Log("[LuaObj] " + msg);
            public void Warn(string msg) => Debug.LogWarning("[LuaObj] " + msg);
            public void Error(string msg) => Debug.LogError("[LuaObj] " + msg);

            public void Destroy() {
                if (gameObject != null)
                    UnityEngine.Object.Destroy(gameObject);
            }
        }

        Script _script;
        Table _env;
        LuaSelf _self;

        DynValue _fnSpawn;
        DynValue _fnUpdate;
        DynValue _fnAction;
        DynValue _fnDestroy;

        public string LastError { get; private set; }

        /// <summary>Initialize and run a Lua script from a file path.</summary>
        public bool InitFromFile(string luaFilePath) {
            LastError = null;

            if (string.IsNullOrEmpty(luaFilePath) || !File.Exists(luaFilePath))
                return false;

            UserData.RegisterAssembly();

            _script = new Script(CoreModules.Preset_Complete ^ CoreModules.IO ^ CoreModules.OS_System);
            _script.Options.ScriptLoader = new FileSystemScriptLoader();
            _script.Options.DebugPrint = s => Debug.Log(s);
            _script.Globals["print"] = (Action<DynValue>)CustomPrint;
            _script.Options.UseLuaErrorLocations = true;

            // Build a per-instance environment to avoid global collisions
            _env = new Table(_script);

            Table mt = new Table(_script);
            mt["__index"] = _script.Globals;
            _env.MetaTable = mt;

            RegisterTypes();

            // self userdata
            _self = new LuaSelf {
                gameObject = gameObject,
                transform = transform
            };
            _env["self"] = UserData.Create(_self);

            try {
                // Load + execute script into environment
                string code = File.ReadAllText(luaFilePath);
                DynValue chunk = _script.LoadString(code, _env, Path.GetFileName(luaFilePath));
                _script.Call(chunk);

                // Cache hook functions (optional)
                _fnSpawn = _env.Get("onCreate");
                _fnUpdate = _env.Get("onUpdate");
                _fnAction = _env.Get("onAction");
                _fnDestroy = _env.Get("onDestroy");

                // Call spawn hook
                Call(_fnSpawn);

                return true;
            } catch (SyntaxErrorException e) {
                LastError = "Lua syntax error:\n" + e.DecoratedMessage;
                Debug.LogError(LastError);
                return false;
            } catch (ScriptRuntimeException e) {
                LastError = "Lua runtime error:\n" + e.DecoratedMessage;
                Debug.LogError(LastError);
                return false;
            } catch (Exception e) {
                LastError = "Lua init error: " + e;
                Debug.LogError(LastError);
                return false;
            }
        }

        void RegisterTypes() {
            // Register userdata types
            UserData.RegisterType<LuaSelf>();
            UserData.RegisterType<GameObject>();
            UserData.RegisterType<Transform>();
            UserData.RegisterType<UnityEngine.AI.NavMeshAgent>();

            UserData.RegisterType<Vector3>();
            _script.Globals["Vector3"] = (Func<float, float, float, Vector3>)((x, y, z) => new Vector3(x, y, z));

            UserData.RegisterType<Quaternion>();
            _script.Globals["Quaternion"] = new Table(_script);
            _script.Globals.Get("Quaternion").Table["identity"] = Quaternion.identity;
            _script.Globals.Get("Quaternion").Table["Euler"] = (Func<float, float, float, Quaternion>)((x, y, z) => Quaternion.Euler(x, y, z));
        }

        public void CallAction() {
            Call(_fnAction);
        }
        public bool HasFunction(string name) {
            return name != null && _script.Globals[name] != null;
        }

        public void RegisterType<T>() {
            UserData.RegisterType<T>();
        }

        public void SetGlobal(string key, Action act) {
            _script.Globals[key] = act;
        }
        public void SetGlobal(string key, object obj) {
            _script.Globals[key] = obj;
        }

        void Update() {
            if (_script == null) return;
            if (_fnUpdate == null || _fnUpdate.Type != DataType.Function) return;

            try {
                _script.Call(_fnUpdate, _env["self"], DynValue.NewNumber(Time.deltaTime));
            } catch (ScriptRuntimeException e) {
                // Don’t spam logs every frame: disable updates on error.
                Debug.LogError("Lua update error:\n" + e.DecoratedMessage);
                _fnUpdate = DynValue.Nil;
            }
        }

        void OnDestroy() {
            Call(_fnDestroy);
            _script = null;
            _env = null;
            _self = null;
        }

        void Call(DynValue fn) {
            if (_script == null) return;
            if (fn == null || fn.Type != DataType.Function) return;

            try {
                _script.Call(fn, _env["self"]);
            } catch (ScriptRuntimeException e) {
                Debug.LogError("Lua hook error:\n" + e.DecoratedMessage);
            }
        }

        void CustomPrint(DynValue value) {
            Debug.Log(value.ToPrintString());
        }
    }
}