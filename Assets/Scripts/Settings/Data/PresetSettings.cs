using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[Serializable]
public class PresetSettings
{
    [Serializable]
    public class PresetSetting
    {
        public string name;
        public PresetSettings.Settings settings;
    }

    [Serializable]
    public class Settings
    {
        public float shadowDistance;
        public ShadowFilteringQuality shadowQuality;
        public int shadowResolution;
        public int shadowCascades;
        public bool ssaoEnabled;
        public int ssaoQuality;
        public bool ssgiEnabled;
        public int ssgiQuality;
        public bool volumetricLightingEnabled;
        public int volumetricLightingQuality;
        public bool bloomEnabled;
        public float bloomIntensity;
        public bool motionBlurEnabled;
        public float motionBlurIntensity;
        public bool depthOfFieldEnabled;
        public float depthOfFieldFocusDistance;
        public bool vignetteEnabled;
        public float vignetteIntensity;
        public int textureQuality;
        public float lodBias;
    }

    public List<PresetSetting> presets;
}
