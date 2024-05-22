using Business.Exceptions;
using Business.Services.Abstacts;
using Core.Models;
using Core.RespositoryAbstracts;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Concretes;

public class PortfolioService : IPortfolioService
{
    IPortfolioRepository _repository;
    IWebHostEnvironment _webHostEnvironment;

    public PortfolioService(IPortfolioRepository repository, IWebHostEnvironment webHostEnvironment)
    {
        _repository = repository;
        _webHostEnvironment = webHostEnvironment;
    }

    public void CrearePortfolio(Portfolio portfolio)
    {
        if(portfolio == null)
        {
            throw new NotFoundFileException("", "Protfolio is not found!");
        }
        if(portfolio.PhotoFile == null)
        {
            throw new NotFoundFileException("PhotoFile","PhotoFile not Found!");
        }
        if (!portfolio.PhotoFile.ContentType.Contains("image/"))
        {
            throw new ContentTypeException("PhotoFile", "PhotoFile content is not valid");
        }
        string path = _webHostEnvironment.WebRootPath + @"\upload\portfolio\" + portfolio.PhotoFile.FileName;
        using(FileStream file = new FileStream(path, FileMode.Create))
        {
            portfolio.PhotoFile.CopyTo(file);
        }
        portfolio.ImgUrl = portfolio.PhotoFile.FileName;
        _repository.Add(portfolio);
        _repository.Commit();
    }

    public void DeletePortfolio(int id)
    {
        Portfolio port = _repository.Get(x=>x.Id == id);
        if(port == null)
        {
            throw new NotFoundFileException("", "Portfolio is not found!");
        }
        string path = _webHostEnvironment.WebRootPath + @"\upload\portfolio\" + port.ImgUrl;
        if (!File.Exists(path)) throw new NotFoundFileException("PhotoFile", "PhotoFile is not found");

        File.Delete(path);
        _repository.Remove(port);
        _repository.Commit();
    }

    public List<Portfolio> GetAllPortfolio(Func<Portfolio, bool>? func = null)
    {
        return _repository.GetAll(func);
    }

    public Portfolio GetPortfolio(Func<Portfolio, bool>? func = null)
    {
        return _repository.Get(func);
    }

    public void UpdatePortfolio(int id, Portfolio newPortfolio)
    {
        Portfolio oldPort = _repository.Get(x => x.Id == id);
        if (oldPort == null)
        {
            throw new NotFoundFileException("", "Portfolio is not found!");
        }

        if(newPortfolio.PhotoFile != null)
        {
            string path = _webHostEnvironment.WebRootPath + @"\upload\portfolio\" + oldPort.ImgUrl;
            if (File.Exists(path))
            {
                File.Delete(path);

            }


            if (!newPortfolio.PhotoFile.ContentType.Contains("image/"))
            {
                throw new ContentTypeException("PhotoFile", "PhotoFile content is not valid");
            }
            string newPath = _webHostEnvironment.WebRootPath + @"\upload\portfolio\" + newPortfolio.PhotoFile.FileName;
            using (FileStream file = new FileStream(newPath, FileMode.Create))
            {
                newPortfolio.PhotoFile.CopyTo(file);
            }

            oldPort.ImgUrl = newPortfolio.PhotoFile.FileName;
        }

        oldPort.Title = newPortfolio.Title;
        oldPort.SubTitle = newPortfolio.SubTitle;

        _repository.Commit();
    }
}
