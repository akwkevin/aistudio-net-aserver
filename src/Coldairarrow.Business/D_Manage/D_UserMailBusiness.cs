using AutoMapper;
using AutoMapper.QueryableExtensions;
using Coldairarrow.Business.Base_Manage;
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
        readonly IBase_UserBusiness _userBus;
        public D_UserMailBusiness(IDbAccessor db, IMapper mapper, IBase_UserBusiness userBus)
            : base(db)
        {
            _mapper = mapper;
            _userBus = userBus;
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

        #region 历史数据查询
        public async Task<int> GetHistoryDataCountAsync(Input<D_UserMailInputDTO> input)
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

            var count = await GetHistoryDataCount(where, input.Search.start, input.Search.end, "CreateTime");

            return count;
        }
        public async Task<List<D_UserMailDTO>> GetHistoryDataListAsync(Input<D_UserMailInputDTO> input)
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

            var dataList = await GetHistoryDataQueryable(where, input.Search.start, input.Search.end, "CreateTime").ProjectTo<D_UserMailDTO>(_mapper.ConfigurationProvider).ToListAsync();

            return dataList;
        }
        public async Task<PageResult<D_UserMailDTO>> GetPageHistoryDataListAsync(PageInput<D_UserMailInputDTO> input)
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

            var dataList = await GetHistoryDataQueryable(where, input.Search.start, input.Search.end, "CreateTime").ProjectTo<D_UserMailDTO>(_mapper.ConfigurationProvider).GetPageResultAsync(input);
            dataList.Data.ForEach(async p =>
            {
                p.Avatar = await _userBus.GetAvatar(p.CreatorId);
            });

            return dataList;
        }
        #endregion
    }


}