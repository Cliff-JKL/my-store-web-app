using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using practice.EF;
using practice.Models;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace practice.Repository
{
    public class MystoreRepository : BaseRepository
    {
        public MystoreRepository(mystoreContext _context) : base( _context ) { }

        // public async Task<T> GetEntity(int id)
        // {
        //     return await context.Entity<T>.FindAsync(id);
        // }

        public async Task<Address> GetAddress(int personId)
        {
            return await context.Address.Where(a => a.PersonId == personId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAddress(Address address)
        {
            context.Address.Update(address);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<CartItem> GetCartItem(int personId, int productId)
        {
            CartItem cartItem = await context.CartItem.Where(
                item => item.PersonId == personId && item.ProductId == productId)
                .FirstOrDefaultAsync();
            
            return cartItem;
        }

        public async Task<List<CartItem>> GetCartItems(int personId)
        {
            var cartItems = await context.CartItem.Where(item => item.PersonId == personId).ToListAsync();
            return cartItems;
        }

        public async Task<CartItem> CreateCartItem(int personId, int productId, int number=1)
        {
            CartItem cartItem = new CartItem();
            cartItem.PersonId = personId;
            cartItem.ProductId = productId;
            cartItem.Number = number;

            context.CartItem.Add(cartItem);
            await context.SaveChangesAsync();

            return cartItem;
        }

        public async Task<bool> DeleteCartItem(CartItem cartItem)
        {
            context.CartItem.Remove(cartItem);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateCartItem(CartItem cartItem)
        {
            context.CartItem.Update(cartItem);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<Category> GetCategory(string categoryName)
        {
            return await context.Category.Where(cat => cat.Name == categoryName).FirstOrDefaultAsync();
        }

        public async Task<Customer> GetCustomer(int personId)
        {
            return await context.Customer.Where(c => c.PersonId == personId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateCustomer(Customer customer)
        {
            context.Customer.Update(customer);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<Image> GetImage(int id)
        {
            var image = await context.Image.FindAsync(id);

            return image;
        }

        public async Task<List<ProductOrder>> GetOrders(Person person)
        {
            List<ProductOrder> orders;

            if (PersonIsAdmin(person))
            {
                orders = await context.ProductOrder.ToListAsync();
            }
            else if (!context.Customer.Any(c => c.PersonId == person.Id))
            {
                return new List<ProductOrder>();
            }
            else
            {
                orders = await context.ProductOrder.Where(order => order.CustomerId == person.Customer.First().Id).ToListAsync();
            }

            foreach (ProductOrder order in orders)
            {
                order.Status = await context.OrderStatus.FindAsync(order.StatusId);
            }

            return orders;
        }

        public async Task<Tuple<ProductOrder, Dictionary<Product, int[]>>> GetOrder(int id)
        {
            var order = await context.ProductOrder.FindAsync(id);
            if (order == null)
                return null;

            order.Address = await context.Address.FindAsync(order.AddressId);
            order.Customer = await context.Customer.FindAsync(order.CustomerId);
            order.Status = await context.OrderStatus.FindAsync(order.StatusId);

            order.ProductLinkProductOrder = context.ProductLinkProductOrder.Where(link => link.OrderId == id).ToHashSet();
            Dictionary<Product, int[]> orderProducts = new Dictionary<Product, int[]>();

            foreach (int productId in order.ProductLinkProductOrder.Select(link => link.ProductId))
            {
                Product product = await context.Product.FindAsync(productId);
                product.Image = await context.Image.Where(img => img.Id == product.ImageId).FirstAsync();
                product.Category = await context.Subcategory.Where(cat => cat.Id == product.CategoryId).FirstAsync();

                int number = order.ProductLinkProductOrder.Where(link => link.ProductId == productId).Select(link => link.Number).First();
                int cost = number * product.Price;
                int[] arr = { number, cost };
                orderProducts.Add(product, arr);
            }

            return new Tuple<ProductOrder, Dictionary<Product, int[]>>(order, orderProducts);
        }

        public async Task<ProductOrder> CreateOrder(Person person, string[] cartData)
        {
            var order = new ProductOrder();

            order.Cost = 0;
            order.OrderDate = DateTime.Now;
            order.StatusId = 1;
            order.CustomerId = person.Customer.First().Id;
            order.AddressId = person.Address.First().Id;

            Dictionary<int, int> orderProducts = new Dictionary<int, int>();

            for (int i = 0; i < cartData.Length; i++)
            {
                string[] data = cartData[i].Split(new char[] { '.' });

                Product product = await context.Product.FindAsync(Convert.ToInt32(data[0]));
                int count = Convert.ToInt32(data[1]);
                //product.Image = await db.Image.Where(img => img.Id == product.ImageId).FirstAsync();
                //product.Category = await db.Subcategory.Where(cat => cat.Id == product.CategoryId).FirstAsync();

                order.Cost += product.Price * count;

                orderProducts.Add(product.Id, count);
            }

            // create product_order entity
            context.ProductOrder.Add(order);
            context.SaveChanges();

            // create product_link_product_order entities
            foreach (var pair in orderProducts)
            {
                ProductLinkProductOrder link = new ProductLinkProductOrder();
                link.OrderId = order.Id;
                link.ProductId = pair.Key;
                link.Number = pair.Value;
                context.ProductLinkProductOrder.Add(link);
            }

            context.SaveChanges();

            // delete cart_item entities
            CartItem[] cartItems = context.CartItem.Where(c => c.PersonId == person.Id).ToArray();
            context.CartItem.RemoveRange(cartItems);
            context.SaveChanges();

            return order;
        }

        public async Task<Person> GetPerson(string email, string password)
        {
            var person = await context.Person.Where(p => p.Email == email && p.Password == password).FirstOrDefaultAsync();
            
            return person;
        }

        public async Task<Person> CreatePerson(Person person)
        {
            context.Person.Add(person);
            await context.SaveChangesAsync();

            return person;
        }

        public async Task<bool> UpdatePerson(Person person)
        {
            if (person == null)
                return false;

            context.Person.Update(person);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Product>> GetProducts(string category = null)
        {
            List<Product> products;

            if (!String.IsNullOrEmpty(category))
            {
                var currentCategory = await context.Category.Where(c => c.Name == category).FirstOrDefaultAsync();
                if (currentCategory != null)
                    products = await context.Product.Where(p => p.Category.CategoryId == currentCategory.Id).ToListAsync();
                else
                    products = await context.Product.ToListAsync();
            }
            else
            {
                products = await context.Product.ToListAsync();
            }

            foreach (Product product in products)
            {
                if (product.ImageId != null)
                    product.Image = await context.Image.Where(img => img.Id == product.ImageId).FirstAsync();
                product.Category = await context.Subcategory.Where(cat => cat.Id == product.CategoryId).FirstAsync();
            }

            return products;
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await context.Product.FindAsync(id);
            if (product == null)
                return null;
            
            product.Category = await context.Subcategory.FindAsync(product.CategoryId);
            if (product.ImageId != null)
                product.Image = await context.Image.FindAsync(product.ImageId);

            return product;
        }
        
        public async Task<Product> CreateProduct(Product product, IFormFile uploadedImage)
        {
            if (uploadedImage != null)
            {
                Image image = new Image();
                image.Filename = uploadedImage.FileName;
                image.Title = uploadedImage.FileName.Remove(uploadedImage.FileName.Count() - 4);
                using (var fs = uploadedImage.OpenReadStream())
                {
                    image.ImageData = new byte[fs.Length];
                    fs.Read(image.ImageData, 0, image.ImageData.Length);
                }

                context.Image.Add(image);
                context.SaveChanges();

                product.ImageId = image.Id;
            }

            context.Product.Add(product);
            context.SaveChanges();

            return product;
        }

        public async Task<bool> DeleteProduct(Product product)
        {
            if (product == null)
                return false;

            List<ProductLinkProductOrder> links = await context.ProductLinkProductOrder.Where(link => link.ProductId == product.Id).ToListAsync();
            if (links != null)
                context.ProductLinkProductOrder.RemoveRange(links);
            context.Product.Remove(product);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            if (product == null)
                return false;

            context.Product.Update(product);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Subcategory>> GetSubcategories()
        {
            return await context.Subcategory.ToListAsync();
        }

        public bool PersonIsAdmin(Person person)
        {
            if (person == null)
                return false;
            return person.AccessLevelId == 1 ? true : false ;
        }

        public async Task<bool> SaveChanges()
        {
            await context.SaveChangesAsync();
            return true;
        }
    }
}
