using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAudioCityGuide.Models;

namespace SmartAudioCityGuide.Services
{
    public class CommentServices : ICommentServices, IDisposable
    {
        private SmartAudioCityGuideEntities db ;

        public CommentServices() : this(new SmartAudioCityGuideEntities())
        {
        }

        public CommentServices(SmartAudioCityGuideEntities context)
        {
            db = context;
        }

        public void addComment(Comments comment)
        {
            db.comments.Add(comment);
            db.SaveChanges();
        }

        public List<Comments> findCommentByIdLocation(int idLocation)
        {
            List<Comments> comments = (from com in db.comments
                                       where com.locationsId == idLocation
                                       select com).ToList();
            return comments;
        }

        public List<Comments> findCommentsByIdUser(int idUser)
        {
            List<Comments> commentsFromUser = new List<Comments>();

            commentsFromUser = (from comments in db.comments
                                where comments.userId == idUser
                                select comments).ToList();

            return commentsFromUser;
        }

        public List<Comments> findCommentsByPhoneId(string phoneId)
        {
            List<Comments> commentsFromUser = new List<Comments>();

            commentsFromUser = (from comments in db.comments
                                join user in db.users on comments.userId equals user.id
                                where user.phoneId == phoneId
                                select comments).ToList();

            return commentsFromUser;

        }

        public Comments findCommentByIdComment(int idComment)
        {
            Comments comment = new Comments();
            comment = db.comments.FirstOrDefault(m => m.id == idComment);
            return comment;
        }

        public void editCommentWithIdAndComment(int idComment, string description)
        {
            Comments comment = new Comments();
            comment = db.comments.FirstOrDefault(m => m.id == idComment);
            comment.description = description;
            db.SaveChanges();
        }

        public void updateCommentForId(int idComment, Comments newComment)
        {
            Comments comment = new Comments();
            comment = db.comments.FirstOrDefault(m => m.id == idComment);
            comment.description = newComment.description;
            comment.sound = newComment.sound;
            comment.typeOfCommentsId = newComment.typeOfCommentsId;
            db.SaveChanges();
        }

        public void deleteComment(int idComment)
        {
            Comments comment = new Comments();
            comment = db.comments.FirstOrDefault(m => m.id == idComment);
            db.comments.Remove(comment);
            db.SaveChanges();

        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}