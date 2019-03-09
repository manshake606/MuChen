using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kfxms.Entity.SysBasic
{
    public class Sys_MenuAuthority : HBEntity
    {

        /// <summary>
        /// 主键Id
        /// </summary>
        [Key]
        public Guid? Id { get; set; }


        /// <summary>
        /// 自增Id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Num { get; set; }


        /// <summary>
        /// 菜单id
        /// </summary>
        public Guid? MenuId { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string AuthorityName { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        public int? OrderValue { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? AddTime { get; set; }

        /// <summary>
        /// 添加用户Id
        /// </summary>
        public Guid? AddUserId { get; set; }

        /// <summary>
        ///  添加用户姓名
        /// </summary>
        public string AddName { get; set; }

        /// <summary>
        ///最后修改时间
        /// </summary>
        public DateTime? LastEditTime { get; set; }

        /// <summary>
        /// 最后修改用户Id
        /// </summary>
        public Guid? LastEditUserID { get; set; }

        /// <summary>
        /// 最后修改用户姓名
        /// </summary>
        public string LastEditName { get; set; }


    }
}
