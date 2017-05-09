using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace Telegram_Bot_API
{
    public partial class Form1 : Form
    {
        HashSet<ChatId> chat_ids = new HashSet<ChatId>();
        string fileToSaveChatsIds = "ChatsToSendMessages.txt";
        public static readonly TelegramBotClient Client = new TelegramBotClient("385714306:AAG134HuQnlJtqopZXYP3QlsJPG8kykNdxE");
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Client.StartReceiving();
            Client.OnMessage += OnMessageHandler;
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            var update = Client.GetUpdatesAsync().Result;
            if (!update.Any())
                return;
            var chat_id = update.FirstOrDefault()?.Message.Chat.Id;
            if (chat_id == null)
                return;
            chat_ids.Add(chat_id);
            if (!chat_ids.Any())
                return;
            
        }

        private void OnMessageHandler(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            var chat_id = message.Chat.Id;
            chat_ids.Add(chat_id);
            chat_ids = new HashSet<ChatId>(chat_ids.Distinct(new ChatIdComparer()));
            //chat_ids.Distinct(new IEqualityComparer)
            XmlSerializer ser = new XmlSerializer(typeof(HashSet<string>));
            StreamWriter writer;
            if (!System.IO.File.Exists(this.fileToSaveChatsIds))
                writer = new StreamWriter(System.IO.File.Create(this.fileToSaveChatsIds));
            else
                writer = new StreamWriter(this.fileToSaveChatsIds);

            writer.WriteLine((string)chat_id);
            writer.Dispose();
            
        }
        public class ChatIdComparer : IEqualityComparer<ChatId>
        {
            public bool Equals(ChatId x, ChatId y)
            {
                var a1 = (string)x;
                var a2 = (string)y;
                return a1 == a2;
            }

            public int GetHashCode(ChatId obj)
            {
                return 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Client.StopReceiving();
        }

        private void SendButton_Click_1(object sender, EventArgs e)
        {
            foreach(var chatId in this.chat_ids)
                Client.SendTextMessageAsync(chatId, "Привет далбаёб");
        }
    }
    static class Bot
    {
        
    }
}
