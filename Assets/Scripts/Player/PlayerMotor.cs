using UnityEngine;

[RequireComponent(typeof(Rigidbody))] // Когда мы подключаем данный скрипт мы должны также добавить компонент типа Rigidbody
// Соответственно, как только мы к какому либо объекту подключаем скрипт PlayerMotor, он подключает также автоматически Rigidbody

public class PlayerMotor : MonoBehaviour // Применяем действия к игроку
{
    [SerializeField] // Делает возможным редактирование в Unity значение поля cam
    private Camera cam; // Создадим переменную cam типа Camera
    private Rigidbody rb; // Создадим переменную rb типа Rigidbody

    private Vector3 velocity = Vector3.zero; // Создадим пустую переменную velocity типа Vector3, 
    // в которую в дальнейшем будем записывать значение переменной velocity из класса PlayerController
    private Vector3 rotation = Vector3.zero; // Создадим пустую переменную rotation типа Vector3
    // в которую в дальнейшем будем записывать значение переменной rotation из класса PlayerController
    private Vector3 camRotation = Vector3.zero; // Создадим пустую переменную camRotation типа Vector3
    // в которую в дальнейшем будем записывать значение переменной camRotation из класса PlayerController

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Присвоим переменной rb компонент Rigidbody
        // Так как мы привязали компонент в самом начале, нам не нужно делать дополнительные проверки на существование компонента
    }

    public void Move(Vector3 _velocity) // Создадим метод Move для движения игрока с параметром _velocity
    {
        velocity = _velocity; // Переменной velocity присвоим значение переменной _velocity, которое передается из вызова метода в классе PlayerController
    }

    public void Rotate(Vector3 _rotation) // Создадим метод Rotate для движения взгяда игрока по оси X с параметром _rotation
    {
        rotation = _rotation; // Переменной rotation присвоим значение переменной _rotation, которое передается из вызова метода в классе PlayerController
    }

    public void RotateCam(Vector3 _camRotation) // Создадим метод RotateCam для движения взгяда камеры по оси Y с параметром _camRotation
    {
        camRotation = _camRotation; // Переменной camRotation присвоим значение переменной _camRotation, которое передается из вызова метода в классе PlayerController
    }

    void FixedUpdate() // Функция типа Update(), которая вызывается каждые 0.02 секунды (используется для плавного движения физических объектов)
    // Обычный Update() вызывается не в определенное время
    {
        PerformMove(); // Вызываем метод PerformMove()
        PerformRotate(); // Вызываем метод PerformRotate()
    }

    void PerformMove() // Создаем метод PerformMove()
    {
        if (velocity != Vector3.zero) // Проверяем не равна ли переменная velocity вектору с параметрами (0, 0, 0), чтобы не двигать игрока, если он не нажимает кнопки
        {
            // Двигаем игрока до тех пор, пока он не столкнется с другим твердым объектом с помощью функции MovePosition()
            // Другие функции двигают игрока, даже если он столнулся с другим твердым объектом
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime); // Передаем в параметры текущую позицию твердого тела (игрока) + velocity (направление движения игрока)
            // и * на Time.fixedDeltaTime (благодаря этой функции игрок будет двигаться плавно по фреймам с течением времени)
        }
    }

    void PerformRotate() // Создаем метод PerformRotate()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation)); // Передаем в параметры текущую позицию вгляда игрока по оси X
        // * Quaternion.Euler(rotation) (направляем взгляд игрока по оси X на величину rotation со скоростью lookSpeed)  

        if (cam != null) // Делаем проверку, камера должна существовать
        {
            cam.transform.Rotate(-camRotation); // Поворачиваем камеру игрока по оси Y, относительно текущего взгляда камеры
            // направляем взгляд камеры игрока по оси Y на величину rotationCamera со скоростью lookSpeed. Знак "-" ставим, чтобы избежать инверсии мыши
        }
    }
}
