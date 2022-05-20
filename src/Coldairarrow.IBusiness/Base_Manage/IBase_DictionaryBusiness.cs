using Coldairarrow.Entity;
using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.IBusiness;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Business.Base_Manage
{
    public interface IBase_DictionaryBusiness : IBaseBusiness<Base_Dictionary>
    {
        Task<List<Base_Dictionary>> GetDataListAsync(Base_DictionaryInputDTO input);
        Task<List<Base_DictionaryDTO>> GetTreeDataListAsync(Base_DictionaryInputDTO input);
        Task<Base_Dictionary> GetTheDataAsync(string id);
        Task AddDataAsync(Base_Dictionary data);
        Task UpdateDataAsync(Base_Dictionary data);
        Task DeleteDataAsync(List<string> ids);
    }

    public class Base_DictionaryInputDTO
    {
        public string[] ActionIds { get; set; }
        public string parentId { get; set; }
        public DictionaryType[] types { get; set; }
        public bool selectable { get; set; }
        public bool checkEmptyChildren { get; set; }
    }

    public class Base_DictionaryDTO : TreeModel
    {
        public DictionaryType Type { get; set; }
        public ControlType ControlType { get; set; }
        public string TypeText { get => ((DictionaryType)Type).ToString(); }
        public object children { get => Children; }
        public string Code { get; set; }
        public string Remark { get; set; }
        //public string title { get => Text; }
        //public string value { get => Id; }
        //public string key { get => Id; }
        public bool selectable { get; set; }
        public int Sort { get; set; }
    }
}