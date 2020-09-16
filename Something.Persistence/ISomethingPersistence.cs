using System.Collections.Generic;

namespace Something.Persistence
{
    public interface ISomethingPersistence
    {
        List<Domain.Models.Something> GetSomethingList();
        void SaveSomething(Domain.Models.Something something);
    }
}