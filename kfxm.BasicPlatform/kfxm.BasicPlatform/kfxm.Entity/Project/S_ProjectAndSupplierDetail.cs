using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kfxms.Entity.Project
{
    public class S_ProjectAndSupplierDetail : HBEntity
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public Guid? Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Num { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? ProjectNum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? SupplierNum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 供应商打分
        /// </summary>
        public int? SupplierScore { get; set; }

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

    }
}

