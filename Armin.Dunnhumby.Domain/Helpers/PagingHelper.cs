﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Armin.Dunnhumby.Domain.Helpers
{
    public interface IPagedResult
    {
        int Page { get; set; }
        int PageSize { get; set; }
        int PageCount { get; set; }
        int RecordCount { get; set; }
        bool HasNext { get; }
        bool HasPrev { get; }
    }

    public class PagedResult<TModel> : IPagedResult
    {
        public PagedResult()
        {
            PageSize = 10;
            Page = 1;
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public List<TModel> Data { get; set; }

        public bool HasNext => Page < PageCount;

        public bool HasPrev => Page > 1;

        public PagedResult<TModel> GetPagedData(int pageNo, IQueryable<TModel> queryableData)
        {
            Page = pageNo;

            int startNumber = (Page - 1) * PageSize;

            RecordCount = queryableData.Count();

            Data = queryableData.Skip(startNumber).Take(PageSize).ToList();

            PageCount = (RecordCount / PageSize) + (RecordCount % PageSize == 0 ? 0 : 1);

            return this;
        }
    }

    public class PagedResult<TInType, TOutType>
    {
        public PagedResult()
        {
            PageSize = 10;
            Page = 1;
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public List<TOutType> Data { get; set; }

        public PagedResult<TInType, TOutType> GetPagedData(int pageNo, IQueryable<TInType> queryableData, Func<TInType, TOutType> setter)
        {
            Page = pageNo;

            int startNumber = (Page - 1) * PageSize;

            RecordCount = queryableData.Count();

            Data = queryableData.Skip(startNumber).Take(PageSize).ToList().Select(setter).ToList();

            PageCount = (RecordCount / PageSize) + (RecordCount % PageSize == 0 ? 0 : 1);

            return this;
        }
    }
}
