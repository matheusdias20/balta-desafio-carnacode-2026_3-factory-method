using System;
using System.Collections.Generic;

namespace DesignPatternChallenge
{
    // Contexto: Sistema de notificações que envia mensagens para clientes
    // Cada tipo de notificação tem requisitos e formatação diferentes
    
    public class NotificationManager
    {
        // Mapa de factories para evitar if/else - Poderia ser injetado via DI em um cenário real
        private readonly Dictionary<string, NotificationFactory> _factories;

        public NotificationManager()
        {
            _factories = new Dictionary<string, NotificationFactory>(StringComparer.OrdinalIgnoreCase)
            {
                { "email", new EmailFactory() },
                { "sms", new SmsFactory() },
                { "push", new PushFactory() },
                { "whatsapp", new WhatsAppFactory() }
            };
        }

        private ISender GetSender(string notificationType, string recipient)
        {
            if (_factories.TryGetValue(notificationType, out var factory))
            {
                return factory.CreateNotification(recipient);
            }
            throw new ArgumentException($"Tipo de notificação '{notificationType}' não suportado");
        }

        public void SendOrderConfirmation(string recipient, string orderNumber, string notificationType)
        {
            var sender = GetSender(notificationType, recipient);
            sender.Send($"Seu pedido {orderNumber} foi confirmado!", "Confirmação de Pedido");
        }

        public void SendShippingUpdate(string recipient, string trackingCode, string notificationType)
        {
            var sender = GetSender(notificationType, recipient);
            sender.Send($"Seu pedido foi enviado! Código de rastreamento: {trackingCode}", "Pedido Enviado");
        }

        public void SendPaymentReminder(string recipient, decimal amount, string notificationType)
        {
            var sender = GetSender(notificationType, recipient);
            sender.Send($"Você tem um pagamento pendente de R$ {amount:N2}", "Lembrete de Pagamento");
        }
    }

    // Classes concretas de notificação implementando ISender
    public class EmailNotification : ISender
    {
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public bool IsHtml { get; set; } = true;

        public void Send(string message, string subject = "")
        {
            if (!string.IsNullOrEmpty(subject)) Subject = subject;

            Console.WriteLine($"📧 Enviando Email para {Recipient}");
            Console.WriteLine($"   Assunto: {Subject}");
            Console.WriteLine($"   Mensagem: {message}");
        }
    }

    public class SmsNotification : ISender
    {
        public string PhoneNumber { get; set; }
        // Mapeando Recipient para PhoneNumber para satisfazer a interface
        public string Recipient { get => PhoneNumber; set => PhoneNumber = value; }

        public void Send(string message, string subject = "")
        {
            Console.WriteLine($"📱 Enviando SMS para {PhoneNumber}");
            Console.WriteLine($"   Mensagem: {message}");
        }
    }

    public class PushNotification : ISender
    {
        public string DeviceToken { get; set; }
        public string Recipient { get => DeviceToken; set => DeviceToken = value; }
        public string Title { get; set; }
        public int Badge { get; set; } = 1;

        public void Send(string message, string subject = "")
        {
            if (!string.IsNullOrEmpty(subject)) Title = subject;

            Console.WriteLine($"🔔 Enviando Push para dispositivo {DeviceToken}");
            Console.WriteLine($"   Título: {Title}");
            Console.WriteLine($"   Mensagem: {message}");
        }
    }

    public class WhatsAppNotification : ISender
    {
        public string PhoneNumber { get; set; }
        public string Recipient { get => PhoneNumber; set => PhoneNumber = value; }
        public bool UseTemplate { get; set; } = true;

        public void Send(string message, string subject = "")
        {
            Console.WriteLine($"💬 Enviando WhatsApp para {PhoneNumber}");
            Console.WriteLine($"   Mensagem: {message}");
            Console.WriteLine($"   Template: {UseTemplate}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Sistema de Notificações (Factory Method) ===\n");

            var manager = new NotificationManager();

            // Cliente 1 prefere Email
            manager.SendOrderConfirmation("cliente@email.com", "12345", "email");
            Console.WriteLine();

            // Cliente 2 prefere SMS
            manager.SendOrderConfirmation("+5511999999999", "12346", "sms");
            Console.WriteLine();

            // Cliente 3 prefere Push
            manager.SendShippingUpdate("device-token-abc123", "BR123456789", "push");
            Console.WriteLine();

            // Cliente 4 prefere WhatsApp
            manager.SendPaymentReminder("+5511888888888", 150.00m, "whatsapp");

             // Testando novo tipo (simulação de extensibilidade)
             // Para adicionar Telegram, bastaria criar TelegramFactory e TelegramNotification
             // e registrar no dicionário do Manager.
        }
    }
}
