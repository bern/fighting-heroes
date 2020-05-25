using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    public CustomPlayerController playerToChange;
    private Texture2D texture;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        texture = spriteRenderer.sprite.texture;
        Debug.Log("outline script loaded texture " + texture.name);
    }

    // Update is called once per frame
    void Update()
    {
        int[] firstBorderPosXatY = new int[texture.height];
        int[] lastBorderPosXatY = new int[texture.height];
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int borderPosX = -1;
            for (int yPos = 0; yPos < texture.height; yPos++)
            {
                bool firstXPosSet = false;
                for (int xPos = 0; xPos < texture.width; xPos++)
                {
                        Color pixel = texture.GetPixel(xPos, yPos); // y-val 0 is at bottom
                        if (pixel.r == 1 && pixel.b == 0 && pixel.g == 0)
                        {
                            if (!firstXPosSet)
                            {
                                firstBorderPosXatY[yPos] = xPos;
                                firstXPosSet = true;
                            }
                        lastBorderPosXatY[yPos] = xPos; 
                        }
                }
            }
            if (borderPosX >= 0)
            {
                Debug.Log("found border at " + borderPosX);
            } else
            {
                Debug.Log("didnt find a border pixel");
            }
            Debug.Log("all borderPosXatY:");
            string borders = "";
            for (int i = 0; i < firstBorderPosXatY.Length; i++)
            {
                int pos = firstBorderPosXatY[i];
                borders += pos + ", ";
                Color[] c = new Color[pos];
                for(int j = 0; j < c.Length; j++)
                {
                    c[j] = new Color(1, 1, 1, 0);
                }
                texture.SetPixels(0, i, pos, 1, c);
                pos = lastBorderPosXatY[i];
                c = new Color[texture.width - pos];
                for (int j = 0; j < c.Length; j++)
                {
                    c[j] = new Color(1, 1, 1, 0);
                }
                texture.SetPixels(pos, i, texture.width - pos, 1, c);
            }
            texture.Apply();
            // spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0f, 0f));

            int TopBorder = 0;
            int BottomBorder = texture.height;
            int LeftBorder = texture.width;
            int RightBorder = 0;
            for(int i = 0; i < firstBorderPosXatY.Length; i++)
            {
                if (firstBorderPosXatY[i] < LeftBorder && firstBorderPosXatY[i] != 0)
                {
                    LeftBorder = firstBorderPosXatY[i];
                }
            }
            for (int i = 0; i < lastBorderPosXatY.Length; i++)
            {
                if (lastBorderPosXatY[i] > RightBorder)
                {
                    RightBorder = lastBorderPosXatY[i];
                }
            }
            bool bottomBorderSet = false;
            for (int i = 0; i < texture.height; i++)
            {
                if (!bottomBorderSet)
                {
                    if (firstBorderPosXatY[i] != 0)
                    {
                        BottomBorder = i;
                        bottomBorderSet = true;
                    } else if (lastBorderPosXatY[i] != 0)
                    {
                        BottomBorder = i;
                        bottomBorderSet = true;
                    }
                }
                if (firstBorderPosXatY[i] != 0)
                {
                    TopBorder = i;
                }
                if (lastBorderPosXatY[i] != 0)
                {
                    TopBorder = i;
                }
            }

            Debug.Log("CUTTING A SPRITE: x="+LeftBorder+" y=0 w="+(RightBorder-LeftBorder)+" h="+texture.height);
            Sprite outlinedSprite = Sprite.Create(texture, new Rect(LeftBorder, BottomBorder, RightBorder - LeftBorder, TopBorder - BottomBorder), new Vector2(0f, 0f));
            playerToChange.UpdateSprite(outlinedSprite);
        }
    }
}
