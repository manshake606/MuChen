using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using kfxms.Entity;
namespace kfxms.Common
{


    public class OrderByHelper<TEntity>  where TEntity : HBEntity
    {

        public OrderByType OrderByType { get; set; }
        private LambdaExpression  Expression;
        
        public void SetExpression<TProp>(Expression<Func<TEntity, TProp>> expression) 
        {
            this.Expression = expression;
        }

        public dynamic GetExpression()
        {
            return this.Expression;
        }

        public OrderByHelper()
        {

        }

    }

    public class OrderByHelper<TEntity, T> where TEntity : HBEntity 
    {
        public OrderByType OrderByType { get; set; }
        public Expression<Func<TEntity, T>> Expression { get; set; }
    }


}
