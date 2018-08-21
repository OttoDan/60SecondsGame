using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsanicubeURL : MonoBehaviour {

    Text text;
    SpriteRenderer spriteRenderer;
    public List<Color> palette;
    private void Awake()
    {
        text = GetComponent<Text>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate () {
        if(AudioManager.Instance.GetSongSpectrumZero()>0.25f)
            if(palette.Count>0)
            {

                if(text!=null)
                    text.color = palette[Random.Range(0, palette.Count)];
                if (spriteRenderer != null)
                    spriteRenderer.material.color = palette[Random.Range(0, palette.Count)];
            }
	}
}
