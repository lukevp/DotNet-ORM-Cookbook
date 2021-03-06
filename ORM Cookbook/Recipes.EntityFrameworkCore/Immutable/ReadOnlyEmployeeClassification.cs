﻿using Recipes.Immutable;
using Recipes.EntityFrameworkCore.Entities;
using System;

namespace Recipes.EntityFrameworkCore.Immutable
{
    public class ReadOnlyEmployeeClassification : IReadOnlyEmployeeClassification
    {
        public ReadOnlyEmployeeClassification(EmployeeClassification entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), $"{nameof(entity)} is null.");
            if (entity.EmployeeClassificationName == null)
                throw new ArgumentNullException(nameof(entity), $"{nameof(entity.EmployeeClassificationName)} is null.");
            if (entity.IsEmployee == null)
                throw new ArgumentNullException(nameof(entity), $"{nameof(entity.IsEmployee)} is null.");

            EmployeeClassificationKey = entity.EmployeeClassificationKey;
            EmployeeClassificationName = entity.EmployeeClassificationName;
            IsExempt = entity.IsExempt;
            IsEmployee = entity.IsEmployee.Value;
        }

        public ReadOnlyEmployeeClassification(int employeeClassificationKey, string employeeClassificationName, bool isExempt, bool isEmployee)
        {
            EmployeeClassificationKey = employeeClassificationKey;
            EmployeeClassificationName = employeeClassificationName;
            IsExempt = isExempt;
            IsEmployee = isEmployee;
        }

        public EmployeeClassification ToEntity()
        {
            return new EmployeeClassification()
            {
                EmployeeClassificationKey = EmployeeClassificationKey,
                EmployeeClassificationName = EmployeeClassificationName,
                IsExempt = IsExempt,
                IsEmployee = IsEmployee
            };
        }

        public int EmployeeClassificationKey { get; }
        public string EmployeeClassificationName { get; }
        public bool IsExempt { get; }
        public bool IsEmployee { get; }
    }
}
