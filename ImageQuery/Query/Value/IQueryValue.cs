using ImageQuery.Canvas;

namespace ImageQuery.Query.Value
{
    public interface IQueryValue
    {
        float Number { get; set; }
        Color Color { get; set; }
        ICanvas Canvas { get; set; }
        bool Boolean { get; set; }

        IQLType GetIQLType();

        IQueryValue Index(IQueryValue x, IQueryValue y);
    }
}
