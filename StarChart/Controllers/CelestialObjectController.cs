using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var currCelObj = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);
            if (currCelObj == null)
            {
                return NotFound();
            }
            var currSatelites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == id).ToList();
            currCelObj.Satellites = currSatelites;
            return Ok(currCelObj);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var cList = _context.CelestialObjects.Where(x => x.Name == name).ToList();
            if (cList.Count==0)
            {
                return NotFound();
            }
            var allList = _context.CelestialObjects.ToList();
            for (int i = 0; i < cList.Count; i++)
            {
                var outer = cList[i];
                for (int j = 0; j < allList.Count; j++)
                {
                    var inner = allList[j];
                    if (outer.Id == inner.OrbitedObjectId)
                    {
                        if (outer.Satellites == null)
                        {
                            outer.Satellites = new List<CelestialObject>();
                        }
                        outer.Satellites.Add(inner);
                    }

                }
            }
            return Ok(cList);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var cList = _context.CelestialObjects.ToList();
            for (int i = 0; i < cList.Count; i++)
            {
                var outer = cList[i];
                for (int j = 0; j < cList.Count; j++)
                {
                    var inner = cList[j];
                    if(outer.Id== inner.OrbitedObjectId)
                    {
                        if (outer.Satellites == null)
                        {
                            outer.Satellites = new List<CelestialObject>();
                        }
                        outer.Satellites.Add(inner);
                    }

                }
            }
            return Ok(cList);
        }
    }
}
