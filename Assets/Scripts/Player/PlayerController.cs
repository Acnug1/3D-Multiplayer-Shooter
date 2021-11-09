using UnityEngine;

[RequireComponent(typeof(PlayerMotor))] // Подключаем в скрипте PlayerController скрипт PlayerMotor, в котором подключается RigidBody для объекта "Игрок"
public class PlayerController : MonoBehaviour // Отслеживает действия игрока
{
    [SerializeField] // Делает возможным редактирование в Unity значение поля speed,
    // при этом чтобы оно оставалось закрытым внутри кода из других классов, для повышения его безопасности
    private float speed = 5f; // Задаем private переменную скорости игрока

    [SerializeField] // Делает возможным редактирование в Unity значение поля lookSpeed
    private float lookSpeed = 3f; // Задаем private переменную скорости обзора игрока

    private PlayerMotor motor; // Создаем экземпляр класса PlayerMotor и запишем его в переменную motor

    // Start is called before the first frame update
    void Start()
    {
        motor = GetComponent<PlayerMotor>(); // Присвоим значение для нашей переменной motor (возвращает компонент PlayerMotor)
        // Так как мы привязали компонент в самом начале, нам не нужно делать дополнительные проверки на существование компонента
    }

    // Update is called once per frame
    void Update()
    {
        // Значения для переменных движения игрока берутся следующим образом:
        // В Unity нажимаем Edit -> Project Settings -> Input
        float xMov = Input.GetAxisRaw("Horizontal"); // Задаем переменную для движения игрока по горизонтали (-1..1). От значения переменной зависит, куда движется игрок
        float zMov = Input.GetAxisRaw("Vertical"); // Задаем переменную для движения игрока по вертикали (-1..1). От значения переменной зависит, куда движется игрок

        // Применяем движение к объекту игрока
        Vector3 movHor = transform.right * xMov; // Движение игрока по оси X влево или вправо, в зависимости от значения переменной xMov (горизонтально). Vector3 тип переменной
        // Значение transform.right по умолчанию (1, 0, 0) - игрок движется вправо. Дает результат (- 1, 0, 0), если игрок движется влево и (0, 0, 0), если игрок не нажал на клавишу
        Vector3 movVer = transform.forward * zMov; // Движение игрока по оси Z вверх или вниз, в зависимости от значения переменной zMov (вертикально). Vector3 тип переменной
        // Значение transform.forward по умолчанию (0, 0, 1) - игрок движется ввверх. Дает результат (0, 0, -1), если игрок движется вниз и (0, 0, 0), если игрок не нажал на клавишу

        // Зададим возможность игроку двигаться в любом направлении (по любой оси движения) вперед, назад, вправо, влево
        Vector3 velocity = (movHor + movVer).normalized * speed; // функция normalized делает сумму векторов (movHor + movVer) в диапазоне от -1 до 1
        // Например, при движении игрока вперед и вправо значение вектора будет равно (1, 0, 1), вперед и влево (-1, 0, 1), назад (0, 0, -1) и т.д.

        motor.Move(velocity); // Двигаем объект, вызывая метод Move и передавая параметр velocity для объекта motor

        float yRot = Input.GetAxisRaw("Mouse X"); // Отслеживаем наши передвижения мыши по оси X
        Vector3 rotation = new Vector3(0f, yRot, 0f) * lookSpeed;  // Движение взгляда игрока по оси X влево или вправо, в зависимости от значения переменной yRot (горизонтально).
        // Создаем новый вектор Vector3, для движения со скоростью lookSpeed

        motor.Rotate(rotation); // Двигаем взгляд игрока по оси X, вызывая метод Rotate и передавая параметр rotation для объекта motor

        float xRot = Input.GetAxisRaw("Mouse Y"); // Отслеживаем наши передвижения мыши по оси Y
      //  Debug.Log(xRot);
        Vector3 camRotation = new Vector3(xRot, 0f, 0f) * lookSpeed;  // Движение взгляда игрока (камеры) по оси Y вверх или вниз, в зависимости от значения переменной xRot (вертикально).
        // Создаем новый вектор Vector3, для движения со скоростью lookSpeed. В данном случае мы не поворачиваем игрока, а поворачиваем только камеру

        motor.RotateCam(camRotation); // Двигаем взгляд игрока по оси Y, вызывая метод RotateCam и передавая параметр camRotation для объекта motor
    }
}
