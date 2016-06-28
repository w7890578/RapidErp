using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace BLL
{
    public class MarerialScrapLogListManager
    {
        /// <summary>
        /// 根据年份绑定12个月的月中月末
        /// </summary>
        /// <param name="year">选中的年份</param>
        /// <param name="drp">要绑定的月份控件</param>
        public static void GetMonthForYear(string year, DropDownList drp)
        {
            drp.Items.Clear();
            string temp = string.Empty;
            for (int i = 1; i < 13; i++)
            {
                temp = i.ToString();
                if (i < 10)
                {
                    temp = "0" + temp;
                }
                drp.Items.Add(new ListItem(i.ToString() + "月中", year + "-" + temp + "-15"));
                drp.Items.Add(new ListItem(i.ToString() + "月末", year + "-" + temp + "-" + GetDaysForMonthAndYear(Convert.ToInt32(year), i)));
            }
        }

        /// <summary>
        /// 根据年份和月份获取当月的天数
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>天数</returns>
        public static int GetDaysForMonthAndYear(int year, int month)
        {
            return System.DateTime.DaysInMonth(year, month);
        }

        /// <summary>
        /// 绑定年份
        /// </summary>
        /// <param name="drp">年份控件</param>
        public static void BindDrpForYear(DropDownList drp)
        {
            drp.Items.Clear();
            //int yearNow = DateTime.Now.Year;//当前年份

            //for (int i = yearNow; i < yearNow + 10; i++)
            //{
            //    drp.Items.Add(new ListItem(i.ToString() + "年", i.ToString()));
            //}
            //drp.SelectedValue = yearNow.ToString();
            drp.Items.Add(new ListItem("2014年", "2014"));
            drp.Items.Add(new ListItem("2015年", "2015"));
            drp.Items.Add(new ListItem("2016年", "2016"));
            drp.Items.Add(new ListItem("2017年", "2017"));
            drp.Items.Add(new ListItem("2018年", "2018"));
            drp.Items.Add(new ListItem("2019年", "2019"));
            drp.Items.Add(new ListItem("2020年", "2020"));
            drp.Items.Add(new ListItem("2021年", "2021"));
            drp.SelectedValue = DateTime.Now.Year.ToString();

        }



    }
}
