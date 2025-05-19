using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(Camera))]
public class CameraPostProcessingController : MonoBehaviour
{
    [Tooltip("Drag in your Post-Process Profile here")]
    public PostProcessProfile profile;

    private PostProcessLayer ppLayer;
    private PostProcessVolume ppVolume;

    void Awake()
    {
        // 1. Add / configure the PostProcessLayer
        ppLayer = gameObject.GetComponent<PostProcessLayer>();
        if (ppLayer == null)
        {
            ppLayer = gameObject.AddComponent<PostProcessLayer>();
            ppLayer.Init(Resources.Load<PostProcessResources>("PostProcessResources"));
            ppLayer.volumeLayer = LayerMask.GetMask("PostProcessing");
        }

        // 2. Add / configure the global PostProcessVolume
        ppVolume = gameObject.GetComponent<PostProcessVolume>();
        if (ppVolume == null)
        {
            ppVolume = gameObject.AddComponent<PostProcessVolume>();
            ppVolume.isGlobal = true;
            ppVolume.sharedProfile = profile;
        }

        // 3. Color Grading → Tonemapping & Post-exposure & Trackballs
        if (profile.TryGetSettings<ColorGrading>(out var cg))
        {
            cg.active = true;
            // ACES tonemapper
            cg.tonemapper.value = Tonemapper.ACES;                        // :contentReference[oaicite:0]{index=0}
            cg.tonemapper.overrideState = true;

            // Brighten scene (Post-exposure EV)
            cg.postExposure.value = 1f;                                  // :contentReference[oaicite:1]{index=1}
            cg.postExposure.overrideState = true;

            // Trackball tweaks for spooky look:
            // Lift (shadows) → slight blue tint
            cg.lift.value = new Vector4(0.9f, 0.9f, 1.1f, 0f);           // :contentReference[oaicite:2]{index=2}
            cg.lift.overrideState = true;
            // Gamma (midtones) → slight blue
            cg.gamma.value = new Vector4(0.9f, 0.9f, 1.1f, 0f);
            cg.gamma.overrideState = true;
            // Gain (highlights) → slight yellow
            cg.gain.value = new Vector4(1.1f, 1.1f, 0.9f, 0f);
            cg.gain.overrideState = true;
        }

        // 4. Bloom → Intensity & Threshold
        if (profile.TryGetSettings<Bloom>(out var bloom))
        {
            bloom.active = true;
            bloom.intensity.value = 2.5f;                               // :contentReference[oaicite:3]{index=3}
            bloom.intensity.overrideState = true;
            bloom.threshold.value = 0.75f;                              // :contentReference[oaicite:4]{index=4}
            bloom.threshold.overrideState = true;
        }

        // 5. Ambient Occlusion → Intensity & Thickness Modifier
        if (profile.TryGetSettings<AmbientOcclusion>(out var ao))
        {
            ao.active = true;
            ao.intensity.value = 0.5f;                                  // :contentReference[oaicite:5]{index=5}
            ao.intensity.overrideState = true;
            ao.thicknessModifier.value = 3.5f;                          // :contentReference[oaicite:6]{index=6}
            ao.thicknessModifier.overrideState = true;
        }

        // 6. Vignette → Intensity & Smoothness
        if (profile.TryGetSettings<Vignette>(out var vig))
        {
            vig.active = true;
            vig.intensity.value = 0.5f;                                 // :contentReference[oaicite:7]{index=7}
            vig.intensity.overrideState = true;
            vig.smoothness.value = 0.3f;                                // :contentReference[oaicite:8]{index=8}
            vig.smoothness.overrideState = true;
        }

        // 7. Lens Distortion → Intensity & Scale
        if (profile.TryGetSettings<LensDistortion>(out var ld))
        {
            ld.active = true;
            ld.intensity.value = 35f;                                   // :contentReference[oaicite:9]{index=9}
            ld.intensity.overrideState = true;
            ld.scale.value = 1.1f;                                      // :contentReference[oaicite:10]{index=10}
            ld.scale.overrideState = true;
        }
    }
}
