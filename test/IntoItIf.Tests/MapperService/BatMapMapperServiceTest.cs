﻿namespace IntoItIf.Tests.MapperService
{
   using Core.Domain.Options;
   using Preparation;
   using Shouldly;
   using Xunit.Abstractions;

   public class BatMapMapperServiceTest
   {
      #region Fields

      private readonly ITestOutputHelper _output;

      #endregion

      #region Constructors and Destructors

      public BatMapMapperServiceTest(ITestOutputHelper output)
      {
         _output = output;
         MapperTestHelper.InitBatMap();
      }

      #endregion

      #region Public Methods and Operators

      //[Fact]
      public void BatMap_DtoSeed_ToEntity_ShouldNotNull_And_ShouldBeSameAs_ExpectedEntity()
      {
         var entity = MapperTestHelper.DtoSeed.ToEntity<MyEntity>();
         var reduced = entity.ReduceOrDefault();
         _output.WriteLine(
            $"DtoSeed: {MapperTestHelper.DtoSeed} => entity: {reduced} => ExpectedEntity: {MapperTestHelper.ExpectedEntity}");
         reduced.ShouldNotBeNull();
         reduced.ShouldBe(MapperTestHelper.ExpectedEntity);
      }

      //[Fact]
      public void BatMap_DtoSeed_ToEntity_ShouldNotNull_And_ShouldNotSameAs_UnexpectedEntity()
      {
         var entity = MapperTestHelper.DtoSeed.ToEntity<MyEntity>();
         var reduced = entity.ReduceOrDefault();
         _output.WriteLine(
            $"DtoSeed: {MapperTestHelper.DtoSeed} => entity: {reduced} => UnexpectedEntity: {MapperTestHelper.UnexpectedEntity}");
         reduced.ShouldNotBeNull();
         reduced.ShouldNotBe(MapperTestHelper.UnexpectedEntity);
      }

      //[Fact]
      public void BatMap_EntitySeed_ToDto_ShouldNotNull_And_ShouldBeSameAs_ExpectedDto()
      {
         var dto = MapperTestHelper.EntitySeed.ToDto<MyDto>();
         var reduced = dto.ReduceOrDefault();
         _output.WriteLine(
            $"EntitySeed: {MapperTestHelper.EntitySeed} => dto: {reduced} => ExpectedDto: {MapperTestHelper.ExpectedDto}");
         reduced.ShouldNotBeNull();
         reduced.ShouldBe(MapperTestHelper.ExpectedDto);
      }

      //[Fact]
      public void BatMap_EntitySeed_ToDto_ShouldNotNull_And_ShouldNotSameAs_UnexpectedDto()
      {
         var dto = MapperTestHelper.EntitySeed.ToDto<MyDto>();
         var reduced = dto.ReduceOrDefault();
         _output.WriteLine(
            $"EntitySeed: {MapperTestHelper.EntitySeed} => dto: {reduced} => UnexpectedDto: {MapperTestHelper.UnexpectedDto}");
         reduced.ShouldNotBeNull();
         reduced.ShouldNotBe(MapperTestHelper.UnexpectedDto);
      }

      #endregion
   }
}