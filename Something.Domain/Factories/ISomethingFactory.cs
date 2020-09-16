
namespace Something.Domain
{
    public interface ISomethingFactory
    {
        Models.Something Create();
        Models.Something Create(string name);
    }
}