﻿using BenchmarkDotNet.Attributes;
using Recipes.NHibernate.SingleModelCrud;
using Recipes.NHibernate.SingleModelCrudAsync;
using System.Threading.Tasks;

namespace Performance
{
    partial class Benchmarks
    {
        [Benchmark]
        public void NHibernate_SingleModelCrudTests_CreateAndReadBack()
        {
            var test = new SingleModelCrudTests();
            test.CreateAndReadBack();
        }

        /// <summary>
        /// Create and delete a row.
        /// </summary>
        [Benchmark]
        public void NHibernate_SingleModelCrudTests_CreateAndDeleteByModel()
        {
            var test = new SingleModelCrudTests();
            test.CreateAndDeleteByModel();
        }

        /// <summary>
        /// Create and delete a row.
        /// </summary>
        [Benchmark]
        public void NHibernate_SingleModelCrudTests_CreateAndDeleteByKey()
        {
            var test = new SingleModelCrudTests();
            test.CreateAndDeleteByKey();
        }

        /// <summary>
        /// Get all rows from a table.
        /// </summary>
        [Benchmark]
        public void NHibernate_SingleModelCrudTests_GetAll()
        {
            var test = new SingleModelCrudTests();
            test.GetAll();
        }

        /// <summary>
        /// Get a row using a primary key.
        /// </summary>
        [Benchmark]
        public void NHibernate_SingleModelCrudTests_GetByKey()
        {
            var test = new SingleModelCrudTests();
            test.GetByKey();
        }

        /// <summary>
        /// Create and update a row.
        /// </summary>
        [Benchmark]
        public void NHibernate_SingleModelCrudTests_CreateAndUpdate()
        {
            var test = new SingleModelCrudTests();
            test.CreateAndUpdate();
        }
    }
}
