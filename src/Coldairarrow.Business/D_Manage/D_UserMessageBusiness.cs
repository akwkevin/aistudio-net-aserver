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

        public async Task<PageResult<D_UserMessage>> GetDataListAsync(PageInput<D_UserMessageInputDTO> pageInput, string condition, string keyword)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<D_UserMessage>();

            //筛选
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<D_UserMessage, bool>(
                    ParsingConfig.Default, false, $@"{condition}.Contains(@0)", keyword);
                where = where.And(newWhere);
            }
            int count = await q.Where(where).CountAsync();

            var list = await q.Where(where).OrderBy($@"{pageInput.SortField} {pageInput.SortType}")
                .Skip((pageInput.PageIndex - 1) * pageInput.PageRows)
                .Take(pageInput.PageRows)
                .ToListAsync();

            return new PageResult<D_UserMessage> { Data = list, Total = count };
        }

        public async Task<D_UserMessage> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(null, id);
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

        public async Task<List<D_UserMessageDTO>> GetHistoryDataDialogListAsync(string condition, string keyword, string creatorId, string creatorAvatar, string userId, string userAvatar, bool isGroup, DateTime? start = null, DateTime? end = null)
        {
            //List<D_UserMessage> dataList = new List<D_UserMessage>();

            //List<DateTime> times = TableTimeFormatHelper.GetAddTimeFormatEndingList(start, end, "yyyyMM");
            //foreach (var time in times)
            //{
            //    string tableSuffix = time.ToString("yyyyMM");
            //    var sqlexist = $"select count(1) from sys.objects where name = '{typeof(D_UserMessage).Name + tableSuffix}'";
            //    var result = SqlQueryAsync(sqlexist, tableSuffix).Result;
            //    if (!(result.Rows.Count == 1 && result.Rows[0].ItemArray.Length == 1 && result.Rows[0][0].ToString() == "1"))
            //    {
            //        continue;
            //    }

            //    Expression<Func<D_UserMessage, bool>> where = x =>
            //     (x.Type == 2);

            //    where = where.And(x =>
            //        (x.CreatorId == creatorId && x.ReceiveId == userId) ||
            //        (x.CreatorId == userId && x.ReceiveId == creatorId)
            //    );

            //    where = where.And(x => EF.Functions.DateDiffDay(start, x.CreateTime) >= 0 && EF.Functions.DateDiffDay(x.CreateTime, end) >= 0);

            //    dataList.AddRange(await GetAllListWhereAsync(where, tableSuffix));

            //}

            Expression<Func<D_UserMessage, bool>> where = LinqHelper.True<D_UserMessage>();

            //筛选
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<D_UserMessage, bool>(
                    ParsingConfig.Default, false, $@"{condition}.Contains(@0)", keyword);
                where = where.And(newWhere);
            }

            if (isGroup == false)
            {
                where = where.And(x => string.IsNullOrEmpty(x.GroupId) &&
                    ((x.CreatorId == creatorId && x.UserIds.Contains("^" + userId + "^")) ||
                    (x.CreatorId == userId && x.UserIds.Contains("^" + creatorId + "^")))
                );
            }
            else
            {
                where = where.And(x =>
                  x.GroupId == userId);
            }

            List<D_UserMessage> dataList = await GetHistoryDataList(where, start, end, "CreateTime");

            var dataListDto = dataList.OrderBy(p => p.CreateTime).Select(p => _mapper.Map<D_UserMessageDTO>(p)).ToList();

            return dataListDto;
        }

        //public async Task<PageResult<D_UserMessageDTO>> GetHistoryDataListAsync(PageInput<D_UserMessageInputDTO> pagination, string condition, string keyword, string creatorId, string userId, bool markflag, bool? isGroup, DateTime? start=null, DateTime? end = null)
        //{
        //    Expression<Func<D_UserMessage, bool>> where = LinqHelper.True<D_UserMessage>();

        //    //筛选
        //    if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
        //    {
        //        var newWhere = DynamicExpressionParser.ParseLambda<D_UserMessage, bool>(
        //            ParsingConfig.Default, false, $@"{condition}.Contains(@0)", keyword);
        //        where = where.And(newWhere);
        //    }

        //    if (isGroup == null)
        //    {
        //        if (!string.IsNullOrEmpty(creatorId))
        //        {
        //            where = where.And(x => x.CreatorId == creatorId || x.GroupId == creatorId);
        //        }
        //    }
        //    else if (isGroup == false)
        //    {
        //        if (!string.IsNullOrEmpty(creatorId))
        //        {
        //            where = where.And(x => string.IsNullOrEmpty(x.GroupId) && x.CreatorId == creatorId);
        //        }
        //    }
        //    else
        //    {
        //        if (!string.IsNullOrEmpty(creatorId))
        //        {
        //            where = where.And(x => x.GroupId == creatorId);
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(userId))
        //    {
        //        if (creatorId != userId)
        //        {
        //            where = where.And(x => x.CreatorId == userId || x.UserIds.Contains("^" + userId + "^"));
        //            if (markflag == true)
        //            {
        //                where = where.And(p => p.ReadingMarks == null || !p.ReadingMarks.Contains("^" + userId + "^"));
        //            }
        //        }
        //        else
        //        {
        //            where = where.And(x => x.UserIds.Contains("^" + userId + "^"));
        //            if (markflag == true)
        //            {
        //                where = where.And(p => p.ReadingMarks == null || !p.ReadingMarks.Contains("^" + userId + "^"));
        //            }
        //        }
        //    }

        //    List<D_UserMessage> dataList = await GetHistoryDataList(pagination, where, start, end, "CreateTime");

        //    var dataListDto = dataList.Select(p => Mapper.Map<D_UserMessageDTO>(p)).ToList();

        //    return dataListDto;
        //}

        //public async Task<List<GroupData>> GetHistoryGroupDataListAsync(string condition, string keyword, string creatorId, string userId, bool markflag, DateTime? start = null, DateTime? end = null)
        //{
        //    Expression<Func<D_UserMessage, bool>> where = LinqHelper.True<D_UserMessage>();

        //    //筛选
        //    if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
        //    {
        //        var newWhere = DynamicExpressionParser.ParseLambda<D_UserMessage, bool>(
        //            ParsingConfig.Default, false, $@"{condition}.Contains(@0)", keyword);
        //        where = where.And(newWhere);
        //    }

        //    if (!string.IsNullOrEmpty(creatorId))
        //    {
        //        where = where.And(x => x.CreatorId == creatorId || x.GroupId == creatorId);
        //    }
        //    if (!string.IsNullOrEmpty(userId))
        //    {
        //        where = where.And(x => x.UserIds.Contains("^" + userId + "^"));
        //        if (markflag == true)
        //        {
        //            where = where.And(p => p.ReadingMarks == null || !p.ReadingMarks.Contains("^" + userId + "^"));
        //        }
        //    }

        //    List<GroupData> dataList = await GetHistoryGroupDataList(where, start, end, "CreateTime");

        //    return dataList;
        //}
        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion
    }






}