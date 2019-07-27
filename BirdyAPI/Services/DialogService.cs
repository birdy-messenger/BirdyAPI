﻿using System;
using System.Collections.Generic;
using System.Linq;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;

namespace BirdyAPI.Services
{
    public class DialogService
    {
        private readonly BirdyContext _context;

        public DialogService(BirdyContext context)
        {
            _context = context;
        }

        public List<DialogPreviewDto> GetDialogsPreview(int userId)
        {
            return _context.DialogUsers.Where(k => k.FirstUserID == userId)
                .Union(_context.DialogUsers.Where(k => k.SecondUserID == userId))
                .Select(k => GetDialogPreview(k.DialogID, userId)).ToList();
        }

        private DialogPreviewDto GetDialogPreview(Guid dialogId, int currentUserId)
        {
            DialogUser currentDialog = _context.DialogUsers.Find(dialogId);

            Message lastMessage = _context.Messages.Where(k => k.ConversationID == dialogId)
                .OrderByDescending(k => k.SendDate).FirstOrDefault();

            return new DialogPreviewDto
            {
                InterlocutorUniqueTag = 
                    currentUserId == currentDialog.FirstUserID ? 
                        GetUserUniqueTag(currentDialog.SecondUserID) : GetUserUniqueTag(currentUserId),
                LastMessage = lastMessage?.Text,
                LastMessageTime = lastMessage?.SendDate ?? DateTime.MinValue
            };
        }

        private string GetUserUniqueTag(int userId)
        {
            return _context.Users.Find(userId).UniqueTag;
        }
    }
}
