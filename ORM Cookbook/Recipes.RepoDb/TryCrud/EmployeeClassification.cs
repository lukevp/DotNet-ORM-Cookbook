﻿using Recipes.TryCrud;

namespace Recipes.RepoDb.TryCrud
{
    public class EmployeeClassification : IEmployeeClassification
    {
        public int EmployeeClassificationKey { get; set; }

        /// <exclude />
        public string? EmployeeClassificationName { get; set; }
    }
}
