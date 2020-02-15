using UnityEngine;
using UnityEngine.Networking; // Подключаем мультиплеерную библиотеку Networking от движка Unity

public class Player : NetworkBehaviour // Наследование будем делать не от основного класса MonoBehaviour, а от мультиплеерного класса NetworkBehaviour
{
    [SerializeField] // Задаем область видимости для юнити в инспекторе
    private GameObject player; // Задаем компонент игрок

    [SerializeField] // Задаем область видимости для юнити в инспекторе
    private float MaxHealth = 100f; // Задаем величину жизней игрока

    [SyncVar] // Это означает, что данная переменная currHealth будет принадлежать каждому конкретному объекту (игроку)
    // Т.е. этот префикс заставляет переменную синхронизироваться с её серверной версией
    private float currHealth; // Текущее количество жизней игрока

    void Awake() // Метод для задания значения текущего количества жизней игрока
    {
        currHealth = MaxHealth; // Текущее количество жизней игрока по умолчанию равно максимальному значение в 100 хп
    }

    public void TakeDamage(float damage) // Вызываем метод для нанесения урона игроку
    {
        currHealth -= damage; // При попадании по игроку вычитаем из его текущего здоровья величину damage

        Debug.Log(transform.name + " имеет уровень здоровья равный: " + currHealth); // Выводим на экран сообщение о количестве жизней игрока

        if (currHealth <= 0) // Если текущее здоровье меньше или равно 0
        {
            Destroy(player); // Уничтожаем игрока
            Debug.Log(transform.name + " убит");  // Выводим на экран сообщение, что игрок убит
        }
    }
}
