using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class SettingsApplier : MonoBehaviour
{
    public void ApplySettings(SettingsData settingsData, HDRenderPipelineAsset hdrpAsset)
    {
        // General Settings - Apply directly in their respective scripts.

        // Shadow Settings
        hdrpAsset.shadowInitParams.maxShadowDistance = settingsData.shadowDistance;
        hdrpAsset.shadowFilteringQuality = settingsData.shadowQuality;
        hdrpAsset.shadowInitParams.shadowCascadeCount = settingsData.shadowCascades;

        foreach (var shadowAtlas in hdrpAsset.shadowsAtlasInfos)
        {
            shadowAtlas.directionalShadowAtlas.width = settingsData.shadowResolution;
            shadowAtlas.directionalShadowAtlas.height = settingsData.shadowResolution;
            shadowAtlas.punctualShadowAtlas.width = settingsData.shadowResolution;
            shadowAtlas.punctualShadowAtlas.height = settingsData.shadowResolution;
            shadowAtlas.areaShadowAtlas.width = settingsData.shadowResolution;
            shadowAtlas.areaShadowAtlas.height = settingsData.shadowResolution;
        }

        // Lighting Settings
        VolumeProfile profile = hdrpAsset.defaultVolumeProfile;
        if (profile.TryGet(out AmbientOcclusion ssao))
        {
            ssao.active = settingsData.ssaoEnabled;
            ssao.quality.value = settingsData.ssaoQuality;
        }

        if (profile.TryGet(out ScreenSpaceGlobalIllumination ssgi))
        {
            ssgi.active = settingsData.ssgiEnabled;
            ssgi.quality.value = settingsData.ssgiQuality;
        }

        if (hdrpAsset.currentPlatformRenderPipelineSettings != null)
        {
            hdrpAsset.currentPlatformRenderPipelineSettings.supportVolumetrics = settingsData.volumetricLightingEnabled;
        }
        else
        {
            Debug.LogError("hdrpAsset.currentPlatformRenderPipelineSettings is null");
        }

        if (profile.TryGet(out VolumetricClouds volumetricClouds))
        {
            volumetricClouds.quality.value = settingsData.volumetricLightingQuality;
        }

        // Effects Settings
        if (profile.TryGet(out Bloom bloom))
        {
            bloom.active = settingsData.bloomEnabled;
            bloom.intensity.value = settingsData.bloomIntensity;
        }

        if (profile.TryGet(out MotionBlur motionBlur))
        {
            motionBlur.active = settingsData.motionBlurEnabled;
            motionBlur.intensity.value = settingsData.motionBlurIntensity;
        }

        if (profile.TryGet(out DepthOfField depthOfField))
        {
            depthOfField.active = settingsData.depthOfFieldEnabled;
            depthOfField.focusDistance.value = settingsData.depthOfFieldFocusDistance; // Змінено
        }

        if (profile.TryGet(out Vignette vignette))
        {
            vignette.active = settingsData.vignetteEnabled;
            vignette.intensity.value = settingsData.vignetteIntensity;
        }

        // Texture and Detail Settings
        QualitySettings.globalTextureMipmapLimit = settingsData.textureQuality;
        QualitySettings.lodBias = settingsData.lodBias;
    }
}
