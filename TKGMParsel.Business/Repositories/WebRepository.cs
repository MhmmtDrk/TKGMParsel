using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TKGMParsel.Data.Cache;
using TKGMParsel.Data.Contexts;
using TKGMParsel.Data.Entities;

namespace TKGMParsel.Business.Repositories
{
    public class WebRepository<T> where T:class
    {
        private readonly Context db;
        //private readonly DataCacheRedis dataCacheRedis;
        private DbSet<T> entities;
        public WebRepository(Context _db)
        {
            db= _db;
            entities=db.Set<T>();
            
        }
        public  IQueryable<T> GetAll()
        {
           return entities;
        }
        public IEnumerable<District> GetByCityValDistrictList(int cityVal)
        {
            return db.District.Where(x => x.City.TKGMValue == cityVal).ToList();
        }
        public IEnumerable<Street> GetByDistrictValStreetList(int districtVal)
        {
            return db.Street.Where(x => x.District.TKGMValue == districtVal).ToList();
            
        }
        public Parcel GetByStreetParsel(int streetVal,string adaVal, string parcelVal)
        {
            return db.Parsel.Where(x => x.mahalleId == streetVal && x.parselNo==parcelVal && x.adaNo==adaVal).FirstOrDefault();

        }

  


        //public T GetBy(Expression<Func<T,bool>> expression)
        //{
        //    return entities.FirstOrDefault(expression);
        //}
        //public bool AddCity(City model)
        //{
        //    if(model!=null)
        //    {
        //        using (var context = db)
        //        {
        //            context.City.Add(model);
        //            context.SaveChanges();
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}
        //public bool AddDistrict(District model)
        //{
        //    if (model != null)
        //    {
        //        using (var context = db)
        //        {
        //            context.District.Add(model);
        //            context.SaveChanges();
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}
        //public bool AddStreet(Street model)
        //{
        //    if (model != null)
        //    {
        //        using (var context = db)
        //        {
        //            context.Street.Add(model);
        //            context.SaveChanges();
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}
        //public void CreateParselRedis(Parcel model)
        //{
        //    string cacheKey = $"Parsel_{model.mahalleId}_{model.adaNo}_{model.parselNo}";
        //    var parselCache = dataCacheRedis.Get<Parcel>(cacheKey);
        //    if(parselCache==null)
        //    {
        //        //dataCacheRedis.Remove(cacheKey);
        //        dataCacheRedis.Set(cacheKey, model);
        //    }
        //}
        //public Parcel? GetParselRedis(string cacheKey)
        //{
            
        //    var parselCache = dataCacheRedis.Get<Parcel>(cacheKey);            
        //    if (parselCache != null)
        //    {
               
        //        //dataCacheRedis.Remove(cacheKey);
        //        return parselCache;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        public void Create(T entity)
        {

            
            db.Set<T>().Add(entity);
                db.SaveChanges();
            
        }
        public  void CreateAll(IEnumerable<T> entity)
        { 
            
                db.Set<T>().AddRange(entity);
                db.SaveChanges();               
            
        }
        public City GetByIdCity(int TKGMValue)
        {
            return db.City.Where(x => x.TKGMValue == TKGMValue).FirstOrDefault();
        }
        //public District GetByIdDistrict(int TKGMCityValue, int TKGMDistrictValue)
        //{
        //    return db.District.Where(x => x.TKGMCityValue == TKGMCityValue & x.TKGMValue== TKGMDistrictValue).FirstOrDefault();
        //}
        //public Street GetByIdStreet(int TKGMDistrictValue, int TKGMstreetValue)
        //{
        //    return db.Street.Where(x => x.TKGMDistrictValue == TKGMDistrictValue & x.TKGMValue == TKGMstreetValue).FirstOrDefault();
        //}
        //public IEnumerable<District> GetByCityIdDistrictList(int TKGMCityValue)
        //{
        //    return db.District.Where(x => x.TKGMCityValue == TKGMCityValue).ToList();
        //}
        //public IEnumerable<Street> GetByDistrictIdStreetList(int TKGMDistrictValue)
        //{
        //    return db.Street.Where(x => x.TKGMDistrictValue == TKGMDistrictValue).ToList();
        //}



    }
}
