using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAudioCityGuide.Models;

namespace SmartAudioCityGuide.Services
{
    public interface ICommentServices : IDisposable
    {
        void addComment(Comments comment);
        List<Comments> findCommentByIdLocation(int idLocation);
        List<Comments> findCommentsByIdUser(int idUser);
        Comments findCommentByIdComment(int idComment);
        void editCommentWithIdAndComment(int idComment, string description);
        void deleteComment(int idComment);
        void updateCommentForId(int idComment, Comments newComment);
    }
}