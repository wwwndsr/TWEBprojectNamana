using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webNamana.Models
{
    public class Payment
    {
        public int Id { get; set; } //уникальный идентификатор платежа
        public decimal Amount { get; set; } //сумма платежа
        public DateTime PaymentDate { get; set; } //дата платежа
        public string PaymentMethod { get; set; } //способ оплаты (например, "Наличные", "Банковская карта")
        public string TransactionId { get; set; } //идентификатор транзакции (если используется внешний платежный шлюз)
        public string Status { get; set; } //статус платежа (например, "В процессе...", "Выполнено", "Ошибка")
        public int OrderId { get; set; } //внешний ключ для заказа
        public Order Order { get; set; } //навигационное свойство для заказа
        public int UserId { get; set; } //внешний ключ для пользователя
        public User User { get; set; } //навигационное свойство для пользователя
    }
}