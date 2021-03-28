using Coldairarrow.Entity.D_Manage;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.D_Manage
{
    public class D_NoticeReadingMarksBusiness : BaseBusiness<D_NoticeReadingMarks>, ID_NoticeReadingMarksBusiness, ITransientDependency
    {
        public D_NoticeReadingMarksBusiness(IDbAccessor db)
            : base(db)
        {
        }

        #region 外部接口

        public async Task<PageResult<D_NoticeReadingMarks>> GetDataListAsync(PageInput<ConditionDTO> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<D_NoticeReadingMarks>();
            var search = input.Search;

            //筛选
            if (!search.condition.IsNullOrEmpty() && !search.keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<D_NoticeReadingMarks, bool>(
                    ParsingConfig.Default, false, $@"{search.condition}.Contains(@0)", search.keyword);
                where = where.And(newWhere);
            }

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<D_NoticeReadingMarks> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(D_NoticeReadingMarks data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(D_NoticeReadingMarks data)
        {
            await UpdateAsync(data);
        }

        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        #endregion

        #region 私有成员

        #endregion
    }
}