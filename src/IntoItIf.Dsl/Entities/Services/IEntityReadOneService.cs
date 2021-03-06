namespace IntoItIf.Dsl.Entities.Services
{
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain.Entities;
   using Core.Domain.Options;

   public interface IEntityReadOneService<TDto>
      where TDto : class, IDto
   {
      #region Public Methods and Operators

      Option<TDto> GetByPredicate(Option<TDto> criteria);
      Task<Option<TDto>> GetByPredicateAsync(Option<TDto> criteria, Option<CancellationToken> ctok);
      Option<bool> IsExist(Option<TDto> criteria);
      Task<Option<bool>> IsExistAsync(Option<TDto> criteria, Option<CancellationToken> ctok);

      #endregion
   }
}