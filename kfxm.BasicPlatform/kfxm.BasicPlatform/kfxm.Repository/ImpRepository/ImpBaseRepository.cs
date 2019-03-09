using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kfxms.IRepository;
using kfxms.Entity;
using System.Data.Entity;
using System.ComponentModel.Composition;
using kfxms.EFData;
using System.Linq.Expressions;
using kfxms.Common;
namespace kfxms.ImpRepository
{
    public class ImpBaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : HBEntity
    {

        /// <summary>
        ///EntityFramework的数据仓储上下文
        /// </summary>
        protected DbContext Context
        {
            get { return this.EFDbContext; }
            private set { this.Context = value; }
        }

        [Import("DefaultData")]
        private EFDataContext EFDbContext { get; set; }

        /// <summary>
        ///  获取当前实体的数据集
        /// </summary>
        protected DbSet<TEntity> entitys 
        {
            get {return Context.Set<TEntity>(); }
            set { this.entitys = value; }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体对象 </param>
        /// <returns>操作影响的行数 </returns>
        public int Add(TEntity entity)
        {
            EntityState state = Context.Entry(entity).State;
            if (state == EntityState.Detached) 
            {
                Context.Entry(entity).State = EntityState.Added;
            }
            this.entitys.Add(entity);
            int num = Context.SaveChanges();
            return num;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Add(IEnumerable<TEntity> entitys)
        {
            int num = 0;
            try
            {
                Context.Configuration.AutoDetectChangesEnabled = false;
                foreach (TEntity entity in entitys)
                {
                    EntityState state = Context.Entry(entity).State;
                    if (state == EntityState.Detached)
                    {
                        Context.Entry(entity).State = EntityState.Added;
                    }
                    this.entitys.Add(entity);
                }
                num = Context.SaveChanges();
            }
            finally
            {
                Context.Configuration.AutoDetectChangesEnabled = true;
            }
            return num;
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(object id)
        {
            TEntity entity = GetByKey(id);
            int num = Delete(entity);
            return num;
        }

        /// <summary>
        ///  删除实体记录
        /// </summary>
        /// <param name="entity">实体对象 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
            int num = Context.SaveChanges();
            return num;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(IEnumerable<TEntity> entitys)
        {
            int num = 0;
            try
            {
                Context.Configuration.AutoDetectChangesEnabled = false;
                foreach (TEntity entity in entitys)
                {
                    Context.Entry(entity).State = EntityState.Deleted;
                }
                num = Context.SaveChanges();
            }
            finally
            {
                Context.Configuration.AutoDetectChangesEnabled = true;
            }
            return num;
        }

        /// <summary>
        ///  删除所有符合特定表达式的数据
        /// </summary>
        /// <param name="where">查询条件谓语表达式 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(Expression<Func<TEntity, bool>> where)
        {
           IEnumerable<TEntity> entitys = this.entitys.Where(where).ToList<TEntity>();
           return Delete(entitys);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        public int Update(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached) 
            {
                Context.Set<TEntity>().Attach(entity);
            }
            Context.Entry(entity).State = EntityState.Modified;
            int num = Context.SaveChanges();
            return num;
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Update(IEnumerable<TEntity> entitys)
        {
            int num = 0;
            try
            {
                Context.Configuration.AutoDetectChangesEnabled = false;
                foreach (TEntity entity in entitys)
                {
                    Context.Entry(entity).State = EntityState.Modified;

                }
                num = Context.SaveChanges();
            }
            finally
            {
                Context.Configuration.AutoDetectChangesEnabled = true;
            }
            return num;
        }


        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public TEntity GetByKey(object key)
        {
            TEntity entity =this.entitys.Find(key);
            return entity;
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAllData() 
        {
            return this.entitys.ToList<TEntity>();
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<TEntity> GetWhereData(Expression<Func<TEntity, bool>> where)
        {
            return this.entitys.Where(where).ToList<TEntity>();   
        }


        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<TEntity> GetWhereData<T>(Expression<Func<TEntity, bool>> where, OrderByHelper<TEntity, T> orderBy)
        {

            var ds = this.entitys.Where(where);
            if (orderBy != null)
            {

                if (orderBy.OrderByType == OrderByType.ASC)
                {

                   ds = ds.OrderBy(orderBy.Expression);
                }
                else if (orderBy.OrderByType == OrderByType.DESC)
                {
                  ds = ds.OrderByDescending(orderBy.Expression);
                }

            }
            return ds.ToList();
        }


        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<TEntity> GetWhereData(Expression<Func<TEntity, bool>> where, params OrderByHelper<TEntity>[] orderBy)
        {

            var ds = this.entitys.Where(where);

            if (orderBy != null && orderBy.Count() > 0)
            {
                foreach (OrderByHelper<TEntity> orderByHelper in orderBy)
                {
                    if (orderByHelper.OrderByType == OrderByType.ASC)
                    {

                        dynamic lambda = orderByHelper.GetExpression();
                        ds = Queryable.OrderBy(ds, lambda);
                    }
                    else if (orderByHelper.OrderByType == OrderByType.DESC)
                    {
                        dynamic lambda = orderByHelper.GetExpression();
                        ds = Queryable.OrderByDescending(ds, lambda);
                    }
                }
            }
             
            return ds.ToList();
        }



        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">记录条数</param>
        /// <param name="Total">总条数</param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetPageDate<T>(Expression<Func<TEntity, bool>> where, int pageIndex, int pageSize, out int total, OrderByHelper<TEntity, T> orderBy)
        {
            var ds = this.entitys.Where(where);

            total = ds.ToList().Count;

            if (orderBy != null)
            {

                if (orderBy.OrderByType == OrderByType.ASC)
                {

                    ds = ds.OrderBy(orderBy.Expression);
                }
                else if (orderBy.OrderByType == OrderByType.DESC)
                {
                    ds = ds.OrderByDescending(orderBy.Expression);
                }

            }
            ds = ds.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return ds.ToList();
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">记录条数</param>
        /// <param name="Total">总条数</param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetPageDate(Expression<Func<TEntity, bool>> where, int pageIndex, int pageSize, out int total, params OrderByHelper<TEntity>[] orderBy) 
        {
            var ds = this.entitys.Where(where);
           
            total = ds.ToList().Count;
            
            if (orderBy != null && orderBy.Count() > 0)
            {
                foreach (OrderByHelper<TEntity> orderByHelper in orderBy)
                {
                    if (orderByHelper.OrderByType == OrderByType.ASC)
                    {

                        dynamic lambda = orderByHelper.GetExpression();
                        ds = Queryable.OrderBy(ds, lambda);
                        
                        
                    }
                    else if (orderByHelper.OrderByType == OrderByType.DESC)
                    {
                        dynamic lambda = orderByHelper.GetExpression();
                        ds = Queryable.OrderByDescending(ds, lambda);
                    }
                }
            }
            ds = ds.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return ds.ToList();

        }



        
    }
}
