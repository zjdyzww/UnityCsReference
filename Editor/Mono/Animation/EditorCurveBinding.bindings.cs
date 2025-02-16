// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using UnityEngine.Playables;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Internal;
using UnityEngine;

namespace UnityEditor
{
    [NativeType(CodegenOptions.Custom, "MonoEditorCurveBinding")]
    public struct EditorCurveBinding : IEquatable<EditorCurveBinding>
    {
        // The path of the game object / bone being animated.
        public string path;

        // The type of the component / material being animated.
        private Type   m_type;

        // The name of the property being animated.
        public string propertyName;

        // is it a PPtr curve
        private int   m_isPPtrCurve;

        //is it a discrete curve
        private int   m_isDiscreteCurve;

        //is it a serialize reference curve
        private int  m_isSerializeReferenceCurve;

        //is it placeholder curve
        private int   m_isPhantom;

        //is it a unknown curve: mean it needs to be processed further more to find out what is this curve
        // This is necessary to support old user code using FloatCurve for non float type
        private int  m_isUnknownCurve;

        // This is only used internally for deleting curves
        internal int  m_ClassID;
        internal int  m_ScriptInstanceID;

        public bool  isPPtrCurve { get { return m_isPPtrCurve != 0; } }
        public bool  isDiscreteCurve { get { return m_isDiscreteCurve != 0; } }

        public bool isSerializeReferenceCurve { get { return m_isSerializeReferenceCurve != 0; } }
        internal bool  isPhantom { get { return m_isPhantom != 0; } set { m_isPhantom = value == true ? 1 : 0; } }

        public static bool operator==(EditorCurveBinding lhs, EditorCurveBinding rhs)
        {
            // Only if classID actually has been setup do we compare it (It might only be type)
            if (lhs.m_ClassID != 0 && rhs.m_ClassID != 0)
            {
                if (lhs.m_ClassID != rhs.m_ClassID || lhs.m_ScriptInstanceID != rhs.m_ScriptInstanceID)
                    return false;
            }

            return lhs.m_isPPtrCurve == rhs.m_isPPtrCurve && lhs.m_isDiscreteCurve == rhs.m_isDiscreteCurve && lhs.m_isSerializeReferenceCurve == rhs.m_isSerializeReferenceCurve && lhs.path == rhs.path && lhs.type == rhs.type && lhs.propertyName == rhs.propertyName;
        }

        public static bool operator!=(EditorCurveBinding lhs, EditorCurveBinding rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return String.Format("{0}:{1}:{2}", path, type.Name, propertyName).GetHashCode();
        }

        public override bool Equals(object other)
        {
            return other is EditorCurveBinding && Equals((EditorCurveBinding)other);
        }

        public bool Equals(EditorCurveBinding other)
        {
            return this == other;
        }

        public Type type
        {
            get { return m_type; }
            set { m_type = value; m_ClassID = 0; m_ScriptInstanceID = 0; }
        }

        static private void BaseCurve(string inPath, System.Type inType, string inPropertyName, out EditorCurveBinding binding)
        {
            binding = new EditorCurveBinding();
            binding.path = inPath;
            binding.type = inType;
            binding.propertyName = inPropertyName;
            binding.m_isPhantom = 0;
        }

        static public EditorCurveBinding FloatCurve(string inPath, System.Type inType, string inPropertyName)
        {
            EditorCurveBinding binding;
            BaseCurve(inPath, inType, inPropertyName, out binding);
            binding.m_isPPtrCurve = 0;
            binding.m_isDiscreteCurve = 0;
            binding.m_isSerializeReferenceCurve = 0;
            binding.m_isUnknownCurve = 1;

            return binding;
        }

        static public EditorCurveBinding PPtrCurve(string inPath, System.Type inType, string inPropertyName)
        {
            EditorCurveBinding binding;
            BaseCurve(inPath, inType, inPropertyName, out binding);
            binding.m_isPPtrCurve = 1;
            binding.m_isDiscreteCurve = 1;
            binding.m_isSerializeReferenceCurve = 0;
            binding.m_isUnknownCurve = 0;
            return binding;
        }

        static public EditorCurveBinding DiscreteCurve(string inPath, System.Type inType, string inPropertyName)
        {
            EditorCurveBinding binding;
            BaseCurve(inPath, inType, inPropertyName, out binding);
            binding.m_isPPtrCurve = 0;
            binding.m_isDiscreteCurve = 1;
            binding.m_isSerializeReferenceCurve = 0;
            binding.m_isUnknownCurve = 0;

            if (!AnimationUtility.IsDiscreteIntBinding(binding))
            {
                Debug.LogWarning(
                    $"Property [" + inPropertyName + "] is not a supported discrete curve binding. " +
                    "Discrete curves only support [" + typeof(Enum) + "] and [" + typeof(int) + " with the `DiscreteEvaluation` attribute].");

                binding.m_isDiscreteCurve = 0;
                binding.m_isUnknownCurve = 1;
            }

            return binding;
        }

        static public EditorCurveBinding SerializeReferenceCurve(string inPath, System.Type inType, long refID, string inPropertyName, bool isPPtr, bool isDiscrete)
        {
            EditorCurveBinding binding;
            BaseCurve(inPath, inType, $"managedReferences[{refID}].{inPropertyName}", out binding);
            binding.m_isPPtrCurve = isPPtr ? 1 : 0;
            binding.m_isDiscreteCurve = isDiscrete || isPPtr ? 1 : 0;
            binding.m_isSerializeReferenceCurve = 1;
            binding.m_isUnknownCurve = 0;

            return binding;
        }
    }
}
