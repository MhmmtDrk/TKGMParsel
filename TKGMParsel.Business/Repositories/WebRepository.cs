using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private  Context db;
        private DbSet<T> entities;
        private IConfiguration configuration;
        public WebRepository(Context _db, IConfiguration _configuration)
        {
            db= _db;
            entities=db.Set<T>();
            configuration= _configuration;
            
        }
        private DbContextOptions<Context> GetAllOptions()
        {
            var optionBuilder = new DbContextOptionsBuilder<Context>();
            optionBuilder.UseSqlServer(configuration.GetConnectionString("WebCS"));
            return optionBuilder.Options;
        }
        public  IQueryable<T> GetAll()
        {
           return entities;
        }
        public City? GetByIdCity(int TKGMValue)
        {
            using (db = new Context(GetAllOptions()))
            {
                return db.City.Where(x => x.TKGMValue == TKGMValue).FirstOrDefault();
            }
        }
        public IEnumerable<District> GetByCityValDistrictList(int cityVal)
        {
            using (db = new Context(GetAllOptions()))
            {
                return db.District.Where(x => x.City.TKGMValue == cityVal).ToList();
            }
        }
        public IEnumerable<Street> GetByDistrictValStreetList(int districtVal)
        {
            using (db = new Context(GetAllOptions()))
            {
                return db.Street.Where(x => x.District.TKGMValue == districtVal).ToList();
            }
        }
        public Parcel? GetByStreetParsel(int streetVal,string adaVal, string parcelVal)
        {
            using (db = new Context(GetAllOptions()))
            {
                return db.Parsel.Where(x => x.mahalleId == streetVal && x.parselNo == parcelVal && x.adaNo == adaVal).FirstOrDefault();
            }
        }        
        public void Create(T entity)
        {
            using (db = new Context(GetAllOptions()))
            {
                db.Set<T>().Add(entity);
                db.SaveChanges();
            }
        }
        public  void CreateAll(IEnumerable<T> entity)
        {
            using (db = new Context(GetAllOptions()))
            {
                db.Set<T>().AddRange(entity);
                db.SaveChanges();
            }
        }
    }
}
