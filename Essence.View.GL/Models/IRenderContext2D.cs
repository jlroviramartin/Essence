using Essence.Geometry.Core;
using Essence.Geometry.Core.Double;

namespace Essence.View.Models
{
    public interface IRenderContext2D
    {
        Transform2 ModelView { get; }

        BoundingBox2d BoundingBoxInView { get; }
        BoundingBox2d BoundingBoxInModel { get; }

        void Transform(Transform2 transform);

        void PushAttrib(int attribs);
        void PopAttrib();
    }
}