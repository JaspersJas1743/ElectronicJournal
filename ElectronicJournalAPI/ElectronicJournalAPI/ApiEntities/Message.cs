using System;
using System.Threading;
using System.Threading.Tasks;

namespace ElectronicJournalAPI.ApiEntities
{
    public class Message
    {
        public string Header { get; set; }
        public string Sender { get; set; }
        public int SenderId { get; set; }
        public string Receiver { get; set; }
        public int ReceiverId { get; set; }
        public DateTime Date { get; set; }
        public bool HaveText { get; set; }
        public string Text { get; set; }
        public bool HaveAttachment { get; set; }
        public Attachment Attachment { get; set; }

        public class SendMessageRequest
        {
            public int ReceiverId { get; set; }
            public string Text { get; set; }
            public int? AttachmentId { get; set; }
        }
        public class SendMessageResponse
        {
            public string Message { get; set; }
        }

        public async Task<string> Send(CancellationToken cancellationToken = default)
        {
            int? attachmentId = null;
            if (Attachment != null)
            {
                Attachment.UploadAttachmentResponse attachment = await Attachment.Upload(cancellationToken: cancellationToken);
                attachmentId = attachment.Id;
            }

            SendMessageResponse response = await ApiClient.PostAsync<SendMessageResponse, SendMessageRequest>(
                apiMethod: "Messages/SendMessage",
                arg: new SendMessageRequest
                {
                    ReceiverId = ReceiverId,
                    Text = Text,
                    AttachmentId = attachmentId
                }
            );
            return response.Message;
        }
    }
}
