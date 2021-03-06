﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tortuga.Anchor.Metadata;

namespace Recipes.Immutable
{
    /// <summary>
    /// This use case performs basic CRUD operations on a simple model without children.
    /// </summary>
    /// <typeparam name="TModel">A EmployeeClassification model or entity</typeparam>
    [TestCategory("ImmutableTests")]
    public abstract class ImmutableTests<TModel> : TestBase
        where TModel : class, IReadOnlyEmployeeClassification
    {
        protected abstract IImmutableRepository<TModel> GetRepository();

        protected abstract TModel CreateWithValues(string name, bool isExempt, bool isEmployee);

        protected abstract TModel UpdateWithValues(TModel original, string name, bool isExempt, bool isEmployee);

        [TestMethod]
        public void GetByKey()
        {
            var repository = GetRepository();
            var row1 = repository.GetByKey(1);

            Assert.IsNotNull(row1);
            Assert.AreEqual(1, row1!.EmployeeClassificationKey);
            Assert.AreEqual("Full Time Salary", row1.EmployeeClassificationName);
            Assert.AreEqual(true, row1.IsExempt);
            Assert.AreEqual(true, row1.IsEmployee);

            var row4 = repository.GetByKey(4);
            Assert.IsNotNull(row4);
            Assert.AreEqual(4, row4!.EmployeeClassificationKey);
            Assert.AreEqual("Contractor", row4.EmployeeClassificationName);
            Assert.AreEqual(false, row4.IsExempt);
            Assert.AreEqual(false, row4.IsEmployee);
        }

        [TestMethod]
        public void FindByName()
        {
            var repository = GetRepository();
            var row1 = repository.FindByName("Full Time Salary");

            Assert.IsNotNull(row1);
            Assert.AreEqual(1, row1!.EmployeeClassificationKey);
            Assert.AreEqual("Full Time Salary", row1.EmployeeClassificationName);
            Assert.AreEqual(true, row1.IsExempt);
            Assert.AreEqual(true, row1.IsEmployee);

            var row4 = repository.FindByName("Contractor");
            Assert.IsNotNull(row4);
            Assert.AreEqual(4, row4!.EmployeeClassificationKey);
            Assert.AreEqual("Contractor", row4.EmployeeClassificationName);
            Assert.AreEqual(false, row4.IsExempt);
            Assert.AreEqual(false, row4.IsEmployee);
        }

        [TestMethod]
        public void GetAll()
        {
            var repository = GetRepository();
            var allRows = repository.GetAll();

            var row1 = allRows.SingleOrDefault(x => x.EmployeeClassificationKey == 1);
            Assert.IsNotNull(row1);
            Assert.AreEqual(1, row1!.EmployeeClassificationKey);
            Assert.AreEqual("Full Time Salary", row1.EmployeeClassificationName);
            Assert.AreEqual(true, row1.IsExempt);
            Assert.AreEqual(true, row1.IsEmployee);

            var row4 = allRows.SingleOrDefault(x => x.EmployeeClassificationKey == 4);
            Assert.IsNotNull(row4);
            Assert.AreEqual(4, row4!.EmployeeClassificationKey);
            Assert.AreEqual("Contractor", row4.EmployeeClassificationName);
            Assert.AreEqual(false, row4.IsExempt);
            Assert.AreEqual(false, row4.IsEmployee);

            // Ensure that the returned list is actually an immutable list.
            var properties = MetadataCache.GetMetadata(allRows.GetType()).Properties;
            Assert.IsFalse(properties.Any(p => p.CanWrite || p.CanWriteIndexed), "Mutable properties are not allowed in this use case");

            if (allRows is IList list)
            {
                Assert.IsTrue(list.IsReadOnly);
            }
            if (allRows is IList<TModel> typedList)
            {
                Assert.IsTrue(typedList.IsReadOnly);
            }
            if (allRows is IList<IReadOnlyEmployeeClassification> interfaceList)
            {
                Assert.IsTrue(interfaceList.IsReadOnly);
            }
        }

        /// <summary>
        /// Create a row.
        /// </summary>
        [TestMethod]
        public void CreateAndReadBack()
        {
            var repository = GetRepository();

            var newRecord = CreateWithValues("Test " + DateTime.Now.Ticks, false, false);
            var newKey = repository.Create(newRecord);
            Assert.IsTrue(newKey >= 1000, "keys under 1000 were not generated by the database");

            {
                var echo = repository.GetByKey(newKey);
                Assert.IsNotNull(echo);
                Assert.AreEqual(newKey, echo!.EmployeeClassificationKey);
                Assert.AreEqual(newRecord.EmployeeClassificationName, echo.EmployeeClassificationName);
                Assert.AreEqual(false, echo.IsExempt);
                Assert.AreEqual(false, echo.IsEmployee);
                var properties = MetadataCache.GetMetadata(echo.GetType()).Properties;
                Assert.IsFalse(properties.Any(p => p.CanWrite || p.CanWriteIndexed), "Mutable properties are not allowed in this use case");
            }

            {
                var search = repository.FindByName(newRecord.EmployeeClassificationName);
                Assert.IsNotNull(search);
                Assert.AreEqual(newKey, search!.EmployeeClassificationKey);
                Assert.AreEqual(newRecord.EmployeeClassificationName, search.EmployeeClassificationName);
                Assert.AreEqual(false, search.IsExempt);
                Assert.AreEqual(false, search.IsEmployee);
                var properties = MetadataCache.GetMetadata(search.GetType()).Properties;
                Assert.IsFalse(properties.Any(p => p.CanWrite || p.CanWriteIndexed), "Mutable properties are not allowed in this use case");
            }
        }

        /// <summary>
        /// Create and update a row.
        /// </summary>
        [TestMethod]
        public void CreateAndUpdate()
        {
            var repository = GetRepository();

            var newRecord = CreateWithValues("Test " + DateTime.Now.Ticks, false, false);
            var newKey = repository.Create(newRecord);
            Assert.IsTrue(newKey >= 1000, "keys under 1000 were not generated by the database");

            var echo = repository.GetByKey(newKey);
            {
                Assert.IsNotNull(echo);
                Assert.AreEqual(newKey, echo!.EmployeeClassificationKey);
                Assert.AreEqual(newRecord.EmployeeClassificationName, echo.EmployeeClassificationName);
                Assert.AreEqual(newRecord.IsExempt, echo.IsExempt);
                Assert.AreEqual(newRecord.IsEmployee, echo.IsEmployee);
            }

            {
                var search = repository.FindByName(newRecord.EmployeeClassificationName);
                Assert.IsNotNull(search);
                Assert.AreEqual(newKey, search!.EmployeeClassificationKey);
                Assert.AreEqual(newRecord.EmployeeClassificationName, search.EmployeeClassificationName);
                Assert.AreEqual(newRecord.IsExempt, search.IsExempt);
                Assert.AreEqual(newRecord.IsEmployee, search.IsEmployee);
            }

            var updater = UpdateWithValues(echo, "Update " + DateTime.Now.Ticks, true, true);
            repository.Update(updater);

            {
                var echo2 = repository.GetByKey(newKey);
                Assert.IsNotNull(echo2);
                Assert.AreEqual(newKey, echo2!.EmployeeClassificationKey);
                Assert.AreEqual(updater.EmployeeClassificationName, echo2.EmployeeClassificationName);
                Assert.AreEqual(updater.IsExempt, echo2.IsExempt);
                Assert.AreEqual(updater.IsEmployee, echo2.IsEmployee);
            }

            {
                var search2 = repository.FindByName(updater.EmployeeClassificationName);
                Assert.IsNotNull(search2);
                Assert.AreEqual(newKey, search2!.EmployeeClassificationKey);
                Assert.AreEqual(updater.EmployeeClassificationName, search2.EmployeeClassificationName);
                Assert.AreEqual(updater.IsExempt, search2.IsExempt);
                Assert.AreEqual(updater.IsEmployee, search2.IsEmployee);
            }
        }

        /// <summary>
        /// Create and delete a row.
        /// </summary>
        [TestMethod]
        public void CreateAndDelete()
        {
            var repository = GetRepository();

            var newRecord = CreateWithValues("Test " + DateTime.Now.Ticks, false, false);
            var newKey = repository.Create(newRecord);
            Assert.IsTrue(newKey >= 1000, "keys under 1000 were not generated by the database");

            var echo = repository.GetByKey(newKey);
            Assert.IsNotNull(echo);
            Assert.AreEqual(newKey, echo!.EmployeeClassificationKey);

            repository.Delete(echo);

            var search = repository.FindByName(newRecord.EmployeeClassificationName);
            Assert.IsNull(search);
        }

        /// <summary>
        /// Ensure that the created model is actually immutable.
        /// </summary>
        [TestMethod]
        public void ModelMutabilityCheck()
        {
            var repository = GetRepository();
            var row = CreateWithValues("Test", false, false);

            var properties = MetadataCache.GetMetadata(row.GetType()).Properties;
            Assert.IsFalse(properties.Any(p => p.CanWrite || p.CanWriteIndexed), "Mutable properties are not allowed in this use case");
        }

        [TestMethod]
        public void Create_ParameterCheck()
        {
            var repository = GetRepository();
            AssertThrowsException<ArgumentNullException>(() => repository.Create(null!));
        }

        [TestMethod]
        public void Update_ParameterCheck()
        {
            var repository = GetRepository();
            AssertThrowsException<ArgumentNullException>(() => repository.Update(null!));
        }

        [TestMethod]
        public void Delete_ParameterCheck()
        {
            var repository = GetRepository();
            AssertThrowsException<ArgumentNullException>(() => repository.Delete(null!));
        }
    }
}
