using Something.Persistence;
using System.Collections.Generic;

namespace Something.Application
{
    public class SomethingReadInteractor : ISomethingReadInteractor
    {
        private readonly ISomethingPersistence persistence;

        public SomethingReadInteractor(ISomethingPersistence persistence)
        {
            this.persistence = persistence;
        }

        public List<Domain.Models.Something> GetSomethingList()
        {
            return persistence.GetSomethingList();
        }
    }
}