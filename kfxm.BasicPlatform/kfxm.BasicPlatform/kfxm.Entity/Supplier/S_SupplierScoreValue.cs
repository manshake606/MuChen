using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kfxms.Entity.Supplier
{
    public class S_SupplierScoreValue : HBEntity
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public int? SupplierScoreValueId { get; set; }

        /// <summary>
        /// 供应商打分值
        /// </summary>
        public string SupplierScoreValueScore { get; set; }

    }
}

