using System.Collections.Generic;
using System.Linq;

namespace Something.Persistence
{
    public class SomethingPersistence : ISomethingPersistence
    {
        private AppDbContext ctx;

        public SomethingPersistence(AppDbContext ctx)
        {
            this.ctx = ctx;
        }

        public void SaveSomething(Domain.Models.Something something)
        {
            ctx.Somethings.Add(something);
            ctx.SaveChanges();
        }

        public List<Domain.Models.Something> GetSomethingList()
        {
            return ctx.Somethings.ToList();
        }
    }
}