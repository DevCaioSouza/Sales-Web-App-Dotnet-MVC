using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using SalesWebMVC.Services.Exceptions;

namespace SalesWebMVC.Services
{
    public class SellerService
    {
        private readonly SalesWebMVCContext _context;

        public SellerService (SalesWebMVCContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }

        public void Insert (Seller obj)
        {
            _context.Add(obj);
            _context.SaveChanges();
        }

        public Seller FindById(int id)
        {
            //Include torna a função um Eager Loading, que é carregar objetos associados ao obj principal
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id);
		}

        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id);
            _context.Seller.Remove(obj);
            _context.SaveChanges();
        }

        public void Update(Seller obj)
        {
            if (!_context.Seller.Any(x => x.Id == obj.Id))
            {
                throw new NotFoundException("Id not found.");
            }
			try
			{
				_context.Update(obj);
				_context.SaveChanges();
			}

			//Essa dinâmica abaixo serve para interceptarmos requisições no nível de acesso de dados
			//e as exibimos ao nível da camada de serviço
			//Daí o controller vai lidar apenas com exceções da camada de serviço
			catch (DbUpdateConcurrencyException e)
			{
				throw new DbConcurrencyException(e.Message);
			}
		}
    }
}
