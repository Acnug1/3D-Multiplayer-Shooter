using UnityEngine;
using UnityEngine.Networking; // Подключаем мультиплеерную библиотеку Networking от движка Unity

public class PlayerShoot : NetworkBehaviour // Наследование будем делать не от основного класса MonoBehaviour, а от мультиплеерного класса NetworkBehaviour
{
    public Weapon weapon; // Создадим экземпляр класса weapon (public ссылка на наш класс Weapon. Её можно будет настроить в юнити)

    [SerializeField] // Задаем область видимости для юнити
    private Camera cam; // Создадим экземпляр класса cam, типа Camera наследуемого из базового класса NetworkBehaviour

    [SerializeField] // Задаем область видимости для юнити
    private LayerMask mask; // Создадим экземпляр класса mask, типа LayerMask наследуемого из базового класса NetworkBehaviour

    // Start is called before the first frame update
    void Start()
    {
        if (cam == null) // Если в юнити не определена камера
        {
            Debug.LogError("PlayerShoot: No camera!"); // Выдаем ошибку для игрока "в скрипте PlayerShoot: отсутствует камера"
            this.enabled = false; // Отключаем данный скрипт (ключевое слово this применяется к данному классу)
        }
    }

    // Update is called once per frame
    void Update() // функционал стрельбы реализован за счёт системы лучей - Raycast
    {
        if (Input.GetButtonDown("Fire1")) // Если игрок нажал клавишу ЛКМ (в юнити находится в Edit -> Project Settings -> Input -> Fire1)
        {
            Shoot(); // Вызываем метод Shoot
        }
    }

    [Client] // Данный метод будет вызываться исключительно на клиентском компьютере (у локального игрока)
    void Shoot()
    {
        // В эту переменную мы будем помещать всю информацию о выстреле (мы выстрелили, попали в какой-то объект, как он называется и т.д.)
        RaycastHit _hit; // Создаем переменную типа RaycastHit с именем _hit (тип берется из базового класса NetworkBehaviour)

        // Делаем проверку, попали ли мы в какой-либо объект после выстрела
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask)) // Пишем в проверке параметры (откуда мы пускаем пулю, т.е. из центра камеры, куда мы пускаем пулю, т.е. в направлении прямо)
        // В переменную _hit запишем всю информацию о нашем выстреле, с помощью ключевого слова out, далее укажем дальность выстрела, т.е. из объекта weapon сделаем ссылку на поле range
        // параметр mask используется, как слой (layer) у объектов в unity, чтобы мы могли попадать только в твердые видимые объекты с коллайдером (не считая других объектов с коллайдером)
        {
            if (_hit.collider.tag == "Player") // Если мы попали в игрока (проверяется тег коллайдера, в который мы попали выстрелом)
            {
                CmdPlayerShoot(_hit.collider.name, weapon.damage); // Вызываем метод CmdPlayerShoot и передаем имя коллайдера игрока и урон от оружия
            }
            else 
            {
                print("Мы выстрелили и попали в " + _hit.collider.name); // Выведем на экран сообщение, в котором выводится имя коллайдера, в который мы попали
            } 
        }
    }

    [Command] // Данный метод будет работать исключительно на сервере
    void CmdPlayerShoot(string _ID, float damage) // Создаем метод Cmd для обработки выстрелов на сервере (со строковым параметром ID). Приписку Cmd перед названием метода в данном случае использовать обязательно, иначе игра не будет работать!
    {
        Debug.Log("В игрока " + _ID + " выстрелили"); // Выводим в консоль сообщение: "В игрока такого-то выстрелили"

        Player player = GameManager.GetPlayer(_ID); // Мы берем из нашего словаря (с помощью метода GetPlayer) какого-то конкретного игрока и записываем его в экземпляр класса player
        player.TakeDamage(damage); // Вызываем для нашего экземпляра класса player метод нанесения урона на величину damage
    }
}
