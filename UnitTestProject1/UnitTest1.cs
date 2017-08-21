using System;
using System.Linq;
using GenSoft.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Create_IPatientInfo_Entity()
        {
            using (var ctx = new GenSoftDBContext())
            {
                var view = ctx.Entity
                    .Include(x => x.EntityAttribute).ThenInclude(x => x.Attributes)
                    .Include(x => x.EntityType.Type)
                    .Include(x => x.EntityType.EntityView)
                    .Include(x => x.EntityType.EntityView.EntityType.EntityTypeAttributes).ToList();

                Assert.IsTrue(view.Any());
                
            }
        }
    }
}
