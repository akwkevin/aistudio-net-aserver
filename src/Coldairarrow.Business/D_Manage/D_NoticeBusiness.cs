using Coldairarrow.Business.Base_Manage;
using Coldairarrow.Entity;
using Coldairarrow.Entity.D_Manage;
using Coldairarrow.IBusiness;
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
    public class D_NoticeBusiness : BaseBusiness<D_Notice>, ID_NoticeBusiness, ITransientDependency
    {
        readonly IOperator _operator;
        readonly IBase_UserBusiness _userBusiness;
        public D_NoticeBusiness(IDbAccessor db, IOperator @operator, IBase_UserBusiness userBusiness)
            : base(db)
        {
            _operator = @operator;
            _userBusiness = userBusiness;
        }

        #region 外部接口

        public async Task<PageResult<D_NoticeDTO>> GetDataListAsync(PageInput<D_NoticeInputDTO> input)
        {
            var search = input.Search;
            var q = await SetQueryable(search);
            var list = await q.GetPageResultAsync(input);
            return list;
        }

        private async Task<IQueryable<D_NoticeDTO>> SetQueryable(D_NoticeInputDTO search)
        {
            Expression<Func<D_Notice, D_NoticeReadingMarks, D_NoticeDTO>> select = (a, b) => new D_NoticeDTO
            {
                UserId = b.CreatorId
            };
            
            select = select.BuildExtendSelectExpre();

            var q_User = Db.GetIQueryable<D_Notice>();

            IQueryable<D_NoticeDTO> q = null;

            if (search.status == 0)
            {
                q = from a in q_User.AsExpandable()
                    join b in Db.GetIQueryable<D_NoticeReadingMarks>() on a.Id equals b.NoticeId into ab
                    from b in ab.DefaultIfEmpty()
                    where b.CreatorId == null
                    select @select.Invoke(a, b);

                var user = await _userBusiness.GetTheDataAsync(search.userId);
                if (user == null)
                {
                    return null;
                }
                Expression<Func<D_NoticeDTO, bool>> a1 = p => p.Mode == NoticeMode.All;
                if (user.RoleIdList != null)
                {
                    foreach (var role in user.RoleIdList)
                    {
                        Expression<Func<D_NoticeDTO, bool>> a2 = p => (p.Mode == NoticeMode.Role && p.AnyId.Contains(role));
                        a1 = a1.Or<D_NoticeDTO>(a2);
                    }
                }
                if (!string.IsNullOrEmpty(user.DepartmentId))
                {
                    Expression<Func<D_NoticeDTO, bool>> a3 = p => (p.Mode == NoticeMode.Department && user.DepartmentId == p.AnyId);
                    a1 = a1.Or<D_NoticeDTO>(a3);
                }

                q = q.Where(a1);
            }
            else if (search.status == 1)
            {
                q = from a in q_User.AsExpandable()
                    join b in Db.GetIQueryable<D_NoticeReadingMarks>() on a.Id equals b.NoticeId into ab
                    from b in ab.DefaultIfEmpty()
                    where b.CreatorId == search.userId
                    select @select.Invoke(a, b);
            }
            else
            {
                q = from a in q_User.AsExpandable()
                    join b in Db.GetIQueryable<D_NoticeReadingMarks>() on a.Id equals b.NoticeId into ab
                    from b in ab.DefaultIfEmpty()
                    where (b.CreatorId == search.userId || b.CreatorId == null)
                    select @select.Invoke(a, b);
            }

            if (!search.noticeId.IsNullOrEmpty())
            {
                q = q.Where(p => p.Id == search.noticeId);
            }

            if (!search.keyword.IsNullOrEmpty())
            {
                var keyword = $"%{search.keyword}%";
                q = q.Where(x =>
                      EF.Functions.Like(x.Title, keyword)
                      || EF.Functions.Like(x.Text, keyword));
            }

            return q;
        }

        public async Task<D_NoticeDTO> GetTheDataAsync(string id)
        {
            return (await GetDataListAsync(new PageInput<D_NoticeInputDTO> { Search = new D_NoticeInputDTO { noticeId = id, status = 2, userId = _operator.UserId } })).Data.FirstOrDefault();
        }

        public async Task AddDataAsync(D_Notice data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(D_Notice data)
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
        public async Task<int> GetHistoryDataCountAsync(Input<D_NoticeInputDTO> input)
        {
            var search = input.Search;
            if (search.markflag == true)
            {
                search.status = 0;
            }
            else
            {
                search.status = 1;
            }
            var q = await SetQueryable(search);
            if (q == null)
            {
                return 0;
            }

            if (search.end == DateTime.MinValue || search.end == null)
            {
                search.end = DateTime.Now;
            }
            if (search.start == DateTime.MinValue || search.start == null)
            {
                search.start = DateTime.Now.AddDays(-30);
            }

            q = q.Where(x => x.CreateTime >= search.start && x.CreateTime <= search.end);

            return await q.CountAsync();
        }
        public async Task<List<D_NoticeDTO>> GetHistoryDataListAsync(Input<D_NoticeInputDTO> input)
        {
            var search = input.Search;
            if (search.markflag == true)
            {
                search.status = 0;
            }
            else
            {
                search.status = 1;
            }
            var q = await SetQueryable(search);

            if (search.end == DateTime.MinValue || search.end == null)
            {
                search.end = DateTime.Now;
            }
            if (search.start == DateTime.MinValue || search.start == null)
            {
                search.start = DateTime.Now.AddDays(-30);
            }

            q = q.Where(x => x.CreateTime >= search.start && x.CreateTime <= search.end);

            return await q.ToListAsync();
        }
        public async Task<PageResult<D_NoticeDTO>> GetPageHistoryDataListAsync(PageInput<D_NoticeInputDTO> input)
        {
            var search = input.Search;
            if (search.markflag == true)
            {
                search.status = 0;
            }
            else
            {
                search.status = 1;
            }
            var q = await SetQueryable(search);

            if (search.end == DateTime.MinValue || search.end == null)
            {
                search.end = DateTime.Now;
            }
            if (search.start == DateTime.MinValue || search.start == null)
            {
                search.start = DateTime.Now.AddDays(-30);
            }

            q = q.Where(x => x.CreateTime >= search.start && x.CreateTime <= search.end);

            return await q.GetPageResultAsync(input);
        }
        #endregion
    }
}