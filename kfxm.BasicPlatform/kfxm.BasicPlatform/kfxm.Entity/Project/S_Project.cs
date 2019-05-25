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

        /// <summary>
        /// 设计主管
        /// </summary>
        public int? CoreDesigner { get; set; }

        /// <summary>
        /// 设计助理
        /// </summary>
        public int? AssistantDesigner { get; set; }

        /// <summary>
        /// 业务主管
        /// </summary>
        public int? BusinessManager { get; set; }

        /// <summary>
        /// 业务助理
        /// </summary>
        public int? BusinessAssistant { get; set; }

        /// <summary>
        /// 项目经理
        /// </summary>
        public int? ProjectManager { get; set; }

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

