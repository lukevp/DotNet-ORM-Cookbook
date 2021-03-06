﻿using ServiceStack.DataAnnotations;

namespace Recipes.ServiceStack.Entities
{
    [Alias("Employee")]
    [Schema("HR")]
    public class Employee
    {
        [PrimaryKey, AutoIncrement] [Alias("EmployeeKey")] public int Id { get; set; }

        [Required] [StringLength(50)] public string? FirstName { get; set; }

        [StringLength(50)] public string? MiddleName { get; set; }

        [Required] [StringLength(50)] public string? LastName { get; set; }

        [StringLength(100)] public string? Title { get; set; }

        [StringLength(15)] public string? OfficePhone { get; set; }

        [StringLength(15)] public string? CellPhone { get; set; }

        [References(typeof(EmployeeClassification))]
        [Alias("EmployeeClassificationKey")]
        public int? EmployeeClassificationId { get; set; }

        [Reference] public virtual EmployeeClassification? EmployeeClassification { get; set; }
    }
}