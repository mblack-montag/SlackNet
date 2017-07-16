﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SlackNet.Events;

namespace SlackNet.Bot
{
    public class SlackMessage : IMessage
    {
        private readonly SlackBot _bot;

        public SlackMessage(MessageEvent message, SlackBot bot)
        {
            MessageEvent = message;
            _bot = bot;
        }

        public MessageEvent MessageEvent { get; }
        public Hub Hub { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
        public string Ts { get; set; }
        public DateTime Timestamp => Ts.ToDateTime().GetValueOrDefault();
        public string ThreadTs { get; set; }
        public DateTime ThreadTimestamp => Ts.ToDateTime().GetValueOrDefault();
        public IList<Attachment> Attachments { get; set; } = new List<Attachment>();
        public bool IsInThread => ThreadTs != null;
        public bool MentionsBot => Text.IndexOf(_bot.Id, StringComparison.OrdinalIgnoreCase) >= 0
            || Text.IndexOf(_bot.Name, StringComparison.OrdinalIgnoreCase) >= 0
            || Hub.IsIm;

        public Task ReplyWith(string text, bool createThread = false) => ReplyWith(new BotMessage { Text = text }, createThread);

        public async Task ReplyWith(Func<Task<BotMessage>> createReply, bool createThread = false)
        {
            await _bot.WhileTyping(Hub.Id, async () =>
                {
                    BotMessage reply = await createReply().ConfigureAwait(false);
                    if (reply != null)
                        await ReplyWith(reply, createThread).ConfigureAwait(false);
                }).ConfigureAwait(false);
        }

        public async Task ReplyWith(BotMessage message, bool createThread = false)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            message.ReplyTo = this;
            message.CreateThread = createThread;
            await _bot.Send(message).ConfigureAwait(false);
        }
    }
}