using To_DoList.Domain.Entity;
using To_DoList.Domain.Interfaces;

namespace To_DoList.DAL.Repositories;

public class TaskRepository : IBaseRepository<TaskEntity>
{
    private readonly ApplicationDbContext _dbContext;

    public TaskRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task CreateAsync(TaskEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException("Entity is null");

        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public IQueryable<TaskEntity> GetAll()
    {
        return _dbContext.Set<TaskEntity>();
    }

    public async Task Remove(TaskEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException("Entity is null");

        _dbContext.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<TaskEntity> Update(TaskEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException("Entity is null");

        _dbContext.Update(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }
}