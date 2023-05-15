using News.Vampire.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace News.Vampire.Service.BusinessLogic.Interfaces
{
    public interface ISourceLogic: IBaseLogic<Source>
    {
        Task<IList<Source>> GetSourcesReadyToLoadAsync();
        Task<IList<Source>> GetAll();
    }
}
