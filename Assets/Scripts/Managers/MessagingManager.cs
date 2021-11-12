using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static DataProvider;

public class MessagingManager : MonoBehaviour
{
    public DataSource Data;

    public class InGameMessage
    {
        string messageId;
        string title;
        string senderName;
        string senderId;
        string senderUserName;
        string content;
        DateTime arrivedAt;
        DateTime readAt = DateTime.MinValue;
        bool contentDownloaded = false;

        public string MessageId
        {
            get
            {
                return messageId;
            }
        }

        public string SenderName
        {
            get
            {
                return senderName;
            }
        }

        public string SenderUserName
        {
            get
            {
                return senderUserName;
            }
        }

        public string SenderId
        {
            get
            {
                return senderId;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
        }

        public string Content
        {
            get
            {
                return content;
            }
        }

        public DateTime ArrivedAt
        {
            get
            {
                return arrivedAt;
            }
        }

        public DateTime ReadAt
        {
            get
            {
                return readAt;
            }
        }

        public bool ContentDownloaded
        {
            get
            {
                return contentDownloaded;
            }
        }

        public void SetContent(string content)
        {
            this.content = content;
            contentDownloaded = true;
        }

        public void SetReadAt(DateTime readAt)
        {
            this.readAt = readAt;
        }

        public InGameMessage(string messageId, string title, string senderId, string senderUserName, string senderName, DateTime arrivedAt)
        {
            this.messageId = messageId;
            this.senderId = senderId;
            this.senderName = senderName;
            this.senderUserName = senderUserName;
            this.arrivedAt = arrivedAt;
            this.title = title;
        }
    }

    public List<InGameMessage> Messages = new List<InGameMessage>();

    private void Start()
    {
    }

    public void QueryNewMessageCount(OnCompletionDelegateWithParameter onComplete)
    {
        Data.GetProvider().GetNewMessageCount(VariableManager.Instance.GetVariable("userid").ToString(), (obj) =>
        {
            onComplete?.Invoke(obj);
        });
    }

    public void GetMessageCount(OnCompletionDelegateWithParameter onComplete)
    {
        Data.GetProvider().GetNewMessageCount(VariableManager.Instance.GetVariable("userid").ToString(), (obj) =>
        {
            onComplete?.Invoke(obj);
        });
    }

    public void GetMessageHeaders(int messageCount, OnCompletionDelegateWithParameter onComplete)
    {
        Data.GetProvider().GetMessageHeaders(VariableManager.Instance.GetVariable("userid").ToString(), messageCount, (obj) =>
        {
            onComplete?.Invoke(obj);
        });
    }

    public void GetSentMessageHeaders(int messageCount, OnCompletionDelegateWithParameter onComplete)
    {
        throw new NotImplementedException();
    }

    public void Send(string to, string title, string content)
    {

    }

    public void Delete(string messageId, OnCompletionDelegate onComplete)
    {
        Data.GetProvider().DeleteMessage(VariableManager.Instance.GetVariable("userid").ToString(), messageId, () =>
        {
            onComplete?.Invoke();
        });
    }

    public void GetMessageBody(string messageId, OnCompletionDelegateWithParameter onComplete)
    {
        Data.GetProvider().GetMessageBody(VariableManager.Instance.GetVariable("userid").ToString(), messageId, (obj) =>
        {
            // return message body
            onComplete?.Invoke(obj);

            // mark the message as read
            Data.GetProvider().MarkMessageAsRead(VariableManager.Instance.GetVariable("userid").ToString(), messageId);
        });
    }
}
