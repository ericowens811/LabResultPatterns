namespace QTB3.Model.Abstractions
{
    public interface IEntity : IId, INullifyNavProp
    {
        string Name { get; }        
    }
}
