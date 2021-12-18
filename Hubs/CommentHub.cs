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
                var existGame = new Game();
                // try {
                //     existGame = _context.Game.FirstOrDefault(g => g.IdGame == newCmt.IdGame);
                //     existGame.AverageRate = (newCmt.Star + existGame.AverageRate*existGame.NumOfRate)/(existGame.NumOfRate+1);
                //     existGame.NumOfRate += 1;
                // } catch (Exception e) {
                //     Console.WriteLine(e);
                //     throw e;
                // }
                

                _context.SaveChanges();
                await Clients.Group(userConnection.Room)
                    .SendAsync("ReceiveCreateComment", userConnection.User, newCmt, existGame.AverageRate);
            }
        }
        public async Task UpdateComment(Comments updateCmt,string idUserLike, string action)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                var existCmt = _context.Comments
                    .FirstOrDefault(cmt => cmt.IdComment == updateCmt.IdComment);
                if (existCmt != null)
                {
                    var existLikeComment = _context.LikeComment
                        .FirstOrDefault(e => e.IdComment == updateCmt.IdComment && e.IdUser == idUserLike);
                    switch (action){
                        case "like":
                            if (existLikeComment == null){
                                updateCmt.Likes ++;
                                LikeComment newLikeCmt = new LikeComment(
                                    updateCmt.IdComment, idUserLike, true
                                );
                                _context.LikeComment.Add(newLikeCmt);
                                _context.SaveChanges();
                            } else
                            if (existLikeComment.IsLike) {
                                updateCmt.Likes --;
                                _context.LikeComment.Remove(existLikeComment);
                                _context.SaveChanges();
                            } else {
                                updateCmt.Likes ++;
                                updateCmt.Dislike --;
                                LikeComment newLikeCmt = existLikeComment;
                                newLikeCmt.IsLike = true;
                                _mapper.Map(newLikeCmt, existLikeComment);
                                _context.SaveChanges();
                            }
                            break;
                        case "dislike":
                            if (existLikeComment == null){
                                updateCmt.Dislike ++;
                                LikeComment newLikeCmt = new LikeComment(
                                    updateCmt.IdComment, idUserLike, false
                                );
                                _context.LikeComment.Add(newLikeCmt);
                                _context.SaveChanges();
                            } else
                            if (!existLikeComment.IsLike){
                                updateCmt.Dislike --;
                                _context.LikeComment.Remove(existLikeComment);
                                _context.SaveChanges();
                            } else {
                                updateCmt.Likes --;
                                updateCmt.Dislike ++;
                                LikeComment newLikeCmt = existLikeComment;
                                newLikeCmt.IsLike = false;
                                _mapper.Map(newLikeCmt, existLikeComment);
                                _context.SaveChanges();
                            }
                            break;
                        case "change-content":
                            break;
                    }
                    existCmt.Likes = updateCmt.Likes;
                    existCmt.Dislike = updateCmt.Dislike;
                    existCmt.Content = updateCmt.Content;
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
                    var likeCmtOfThisCmt = _context.LikeComment
                        .Where(e => e.IdUser == existCmt.IdUser)
                        .ToList();
                    _context.LikeComment.RemoveRange(likeCmtOfThisCmt);

                    var existGame = _context.Game.FirstOrDefault(g => g.IdGame == existCmt.IdGame);
                    if (existGame.NumOfRate == 1) existGame.AverageRate = (existGame.AverageRate*existGame.NumOfRate - existCmt.Star);
                        else existGame.AverageRate = (existGame.AverageRate*existGame.NumOfRate - existCmt.Star)/(existGame.NumOfRate-1);
                    existGame.NumOfRate -= 1;
                    _context.SaveChanges();
                    await Clients.Group(userConnection.Room)
                        .SendAsync("ReceiveDeleteComment", userConnection.User, idComment, existGame.AverageRate);
                } else
                {
                    await Clients.Group(userConnection.Room)
                      .SendAsync("ReceiveMessage", userConnection.User, "404");
                }
            }
        }

      
    }
}
