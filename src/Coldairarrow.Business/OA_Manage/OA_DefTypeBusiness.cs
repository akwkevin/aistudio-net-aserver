using AutoMapper;
using Coldairarrow.Entity.OA_Manage;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.OA_Manage
{
    public class OA_DefTypeBusiness : BaseBusiness<OA_DefType>, IOA_DefTypeBusiness, ITransientDependency
    {
        readonly IMapper _mapper;
        public OA_DefTypeBusiness(IDbAccessor db, IMapper mapper)
            : base(db)
        {
            _mapper = mapper;
        }

        #region 外部接口

        public async Task<PageResult<OA_DefType>> GetDataListAsync(PageInput<OA_DefTypeInputDTO> pagination, string condition, string keyword)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<OA_DefType>();

            //筛选
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<OA_DefType, bool>(
                    ParsingConfig.Default, false, $@"{condition}.Contains(@0)", keyword);
                where = where.And(newWhere);
            }

            return await q.Where(where).GetPageResultAsync(pagination);
        }

        public async Task<OA_DefType> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(null, id);
        }

        public async Task AddDataAsync(OA_DefType data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(OA_DefType data)
        {
            await UpdateAsync(data);
        }

        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        public async Task<List<OA_DefTypeDTO>> GetTreeDataListAsync(string type = null)
        {
            var where = LinqHelper.True<OA_DefType>();
            if (!type.IsNullOrEmpty())
                where = where.And(x => x.Type == type);


            var list = await GetIQueryable().Where(where).ToListAsync();
            var treeList = list
                .Select(x => new OA_DefTypeDTO
                {
                    ParentId = x.Type == "分类"?null: x.Type,
                    Id = x.Name,
                    Name = x.Name,
                    Type = x.Type,
                    Unit = x.Unit,
                    RealId = x.Id,
                }).ToList();

            return TreeHelper.BuildTree(treeList);
        }

        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion
    }


}