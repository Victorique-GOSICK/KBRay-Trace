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
        Random random = new Random();
        PerspectiveCamera camera = new PerspectiveCamera();
        KBSphere sphere = new KBSphere(new KBVector3(0.0f, 0.0f, -1.0f), 0.5f);
        Scene scene = new Scene();
        scene.AddSpaceObjects(sphere);
        Bitmap image = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        Int32 subPiexCount = 4;
        for (Int32 iterX = 0; iterX < image.Width; ++iterX)
        {
            for (Int32 iterY = 0; iterY < image.Height; ++iterY)
            {
                KBColor color = KBColor.Black;
                for (Int32 Sub = 0; Sub < subPiexCount; ++Sub)
                {
                    float u = (float)(iterX + random.Next(0, 1)) / (float)image.Width;
                    float v = (float)(iterY + random.Next(0, 1)) / (float)image.Height;
                    KBRay ray = camera.RayCast(u, v);
                    IntersectParams intersectParams = new IntersectParams();
                    if (scene.IntersectObjects(ray, 0.0f, 1000.0f, ref intersectParams))
                    {
                        KBColor iterColor = new KBColor(intersectParams.Normal.X * 0.5f + 0.5f, intersectParams.Normal.Y * 0.5f + 0.5f, intersectParams.Normal.Z * 0.5f + 0.5f);
                        color += iterColor;
                    }
                }
                //
                KBColor finalColor = color / subPiexCount;
                Int32 R = (Int32)(255 * finalColor.R);
                Int32 G = (Int32)(255 * finalColor.G);
                Int32 B = (Int32)(255 * finalColor.B);
                image.SetPixel(iterX, iterY, Color.FromArgb(R, G, B));
            }
        }
        //
        return image;
    }
}
