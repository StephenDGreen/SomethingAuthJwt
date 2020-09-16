namespace Something.Domain
{
    public class SomethingFactory : ISomethingFactory
    {
        public Models.Something Create()
        {
            return new Models.Something();
        }

        public Models.Something Create(string name)
        {
            return new Models.Something() { Name = name };
        }
    }
}