using AutoMapper;
using AutoMapper.QueryableExtensions;
using Coldairarrow.Entity.Quartz_Manage;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.Quartz_Manage
{
    public class Quartz_TaskBusiness : BaseBusiness<Quartz_Task>, IQuartz_TaskBusiness, ITransientDependency
    {
        readonly IMapper _mapper;

        public Quartz_TaskBusiness(IDbAccessor db, IMapper mapper)
            : base(db)
        {
            _mapper = mapper;
        }

        #region 外部接口

        public async Task<PageResult<Quartz_Task>> GetDataListAsync(PageInput<Quartz_TaskInputDTO> pagination, string condition, string keyword)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<Quartz_Task>();

            //筛选
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<Quartz_Task, bool>(
                    ParsingConfig.Default, false, $@"{condition}.Contains(@0)", keyword);
                where = where.And(newWhere);
            }

            //按字典筛选
            if (pagination.SearchKeyValues != null)
            {
                foreach (var keyValuePair in pagination.SearchKeyValues)
                {
                    var newWhere = DynamicExpressionParser.ParseLambda<Quartz_Task, bool>(
                        ParsingConfig.Default, false, $@"{keyValuePair.Key}.Contains(@0)", keyValuePair.Value);
                    where = where.And(newWhere);
                }
            }

            return await q.Where(where).ProjectTo<Quartz_Task>(_mapper.ConfigurationProvider).GetPageResultAsync(pagination);
        }

        public async Task<Quartz_Task> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(Quartz_Task data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(Quartz_Task data)
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

        #region 数据模型

        #endregion
    }


}