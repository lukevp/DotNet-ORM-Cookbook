﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Recipes.NHibernate.Models;
using Recipes.PartialUpdate;

namespace Recipes.NHibernate.PartialUpdate
{
    [TestClass]
    public class PartialUpdateTests : PartialUpdateTests<EmployeeClassification>
    {
        protected override IPartialUpdateRepository<EmployeeClassification> GetRepository()
        {
            return new PartialUpdateRepository(Setup.SessionFactory);
        }
    }
}
