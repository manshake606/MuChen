using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kfxms.Entity.Project
{
    public class S_ProjectContractDetail : HBEntity
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
        /// 项目编号
        /// </summary>
        public int? ProjectNum { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 合同名称
        /// </summary>
        public string ProjectContractName { get; set; }

        /// <summary>
        /// 合同详细
        /// </summary>
        public string ProjectContractDetail { get; set; }

        /// <summary>
        /// 合同金额
        /// </summary>
        public decimal? ProjectContractAmount { get; set; }

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
        /// 最后修改用户Id
        /// </summary>
        public Guid? LastEditUserID { get; set; }

        /// <summary>
        /// 最后修改用户姓名
        /// </summary>
        public string LastEditName { get; set; }

    }
}

