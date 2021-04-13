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
    public abstract class BaseRepository
    {
        protected mystoreContext context;
        public BaseRepository(mystoreContext _context)
        {
            context = _context;
        }
    }
}