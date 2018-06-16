using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using NHibernate.Extensions.WebTest.Models;

namespace NHibernate.Extensions.WebTest.Controllers {

    [Route("api/[controller]")]
    public class TodoController : ControllerBase {

        private static IList<ToDoItemModel> items;

        public TodoController() {
            if (items == null) {
                items = new List<ToDoItemModel> {
                    new ToDoItemModel {
                        Id = 1,
                        Title = "To do item 1",
                        Completed = false
                    },
                    new ToDoItemModel {
                        Id = 2,
                        Title = "To do item 2",
                        Completed = false
                    },
                    new ToDoItemModel {
                        Id = 3,
                        Title = "To do item 3",
                        Completed = false
                    }
                };
            }
        }

        [HttpGet("")]
        public IActionResult GetAll() {
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id) {
            var item = items.FirstOrDefault(i => i.Id == id);
            if (item == null) {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost("")]
        public IActionResult Save([FromBody]ToDoItemModel item) {
            item.Id = items.Max(i => i.Id) + 1;
            items.Add(item);
            return Ok(item);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody]ToDoItemModel item) {
            var todo = items.FirstOrDefault(i => i.Id == id);
            if (todo == null) {
                return NotFound();
            }
            todo.Title = item.Title;
            todo.Description = item.Description;
            todo.Completed = item.Completed;
            //
            return Ok(todo);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id) {
            var item = items.FirstOrDefault(i => i.Id == id);
            if (item != null) {
                items.Remove(item);
            }
            return NoContent();
        }
    }

}
