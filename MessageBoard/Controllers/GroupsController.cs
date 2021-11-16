using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MessageBoard.Models;

namespace MessageBoard.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class GroupsController : ControllerBase
  {
    private readonly MessageBoardContext _db;

    public GroupsController(MessageBoardContext db)
    {
      _db = db;
    }

    // GET api/Messages
    [HttpGet]
    public ActionResult<IEnumerable<Group>> Get(string name)
    {
      var query = _db.Groups.AsQueryable();

      if (name != null)
      {
        query = query.Where(entry => entry.Name == name);
      }
      
      return query.ToList();
    }

    // POST api/Groups
    [HttpPost]
    public async Task<ActionResult<Group>> Post(Group group)
    {
      _db.Groups.Add(group);
      await _db.SaveChangesAsync();

      return CreatedAtAction(nameof(GetGroup), new { id = group.GroupId }, group);
    }
    // GET: api/Groups/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Group>> GetGroup(int id)
    {
      var Group = await _db.Groups.FindAsync(id);

      if (Group == null)
      {
          return NotFound();
      }

      return Group;
    }
     // PUT: api/Group/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Group Group)
    {
      if (id != Group.GroupId)
      {
        return BadRequest();
      }

      _db.Entry(Group).State = EntityState.Modified;

      try
      {
        await _db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!GroupExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

    private bool GroupExists(int id)
    {
      return _db.Groups.Any(e => e.GroupId == id);
    }
    // DELETE: api/Groups/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(int id)
    {
      var Group = await _db.Groups.FindAsync(id);
      if (Group == null)
      {
        return NotFound();
      }

      _db.Groups.Remove(Group);
      await _db.SaveChangesAsync();

      return NoContent();
    }
  }
}