using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kfxms.Entity.Project
{
    public class S_Project: HBEntity
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public Guid? Id { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Num { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public int? ClientId { get; set; }

        /// <summary>
        /// 合同金额
        /// </summary>
        public decimal ContractAmout { get; set; }

        /// <summary>
        /// 结账基数
        /// </summary>
        public decimal SettlementBase { get; set; }

        /// <summary>
        /// 项目状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}

