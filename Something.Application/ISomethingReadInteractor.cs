using System.Collections.Generic;

namespace Something.Application
{
    public interface ISomethingReadInteractor
    {
        List<Domain.Models.Something> GetSomethingList();
    }
}