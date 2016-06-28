namespace BLL
{
    using DAL;
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class OrderCheckBLL
    {
        private static readonly BLL.OrderCheckBLL instance = new BLL.OrderCheckBLL();

        private OrderCheckBLL()
        {
        }

        public void Create(OrderCheck entity)
        {
            this.Validate(entity);
            OrderCheck check = new OrderCheck();
            check.WarehouseNumber = entity.WarehouseNumber;
            check.MaterialNumber = entity.MaterialNumber; 
            IList<OrderCheck> list = this.GetList(check);
            if ((list != null) && (list.Count > 0))
            {
                throw new Exception(string.Format("当前出入库单已抽检原材料：{0},请勿重复抽检。", entity.MaterialNumber));
            }
            OrderCheckDAL.Instance.Create(entity);
        }

        public void Delete(Guid Id)
        {
            OrderCheckDAL.Instance.Delete(Id);
        }

        public OrderCheck Get(Guid Id)
        {
            return OrderCheckDAL.Instance.Get(Id);
        }

        public IList<OrderCheck> GetList(OrderCheck entity)
        {
            return OrderCheckDAL.Instance.GetList(entity);
        }

        public IList<OrderCheck> GetList(OrderCheck entity, int pageIndex, int pageSize, string order = " CreateTime desc ")
        {
            return OrderCheckDAL.Instance.GetList(entity, pageIndex, pageSize, order);
        }

        public void Update(OrderCheck entity)
        {
            this.Validate(entity);
            OrderCheckDAL.Instance.Update(entity);
        }

        private void Validate(OrderCheck entity)
        {
            if (!BLL.MarerialInfoTableManager.IsExit(entity.MaterialNumber))
            {
                throw new Exception("系统不存在该原材料编号：" + entity.MaterialNumber);
            }
            entity.TakeUserId= BLL.PM_UserManager.Instance.GetUserIdByUserName(entity.TakeUserName) ;
            if (string.IsNullOrEmpty(entity.TakeUserId))
            {
                throw new Exception(string.Format("系统不存在该用户:{0}", entity.TakeUserName));
            }
        }

        public static BLL.OrderCheckBLL Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
