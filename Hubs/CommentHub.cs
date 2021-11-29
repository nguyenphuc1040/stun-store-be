using game_store_be.CustomModel;
using game_store_be.Controllers;
using game_store_be.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using game_store_be.Dtos;

namespace game_store_be.Hubs
{
    public class CommentHub : Hub
    {
        private readonly IDictionary<string, UserConnection> _connections;
        private readonly game_storeContext _context;
        private readonly IMapper _mapper;
        public CommentHub(IDictionary<string, UserConnection> connections, game_storeContext context, IMapper mapper)
        {
            _connections = connections;
            _context = context;
            _mapper = mapper;
        }

        public async Task SendMessage(string comment)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {

                await Clients.Group(userConnection.Room)
                    .SendAsync("ReceiveMessage", userConnection.User, comment);
            }
        }

        public async Task JoinRoom(UserConnection userConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);
           
            _connections[Context.ConnectionId] = userConnection;
            await Clients.Group(userConnection.Room)
                    .SendAsync("ReceiveMessage", userConnection.User, "user join room");
        }

        public async Task CreateComment(Comments newCmt)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                newCmt.IdComment = Guid.NewGuid().ToString();
                newCmt.Likes = newCmt.Dislike = 0;
                newCmt.Time = DateTime.UtcNow;
                _context.Comments.Add(newCmt);
                _context.SaveChanges();
              /*  var commentsDto = _mapper.Map<Comments, CommentsDto>(newCmt);*/
                await Clients.Group(userConnection.Room)
                    .SendAsync("ReceiveCreateComment", userConnection.User, newCmt);
            }
        }
        public async Task UpdateComment(Comments updateCmt)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                var existCmt = _context.Comments
                    .FirstOrDefault(cmt => cmt.IdComment == updateCmt.IdComment);
                if (existCmt != null)
                {
                    _mapper.Map(updateCmt, existCmt);
                    _context.SaveChanges();
                    await Clients.Group(userConnection.Room)
                        .SendAsync("ReceiveUpdateComment", userConnection.User, updateCmt);
                } else
                {
                    await Clients.Group(userConnection.Room)
                        .SendAsync("ReceiveMessage", userConnection.User, "404");
                }
            }
        }

        public async Task DeleteComment(string idComment)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                var existCmt = _context.Comments
                .FirstOrDefault(cmt => cmt.IdComment == idComment);
                if (existCmt != null)
                {
                    _context.Comments.Remove(existCmt);
                    await Clients.Group(userConnection.Room)
                        .SendAsync("ReceiveDeleteComment", userConnection.User, idComment);
                } else
                {
                    await Clients.Group(userConnection.Room)
                      .SendAsync("ReceiveMessage", userConnection.User, "404");
                }
            }
        }
    }
}
