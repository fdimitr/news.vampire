using News.Vampire.Service.Models;

namespace News.Vampire.Service.BusinessLogic.Interfaces
{
    public interface ISourceLogic: IBaseLogic<Source>
    {
        Task<IList<Source>> GetSourcesReadyToLoadAsync();
        IQueryable<Source> GetAll();
    }
}
