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
        public City? GetByIdCity(int TKGMValue)
        {
            return db.City.Where(x => x.TKGMValue == TKGMValue).FirstOrDefault();
        }
        public IEnumerable<District> GetByCityValDistrictList(int cityVal)
        {
            return db.District.Where(x => x.City.TKGMValue == cityVal).ToList();
        }
        public IEnumerable<Street> GetByDistrictValStreetList(int districtVal)
        {
            return db.Street.Where(x => x.District.TKGMValue == districtVal).ToList();            
        }
        public Parcel? GetByStreetParsel(int streetVal,string adaVal, string parcelVal)
        {
            return db.Parsel.Where(x => x.mahalleId == streetVal && x.parselNo==parcelVal && x.adaNo==adaVal).FirstOrDefault();
        }        
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
    }
}
