﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kfxms.Entity.Project
{
    public class S_ProjectInfo
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
        /// 收款比例
        /// </summary>
        public string RevenueRate { get; set; }

        /// <summary>
        /// 总收款额
        /// </summary>
        public decimal SumRevenue { get; set; }


        /// <summary>
        /// 总内部付款额
        /// </summary>
        public decimal SumInternalPayment { get; set; }

        /// <summary>
        /// 总外部付款额
        /// </summary>
        public decimal SumExternalPayment { get; set; }

        /// <summary>
        /// 总公关费用
        /// </summary>
        public decimal SumPublicRelations { get; set; }

        /// <summary>
        /// 总开票额
        /// </summary>
        public decimal SumInvoice { get; set; }

        /// <summary>
        /// 总付款额
        /// </summary>
        public decimal SumPayment { get; set; }

        /// <summary>
        /// 项目状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 项目状态名称
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 甲方名称
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// 统一社会信用编号
        /// </summary>
        public string SocialSecurityNum { get; set; }

        /// <summary>
        /// 甲方地址
        /// </summary>
        public string ClientAddress { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string TelephoneNum { get; set; }

        /// <summary>
        /// 开户银行
        /// </summary>
        public string Bank { get; set; }

        /// <summary>
        /// 银行账号
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 甲方联系人电话
        /// </summary>
        public string ClientContactMobile { get; set; }

        /// <summary>
        /// 甲方联系人
        /// </summary>
        public string ClientContact { get; set; }

        /// <summary>
        /// 甲方联系人职位
        /// </summary>
        public string ClientContactPosition { get; set; }


    }
}

