using AutoMapper;
using Coldairarrow.Entity.D_Manage;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Coldairarrow.Business.D_Manage
{
    public class D_UserMailBusiness : BaseBusiness<D_UserMail>, ID_UserMailBusiness, ITransientDependency
    {
        readonly IMapper _mapper;
        public D_UserMailBusiness(IDbAccessor db, IMapper mapper)
            : base(db)
        {
            _mapper = mapper;
        }

        #region 外部接口

        public async Task<PageResult<D_UserMail>> GetDataListAsync(PageInput<D_UserMailInputDTO> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<D_UserMail>();

            //筛选
            if (!input.Search.condition.IsNullOrEmpty() && !input.Search.keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<D_UserMail, bool>(
                    ParsingConfig.Default, false, $@"{input.Search.condition}.Contains(@0)", input.Search.keyword);
                where = where.And(newWhere);
            }
            if (!input.Search.userId.IsNullOrEmpty())
            {
                where = where.And(p => p.UserIds.Contains("^" + input.Search.userId + "^"));
            }
            if (!input.Search.creatorId.IsNullOrEmpty())
            {
                where = where.And(p => p.CreatorId.Contains(input.Search.creatorId));
            }
            where = where.And(p => p.IsDraft == input.Search.draft);

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<PageResult<D_UserMail>> GetHistoryDataListAsync(PageInput<D_UserMailInputDTO> input)
        {
            Expression<Func<D_UserMail, bool>> where = LinqHelper.True<D_UserMail>();

            //筛选
            if (!input.Search.condition.IsNullOrEmpty() && !input.Search.keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<D_UserMail, bool>(
                    ParsingConfig.Default, false, $@"{input.Search.condition}.Contains(@0)", input.Search.keyword);
                where = where.And(newWhere);
            }
            if (!input.Search.userId.IsNullOrEmpty())
            {
                where = where.And(p => p.UserIds.Contains("^" + input.Search.userId + "^"));
                if (input.Search.markflag == true)
                {
                    where = where.And(p => p.ReadingMarks == null || !p.ReadingMarks.Contains("^" + input.Search.userId + "^"));
                }
            }
            if (!input.Search.creatorId.IsNullOrEmpty())
            {
                where = where.And(p => p.CreatorId.Contains(input.Search.creatorId));
            }
            where = where.And(p => p.IsDraft == input.Search.draft);

            var dataList = await GetPageHistoryDataList(input, where, input.Search.start, input.Search.end, "CreateTime");

            return dataList;
        }

        public async Task<D_UserMail> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(D_UserMail data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(D_UserMail data)
        {
            await UpdateAsync(data);
        }

        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        public async Task<int> GetReceiveCount(string userId)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<D_UserMail>();
            where = where.And(p => EF.Functions.Contains(p.UserIds, userId));
            where = where.And(p => !EF.Functions.Contains(p.ReadingMarks, userId));
            return await q.Where(where).CountAsync();
        }

        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion
    }


}