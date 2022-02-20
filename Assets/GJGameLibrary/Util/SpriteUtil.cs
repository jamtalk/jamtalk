using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GJGameLibrary.Util
{
    public class SpriteUtil
    {
        public static byte[] ConvertJPG(Sprite sprite)
        {
            var texture = sprite.texture;

            RenderTexture renderTexture = RenderTexture.GetTemporary(
                        texture.width,
                        texture.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

            Graphics.Blit(texture, renderTexture);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTexture;
            Texture2D readableTextur2D = new Texture2D(texture.width, texture.height);
            readableTextur2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            readableTextur2D.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTexture);

            return readableTextur2D.EncodeToJPG();
        }
    }
}
