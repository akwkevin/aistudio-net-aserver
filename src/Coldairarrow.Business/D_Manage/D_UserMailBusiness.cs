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

        public async Task<PageResult<D_UserMail>> GetDataListAsync(PageInput<D_UserMailInputDTO> pagination, string condition, string keyword, string userId, string creatorId, bool draft)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<D_UserMail>();

            //筛选
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<D_UserMail, bool>(
                    ParsingConfig.Default, false, $@"{condition}.Contains(@0)", keyword);
                where = where.And(newWhere);
            }
            if (!userId.IsNullOrEmpty())
            {
                where = where.And(p => p.UserIds.Contains("^" + userId + "^"));
            }
            if (!creatorId.IsNullOrEmpty())
            {
                where = where.And(p => p.CreatorId.Contains(creatorId));
            }
            where = where.And(p => p.IsDraft == draft);

            return await q.Where(where).GetPageResultAsync(pagination);
        }

        public async Task<PageResult<D_UserMail>> GetHistoryDataListAsync(PageInput<D_UserMailInputDTO> pagination, string condition, string keyword, string userId, string creatorId, bool draft, bool markflag, DateTime? start = null, DateTime? end = null)
        {
            Expression<Func<D_UserMail, bool>> where = LinqHelper.True<D_UserMail>();

            //筛选
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<D_UserMail, bool>(
                    ParsingConfig.Default, false, $@"{condition}.Contains(@0)", keyword);
                where = where.And(newWhere);
            }
            if (!userId.IsNullOrEmpty())
            {
                where = where.And(p => p.UserIds.Contains("^" + userId + "^"));
                if (markflag == true)
                {
                    where = where.And(p => p.ReadingMarks == null || !p.ReadingMarks.Contains("^" + userId + "^"));
                }
            }
            if (!creatorId.IsNullOrEmpty())
            {
                where = where.And(p => p.CreatorId.Contains(creatorId));
            }
            where = where.And(p => p.IsDraft == draft);

            var dataList = await GetPageHistoryDataList(pagination, where, start, end, "CreateTime");

            return dataList;
        }

        public async Task<D_UserMail> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(null, id);
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