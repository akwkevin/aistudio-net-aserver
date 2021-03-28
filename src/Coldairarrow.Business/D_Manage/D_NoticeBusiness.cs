using Coldairarrow.Entity;
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
    public class D_NoticeBusiness : BaseBusiness<D_Notice>, ID_NoticeBusiness, ITransientDependency
    {
        public D_NoticeBusiness(IDbAccessor db)
            : base(db)
        {
        }

        #region 外部接口

        public async Task<PageResult<D_NoticeDTO>> GetDataListAsync(PageInput<D_NoticeInputDTO> input)
        {

            Expression<Func<D_Notice, D_NoticeReadingMarks, D_NoticeDTO>> select = (a, b) => new D_NoticeDTO
            {
                UserId = b.CreatorId
            };
            var search = input.Search;
            select = select.BuildExtendSelectExpre();

            var q_User = Db.GetIQueryable<D_Notice>();
            var q = from a in q_User.AsExpandable()
                    join b in Db.GetIQueryable<D_NoticeReadingMarks>() on a.Id equals b.NoticeId  into ab
                    from b in ab.DefaultIfEmpty()
                    where b.CreatorId == search.userId
                    select @select.Invoke(a, b);

            q = q.Where(p => p.Mode == NoticeMode.All || (p.Mode == NoticeMode.User && p.AnyId.Contains(search.userId)) || (p.Mode == NoticeMode.Role && search.roleId.Contains(p.AnyId) || (p.Mode == NoticeMode.Department && search.departmentId == p.AnyId)));

            if (!search.keyword.IsNullOrEmpty())
            {
                var keyword = $"%{search.keyword}%";
                q = q.Where(x =>
                      EF.Functions.Like(x.Title, keyword)
                      || EF.Functions.Like(x.Text, keyword));
            }

            if (search.status == 0)
            {
                q = q.Where(x => x.UserId == null);
            }
            else if (search.status == 1)
            {
                q = q.Where(x => x.UserId == search.userId);
            }

            var list = await q.GetPageResultAsync(input);


            return list;
        }

        public async Task<D_Notice> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
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
    }
}