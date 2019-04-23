using DSAA.EntityFrameworkCore.Entity;
using DSAA.Repository.IRepository;
using DSAA.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DSAA.Service
{
    /// <summary>
    /// 服务基类
    /// </summary>
    public abstract class ServiceBase<TEntity, TPrimaryKey> : IService<TEntity, TPrimaryKey> where TEntity : Entity<TPrimaryKey>
    {
        //定义数据访问上下文对象
        protected readonly IRepository<TEntity, TPrimaryKey> _repository;

        /// <summary>
        /// 通过构造函数注入得到数据上下文对象实例
        /// </summary>
        /// <param name="dbContext"></param>
        public ServiceBase(IRepository<TEntity, TPrimaryKey> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <returns></returns>
        public List<TEntity> GetAllList()
        {
            return _repository.GetAllList();
        }

        /// <summary>
        /// 根据lambda表达式条件获取实体集合
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return _repository.GetAllList(predicate);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <returns></returns>
        public TEntity Get(TPrimaryKey id)
        {
            return _repository.Get(id);
        }

        /// <summary>
        /// 根据主键查询实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <returns></returns>
        public TEntity Find(TPrimaryKey id)
        {
            return _repository.Find(id);
        }

        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _repository.FirstOrDefault(predicate);
        }

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public void Insert(TEntity entity)
        {
            _repository.Insert(entity);
        }


        /// <summary>
        /// 增加一列数据
        /// </summary>
        /// <param name="entities">对象实体列表</param>
        /// <returns>实体ID,不成功则返回-1</returns>
        public int InsertList(List<TEntity> entities)
        {
            return _repository.InsertList(entities);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        public void Update(TEntity entity)
        {

            _repository.Update(entity);
        }

        /// <summary>
        /// 新增或更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        public void InsertOrUpdate(TEntity entity)
        {
            if (Get(entity.Id) != null)
                Update(entity);
            else
                Insert(entity);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        public void Delete(TEntity entity)
        {
            _repository.Delete(entity);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">实体主键</param>
        public void Delete(TPrimaryKey id)
        {
            _repository.Delete(id);
        }

        /// <summary>
        /// 事务性保存
        /// </summary>
        public int Save()
        {
            return _repository.Save();
        }

    }

    /// <summary>
    /// 主键为Guid类型的仓储基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public abstract class ServiceBase<TEntity> : ServiceBase<TEntity, Int32> where TEntity : Entity
    {
        public ServiceBase(IRepository<TEntity> repository) : base(repository)
        {
        }
    }
}
