using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kfxms.Entity.Payment
{
    public class S_PublicRelationsDetail : HBEntity
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public Guid? Id { get; set; }

        /// <summary>
        /// 公关编号
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
        /// 公关联系人
        /// </summary>
        public string PRContact { get; set; }

        /// <summary>
        /// 公关联系人手机
        /// </summary>
        public string PRContactPhone { get; set; }

        /// <summary>
        /// 公关费用发生时间
        /// </summary>
        public DateTime? PRTime { get; set; }

        /// <summary>
        /// 公关明细
        /// </summary>
        public string PRContent { get; set; }

        /// <summary>
        /// 公关费用
        /// </summary>
        public decimal PRAmout { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? AddTime { get; set; }

        /// <summary>
        /// 添加用户Id
        /// </summary>
        public Guid? AddUserId { get; set; }

    }
}

