using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class LambertianMaterial : Material
{
    public KBColor Albedo;

    public LambertianMaterial(KBColor albedo)
    {
        Albedo = albedo;
    }

    public override bool Scatter(KBRay ray, IntersectParams intersectParams, ref KBColor attenuation, ref KBRay scatterRay)
    {
        KBVector3 tagert = intersectParams.Point + intersectParams.Normal + KBMath3D.RandomDir_In_Unit_Sphere();
        scatterRay = new KBRay(intersectParams.Point, (tagert - intersectParams.Point).Normalize());
        attenuation = Albedo;
        return true;
    }
}
