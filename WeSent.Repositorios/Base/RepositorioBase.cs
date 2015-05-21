using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using WeSent.Entidades;
using WeSent.IRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WeSent.Repositorios
{
	public class RepositorioBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : EntidadeBase<TPrimaryKey>
	{
		private readonly ISession _session;
		private const string Codigo = "Codigo";

		public RepositorioBase (ISession session)
		{
			_session = session;
		}

		public ICriteria CreateCriteriaForEntity (string alias = null)
		{
			return alias == null ? _session.CreateCriteria<TEntity> () : _session.CreateCriteria<TEntity> (alias);
		}

		public virtual int Count (Expression<Func<TEntity, bool>> @where)
		{
			var count = (from t in _session.Query<TEntity> ().Where (@where)
			              select t).Count ();
			return count;
		}

		protected virtual int Count (ICriteria criteria)
		{
			var count = (Int32)criteria.SetProjection (Projections.Count (Projections.Id ())).UniqueResult ();
			return count;
		}

		public virtual int Count (ICriterion filter)
		{
			var count = (Int32)_session.CreateCriteria (typeof(TEntity))
					.SetProjection (Projections.Count (Codigo))
					.Add (filter)
					.UniqueResult ();

			return count;
		}

		public virtual int Count (ICriterion[] filters)
		{
			var count = _session.CreateCriteria (typeof(TEntity));

			count.SetProjection (new IProjection[] { Projections.Count (Codigo) });

			foreach (var item in filters) {
				count.Add (item);
			}
			var results = (Int32)count.UniqueResult ();

			return results;
		}

		//public int Count(TEntity entity, bool active)
		//{
		//    _session.Clear();
		//    var count = (Int32)_session.CreateCriteria(typeof(TEntity))
		//            .Add(Restrictions.Eq("Ativo", active))
		//           .SetProjection(Projections.Count("Codigo"))
		//           .UniqueResult();

		//    _session.Evict(typeof(TEntity));

		//    return count;
		//}

		public TEntity GetById (TPrimaryKey id)
		{
			var entity = _session.Get<TEntity> (id);

			_session.Evict (typeof(TEntity));

			return entity;
		}

		public IList<TEntity> GetWithFilters (Expression<Func<TEntity, bool>> @where)
		{
			var lista = (from t in _session.Query<TEntity> ().Where (@where)
			              select t).ToList ();
			return lista;
		}

		//public TEntity GetPrevious(int id)
		//{
		//    var entity = (TEntity)_session.CreateCriteria(typeof(TEntity))
		//        .Add(Restrictions.Lt("Codigo", id))
		//        .SetMaxResults(1)
		//        .AddOrder(Order.Desc("Codigo"))
		//        .UniqueResult();

		//    _session.Evict(typeof(TEntity));

		//    return entity;
		//}

		//public TEntity GetNext(int id)
		//{
		//    var entity = (TEntity)_session.CreateCriteria(typeof(TEntity))
		//        .Add(Restrictions.Gt("Codigo", id))
		//        .SetMaxResults(1)
		//        .UniqueResult();

		//    _session.Evict(typeof(TEntity));

		//    return entity;
		//}

		public IList<TEntity> GetAll ()
		{
			var entities = _session.CreateCriteria (typeof(TEntity)).List<TEntity> () as List<TEntity>;

			_session.Evict (typeof(TEntity));

			return entities;
		}

		//public IList<TEntity> GetActive(bool active)
		//{
		//    var criteria = _session.CreateCriteria(typeof(TEntity));

		//    criteria.Add(Restrictions.Eq("Ativo", active));

		//    var list = criteria.List<TEntity>() as List<TEntity>;

		//    _session.Evict(typeof(TEntity));

		//    return list;
		//}

		public bool Delete (TEntity entity)
		{
			using (var transaction = _session.BeginTransaction ()) {
				try {
					_session.Delete (entity);
					transaction.Commit ();
				} catch {
					transaction.Rollback ();
					throw;
				}
			}
			return true;
		}

		public bool Delete (TPrimaryKey id)
		{
			using (var transaction = _session.BeginTransaction ()) {
				try {
					_session.Delete (_session.Load<TEntity> (id));
					transaction.Commit ();
				} catch {
					transaction.Rollback ();
					throw;
				}
			}
			return true;
		}


		//public bool DeleteLogical(TEntity entity)
		//{
		//    using (var transaction = _session.BeginTransaction())
		//    {
		//        try
		//        {
		//            _session.Update(entity);
		//            transaction.Commit();
		//        }
		//        catch
		//        {
		//            transaction.Rollback();
		//            throw;
		//        }
		//    }
		//    return true;
		//}

		public TEntity Insert (TEntity entity)
		{
			using (var transaction = _session.BeginTransaction ()) {
				try {
					_session.Save (entity);
					transaction.Commit ();
				} catch {
					transaction.Rollback ();
					throw;
				}
			}

			return entity;
		}

		public TEntity Update (TEntity entity)
		{
			using (var transaction = _session.BeginTransaction ()) {
				try {
					_session.Merge (entity);
					//_session.Update(entity);
					transaction.Commit ();
				} catch {
					transaction.Rollback ();
					throw;
				}
			}

			return entity;
		}

		//public virtual bool CheckExistingRecord(Dictionary<string, object> parameters, int? id)
		//{
		//    throw new NotImplementedException();
		//}

		//public Grid<TEntity> GetGrid(int pageIndex)
		//{
		//    Grid<TEntity> grid;

		//    using (var transaction = _session.BeginTransaction())
		//    {
		//        var rowCount = _session.CreateCriteria<TEntity>()
		//                            .SetProjection(Projections.RowCount())
		//                            .FutureValue<Int32>();

		//        var results = _session.CreateCriteria<TEntity>()
		//            .SetFirstResult((pageIndex - 1) * 10)
		//            .SetMaxResults(10)
		//            .Future<TEntity>()
		//            .ToList<TEntity>();

		//        grid = new Grid<TEntity>(results, pageIndex, rowCount.Value);

		//        transaction.Commit();
		//    }

		//    return grid;
		//}
	}
}
