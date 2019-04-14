using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kfxms.Entity.SupplierType
{
    public class S_SupplierType: HBEntity
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public int? SupplierTypeId { get; set; }

        /// <summary>
        /// 供应商类型
        /// </summary>
        public string SupplierTypeName { get; set; }

        /// <summary>
        /// 法人姓名
        /// </summary>
        public int? IsActive { get; set; }

    }
}

