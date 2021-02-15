using Coldairarrow.Entity.OA_Manage;
using Coldairarrow.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Coldairarrow.Business.Base_Manage;
using System.Collections.Concurrent;
using System.Threading;
using LinqKit;
using AutoMapper;
using EFCore.Sharding;
using AutoMapper.QueryableExtensions;

namespace Coldairarrow.Business.OA_Manage
{
    public class OA_UserFormBusiness : BaseBusiness<OA_UserForm>, IOA_UserFormBusiness, ITransientDependency
    {
        readonly IMapper _mapper;
        readonly IBase_UserBusiness _userBus;
        public OA_UserFormBusiness(IBase_UserBusiness userBus, IDbAccessor db, IMapper mapper)
            : base(db)
        {
            _mapper = mapper;
            _userBus = userBus;
        }

        private static ConcurrentBag<string> _queues = new ConcurrentBag<string>();

        public Task QueueWork(string id)
        {
            _queues.Add(id);
            return Task.CompletedTask;
        }

        public async Task<string> DequeueWork(string id)
        {
            for (int i = 0; i < 30; i++)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100));

                if (_queues.Contains(id))
                {
                    if (_queues.TryTake(out id))//3m超时
                        return id;
                }
            }

            return null;
        }



        #region 外部接口

        public async Task<PageResult<OA_UserFormDTO>> GetDataListAsync(PageInput<OA_UserFormInputDTO> pagination, string condition, string keyword,
           string userId, string applicantUserId, string creatorId, string alreadyUserIds)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<OA_UserForm>();

            //筛选
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<OA_UserForm, bool>(
                    ParsingConfig.Default, false, $@"{condition}.Contains(@0)", keyword);
                where = where.And(newWhere);
            }

            if (!userId.IsNullOrEmpty())
            {
                where = where.And(p => p.UserIds.Contains("^" + userId + "^") && p.Status == (int)OAStatus.Being);
            }

            if (!applicantUserId.IsNullOrEmpty())
            {
                where = where.And(p => p.ApplicantUserId == applicantUserId && p.Status == (int)OAStatus.Being);
            }

            if (!alreadyUserIds.IsNullOrEmpty())
            {
                where = where.And(p => p.AlreadyUserIds.Contains("^" + alreadyUserIds + "^"));
            }

            if (!creatorId.IsNullOrEmpty())
            {
                where = where.And(p => p.CreatorId == creatorId);
            }


            return await q.Where(where).ProjectTo<OA_UserFormDTO>(_mapper.ConfigurationProvider).GetPageResultAsync(pagination);
        }

    

        public int GetDataListCount(List<string> jsonids, OAStatus status)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<OA_UserForm>();

            if (!jsonids.IsNullOrEmpty())
            {
                where = where.And(p => jsonids.Contains(p.DefFormJsonId));
            }

            if ((int)status >= 0)
            {
                where = where.And(p => p.Status == (int)status);
            }

            return q.Where(where).Count();
        }


        public async Task<OA_UserFormDTO> GetTheDataAsync(string id)
        {
            //return Mapper.Map<OA_UserFormDTO>(await GetEntityAsync(null, id));
            var form = await (from a in Db.GetIQueryable<OA_UserForm>()
                              join b in Db.GetIQueryable<OA_UserFormStep>() on a.Id equals b.UserFormId into j1
                              where a.Id.Equals(id)
                              select new
                              {
                                  UserForm = a,
                                  Comments = j1.OrderBy(p => p.CreateTime).ToList()
                              }).FirstOrDefaultAsync();

            OA_UserFormDTO formdto = _mapper.Map<OA_UserFormDTO>(form.UserForm);
            formdto.Comments = _mapper.Map<List<OA_UserFormStepDTO>>(form.Comments);

            foreach (var comment in formdto.Comments)
            {
                //comment.Avatar = await _userBus.GetAvatar(comment.CreatorId);
            }

            return formdto;

        }

        public async Task AddDataAsync(OA_UserForm data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(OA_UserForm data)
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