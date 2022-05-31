using AutoMapper;
using Coldairarrow.Entity;
using Coldairarrow.Entity.D_Manage;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Coldairarrow.Business.D_Manage
{
    public class D_UserMessageBusiness : ShardingBaseBusiness<D_UserMessage>, ID_UserMessageBusiness, ITransientDependency
    {
        readonly IMapper _mapper;
        public D_UserMessageBusiness(IShardingDbAccessor shardingDb, IDbAccessor db, IMapper mapper)
            : base(shardingDb, db)
        {
            _mapper = mapper;
        }

        #region 外部接口

        public async Task<PageResult<D_UserMessage>> GetDataListAsync(PageInput<D_UserMessageInputDTO> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<D_UserMessage>();

            //筛选
            if (!input.Search.condition.IsNullOrEmpty() && !input.Search.keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<D_UserMessage, bool>(
                    ParsingConfig.Default, false, $@"{input.Search.condition}.Contains(@0)", input.Search.keyword);
                where = where.And(newWhere);
            }

            //按字典筛选
            if (input.SearchKeyValues != null)
            {
                foreach (var keyValuePair in input.SearchKeyValues)
                {
                    var newWhere = DynamicExpressionParser.ParseLambda<D_UserMessage, bool>(
                        ParsingConfig.Default, false, $@"{keyValuePair.Key}.Contains(@0)", keyValuePair.Value);
                    where = where.And(newWhere);
                }
            }

            int count = await q.Where(where).CountAsync();

            var list = await q.Where(where).OrderBy($@"{input.SortField} {input.SortType}")
                .Skip((input.PageIndex - 1) * input.PageRows)
                .Take(input.PageRows)
                .ToListAsync();

            return new PageResult<D_UserMessage> { Data = list, Total = count };
        }

        public async Task<D_UserMessage> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(D_UserMessage data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(D_UserMessage data)
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

        #region 历史数据查询
        public async Task<int> GetHistoryDataCountAsync(Input<D_UserMessageInputDTO> input)
        {
            Expression<Func<D_UserMessage, bool>> where = LinqHelper.True<D_UserMessage>();

            //筛选
            if (!input.Search.condition.IsNullOrEmpty() && !input.Search.keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<D_UserMessage, bool>(
                    ParsingConfig.Default, false, $@"{input.Search.condition}.Contains(@0)", input.Search.keyword);
                where = where.And(newWhere);
            }

            //按字典筛选
            if (input.SearchKeyValues != null)
            {
                foreach (var keyValuePair in input.SearchKeyValues)
                {
                    var newWhere = DynamicExpressionParser.ParseLambda<D_UserMessage, bool>(
                        ParsingConfig.Default, false, $@"{keyValuePair.Key}.Contains(@0)", keyValuePair.Value);
                    where = where.And(newWhere);
                }
            }

            if (input.Search.isGroup == false)
            {
                where = where.And(x => string.IsNullOrEmpty(x.GroupId) &&
                    ((x.CreatorId == input.Search.creatorId && x.UserIds.Contains("^" + input.Search.userId + "^")) ||
                    (x.CreatorId == input.Search.userId && x.UserIds.Contains("^" + input.Search.creatorId + "^")))
                );
            }
            else
            {
                where = where.And(x =>
                  x.GroupId == input.Search.userId);
            }
            if (input.Search.markflag == true)
            {
                where = where.And(p => p.ReadingMarks == null || !p.ReadingMarks.Contains("^" + input.Search.userId + "^"));
            }

            int count = await GetHistoryDataCount(where, input.Search.start, input.Search.end, "CreateTime");

            return count;
        }
        public async Task<List<D_UserMessage>> GetHistoryDataListAsync(Input<D_UserMessageInputDTO> input)
        {
            Expression<Func<D_UserMessage, bool>> where = LinqHelper.True<D_UserMessage>();

            //筛选
            if (!input.Search.condition.IsNullOrEmpty() && !input.Search.keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<D_UserMessage, bool>(
                    ParsingConfig.Default, false, $@"{input.Search.condition}.Contains(@0)", input.Search.keyword);
                where = where.And(newWhere);
            }

            //按字典筛选
            if (input.SearchKeyValues != null)
            {
                foreach (var keyValuePair in input.SearchKeyValues)
                {
                    var newWhere = DynamicExpressionParser.ParseLambda<D_UserMessage, bool>(
                        ParsingConfig.Default, false, $@"{keyValuePair.Key}.Contains(@0)", keyValuePair.Value);
                    where = where.And(newWhere);
                }
            }

            if (input.Search.isGroup == false)
            {
                where = where.And(x => string.IsNullOrEmpty(x.GroupId) &&
                    ((x.CreatorId == input.Search.creatorId && x.UserIds.Contains("^" + input.Search.userId + "^")) ||
                    (x.CreatorId == input.Search.userId && x.UserIds.Contains("^" + input.Search.creatorId + "^")))
                );
            }
            else
            {
                where = where.And(x =>
                  x.GroupId == input.Search.userId);
            }
            if (input.Search.markflag == true)
            {
                where = where.And(p => p.ReadingMarks == null || !p.ReadingMarks.Contains("^" + input.Search.userId + "^"));
            }

            List<D_UserMessage> dataList = await GetHistoryDataList(where, input.Search.start, input.Search.end, "CreateTime");

            return dataList;
        }

        public async Task<PageResult<D_UserMessage>> GetPageHistoryDataListAsync(PageInput<D_UserMessageInputDTO> input)
        {
            Expression<Func<D_UserMessage, bool>> where = LinqHelper.True<D_UserMessage>();

            //筛选
            if (!input.Search.condition.IsNullOrEmpty() && !input.Search.keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<D_UserMessage, bool>(
                    ParsingConfig.Default, false, $@"{input.Search.condition}.Contains(@0)", input.Search.keyword);
                where = where.And(newWhere);
            }

            //按字典筛选
            if (input.SearchKeyValues != null)
            {
                foreach (var keyValuePair in input.SearchKeyValues)
                {
                    var newWhere = DynamicExpressionParser.ParseLambda<D_UserMessage, bool>(
                        ParsingConfig.Default, false, $@"{keyValuePair.Key}.Contains(@0)", keyValuePair.Value);
                    where = where.And(newWhere);
                }
            }

            if (input.Search.isGroup == null)
            {
                if (!string.IsNullOrEmpty(input.Search.creatorId))
                {
                    where = where.And(x => x.CreatorId == input.Search.creatorId || x.GroupId == input.Search.creatorId);
                }
            }
            else if (input.Search.isGroup == false)
            {
                if (!string.IsNullOrEmpty(input.Search.creatorId))
                {
                    where = where.And(x => string.IsNullOrEmpty(x.GroupId) && x.CreatorId == input.Search.creatorId);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(input.Search.creatorId))
                {
                    where = where.And(x => x.GroupId == input.Search.creatorId);
                }
            }

            if (!string.IsNullOrEmpty(input.Search.userId))
            {
                if (input.Search.creatorId != input.Search.userId)
                {
                    where = where.And(x => x.CreatorId == input.Search.userId || x.UserIds.Contains("^" + input.Search.userId + "^"));
                }
                else
                {
                    where = where.And(x => x.UserIds.Contains("^" + input.Search.userId + "^"));                  
                }
                if (input.Search.markflag == true)
                {
                    where = where.And(p => p.ReadingMarks == null || !p.ReadingMarks.Contains("^" + input.Search.userId + "^"));
                }
            }

            var dataList = await GetPageHistoryDataList(input, where, input.Search.start, input.Search.end, "CreateTime");

            return dataList;
        }

        public async Task<List<GroupData>> GetHistoryGroupDataListAsync(Input<D_UserMessageInputDTO> input)
        {
            Expression<Func<D_UserMessage, bool>> where = LinqHelper.True<D_UserMessage>();

            //筛选
            if (!input.Search.condition.IsNullOrEmpty() && !input.Search.keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<D_UserMessage, bool>(
                    ParsingConfig.Default, false, $@"{input.Search.condition}.Contains(@0)", input.Search.keyword);
                where = where.And(newWhere);
            }

            //按字典筛选
            if (input.SearchKeyValues != null)
            {
                foreach (var keyValuePair in input.SearchKeyValues)
                {
                    var newWhere = DynamicExpressionParser.ParseLambda<D_UserMessage, bool>(
                        ParsingConfig.Default, false, $@"{keyValuePair.Key}.Contains(@0)", keyValuePair.Value);
                    where = where.And(newWhere);
                }
            }

            if (!string.IsNullOrEmpty(input.Search.creatorId))
            {
                where = where.And(x => x.CreatorId == input.Search.creatorId || x.GroupId == input.Search.creatorId);
            }
            if (!string.IsNullOrEmpty(input.Search.userId))
            {
                where = where.And(x => x.UserIds.Contains("^" + input.Search.userId + "^"));
                if (input.Search.markflag == true)
                {
                    where = where.And(p => p.ReadingMarks == null || !p.ReadingMarks.Contains("^" + input.Search.userId + "^"));
                }
            }

            if (input.Search.end == DateTime.MinValue || input.Search.end == null)
            {
                input.Search.end = DateTime.Now;
            }
            if (input.Search.start == DateTime.MinValue || input.Search.start == null)
            {
                input.Search.start = DateTime.Now.AddDays(-30);
            }

            var newWhere2 = DynamicExpressionParser.ParseLambda<D_UserMessage, bool>(
                  ParsingConfig.Default, false, $@"CreateTime > @0 && CreateTime < @1", new object[] { input.Search.start, input.Search.end });
            where = where.And(newWhere2);

            var dataList = (await GetIQueryable().Where(where).ToListAsync()).GroupBy(c => c.CreatorId, (k, g) => new GroupData()
            {
                Total = g.Count(),
                CreatorId = k,
                D_UserMessage = g.Last(),
            }).ToList();

            return dataList;
        }
        #endregion
    }






}