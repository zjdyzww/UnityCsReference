// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using JetBrains.Annotations;
using UnityEditor.Modules;
using UnityEngine;
using UnityEngine.Bindings;

namespace UnityEditor.Build.Profile
{
    internal sealed partial class BuildProfile
    {
        [UsedImplicitly]
        internal static event Action<BuildProfile> onBuildProfileEnable;
        [VisibleToOtherModules]
        internal static void AddOnBuildProfileEnable(Action<BuildProfile> action) => onBuildProfileEnable += action;
        [VisibleToOtherModules]
        internal static void RemoveOnBuildProfileEnable(Action<BuildProfile> action) => onBuildProfileEnable -= action;

        internal static BuildProfile CreateInstance(BuildTarget buildTarget, StandaloneBuildSubtarget subtarget)
        {
            string moduleName = ModuleManager.GetTargetStringFrom(buildTarget);
            var buildProfile = CreateInstance<BuildProfile>();
            buildProfile.buildTarget = buildTarget;
            buildProfile.subtarget = subtarget;
            buildProfile.moduleName = moduleName;
            buildProfile.OnEnable();
            return buildProfile;
        }

        /// <summary>
        /// Internal helper function for creating new build profile assets and invoking the onBuildProfileCreated
        /// event after an asset is created by AssetDatabase.CreateAsset.
        /// </summary>
        [VisibleToOtherModules("UnityEditor.BuildProfileModule")]
        internal static void CreateInstance(string moduleName, StandaloneBuildSubtarget subtarget, string assetPath)
        {
            var buildProfile = CreateInstance<BuildProfile>();
            buildProfile.buildTarget = BuildProfileModuleUtil.GetBuildTarget(moduleName);
            buildProfile.subtarget = subtarget;
            buildProfile.moduleName = moduleName;
            AssetDatabase.CreateAsset(
                buildProfile,
                AssetDatabase.GenerateUniqueAssetPath(assetPath));
            buildProfile.OnEnable();
        }

        void TryCreatePlatformSettings()
        {
            if (platformBuildProfile != null)
            {
                Debug.LogError("[BuildProfile] Platform settings already created.");
                return;
            }

            IBuildProfileExtension buildProfileExtension = ModuleManager.GetBuildProfileExtension(moduleName);
            if (buildProfileExtension != null && ModuleManager.IsPlatformSupportLoaded(moduleName))
            {
                platformBuildProfile = buildProfileExtension.CreateBuildProfilePlatformSettings();
                EditorUtility.SetDirty(this);
            }
        }
    }
}
