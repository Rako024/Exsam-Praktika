using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Abstacts
{
    public interface IPortfolioService
    {
        void CrearePortfolio(Portfolio portfolio);
        void DeletePortfolio(int id);
        void UpdatePortfolio(int id, Portfolio portfolio);

        Portfolio GetPortfolio(Func<Portfolio,bool>? func = null);
        List<Portfolio> GetAllPortfolio(Func<Portfolio, bool>? func = null);
    }
}
