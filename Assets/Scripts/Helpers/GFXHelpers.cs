using UnityEngine;
using System.Collections;

public static class GFXHelpers
{
    public static IEnumerator FlashEffect(SpriteRenderer spriteRenderer, int flashLen)
    {
        spriteRenderer.material.SetFloat("_FlashAmount", 1f);
        while (flashLen > 0)
        {
            flashLen--;
            yield return null;
        }
        spriteRenderer.material.SetFloat("_FlashAmount", 0f);
    }
}
