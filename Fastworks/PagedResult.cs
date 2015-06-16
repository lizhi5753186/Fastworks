using System.Collections;
using System.Collections.Generic;

namespace Fastworks
{
    public class PagedResult<T> : ICollection<T>
    {
        #region Public Properties

        public int? TotalRecords { get; set; }

        public int? TotalPages { get; set; }

        public int? PageSize { get; set; }

        public int? PageNumber { get; set; }

        public IList<T> Data
        {
            get;
            private set;
        }


        #endregion 

        #region Ctor
        public PagedResult()
        {
            this.Data = new List<T>();
        }

        public PagedResult(int? totalRecords, int? totalPages, int? pageSize, int? pageNumber, IList<T> data)
        {
            this.TotalRecords = totalRecords;
            this.TotalPages = totalPages;
            this.PageSize = pageSize;
            this.PageNumber = pageNumber;
            this.Data = data;
        }

        #endregion 

        #region  ICollection<T> Members
        public void Add(T item)
        {
            Data.Add(item);
        }

        public void Clear()
        {
            Data.Clear();
        }

        public bool Contains(T item)
        {
            return Data.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Data.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return Data.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return Data.Remove(item);
        }
        #endregion 


        #region IEnumerable<T> Members
        public IEnumerator<T> GetEnumerator()
        {
            return Data.GetEnumerator();
        }
        #endregion 

        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Data.GetEnumerator();
        }
        #endregion 
    }
}
