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
  public class MessagesController : ControllerBase
  {
    private readonly MessageBoardContext _db;

    public MessagesController(MessageBoardContext db)
    {
      _db = db;
    }

    // GET api/Messages
    [HttpGet]
    public ActionResult<IEnumerable<Message>> Get(string header, string body)
    {
      var query = _db.Messages.AsQueryable();

      if (header != null)
      {
        query = query.Where(entry => entry.Header == header);
      }
      if (body != null)
      {
        query = query.Where(entry => entry.Body == body);
      }
      
      return query.ToList();
    }

    // POST api/Messages
    [HttpPost]
    public async Task<ActionResult<Message>> Post(Message Message)
    {
      _db.Messages.Add(Message);
      await _db.SaveChangesAsync();

      return CreatedAtAction(nameof(GetMessage), new { id = Message.MessageId }, Message);
    }
    // GET: api/Messages/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Message>> GetMessage(int id)
    {
      var Message = await _db.Messages.FindAsync(id);

      if (Message == null)
      {
          return NotFound();
      }

      return Message;
    }
     // PUT: api/Message/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Message Message)
    {
      if (id != Message.MessageId)
      {
        return BadRequest();
      }

      _db.Entry(Message).State = EntityState.Modified;

      try
      {
        await _db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!MessageExists(id))
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

    private bool MessageExists(int id)
    {
      return _db.Messages.Any(e => e.MessageId == id);
    }
    // DELETE: api/Messages/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMessage(int id)
    {
      var Message = await _db.Messages.FindAsync(id);
      if (Message == null)
      {
        return NotFound();
      }

      _db.Messages.Remove(Message);
      await _db.SaveChangesAsync();

      return NoContent();
    }
  }
}