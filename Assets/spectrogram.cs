using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class spectrogram : MonoBehaviour
{
    AudioSource source;
    private float[] spectrum;

    public Texture2D texture;
    Renderer m_Renderer;

    public float spectrumRefreshRate = .1f, amplitudeScale = 150;
    private float spectrumRefreshTime = 0;

    void Start()
    {
        source = GetComponent<AudioSource>();
        spectrum = new float[texture.width];
        texture.name = "Spectrum";
        m_Renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (Time.time - spectrumRefreshTime > spectrumRefreshRate)
        {
            spectrumRefreshTime = Time.time;
            UpdateSpectrum();
            texture.Apply();
            m_Renderer.material.SetTexture("_MainTex", texture);
        }    
    }

    void UpdateSpectrum()
    {
        source.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        for (int y = texture.height; y > -1; y--)
        {
            if (y == 0)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    float amplitude = 1 - (spectrum[x] * amplitudeScale);
                    print(amplitude);
                    Color col = new Color(amplitude, amplitude, amplitude, amplitude);
                    texture.SetPixel(x, 0, col);
                }
            }
            else
            {
                for (int x = 0; x < texture.width; x++)
                {
                    texture.SetPixel(x, y, texture.GetPixel(x, y - 1));
                }
            }
        }        
    }

}
