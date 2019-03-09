using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kfxms.Entity.Supplier
{
    public class S_Task:HBEntity
    {
                
        /// <summary>
        ///办件类型ID
        /// </summary>
        [Key]
        public Guid? ID { get; set; }
        
        
        
        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }
        
        
        
        /// <summary>
        ///办件类型名称
        /// </summary>
        public string Name { get; set; }
        
        
        
        /// <summary>
        ///期号/会议期号
        /// </summary>
        public string Number { get; set; }
        
        
        
        /// <summary>
        ///基础数据：解决类别：A（当年能够解决的）、B（列入计划待落实）、C（解决不了的）
        /// </summary>
        public Guid? SolutionType { get; set; }
        
        
        
        /// <summary>
        ///事项类别ID
        /// </summary>
        public Guid? TaskTypeID { get; set; }
        
        
        
        /// <summary>
        ///办件类型ID
        /// </summary>
        public Guid? OfficeTypeID { get; set; }
        
        
        
        /// <summary>
        ///紧急程度ID
        /// </summary>
        public Guid? EmergencyLevelID { get; set; }
        
        
        
        /// <summary>
        ///完成期限
        /// </summary>
        public DateTime? Deadlines { get; set; }
        
        
        
        /// <summary>
        ///批示领导ID
        /// </summary>
        public string LeaderName { get; set; }
        
        
        
        /// <summary>
        ///事项来源ID
        /// </summary>
        public Guid? EventSourceID { get; set; }
        
        
        
        /// <summary>
        ///主要内容(议定事项)
        /// </summary>
        public string AgreedMatters { get; set; }
        
        
        
        /// <summary>
        ///交接时间
        /// </summary>
        public DateTime? TransferTime { get; set; }
        
        
        
        /// <summary>
        ///交接人员ID
        /// </summary>
        public Guid? TransferUserID { get; set; }
        
        
        
        /// <summary>
        ///交接人员姓名
        /// </summary>
        public string TransferUserName { get; set; }
        
        
        
        /// <summary>
        ///分解个数
        /// </summary>
        public int? DecompositionNum { get; set; }
        
        
        
        /// <summary>
        ///下达时间
        /// </summary>
        public DateTime? ReleaseDate { get; set; }
        
        
        
        /// <summary>
        ///单位反馈时字符限制
        /// </summary>
        public int? WordLimit { get; set; }
        
        
        
        /// <summary>
        ///状态(-1：删除,0：正常,1：停用)
        /// </summary>
        public int? State { get; set; }
        
        
        
        /// <summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }
        
        
        
        /// <summary>
        ///添加时间
        /// </summary>
        public DateTime? AddTime { get; set; }
        
        
        
        /// <summary>
        ///添加者ID
        /// </summary>
        public Guid? AddUserID { get; set; }
        
        
        
        /// <summary>
        ///添加者姓名
        /// </summary>
        public string AddUserName { get; set; }
        
        
        
        /// <summary>
        ///最后修改时间
        /// </summary>
        public DateTime? LastEditTime { get; set; }
        
        
        
        /// <summary>
        ///最后修改人员ID
        /// </summary>
        public Guid? LastEditUserID { get; set; }
        
        
        
        /// <summary>
        ///最后修改人员姓名
        /// </summary>
        public string LastEditUserName { get; set; }
        
        
        
        /// <summary>
        ///是否短信反馈(0:不需要,1:需要)
        /// </summary>
        public int? IsMessageFeedback { get; set; }
        
        
        
        /// <summary>
        ///查阅标示(0:未阅,1:已阅)
        /// </summary>
        public int? SeeIdentity { get; set; }
        
        
        
        /// <summary>
        ///联系人
        /// </summary>
        public string Linkman { get; set; }
        
        
        
        /// <summary>
        ///联系电话
        /// </summary>
        public string LinkmanTel { get; set; }
        
        
        
        /// <summary>
        ///是否重点督查（1：是，0：否）
        /// </summary>
        public int? IsKeyProject { get; set; }
        
        
        
        /// <summary>
        ///是否公开（1：是，0：否）
        /// </summary>
        public int? IsOpen { get; set; }
        
        
        
        /// <summary>
        ///代表/委员姓名
        /// </summary>
        public string RepresentativeName { get; set; }
        
        
        
        /// <summary>
        ///批文时间
        /// </summary>
        public DateTime? ApprovalTime { get; set; }
        
        
        
        /// <summary>
        ///一级分类
        /// </summary>
        public Guid? ZXDC_YJAssortment { get; set; }
        
        
        
        /// <summary>
        ///二级分类
        /// </summary>
        public Guid? ZXDC_EJAssortment { get; set; }
        
        
        
        /// <summary>
        ///项目法人或牵头单位
        /// </summary>
        public string ZXDC_LeadUnit { get; set; }
        
        
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ZXDC_StartTime { get; set; }
        
        
        
        /// <summary>
        ///开工时间
        /// </summary>
        public DateTime? ZXDC_MakespanTime { get; set; }
        
        
        
        /// <summary>
        ///总投资
        /// </summary>
        public decimal? ZXDC_TotalInvestment { get; set; }
        
        
        
        /// <summary>
        ///本店度计划总投资
        /// </summary>
        public decimal? ZXDC_TotalInvestmentThisYear { get; set; }
        
        
        
        /// <summary>
        ///主要建设内容和规模
        /// </summary>
        public string ZXDC_BuildContent { get; set; }
        
        
        
        /// <summary>
        ///年底累计完成投资
        /// </summary>
        public decimal? ZXDC_EndInvestment { get; set; }
        
        
        
        /// <summary>
        ///主导分类
        /// </summary>
        public Guid? ZXDC_LeadingType { get; set; }
        
        
        
        /// <summary>
        ///区级联系领导
        /// </summary>
        public string ZXDC_Leader { get; set; }
        
        
        
        /// <summary>
        ///区级责任领导
        /// </summary>
        public string ZXDC_PersonLiable { get; set; }
        
        
        
        /// <summary>
        ///填报要求
        /// </summary>
        public string Requirement { get; set; }
        
        
        
        /// <summary>
        ///回复内容
        /// </summary>
        public string ZWZTC_ReplyContent { get; set; }
        
        
        
        /// <summary>
        ///回复时间
        /// </summary>
        public DateTime? ZWZTC_ReplyTime { get; set; }
        
        
        
        /// <summary>
        ///回复人员姓名
        /// </summary>
        public string ZWZTC_ReplyUserName { get; set; }
        
        
        
        /// <summary>
        ///回复人员ID
        /// </summary>
        public Guid? ZWZTC_ReplyUserID { get; set; }
        
        

    }
}

