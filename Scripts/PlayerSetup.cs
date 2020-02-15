using UnityEngine;
using UnityEngine.Networking; // Подключаем мультиплеерную библиотеку Networking от движка Unity

[RequireComponent(typeof(Player))] // Когда мы подключаем PlayerSetup автоматически подключаем к данному скрипту Player скрипт

public class PlayerSetup : NetworkBehaviour // Наследование будем делать не от основного класса MonoBehaviour, а от мультиплеерного класса NetworkBehaviour
// соответственно, при наследовании классов мы получим доступ ко всем методам и полям базового класса NetworkBehaviour
{
    [SerializeField] // Делаем видимыми для редактирования элементы массива в Unity
    Behaviour[] componentsToDisable; // Создаем массив для скрытия компонентов игрока (скрипты игрока, камера игрока, audio listener: микрофон),
    //  если мы не являемся локальным игроком (т.е играем на сервере)

    [SerializeField] // Задаем область видимости для юнити в инспекторе
    private string remoteLayer = "RemotePlayer"; // Создадим локальное строковое поле remoteLayer с значением по умолчанию "RemotePlayer"
    private Camera sceneCamera; // Создаем экземпляр класса sceneCamera с типом Camera

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false; // Скрываем курсор мыши, при начале игры
        Screen.lockCursor = true; // Блокируем курсор мыши в центре экрана

        // Получаем доступ к полям класса NetworkBehaviour
        // Создаются игроки
        if (!isLocalPlayer) // Проверяем, если игрок не является локальным игроком (т.е подключается с другого ПК)
        // Отключаем все компоненты у этого игрока (т.е. у всех других игроков, которые играют вместе с нами)
        {
            for (int i = 0; i < componentsToDisable.Length; i++) // Пробегаемся по элементам массива componentsToDisable, до окончания его длины
            {
                componentsToDisable[i].enabled = false; // Присваиваем каждому элементу массива значение false, таким образом отключая компоненты в Unity
            }
            gameObject.layer = LayerMask.NameToLayer(remoteLayer); // Для игрока, который не является локальным задаем имя слоя (в префабе player) "RemotePlayer"
            // LayerMask (маска слоя) - экземпляр класса, наследуемый от базового класса NetworkBehaviour
        }
        else // Иначе, когда при создании игроков создается наш локальный игрок
        {
            sceneCamera = Camera.main; // Укажем, что экземпляр класса sceneCamera принимает значение основной (main) камеры (вызываем основную камеру через тег MainCamera)
            if (sceneCamera != null) // Если основная камера присутствует на сцене
            {
                sceneCamera.gameObject.SetActive(false); // То скрываем её со сцены
            }
        }
    }

    // Когда игрок создается (при входе в игру) мы регистрируем его с помощью метода RegisterPlayer, передавая в него идентификатор игрока и скрипт игрока и добавляем игрока в словарь
    public override void OnStartClient() // Перепишем метод OnStartClient(), который по умолчанию содержится в мультиплеерном классе NetworkBehaviour (с помощью спецификатора override - Полиморфизм)
    {
        base.OnStartClient(); // Берем основной код из метода в базовом классе NetworkBehaviour и далее дописываем свой собственный код
        string netID = GetComponent<NetworkIdentity>().netId.ToString(); // Берем компонент NetworkIdentity, прикрепленный к префабу игрока и берем у него поле уникального идентификатора каждого созданного игрока netId
        // Затем преобразуем идентификатор игрока из int в строку string, с помощью конвертера ToString() и запишем значение в поле net_ID
        Player player = GetComponent<Player>(); // Создадим экземпляр класса player и запустим скрипт Player
        GameManager.RegisterPlayer(netID, player); // Т.к. методу RegisterPlayer мы задали модификатор доступа public static мы можем спокойно обратиться к данному методу в классе GameManager из любого другого класса
        // и передать через его параметры необходимые нам значения net_ID, player
    }


    // Игрок уничтожается, при выходе с сервера и вызывается метод void OnDisable() из базового класса NetworkBehaviour
    void OnDisable() // Если игрок вышел с сервера, мы можем вызвать данный метод и включить камеру обратно
    {
        Cursor.visible = true; // Отображаем курсор мыши, когда игрок отсутствует на сервере
        Screen.lockCursor = false; // Разблокируем курсор мыши в центре экрана

        if (sceneCamera != null) // Если основная камера присутствует на сцене
        {
            sceneCamera.gameObject.SetActive(true); // То включаем её обратно
        }

        // Удаляем игрока (при выходе из игры) из словаря (списка)
        GameManager.UnRegisterPlayer(transform.name); // Вызываем public static метод UnRegisterPlayer из класса GameManager и передаем параметр transform.name (имя текущего игрока)
    }
}
