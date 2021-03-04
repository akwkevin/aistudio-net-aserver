using AutoMapper;
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
    public class D_UserGroupBusiness : BaseBusiness<D_UserGroup>, ID_UserGroupBusiness, ITransientDependency
    {
        readonly IMapper _mapper;
        public D_UserGroupBusiness(IDbAccessor db, IMapper mapper)
            : base(db)
        {
            _mapper = mapper;
        }

        #region 外部接口

        public async Task<PageResult<D_UserGroup>> GetDataListAsync(PageInput<D_UserGroupInputDTO> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<D_UserGroup>();

            //筛选
            if (!input.Search.condition.IsNullOrEmpty() && !input.Search.keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<D_UserGroup, bool>(
                    ParsingConfig.Default, false, $@"{input.Search.condition}.Contains(@0)", input.Search.keyword);
                where = where.And(newWhere);
            }

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<D_UserGroup> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(D_UserGroup data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(D_UserGroup data)
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