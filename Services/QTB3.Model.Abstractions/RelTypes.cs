
namespace QTB3.Model.Abstractions
{
    public enum RelTypes
    {
        notfound, // included so that Enum parsing will return notfound if it fails
        self,
        next,
        last,
        first,
        prev
    }
}
