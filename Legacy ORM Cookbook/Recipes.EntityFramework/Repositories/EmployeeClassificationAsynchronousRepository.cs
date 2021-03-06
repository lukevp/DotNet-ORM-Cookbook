using Recipes.EF.Models;
using Recipes.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.EF.Repositories
{



    public class EmployeeClassificationAsynchronousRepository : IEmployeeClassificationAsynchronousRepository<EmployeeClassification>
    {

        public async Task<int> CreateAsync(EmployeeClassification classification)
        {
            using (var context = new OrmCookbook())
            {
                context.EmployeeClassifications.Add(classification);
                await context.SaveChangesAsync();
                return classification.EmployeeClassificationKey;
            }
        }

        public async Task DeleteAsync(int employeeClassificationKey)
        {
            using (var context = new OrmCookbook())
            {
                var temp = await context.EmployeeClassifications.FindAsync(employeeClassificationKey);
                if (temp != null)
                {
                    context.EmployeeClassifications.Remove(temp);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteAsync(EmployeeClassification classification)
        {
            using (var context = new OrmCookbook())
            {
                var temp = await context.EmployeeClassifications.FindAsync(classification.EmployeeClassificationKey);
                if (temp != null)
                {
                    context.EmployeeClassifications.Remove(temp);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<EmployeeClassification> FindByNameAsync(string employeeClassificationName)
        {
            using (var context = new OrmCookbook())
            {
                return await context.EmployeeClassifications.Where(ec => ec.EmployeeClassificationName == employeeClassificationName).SingleOrDefaultAsync();
            }
        }

        public async Task<IList<EmployeeClassification>> GetAllAsync()
        {
            using (var context = new OrmCookbook())
            {
                return await context.EmployeeClassifications.ToListAsync();
            }
        }

        public async Task<EmployeeClassification> GetByKeyAsync(int employeeClassificationKey)
        {
            using (var context = new OrmCookbook())
            {
                return await context.EmployeeClassifications.FindAsync(employeeClassificationKey);
            }
        }


        public async Task UpdateAsync(EmployeeClassification classification)
        {
            using (var context = new OrmCookbook())
            {
                var temp = await context.EmployeeClassifications.FindAsync(classification.EmployeeClassificationKey);
                temp.EmployeeClassificationName = classification.EmployeeClassificationName;
                await context.SaveChangesAsync();
            }
        }
    }
}


