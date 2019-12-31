﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Recipes.PartialUpdate
{
    /// <summary>
    /// This use case performs basic CRUD operations on a simple model without children.
    /// </summary>
    /// <typeparam name="TModel">A EmployeeClassification model or entity</typeparam>
    public abstract class PartialUpdateTests<TModel>
        where TModel : class, IEmployeeClassification, new()
    {
        protected abstract IPartialUpdateRepository<TModel> GetRepository();

        [TestMethod]
        public void PartialUpdate_OneRepositoryTwoMessages()
        {
            var repositoryA = GetRepository();

            //Setup the new record
            var newRecord = new TModel() { EmployeeClassificationName = "Test " + DateTime.Now.Ticks, IsExempt = true, IsEmployee = false };
            var newKey = repositoryA.Create(newRecord);
            Assert.IsTrue(newKey >= 1000, "keys under 1000 were not generated by the database");

            {
                //Get a copy of the record using repository A
                var versionA1 = repositoryA.GetByKey(newKey);
                Assert.IsNotNull(versionA1);
                Assert.AreEqual(newKey, versionA1!.EmployeeClassificationKey);
                Assert.AreEqual(newRecord.EmployeeClassificationName, versionA1.EmployeeClassificationName);
                Assert.AreEqual(newRecord.IsExempt, versionA1.IsExempt);
                Assert.AreEqual(newRecord.IsEmployee, versionA1.IsEmployee);
            }

            //Create the updaters
            var updateMessage1 = new EmployeeClassificationFlagsUpdater()
            {
                EmployeeClassificationKey = newKey,
                IsExempt = false,
                IsEmployee = true
            };

            var updateMessage2 = new EmployeeClassificationNameUpdater()
            {
                EmployeeClassificationKey = newKey,
                EmployeeClassificationName = "Updated " + DateTime.Now.Ticks
            };

            repositoryA.Update(updateMessage1);
            repositoryA.Update(updateMessage2);

            {
                //get the final version using repository A
                var versionA2 = repositoryA.GetByKey(newKey);
                Assert.IsNotNull(versionA2);
                Assert.AreEqual(newKey, versionA2!.EmployeeClassificationKey);
                Assert.AreEqual(updateMessage2.EmployeeClassificationName, versionA2.EmployeeClassificationName);
                Assert.AreEqual(updateMessage1.IsExempt, versionA2.IsExempt);
                Assert.AreEqual(updateMessage1.IsEmployee, versionA2.IsEmployee);
            }
        }

        [TestMethod]
        public void PartialUpdate_TwoRepositories()
        {
            var repositoryA = GetRepository();
            var repositoryB = GetRepository();

            //Setup the new record
            var newRecord = new TModel() { EmployeeClassificationName = "Test " + DateTime.Now.Ticks, IsExempt = true, IsEmployee = false };
            newRecord.EmployeeClassificationName = "Test " + DateTime.Now.Ticks;
            var newKey = repositoryA.Create(newRecord);
            Assert.IsTrue(newKey >= 1000, "keys under 1000 were not generated by the database");

            //Get a copy of the record using repository A
            var versionA1 = repositoryA.GetByKey(newKey);
            Assert.IsNotNull(versionA1);
            Assert.AreEqual(newKey, versionA1!.EmployeeClassificationKey);
            Assert.AreEqual(newRecord.EmployeeClassificationName, versionA1.EmployeeClassificationName);
            Assert.AreEqual(newRecord.IsExempt, versionA1.IsExempt);
            Assert.AreEqual(newRecord.IsEmployee, versionA1.IsEmployee);

            //Create the updaters
            var updateMessage1 = new EmployeeClassificationFlagsUpdater()
            {
                EmployeeClassificationKey = versionA1.EmployeeClassificationKey,
                IsExempt = false,
                IsEmployee = true
            };

            var updateMessage2 = new EmployeeClassificationNameUpdater()
            {
                EmployeeClassificationKey = versionA1.EmployeeClassificationKey,
                EmployeeClassificationName = "Updated " + DateTime.Now.Ticks
            };

            //Update using both repositories
            repositoryA.Update(updateMessage1);
            repositoryB.Update(updateMessage2);

            {
                //get the final version using repository A
                var versionA2 = repositoryA.GetByKey(newKey);
                Assert.IsNotNull(versionA2);
                Assert.AreEqual(newKey, versionA2!.EmployeeClassificationKey);
                Assert.AreEqual(updateMessage2.EmployeeClassificationName, versionA2.EmployeeClassificationName);
                Assert.AreEqual(updateMessage1.IsExempt, versionA2.IsExempt);
                Assert.AreEqual(updateMessage1.IsEmployee, versionA2.IsEmployee);
            }

            {
                //get the final version using repository B
                var versionB2 = repositoryA.GetByKey(newKey);
                Assert.IsNotNull(versionB2);
                Assert.AreEqual(newKey, versionB2!.EmployeeClassificationKey);
                Assert.AreEqual(updateMessage2.EmployeeClassificationName, versionB2.EmployeeClassificationName);
                Assert.AreEqual(updateMessage1.IsExempt, versionB2.IsExempt);
                Assert.AreEqual(updateMessage1.IsEmployee, versionB2.IsEmployee);
            }
        }

        [TestMethod]
        public void PartialUpdate_IndividualParameters()
        {
            var repositoryA = GetRepository();

            //Setup the new record
            var newRecord = new TModel() { EmployeeClassificationName = "Test " + DateTime.Now.Ticks, IsExempt = true, IsEmployee = false };
            var newKey = repositoryA.Create(newRecord);
            Assert.IsTrue(newKey >= 1000, "keys under 1000 were not generated by the database");

            {
                //Get a copy of the record using repository A
                var versionA1 = repositoryA.GetByKey(newKey);
                Assert.IsNotNull(versionA1);
                Assert.AreEqual(newKey, versionA1!.EmployeeClassificationKey);
                Assert.AreEqual(newRecord.EmployeeClassificationName, versionA1.EmployeeClassificationName);
                Assert.AreEqual(newRecord.IsExempt, versionA1.IsExempt);
                Assert.AreEqual(newRecord.IsEmployee, versionA1.IsEmployee);
            }

            //Update the record

            var updatedIsExempt = false;
            var updatedIsEmployee = true;

            repositoryA.UpdateFlags(newKey, updatedIsExempt, updatedIsEmployee);
            {
                //get the final version using repository A
                var versionA2 = repositoryA.GetByKey(newKey);
                Assert.IsNotNull(versionA2);
                Assert.AreEqual(newKey, versionA2!.EmployeeClassificationKey);
                Assert.AreEqual(newRecord.EmployeeClassificationName, versionA2.EmployeeClassificationName);
                Assert.AreEqual(updatedIsExempt, versionA2.IsExempt);
                Assert.AreEqual(updatedIsEmployee, versionA2.IsEmployee);
            }
        }

        [TestMethod]
        public void Create_ParameterCheck()
        {
            var repository = GetRepository();
            Assert.ThrowsException<ArgumentNullException>(() => repository.Create(null!));
        }

        [TestMethod]
        public void Update1_ParameterCheck()
        {
            var repository = GetRepository();
            EmployeeClassificationFlagsUpdater value = null!;
            Assert.ThrowsException<ArgumentNullException>(() => repository.Update(value));
        }

        [TestMethod]
        public void Update2_ParameterCheck()
        {
            var repository = GetRepository();
            EmployeeClassificationNameUpdater value = null!;
            Assert.ThrowsException<ArgumentNullException>(() => repository.Update(value));
        }
    }
}
