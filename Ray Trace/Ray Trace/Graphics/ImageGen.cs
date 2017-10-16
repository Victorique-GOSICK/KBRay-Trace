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
    static KBVector3 _GetRandomDir_In_Unit_Sphere()
    {
        KBVector3 tagertVector;
        do
        {
            KBVector3 randomPos = new KBVector3(_random.Next(0, 100) * 0.01f, _random.Next(0, 100)* 0.01f, _random.Next(0, 100) * 0.01f);
            tagertVector = randomPos * 2.0f - KBVector3.ONE;
        } while (KBVector3.Dot(tagertVector, tagertVector) >= 1.0f);
        //
        return tagertVector;
    }

    static KBColor _GetColor(KBRay ray, Scene scene)
    {
        IntersectParams intersectParams = new IntersectParams();
        if (scene.IntersectObjects(ray, 0.0f, float.MaxValue, ref intersectParams))
        {
            KBVector3 tagert = intersectParams.Point + intersectParams.Normal + _GetRandomDir_In_Unit_Sphere();
            KBRay newRay = new KBRay(intersectParams.Point, (tagert - intersectParams.Point).Normalize());
            return _GetColor(newRay, scene) * 0.5f;
        }
        else
        {
            return new KBColor(1, 1, 1);
        }
    }

    public static Bitmap DrawToBitmap(Int32 width, Int32 height)
    {
        PerspectiveCamera camera = new PerspectiveCamera();
        KBSphere sphere = new KBSphere(new KBVector3(0.0f, 0.0f, -1.0f), 0.5f);
        KBSphere sphere1 = new KBSphere(new KBVector3(0.0f, 100.5f, -1.0f), 100.0f);
        Scene scene = new Scene();
        scene.AddSpaceObjects(sphere);
        scene.AddSpaceObjects(sphere1);
        Bitmap image = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        Int32 subPiexCount = 4;
        for (Int32 iterX = 0; iterX < image.Width; ++iterX)
        {
            for (Int32 iterY = 0; iterY < image.Height; ++iterY)
            {
                KBColor color = KBColor.Black;
                for (Int32 Sub = 0; Sub < subPiexCount; ++Sub)
                {
                    float u = (float)(iterX + _random.Next(0, 100) * 0.01f) / (float)image.Width;
                    float v = (float)(iterY + _random.Next(0, 100) * 0.01f) / (float)image.Height;
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
