using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JingBaiHui.Common;

namespace DAL
{
    /// <summary>
    /// OrderCheck数据访问层
    /// </summary>
    public class OrderCheckDAL : BaseDAL
    {
        #region singleton
        private static readonly OrderCheckDAL instance = new OrderCheckDAL();
        private OrderCheckDAL() { }
        public static OrderCheckDAL Instance
        {
            get { return instance; }
        }
        #endregion

        /// <summary>
        ///  创建
        /// </summary>
        /// <param name="entity"></param>
        public void Create(OrderCheck entity)
        {
            string sql = @"
				        INSERT INTO [dbo].[OrderCheck]
				           	(
			    	            [Id],
	            	            [WarehouseNumber],
	            	            [MaterialNumber],
	            	            [TakeQty],
	            	            [TakeDateTime],
	            	            [Status],
	            	            [TakeUserId],
	            	            [Remark],
	            	            [CreateTime],
	            	            [OrderType]
	              			)
				     	VALUES
				           (
					            @Id,
	            	            @WarehouseNumber,
	            	            @MaterialNumber,
	            	            @TakeQty,
	            	            @TakeDateTime,
	            	            @Status,
	            	            @TakeUserId,
	            	            @Remark,
	            	            @CreateTime,
	            	            @OrderType
	            			)
        			";
            Dictionary<string, object> parameters = new Dictionary<string, object>() 
        	 {
          	      {"@Id",entity.Id},
	      	      {"@WarehouseNumber",entity.WarehouseNumber},
	      	      {"@MaterialNumber",entity.MaterialNumber},
	      	      {"@TakeQty",entity.TakeQty},
	      	      {"@TakeDateTime",entity.TakeDateTime},
	      	      {"@Status",entity.Status},
	      	      {"@TakeUserId",entity.TakeUserId},
	      	      {"@Remark",entity.Remark},
	      	      {"@CreateTime",entity.CreateTime},
	      	      {"@OrderType",entity.OrderType}
	          };
            db.ExecuteNoneQuery(sql, parameters);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        public void Update(OrderCheck entity)
        {
            string sql = @"
                    UPDATE [dbo].[OrderCheck]
                       SET    
	            	            [TakeQty] = @TakeQty,
	            	            [TakeDateTime] = @TakeDateTime,
	            	            [Status] = @Status,
	            	            [TakeUserId] = @TakeUserId,
	            	            [Remark] = @Remark 
	              		WHERE [Id]=@Id
                        ";
            Dictionary<string, object> parameters = new Dictionary<string, object>() 
        	 { 
	      	      {"@Id",entity.Id}, 
	      	      {"@TakeQty",entity.TakeQty},
	      	      {"@TakeDateTime",entity.TakeDateTime},
	      	      {"@Status",entity.Status},
	      	      {"@TakeUserId",entity.TakeUserId},
	      	      {"@Remark",entity.Remark} 
	          };
            db.ExecuteNoneQuery(sql, parameters);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        public void Delete(Guid Id)
        {
            string sql = @" 
                DELETE FROM [dbo].[OrderCheck]
                  WHERE [Id]=@Id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() 
        	{
          	     {"@Id",Id} 
	        };
            db.ExecuteNoneQuery(sql, parameters);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public OrderCheck Get(Guid Id)
        {
            string sql = @"  
SELECT  TOP 1 *
FROM
  ( SELECT A.*,
                 B.USER_NAME
   FROM [dbo].[OrderCheck] A
   LEFT JOIN [dbo].[PM_USER] B ON A.TakeUserId=B.USER_ID) T
WHERE [Id]=@Id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() 
        	{
          	     {"@Id",Id} 
	        };
            return db.GetEntity<OrderCheck>(
                delegate(IDataReader reader, OrderCheck entity)
                {
                    BuildOrderCheck(reader, entity);
                },
                sql, parameters);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IList<OrderCheck> GetList(OrderCheck entity)
        {
            StringBuilder sql = new StringBuilder(@" 
SELECT *
FROM
  ( SELECT  A.*,
                 B.USER_NAME
   FROM [dbo].[OrderCheck] A
   LEFT JOIN [dbo].[PM_USER] B ON A.TakeUserId=B.USER_ID) T ");
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            LoadCondition(entity, sql, ref parameters);

            sql.Append(" ORDER BY CREATETIME DESC ");

            return db.GetList<OrderCheck>(
                 delegate(IDataReader reader, OrderCheck dataModel)
                 {
                     BuildOrderCheck(reader, dataModel);
                 },
                 sql.ToString(), parameters);
        }

        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="entity">查询实体</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">一页显示条数</param>
        /// <param name="order">排序</param>
        /// <returns></returns>
        public IList<OrderCheck> GetList(OrderCheck entity, int pageIndex, int pageSize, string order = Const.Order)
        {
            StringBuilder sql = new StringBuilder(@" 
SELECT *
FROM
  ( SELECT  A.*,
                 B.USER_NAME
   FROM [dbo].[OrderCheck] A
   LEFT JOIN [dbo].[PM_USER] B ON A.TakeUserId=B.USER_ID) T ");
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            LoadCondition(entity, sql, ref parameters);

            return db.GetList<OrderCheck>(
                 delegate(IDataReader reader, OrderCheck dataModel)
                 {
                     BuildOrderCheck(reader, dataModel);
                 },
                 sql.ToString(), parameters, pageIndex, pageSize, order);
        }

        #region private

        /// <summary>
        /// 映射
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entity"></param>
        private void BuildOrderCheck(IDataReader reader, OrderCheck entity)
        {
            entity.Id = reader.GetValue<Guid>("Id");
            entity.WarehouseNumber = reader.GetValue<string>("WarehouseNumber");
            entity.MaterialNumber = reader.GetValue<string>("MaterialNumber");
            entity.TakeQty = reader.GetValue<double>("TakeQty");
            entity.TakeDateTime = reader.GetValue<DateTime>("TakeDateTime");
            entity.Status = (OrderCheckStatus)reader.GetValue<int>("Status");
            entity.TakeUserId = reader.GetValue<string>("TakeUserId");
            entity.Remark = reader.GetValue<string>("Remark");
            entity.CreateTime = reader.GetValue<DateTime>("CreateTime");
            entity.OrderType = (CheckOrderType)reader.GetValue<int>("OrderType");
            entity.TakeUserName = reader.GetValue<string>("USER_NAME");
        }

        /// <summary>
        /// 加载条件
        /// </summary>
        /// <param name="entity">查询实体</param>
        /// <param name="sql">SQL命令</param>
        /// <param name="parameters">参数</param>
        private void LoadCondition(OrderCheck entity, StringBuilder sql, ref Dictionary<string, object> parameters)
        {
            if (entity == null)
                return;

            sql.AppendFormat(" WHERE 1=1 ");

            if (entity.Id != Guid.Empty)
            {
                sql.AppendFormat(" AND [Id]=@Id ");
                parameters.Add("@Id", entity.Id);
            }
            if (!string.IsNullOrEmpty(entity.WarehouseNumber))
            {
                sql.AppendFormat(" AND [WarehouseNumber]=@WarehouseNumber ");
                parameters.Add("@WarehouseNumber", entity.WarehouseNumber);
            }
            if (!string.IsNullOrEmpty(entity.MaterialNumber))
            {
                sql.AppendFormat(" AND [MaterialNumber]=@MaterialNumber ");
                parameters.Add("@MaterialNumber", entity.MaterialNumber);
            }


            if (entity.Status > 0)
            {
                sql.AppendFormat(" AND [Status]=@Status ");
                parameters.Add("@Status", entity.Status);
            }
            if (!string.IsNullOrEmpty(entity.TakeUserId))
            {
                sql.AppendFormat(" AND [TakeUserId]=@TakeUserId ");
                parameters.Add("@TakeUserId", entity.TakeUserId);
            }
            if (!string.IsNullOrEmpty(entity.Remark))
            {
                sql.AppendFormat(" AND [Remark]=@Remark ");
                parameters.Add("@Remark", entity.Remark);
            }

            if (entity.OrderType > 0)
            {
                sql.AppendFormat(" AND [OrderType]=@OrderType ");
                parameters.Add("@OrderType", entity.OrderType);
            }
        }

        #endregion
    }
}
