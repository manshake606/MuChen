using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kfxms.Entity.SysBasic
{
    public class Sys_Department:HBEntity
    {
                
        /// <summary>
        ///单位ID
        /// </summary>
        [Key]
        public Guid? DptID { get; set; }
        
        
        
        /// <summary>
        ///单位名称
        /// </summary>
        public string DptName { get; set; }
        
        
        
        /// <summary>
        ///上级单位编号
        /// </summary>
        public Guid? ParentDptID { get; set; }
        
        
        
        /// <summary>
        ///1.业务科室、2.单位
        /// </summary>
        public int? Type { get; set; }
        
        
        
        /// <summary>
        ///排序顺序
        /// </summary>
        public int? OrderIndex { get; set; }
        
        
        
        /// <summary>
        ///单位编码
        /// </summary>
        public string DptNO { get; set; }
        
        
        
        /// <summary>
        ///使用状态   -1已删除  1 使用中  0 已停用
        /// </summary>
        public int? State { get; set; }
        
        
        
        /// <summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }
        
        
        
        /// <summary>
        ///添加时间
        /// </summary>
        public DateTime? AddTime { get; set; }
        
        
        
        /// <summary>
        ///添加者ID
        /// </summary>
        public Guid? AddUserID { get; set; }
        
        
        
        /// <summary>
        ///添加者姓名
        /// </summary>
        public string AddName { get; set; }
        
        
        
        /// <summary>
        ///最后修改时间
        /// </summary>
        public DateTime? LastEditTime { get; set; }
        
        
        
        /// <summary>
        ///最后修改者ID
        /// </summary>
        public Guid? LastEditUserID { get; set; }
        
        

    }
}
