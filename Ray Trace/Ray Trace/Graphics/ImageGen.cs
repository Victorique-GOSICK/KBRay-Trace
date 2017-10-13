using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class ImageGen
{
    public static Bitmap DrawToBitmap(Int32 width, Int32 height)
    {
        KBVector3 originPos = KBVector3.ZERO;
        KBVector3 lower_left_corner = new KBVector3(-2.0f, -1.0f, -1.0f);
        KBVector3 horizontal = new KBVector3(4.0f, 0.0f, 0.0f);
        KBVector3 vertical = new KBVector3(0.0f, 2.0f, 0.0f);

        Bitmap image = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        for (Int32 iterX = 0; iterX < image.Width; ++iterX)
        {
            for (Int32 iterY = 0; iterY < image.Height; ++iterY)
            {
                float u = (float)iterX / (float)image.Width;
                float v = (float)iterY / (float)image.Height;
                KBRay ray = new KBRay(originPos, (lower_left_corner + u * horizontal + v * vertical).Normalize());
                Int32 R = (int)(255 * (ray.Direction.X * 0.5f + 0.5f));
                Int32 G = (int)(255 * (ray.Direction.Y * 0.5f + 0.5f));
                Int32 B = (int)(255 * (ray.Direction.Y * 0.5f + 0.5f));
                Color iterColor = Color.FromArgb(R, G, B);
                image.SetPixel(iterX, iterY, iterColor);
            }
        }
        //
        return image;
    }
}
