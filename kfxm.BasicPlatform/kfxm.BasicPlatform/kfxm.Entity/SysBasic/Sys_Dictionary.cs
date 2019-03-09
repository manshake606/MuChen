using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kfxms.Entity.SysBasic
{
    public class Sys_Dictionary:HBEntity
    {
                
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public Guid? ID { get; set; }
        
        
        
        /// <summary>
        ///父ID
        /// </summary>
        public Guid? ParentID { get; set; }
        
        
        
        /// <summary>
        ///类别名称
        /// </summary>
        public string TypeName { get; set; }
        
        
        
        /// <summary>
        ///项目内容
        /// </summary>
        public string Item { get; set; }
        
        
        
        /// <summary>
        ///状态(-1删除   0正常使用  1停用)
        /// </summary>
        public int? State { get; set; }
        
        
        
        /// <summary>
        ///排序号
        /// </summary>
        public int? OrderIndex { get; set; }
        
        
        
        /// <summary>
        ///添加时间
        /// </summary>
        public DateTime? AddTime { get; set; }
        
        
        
        /// <summary>
        ///添加人员ID
        /// </summary>
        public Guid? AddUserID { get; set; }
        
        
        
        /// <summary>
        ///添加人员姓名
        /// </summary>
        public string AddName { get; set; }
        
        
        
        /// <summary>
        ///最后修改时间
        /// </summary>
        public DateTime? LastEditTime { get; set; }
        
        
        
        /// <summary>
        ///最后修改人员ID
        /// </summary>
        public Guid? LastEditUserID { get; set; }
        
        

    }
}
