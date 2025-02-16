// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using System.Runtime.InteropServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEditor
{
    [NativeClass(null)]
    [NativeHeader("Runtime/BaseClasses/TagManager.h")]
    internal sealed class TagManager : ProjectSettingsBase
    {
        private TagManager() {}

        [StaticAccessor("GetTagManager()", StaticAccessorType.Dot)]
        internal static extern int GetDefinedLayerCount();

        public extern string[] tags { [NativeMethod("GetTagNames")] get; }

        [NativeMethod]
        internal extern void AddSortingLayer();

        [NativeMethod]
        internal extern int GetSortingLayerCount();

        [NativeMethod]
        internal extern void UpdateSortingLayersOrder();

        [NativeMethod]
        internal extern bool IsSortingLayerDefault(int index);

        [NativeMethod]
        internal extern string GetSortingLayerName(int index);

        [NativeMethod]
        internal extern void SetSortingLayerName(int index, string name);

        [NativeMethod("StringToTagAddIfUnavailable")]
        extern int StringToTagAddIfUnavailable(string tag);

        /*
        * After adding a tag, set the TagManager as dirty. We may not call "SetDirty" in "StringToTagAddIfUnavailable" itself because
        * it may be called off of the main thread.
        */
        internal int AddTag(string tag)
        {
            int result = StringToTagAddIfUnavailable(tag);
            EditorUtility.SetDirty(this);

            return result;
        }

        [NativeMethod]
        internal extern void RemoveTag(string tag);

        internal static void GetDefinedLayers(ref string[] layerNames, ref int[] layerValues)
        {
            var definedLayerCount = GetDefinedLayerCount();

            if (layerNames == null)
                layerNames = new string[definedLayerCount];
            else if (layerNames.Length != definedLayerCount)
                Array.Resize(ref layerNames, definedLayerCount);

            if (layerValues == null)
                layerValues = new int[definedLayerCount];
            else if (layerValues.Length != definedLayerCount)
                Array.Resize(ref layerValues, definedLayerCount);

            Internal_GetDefinedLayers(layerNames, layerValues);
        }

        [FreeFunction("GetTagManager().GetDefinedLayers")]
        static extern void Internal_GetDefinedLayers([Out] string[] layerNames, [Out] int[] layerValues);

        [NativeMethod]
        internal extern bool TrySetRenderingLayerName(int index, string name);

        [NativeMethod]
        internal extern bool IsIndexReservedForDefaultRenderingLayer(int index);

        [StaticAccessor("GetTagManager()", StaticAccessorType.Dot)]
        internal static extern int GetDefinedRenderingLayerCount();

        [StaticAccessor("GetTagManager()", StaticAccessorType.Dot)]
        internal static extern int GetLastDefinedRenderingLayerIndex();

        internal static void GetDefinedRenderingLayers(out string[] renderingLayerNames, out int[] renderingLayerValues)
        {
            var definedLayerCount = GetDefinedRenderingLayerCount();

            renderingLayerNames = new string[definedLayerCount];
            renderingLayerValues = new int[definedLayerCount];

            Internal_GetDefinedRenderingLayers(renderingLayerNames, renderingLayerValues);
        }

        [FreeFunction("GetTagManager().GetDefinedRenderingLayers")]
        static extern void Internal_GetDefinedRenderingLayers([Out] string[] layerNames, [Out] int[] layerValues);

        [NativeMethod]
        internal extern int StringToRenderingLayer(string name);

        [NativeMethod]
        internal extern string RenderingLayerToString(int index);
    }
}
