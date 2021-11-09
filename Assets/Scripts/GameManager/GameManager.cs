using System.Collections.Generic; // Данная библиотека позволяет использовать словари
using UnityEngine;

// В данном классе мы будем отслеживать и помещать всех наших игроков в словарь dictionaries (для того, чтобы брать оттуда игроков и взаимодействовать с ними)
public class GameManager : MonoBehaviour
{
    private const string PLAYER_ID = "Player "; // Добавим строковую константу PLAYER_ID, которую обязательно записываем с большой буквы и присвоим ей значение "Player "
    private static Dictionary<string, Player> players = new Dictionary<string, Player>(); // Создаем новый словарь players. В параметрах укажем ключ string и значение, где привяжем класс Player
    // т.к. словарь имеет модификатор доступа private static мы можем спокойно обратиться к данному словарю из любого метода внутри данного класса

    // Создадим методы для присваивания значения в словарь players и чтобы брать значения оттуда

    // Данный метод будет регистрировать игроков и записывать их в словать players
    public static void RegisterPlayer(string _netID, Player player) // Мы будем ссылаться на этот метод из других классов, по этой причине сделаем модификаторы доступа public static
    // В параметрах метода зададим: _netID - уникальный идентификатор каждого игрока 1,2,3 ... и экземпляр класса player (значение для нашего словаря)
    {
        string playerID = PLAYER_ID + _netID; // Значение playerID теперь будет представлять из себя строку вида: player 1, player 2, player 3 и т.д.
        players.Add(playerID, player); // Добавим к нашему словарю новый элемент (добавляется благодаря параметрам: ключ string playerID и значение, т.е. наш экземпляр класса player)
        player.transform.name = playerID; // Зададим имя для каждого игрока в экземпляре класса вида: player 1, player 2, player 3 и т.д.
    }

    public static void UnRegisterPlayer(string playerID) // Создадим метод для удаления игрока (при выходе с сервера)
    {
        players.Remove(playerID); // Удаляем из словаря передаваемого в параметре метода игрока с помощью ключевого слова Remove
    }

    public static Player GetPlayer(string playerID) // Создадим метод возвращающий игрока по его ID (тип возвращаемого значения Player, т.е. мы возвращаем конктретного игрока из словаря)
    {
        return players[playerID]; // Возвращаем игрока из словаря players по его идентификатору playerID
    }
}
