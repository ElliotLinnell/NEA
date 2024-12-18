using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

using TMPro;

public class Settings : MonoBehaviour
{
    public Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>();
    public TMP_Text upKeyText;
    public TMP_Text downKeyText;
    public TMP_Text leftKeyText;
    public TMP_Text rightKeyText;
    public TMP_Text volumeText;
    public TMP_Text musicVolumeText;
    public Slider volumeSlider;
    public Slider musicVolumeSlider;
    public float volumeValue;
    public float musicVolumeValue;
    public FullScreenMode fullScreenMode;
    public TMP_Dropdown fullScreenModeDropdown;
    public int resolutionWidth;
    public int resolutionHeight;
    public bool fullScreen;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public Toggle vSyncToggle;
    public int antiAliasing;
    public TMP_Dropdown antiAliasingDropdown;
    public int textureQuality;
    public TMP_Dropdown textureQualityDropdown;
    public AnisotropicFiltering anisotropicFiltering;
    public TMP_Dropdown anisotropicFilteringDropdown;
    public ShadowQuality shadowQuality;
    public TMP_Dropdown shadowQualityDropdown;
    public ShadowResolution shadowResolution;
    public TMP_Dropdown shadowResolutionDropdown;
    public float shadowDistance;
    public Slider shadowDistanceSlider;
    public int shadowCascades;
    public TMP_Dropdown shadowCascadesDropdown;
    public ShadowProjection shadowProjection;
    public TMP_Dropdown shadowProjectionDropdown;
    public float shadowNearPlaneOffset;
    public Slider shadowNearPlaneOffsetSlider;
    public bool softParticles;
    public Toggle softParticlesToggle;
    private string keyToChange = "";
    private void Awake()
    {
        keyBindings.Add("up", KeyCode.W);
        keyBindings.Add("down", KeyCode.S);
        keyBindings.Add("left", KeyCode.A);
        keyBindings.Add("right", KeyCode.D);
    }

    private void Start()
    {
        LoadBindings();
        UpdateBindingDisplay();
    }

    public void back()
    {
        GameManager.StartLoading("menuscene");
    }

    public void ChangeKey(string action)
    {
        keyToChange = action;
    }

    private void Update()
    {
        if (keyToChange != "")
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        keyBindings[keyToChange] = keyCode;
                        keyToChange = "";
                        SaveBindings();
                        UpdateBindingDisplay();
                        break;
                    }
                }
            }
        }
    }

    public void SaveBindings()
    {
        using (StreamWriter file = new StreamWriter("bindings.txt"))
        {
            foreach (var binding in keyBindings)
            {
                file.WriteLine(binding.Key + ":" + binding.Value.ToString());
            }
        }
    }

    public void LoadBindings()
    {
        if (File.Exists("bindings.txt"))
        {
            using (StreamReader file = new StreamReader("bindings.txt"))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    string[] splitLine = line.Split(':');
                    if (splitLine.Length == 2)
                    {
                        keyBindings[splitLine[0]] = (KeyCode)System.Enum.Parse(typeof(KeyCode), splitLine[1]);
                    }
                }
            }
        }
        else
        {
            keyBindings["up"] = KeyCode.UpArrow;
            keyBindings["down"] = KeyCode.DownArrow;
            keyBindings["left"] = KeyCode.LeftArrow;
            keyBindings["right"] = KeyCode.RightArrow;
        }
    }

    public void UpdateBindingDisplay()
    {
        upKeyText.text = "Up: " + keyBindings["up"].ToString();
        downKeyText.text = "Down: " + keyBindings["down"].ToString();
        leftKeyText.text = "Left: " + keyBindings["left"].ToString();
        rightKeyText.text = "Right: " + keyBindings["right"].ToString();
    }

    public void resetbindings()
    {
        if (File.Exists("bindings.txt"))
        {
            File.Delete("bindings.txt");
        }
        LoadBindings();
        UpdateBindingDisplay();
    }

    public void volume(float newVolume)
    {
        AudioListener.volume = Mathf.Clamp01(newVolume);
    }

    public void setMusicVolume(float newVolume)
    {
        musicVolumeValue = newVolume;
        musicVolumeText.text = "Music Volume: " + (newVolume * 100).ToString("F0") + "%";
    }
    public void setFullScreenMode(FullScreenMode mode)
    {
        Screen.fullScreenMode = mode;
    }
    public void setResolution(int width, int height, bool fullScreen)
    {
        Screen.SetResolution(width, height, fullScreen);
    }
    public void setQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void setVSync(bool vSync)
    {
        QualitySettings.vSyncCount = vSync ? 1 : 0;
    }
    public void setAntiAliasing(int antiAliasing)
    {
        QualitySettings.antiAliasing = antiAliasing;
    }
    public void setTextureQuality(int textureQuality)
    {
        QualitySettings.globalTextureMipmapLimit = textureQuality;
    }
    public void setAnisotropicFiltering(AnisotropicFiltering anisotropicFiltering)
    {
        QualitySettings.anisotropicFiltering = anisotropicFiltering;
    }
    public void setShadows(ShadowQuality shadowQuality)
    {
        QualitySettings.shadows = shadowQuality;
    }
    public void setShadowResolution(ShadowResolution shadowResolution)
    {
        QualitySettings.shadowResolution = shadowResolution;
    }
    public void setShadowDistance(float shadowDistance)
    {
        QualitySettings.shadowDistance = shadowDistance;
    }
    public void setShadowCascades(int shadowCascades)
    {
        QualitySettings.shadowCascades = shadowCascades;
    }
    public void setShadowProjection(ShadowProjection shadowProjection)
    {
        QualitySettings.shadowProjection = shadowProjection;
    }
    public void setShadowNearPlaneOffset(float shadowNearPlaneOffset)
    {
        QualitySettings.shadowNearPlaneOffset = shadowNearPlaneOffset;
    }
    public void setSoftParticles(bool softParticles)
    {
        QualitySettings.softParticles = softParticles;
    }

}
