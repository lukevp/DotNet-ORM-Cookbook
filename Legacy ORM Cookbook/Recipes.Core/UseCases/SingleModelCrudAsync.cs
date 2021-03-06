using Recipes.Models;
using Recipes.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Recipes.UseCases
{

    /// <summary>
    /// This set of use cases demonstrates how to perform CRUD operations on a single model.
    /// </summary>
    public abstract class SingleModelCrudAsync<TModel>
        where TModel : IEmployeeClassification, new()
    {

        [Fact]
        public abstract Task CreateAndReadBack();

        public async Task CreateAndReadBack(IEmployeeClassificationAsynchronousRepository<TModel> repository)
        {
            if (repository == null)
                throw new ArgumentNullException("repository", "repository is null.");

            var newRecord = new TModel();
            newRecord.EmployeeClassificationName = "Test " + DateTime.Now.Ticks;
            var newKey = await repository.CreateAsync(newRecord);
            Assert.True(newKey >= 1000); //keys under 1000 were not generated by the database

            var echo = await repository.GetByKeyAsync(newKey);
            Assert.Equal(newKey, echo.EmployeeClassificationKey);
            Assert.Equal(newRecord.EmployeeClassificationName, echo.EmployeeClassificationName);


            var search = await repository.FindByNameAsync(newRecord.EmployeeClassificationName);
            Assert.Equal(newKey, search.EmployeeClassificationKey);
            Assert.Equal(newRecord.EmployeeClassificationName, search.EmployeeClassificationName);
        }

        [Fact]
        public abstract Task CreateAndDelete();

        public async Task CreateAndDelete(IEmployeeClassificationAsynchronousRepository<TModel> repository)
        {
            if (repository == null)
                throw new ArgumentNullException("repository", "repository is null.");

            var newRecord = new TModel();
            newRecord.EmployeeClassificationName = "Test " + DateTime.Now.Ticks;
            var newKey = await repository.CreateAsync(newRecord);
            Assert.True(newKey >= 1000); //keys under 1000 were not generated by the database

            await repository.DeleteAsync(newKey);

            var all = await repository.GetAllAsync();
            Assert.False(all.Any(ec => ec.EmployeeClassificationKey == newKey));
        }

        [Fact]
        public abstract Task GetAll();

        public async Task GetAll(IEmployeeClassificationAsynchronousRepository<TModel> repository)
        {
            if (repository == null)
                throw new ArgumentNullException("repository", "repository is null.");

            var all = await repository.GetAllAsync();
            Assert.NotNull(all);
            Assert.NotEmpty(all);
        }

        [Fact]
        public abstract Task GetByKey();

        public async Task GetByKey(IEmployeeClassificationAsynchronousRepository<TModel> repository)
        {
            if (repository == null)
                throw new ArgumentNullException("repository", "repository is null.");


            var ec1 = await repository.GetByKeyAsync(1);
            var ec2 = await repository.GetByKeyAsync(2);
            var ec3 = await repository.GetByKeyAsync(3);

            Assert.Equal(1, ec1.EmployeeClassificationKey);
            Assert.Equal(2, ec2.EmployeeClassificationKey);
            Assert.Equal(3, ec3.EmployeeClassificationKey);

        }

        [Fact]
        public abstract Task CreateAndUpdate();

        public async Task CreateAndUpdate(IEmployeeClassificationAsynchronousRepository<TModel> repository)
        {
            if (repository == null)
                throw new ArgumentNullException("repository", "repository is null.");

            var newRecord = new TModel();
            newRecord.EmployeeClassificationName = "Test " + DateTime.Now.Ticks;
            var newKey = await repository.CreateAsync(newRecord);
            Assert.True(newKey >= 1000); //keys under 1000 were not generated by the database

            var echo = await repository.GetByKeyAsync(newKey);
            Assert.Equal(newRecord.EmployeeClassificationName, echo.EmployeeClassificationName);
            echo.EmployeeClassificationName = "Updated " + DateTime.Now.Ticks;
            await repository.UpdateAsync(echo);

            var updated = await repository.GetByKeyAsync(newKey);
            Assert.Equal(echo.EmployeeClassificationName, updated.EmployeeClassificationName);


        }
    }
}
