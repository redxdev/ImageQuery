using System.Security.Cryptography.X509Certificates;
using ImageQuery.Canvas;

namespace ImageQuery
{
    public interface IQueryValue
    {
        float Number { get; set; }
        Color Color { get; set; }
        ICanvas Canvas { get; set; }
        bool Boolean { get; set; }

        IQLType GetIQLType();
    }
}
