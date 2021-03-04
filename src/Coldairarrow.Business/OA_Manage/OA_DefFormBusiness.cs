using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class OA_DefFormBusiness : BaseBusiness<OA_DefForm>, IOA_DefFormBusiness, ITransientDependency
    {
        readonly IMapper _mapper;
        public OA_DefFormBusiness(IDbAccessor db, IMapper mapper)
            : base(db)
        {
            _mapper = mapper;
        }

        #region 外部接口

        public async Task<List<OA_DefFormTree>> GetTreeDataListAsync(string type, List<string> roleidlist)
        {
            if (roleidlist == null)
            {
                roleidlist = new List<string>();
            }

            var where = LinqHelper.True<OA_DefForm>();
            if (!type.IsNullOrEmpty())
                where = where.And(x => x.Type == type);

            where = where.And(x => x.Status == 1);

            var list = await GetIQueryable().Where(where).ProjectTo<OA_DefFormDTO>(_mapper.ConfigurationProvider).ToListAsync();
            list = list.Where(p => string.IsNullOrEmpty(p.Value) || roleidlist.Intersect(p.ValueRoles).Count() > 0).ToList();

            List<OA_DefFormTree> treeList = new List<OA_DefFormTree>();
            foreach (var data in list.GroupBy(p => p.Type))
            {
                OA_DefFormTree node = new OA_DefFormTree()
                {
                    Id = data.Key,
                    Text = data.Key,
                    Value = data.Key,
                    scopedSlots = new { title = "title" },
                };

                node.Children = data.Select(x => (object)(new OA_DefFormTree
                {
                    Id = x.Id,
                    Text = x.Name,
                    Value = x.Id,
                    type = data.Key,
                    jsonId = x.JSONId,
                    jsonVersion = x.JSONVersion,
                    json = x.WorkflowJSON,
                    scopedSlots = new { title = "titleExtend" },
                })).ToList();

                treeList.Add(node);
            }

            return treeList;
        }



        public async Task<PageResult<OA_DefFormDTO>> GetDataListAsync(PageInput<OA_DefFormInputDTO> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<OA_DefForm>();

            //筛选
            if (!input.Search.condition.IsNullOrEmpty() && !input.Search.keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<OA_DefForm, bool>(
                    ParsingConfig.Default, false, $@"{input.Search.condition}.Contains(@0)", input.Search.keyword);
                where = where.And(newWhere);
            }

            return await q.Where(where).ProjectTo<OA_DefFormDTO>(_mapper.ConfigurationProvider).GetPageResultAsync(input);
        }

        public async Task<OA_DefFormDTO> GetTheDataAsync(string id)
        {
            return _mapper.Map<OA_DefFormDTO>(await GetEntityAsync(id));
        }

        public async Task AddDataAsync(OA_DefForm data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(OA_DefForm data)
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