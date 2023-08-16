using System;
using Microsoft.EntityFrameworkCore;
using RiodeAppMVC.DataAccess;
using RiodeAppMVC.ExtensionServices.Interfaces;
using RiodeAppMVC.Models;
using RiodeAppMVC.Services.Interfaces;
using RiodeAppMVC.ViewModels.ProductVMs;

namespace RiodeAppMVC.Services.Implements
{
    public class ProductService : IProductService
    {
        readonly RiodeDbContext _context;
        readonly IFileService _fileService;
        readonly ICategoryService _catService;

        public ProductService(RiodeDbContext context, IFileService fileService, ICategoryService catService)
        {
            _context = context;
            _fileService = fileService;
            _catService = catService;
        }

        public IQueryable<Product> GetTable { get => _context.Set<Product>(); }

        public async Task Create(CreateProductVM productVM)
        {
            if(productVM.CategoryIds.Count>4)
            {
                throw new Exception();
            }
            if (!await _catService.IsAllExist(productVM.CategoryIds))
            {
                throw new ArgumentException();
            }

            List<ProductCategory> prodCategories = new List<ProductCategory>();
            foreach (var id in productVM.CategoryIds)
            {
                prodCategories.Add(new ProductCategory
                {
                    CategoryId = id
                });
            }

            Product entity=new Product(){
                Name=productVM.Name,
                Description=productVM.Description,
                Price=productVM.Price,
                Discount=productVM.Discount,
                StockCount=productVM.StockCount,
                Rating=productVM.Rating,
                SellCount=productVM.SellCount,
                MainImage=await _fileService.UploadAsync(productVM.MainImageFile,Path.Combine("imgs","products")),
                ProductCategories=prodCategories
            };
            if(productVM.ImageFiles != null)
            {
                List<ProductImage> imgs = new();
                foreach (var item in productVM.ImageFiles)
                {
                    string filename = await _fileService.UploadAsync(item, Path.Combine("imgs", "products"));
                    imgs.Add(new ProductImage
                    {
                        Name = filename
                    });
                }
                entity.ProductImages = imgs;
            }
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int? id)
        {
            var entity = await GetById(id,true);
            _context.Products.Remove(entity);
            _fileService.Delete(entity.MainImage);

            if(entity.ProductImages != null)
            {
                foreach (var img in entity.ProductImages)
                {
                     _fileService.Delete(img.Name);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteImage(int? id)
        {
            if (id == null || id <= 0) throw new Exception();
            var entity = await _context.ProductImages.FindAsync(id);
            if (entity == null) throw new ArgumentNullException();
            _fileService.Delete(entity.Name);
            _context.ProductImages.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Product>> GetAll(bool TakeAll)
        {
            if(TakeAll)
            {
                return await _context.Products.ToListAsync();
            }
            return await _context.Products.Where(p=>p.isDeleted==false).ToListAsync();
        }

        public async Task<Product> GetById(int? id, bool TakeAll = false)
        {
            if (id == null || id <= 0) throw new NullReferenceException();

            Product? entity;

            if(TakeAll)
            {
                entity = await _context.Products.FindAsync(id);
            }
            else {
                entity = await _context.Products.SingleOrDefaultAsync(p => p.isDeleted == false && p.Id == id);
            }

            if (entity is null) throw new NullReferenceException();

            return entity;
        }

        public async Task SoftDelete(int? id)
        {
            var entity = await GetById(id, true);
            entity.isDeleted = !entity.isDeleted;
            await _context.SaveChangesAsync();
        }

        public async Task Update(int? id, UpdateProductVM productVM)
        {
            if(productVM.CategoryIds.Count>4)
            {
                throw new Exception();
            }
            if (!await _catService.IsAllExist(productVM.CategoryIds))
            {
                throw new ArgumentException();
            }
            List<ProductCategory> productCategories = new List<ProductCategory>();

            foreach (var catId in productVM.CategoryIds)
            {
               productCategories.Add(new ProductCategory
               {
                  CategoryId = catId
               });
            }


            var entity = await _context.Products.Include(p => p.ProductCategories).SingleOrDefaultAsync(p=>p.Id==id);
            if (entity is null) throw new ArgumentNullException();

            if(entity.ProductCategories != null)
            {
                entity.ProductCategories.Clear();
            }

            entity.Name = productVM.Name;
            entity.Description = productVM.Description;
            entity.Price = productVM.Price;
            entity.Discount = productVM.Discount;
            entity.Rating = productVM.Rating;
            entity.StockCount = productVM.StockCount;
            entity.ProductCategories = productCategories;

            if(productVM.MainImage != null)
            {
                _fileService.Delete(entity.MainImage);
                entity.MainImage = await _fileService.UploadAsync(productVM.MainImage, Path.Combine("imgs", "products"));
            }

            if(productVM.ProductImagesFiles != null)
            {
                if (entity.ProductImages == null) entity.ProductImages = new List<ProductImage>();
                foreach (var img in productVM.ProductImagesFiles)
                {
                    ProductImage image = new ProductImage
                    {
                        Name = await _fileService.UploadAsync(img, Path.Combine("imgs", "products"))
                    };
                    entity.ProductImages.Add(image);
                }
            }
            await _context.SaveChangesAsync();
        }

    }
}

