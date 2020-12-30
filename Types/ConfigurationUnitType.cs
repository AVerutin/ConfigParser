namespace ConfigParser.Types
{
    public enum ConfigurationUnitType
    {
        MillConfig = 0, // Общие параметры стана
        Connection,     // Параметры подключения
        TcpServer,      // TCP-сервер
        MtsService,     // Параметры подключения к MTS Service
        Signal,         // Сигнал 
        Subscription,   // Подписка
        DataBlock,      // Блок данных
        Thread,         // Нить
        Rollgang,       // Рольганг
        Label,          // Метка
        Sensor,         // Датчик
        Stopper,        // Упор
        LinearMoving,   // Агрегат линейного перемещения 
        StepperMoving,  // Агрегат шагающего перемещения
        Deleter,        // Удаление застравших
        Cage,           // Клеть
        TechnicalUnit,  // Техузел
        Aggregate,      // Агрегат
        IngotParams,    // Параметры ЕУ
        Default         // Не определено
    }
}