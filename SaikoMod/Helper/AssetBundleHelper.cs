using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaikoMod.Helper
{
    public static class AssetBundleHelper
    {
        /// <summary>
        /// Cached bundles so repeated loads do NOT reload from disk.
        /// Key = full filepath
        /// </summary>
        static readonly Dictionary<string, AssetBundle> bundleCache = new Dictionary<string, AssetBundle>();

        /// <summary>
        /// Loads an AssetBundle from file, using cache if available.
        /// </summary>
        public static AssetBundle LoadBundle(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("[AssetBundleHelper] LoadBundle failed: path is null.");
                return null;
            }

            if (bundleCache.TryGetValue(path, out AssetBundle cached))
            {
                if (cached != null) return cached;
                bundleCache.Remove(path);
            }

            if (!System.IO.File.Exists(path))
            {
                Debug.LogError("[AssetBundleHelper] File not found: " + path);
                return null;
            }

            AssetBundle bundle = AssetBundle.LoadFromFile(path);

            if (bundle == null)
            {
                Debug.LogError("[AssetBundleHelper] Failed to load AssetBundle: " + path);
                return null;
            }

            bundleCache[path] = bundle;
            return bundle;
        }

        /// <summary>
        /// Loads an AssetBundle from a byte[] array in memory.
        /// CacheKey is used to uniquely identify the memory bundle.
        /// </summary>
        public static AssetBundle LoadFromMemory(byte[] data, string cacheKey)
        {
            if (data == null || data.Length == 0)
            {
                Debug.LogError("[AssetBundleHelper] LoadFromMemory failed: data is null or empty.");
                return null;
            }

            if (string.IsNullOrEmpty(cacheKey))
                cacheKey = "memory_bundle_" + data.GetHashCode();

            if (bundleCache.TryGetValue(cacheKey, out var cached) && cached != null)
                return cached;

            AssetBundle bundle = AssetBundle.LoadFromMemory(data);
            if (bundle == null)  {
                Debug.LogError("[AssetBundleHelper] LoadFromMemory failed: invalid memory data.");
                return null;
            }

            bundleCache[cacheKey] = bundle;
            return bundle;
        }

        /// <summary>
        /// Loads a typed asset from a bundle.
        /// Example: LoadAsset<Texture2D>(bundle, "myTexture");
        /// </summary>
        public static T LoadAsset<T>(AssetBundle bundle, string assetName) where T : UnityEngine.Object
        {
            if (bundle == null)
            {
                Debug.LogError("[AssetBundleHelper] LoadAsset failed: bundle is null.");
                return null;
            }

            if (string.IsNullOrEmpty(assetName))
            {
                Debug.LogError("[AssetBundleHelper] LoadAsset failed: assetName is null.");
                return null;
            }

            T asset = bundle.LoadAsset<T>(assetName);
            if (asset == null) Debug.LogError($"[AssetBundleHelper] Asset '{assetName}' not found in bundle.");

            return asset;
        }

        /// <summary>
        /// Loads all assets of a given type.
        /// Example: LoadAllAssets<Texture2D>(bundle)
        /// </summary>
        public static T[] LoadAllAssets<T>(AssetBundle bundle) where T : UnityEngine.Object
        {
            if (bundle == null)
            {
                Debug.LogError("[AssetBundleHelper] LoadAllAssets failed: bundle is null.");
                return Array.Empty<T>();
            }

            return bundle.LoadAllAssets<T>();
        }

        /// <summary>
        /// Loads a prefab and instantiates it properly.
        /// </summary>
        public static GameObject InstantiatePrefab(AssetBundle bundle, string prefabName, Vector3? pos = null, Quaternion? rot = null)
        {
            GameObject prefab = LoadAsset<GameObject>(bundle, prefabName);

            if (prefab == null) return null;

            GameObject instance = UnityEngine.Object.Instantiate(prefab,
                pos ?? Vector3.zero,
                rot ?? Quaternion.identity
            );

            instance.name = prefab.name; // remove "(Clone)" if desired
            return instance;
        }

        /// <summary>
        /// Checks if a bundle contains an asset name.
        /// </summary>
        public static bool Contains(AssetBundle bundle, string name)
        {
            if (bundle == null) return false;
            foreach (string n in bundle.GetAllAssetNames())
                if (n.EndsWith(name, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        /// <summary>
        /// Unload a specific loaded bundle.
        /// </summary>
        public static void UnloadBundle(string path, bool unloadAllObjects = false)
        {
            if (bundleCache.TryGetValue(path, out AssetBundle bundle) && bundle != null)
            {
                bundle.Unload(unloadAllObjects);
            }

            bundleCache.Remove(path);
        }

        /// <summary>
        /// Unload ALL loaded bundles.
        /// </summary>
        public static void UnloadAll(bool unloadAllObjects = false)
        {
            foreach (KeyValuePair<string, AssetBundle> kv in bundleCache)
            {
                kv.Value?.Unload(unloadAllObjects);
            }

            bundleCache.Clear();
        }
    }
}
