using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kfxms.Entity.Payment
{
    public class S_InternalPayment: HBEntity
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public Guid? Id { get; set; }

        /// <summary>
        /// 内部支付编号
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Num { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public int? ProjectNum { get; set; }

        /// <summary>
        /// 内部付款明细
        /// </summary>
        public string InternalPaymentContent { get; set; }


        /// <summary>
        /// 内部付款金额
        /// </summary>
        public decimal InternalPaymentAmout { get; set; }


        /// <summary>
        /// 内部付款时间
        /// </summary>
        public DateTime? InternalPaymentTime { get; set; }

        /// <summary>
        /// 内部付款人
        /// </summary>
        public int? InternalPaymentUser { get; set; }

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

