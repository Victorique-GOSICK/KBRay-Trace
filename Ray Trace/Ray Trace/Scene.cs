using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Scene
{
    public void AddSpaceObjects(KBShape shape)
    {
        if(_shapes.Contains(shape))
        {
            return;
        }
        //
        _shapes.Add(shape);
    }

    public bool IntersectObjects(KBRay ray, float minDistance, float maxDistance, ref IntersectParams intersectParams)
    {
        bool isIntersectAnything = false;
        float closetDistance = maxDistance;
        for (Int32 iter = 0; iter < _shapes.Count; ++iter)
        {
            KBShape iterShape = _shapes[iter];
            if(iterShape.Intersect(ray, minDistance, closetDistance, ref intersectParams))
            {
                isIntersectAnything = true;
                closetDistance = intersectParams.T;
            }
        }
        //
        return isIntersectAnything;
    }

    List<KBShape> _shapes = new List<KBShape>();
}
