using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel
{
    public class DatableHelper
    {
        /// <summary>
        /// DataTable分页
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="PageIndex">页索引,注意：从1开始</param>
        /// <param name="PageSize">每页大小</param>
        /// <returns></returns>
        public static DataTable GetPagedTable(DataTable dt, int PageIndex, int PageSize)
        {
            //判断当前索引
            if (PageIndex == 0)
                return dt;
            //从数据集合拷贝数据
            DataTable newdt = dt.Copy();
            //数据清空
            newdt.Clear();
            //开始数据索引 = 当前页-1 x 每页大小
            int rowbegin = (PageIndex - 1) * PageSize;
            //结束数据索引 = 当前页 x 每页大小
            int rowend = PageIndex * PageSize;
            //开始数据索引 大于等于 当前数据集合大小
            if (rowbegin >= dt.Rows.Count)
                return newdt;
            //结束数据索引 大于 当前数据集合大小
            if (rowend > dt.Rows.Count)
                rowend = dt.Rows.Count;
            //遍历数据
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }

            return newdt;
        }
    }
}
