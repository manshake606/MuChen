using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kfxms.Entity.SysBasic
{
    public class Sys_AuthorityRequest:HBEntity
    {
                
        /// <summary>
        ///主键id
        /// </summary>
        [Key]
        public Guid? Id { get; set; }
        
        
        
        /// <summary>
        ///自增字段
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Num { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
        public Guid? AuthorityId { get; set; }
        
        
        /// <summary>
        ///请求URL
        /// </summary>
        public string Url { get; set; }
        
        
        
        /// <summary>
        ///按钮id
        /// </summary>
        public string ButtonId { get; set; }
        
        
        
        /// <summary>
        ///添加时间
        /// </summary>
        public DateTime? AddTime { get; set; }
        
        
        
        /// <summary>
        ///添加用户id
        /// </summary>
        public Guid? AddUserId { get; set; }
        
        
        
        /// <summary>
        ///添加用户姓名
        /// </summary>
        public string AddName { get; set; }
        
        
        
        /// <summary>
        ///最后修改时间
        /// </summary>
        public DateTime? LastEditTime { get; set; }
        
        
        
        /// <summary>
        ///最后修改用户Id
        /// </summary>
        public Guid? LastEditUserID { get; set; }
        
        
        
        /// <summary>
        ///最后修改用户姓名
        /// </summary>
        public string LastEditName { get; set; }
        
        

    }
}
