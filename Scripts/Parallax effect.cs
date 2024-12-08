using System;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private Vector2 parafactor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
  private Transform cam;
  private Vector3 prevpos;
  private float spriteWidthUnits;
  private int Repeatbacktimes = 3;
    void Start()
    {
        cam = Camera.main.transform;
        prevpos = cam.position;

        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        spr.drawMode = SpriteDrawMode.Tiled;
        spr.size = new Vector2(spr.size.x * Repeatbacktimes, spr.size.y);

        float textureWidth = spr.sprite.texture.width;
        float ppu = spr.sprite.pixelsPerUnit;
        spriteWidthUnits = textureWidth/ppu;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 currentpos = cam.position;
        Vector3 cammovframe = currentpos - prevpos;
        transform.position -= new Vector3(cammovframe.x * parafactor.x,cammovframe.y * parafactor.y,0);
        prevpos = currentpos;

        if(Mathf.Abs(cam.position.x - transform.position.x) >= spriteWidthUnits) {
            transform.position = new Vector3(cam.position.x, transform.position.y, transform.position.z);
        }
    }
}
