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
    public class OA_UserFormStepBusiness : BaseBusiness<OA_UserFormStep>, IOA_UserFormStepBusiness, ITransientDependency
    {
        readonly IMapper _mapper;
        public OA_UserFormStepBusiness(IDbAccessor db, IMapper mapper)
            : base(db)
        {
            _mapper = mapper;
        }

        #region 外部接口

        public async Task<PageResult<OA_UserFormStep>> GetDataListAsync(PageInput<OA_UserFormStepInputDTO> pagination, string condition, string keyword)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<OA_UserFormStep>();

            //筛选
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<OA_UserFormStep, bool>(
                    ParsingConfig.Default, false, $@"{condition}.Contains(@0)", keyword);
                where = where.And(newWhere);
            }

            return await q.Where(where).GetPageResultAsync(pagination);
        }

        public async Task<OA_UserFormStep> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(null, id);
        }

        public async Task AddDataAsync(OA_UserFormStep data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(OA_UserFormStep data)
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