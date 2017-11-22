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
    static KBColor _GetColor(KBRay ray, Scene scene)
    {
        IntersectParams intersectParams = new IntersectParams();
        if (scene.IntersectObjects(ray, 0.0f, float.MaxValue, ref intersectParams))
        {
            KBColor attenuation = KBColor.Black;
            KBRay newRay = null;
            if (intersectParams.Material.Scatter(ray, intersectParams, ref attenuation, ref newRay))
            {
                return attenuation * _GetColor(newRay, scene);
            }
            else
            {
                return KBColor.Black;
            }
        }
        else
        {
            KBVector3 unit_direction = ray.Direction;
            float t = 0.5f * (ray.Direction.Y + 1.0f);
            return KBColor.White * (1.0f - t) + new KBColor(0.5f, 0.7f, 1.0f) * t;
        }
    }

    public static Bitmap DrawToBitmap(Int32 width, Int32 height)
    {
        PerspectiveCamera camera = new PerspectiveCamera();
        KBSphere sphere = new KBSphere(new KBVector3(0.0f, 0.0f, -1.0f), 0.5f);
        sphere.Material = new LambertianMaterial(new KBColor(0.8f, 0.3f, 0.3f));
        KBSphere sphere1 = new KBSphere(new KBVector3(0.0f, 100.5f, -1.0f), 100.0f);
        sphere1.Material = new LambertianMaterial(new KBColor(0.8f, 0.8f, 0.0f));
        KBSphere sphere2 = new KBSphere(new KBVector3(1.0f, 0.0f, -1.0f), 0.5f);
        sphere2.Material = new MetalMaterial(new KBColor(0.8f, 0.6f, 0.2f));
        KBSphere sphere3 = new KBSphere(new KBVector3(-1.0f, 0.0f, -1.0f), 0.5f);
        sphere3.Material = new MetalMaterial(new KBColor(0.8f, 0.8f, 0.8f));

        Scene scene = new Scene();
        scene.AddSpaceObjects(sphere);
        scene.AddSpaceObjects(sphere1);
        scene.AddSpaceObjects(sphere2);
        scene.AddSpaceObjects(sphere3);

        Bitmap image = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        Int32 subPiexCount = 20;
        for (Int32 iterX = 0; iterX < image.Width; ++iterX)
        {
            for (Int32 iterY = 0; iterY < image.Height; ++iterY)
            {
                KBColor color = KBColor.Black;
                for (Int32 Sub = 0; Sub < subPiexCount; ++Sub)
                {
                    float u = (float)(iterX + KBRandom.Next(0.0f, 1.0f)) / (float)image.Width;
                    float v = (float)(iterY + KBRandom.Next(0.0f, 1.0f)) / (float)image.Height;
                    KBRay ray = camera.RayCast(u, v);
                    color += _GetColor(ray, scene);
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

    static Random _random = new Random(Int32.MaxValue);
}
