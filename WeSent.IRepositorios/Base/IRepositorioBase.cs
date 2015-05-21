using System;
using NHibernate;
using WeSent.Entidades;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WeSent.IRepositorios
{
	public interface IRepositorioBase
	{
	}
	/// <summary>
	/// This interface is implemented by all repositories to ensure implementation of fixed methods.
	/// </summary>
	/// <typeparam name="TEntity">Main Entity type this repository works on</typeparam>
	/// <typeparam name="TPrimaryKey"></typeparam>
	public interface IRepository<TEntity, TPrimaryKey> : IRepositorioBase where TEntity : EntidadeBase<TPrimaryKey>
	{
		ICriteria CreateCriteriaForEntity(string alias = null);

		//bool CheckExistingRecord(Dictionary<string, object> parameters, int? id);

		//Int32 Count(TEntity entity, bool active);

		bool Delete(TPrimaryKey id);

		bool Delete(TEntity entity);

		//bool DeleteLogical(TEntity entity);

		//IList<TEntity> GetActive(bool active);

		IList<TEntity> GetAll();

		TEntity GetById(TPrimaryKey id);

		IList<TEntity> GetWithFilters(Expression<Func<TEntity, bool>> @where);

		int Count(Expression<Func<TEntity, bool>> @where);

		//Grid<TEntity> GetGrid(int pageIndex);

		//TEntity GetPrevious(int id);

		//TEntity GetNext(int id);

		TEntity Insert(TEntity entity);

		TEntity Update(TEntity entity);
	}
}
