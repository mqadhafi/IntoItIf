namespace IntoItIf.Core.Domain.Entities
{
   using System.Threading.Tasks;
   using Options;

   public abstract class BaseDto<TDto, TValidator> : BaseDto<TDto>
      where TDto : BaseDto<TDto, TValidator>
      where TValidator : IDataValidator<TDto>, new()
   {
      #region Constructors and Destructors

      protected BaseDto() : this(new TValidator())
      {
      }

      protected BaseDto(Option<IDataValidator<TDto>> validator) : base(validator)
      {
      }

      protected BaseDto(Option<IMapperService> mapperService, Option<IDataValidator<TDto>> validator) : base(
         mapperService,
         validator)
      {
      }

      #endregion
   }

   public abstract class BaseDto<TDto> : ValueObject<TDto>, IDto
      where TDto : BaseDto<TDto>
   {
      #region Constructors and Destructors

      protected BaseDto(Option<IDataValidator<TDto>> validator) : this(
         InjecterGetter.GetBaseMapperService(),
         validator)
      {
      }

      protected BaseDto(Option<IMapperService> mapperService, Option<IDataValidator<TDto>> validator)
      {
         MapperService = mapperService;
         DataValidator = validator;
      }

      #endregion

      #region Properties

      protected Option<IDataValidator<TDto>> DataValidator { get; }
      protected Option<IMapperService> MapperService { get; }
      protected Option<TDto> This => this as TDto;

      #endregion

      #region Public Methods and Operators

      public Option<TEntity> ToEntity<TEntity>()
         where TEntity : class, IEntity
      {
         return GetMapperServiceThisOption(This).MapFlatten(x => x.MapperService.ToEntity<TDto, TEntity>(x.This));
      }

      public Option<ValidationResult> Validate(params string[] args)
      {
         return GetDataValidatorThisOption(This).MapFlatten(x => x.DataValidator.Validate(x.This));
      }

      public Task<Option<ValidationResult>> ValidateAsync(params string[] args)
      {
         return GetDataValidatorThisOption(This).MapFlattenAsync(x => x.DataValidator.ValidateAsync(x.This));
      }

      #endregion

      #region Methods

      protected Option<(IDataValidator<TDto> DataValidator, TDto This)> GetDataValidatorThisOption(Option<TDto> dto)
      {
         return dto.Combine(DataValidator).Map(x => (DataValidator: x.Item2, This: x.Item1));
      }

      protected Option<(IMapperService MapperService, TDto This)> GetMapperServiceThisOption(Option<TDto> dto)
      {
         return dto.Combine(MapperService).Map(x => (MapperService: x.Item2, This: x.Item1));
      }

      #endregion
   }
}