using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kfxms.Entity.Client
{
    public class S_Client: HBEntity
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public Guid? Id { get; set; }

        /// <summary>
        /// 客户编号
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Num { get; set; }


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

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        

    }
}

