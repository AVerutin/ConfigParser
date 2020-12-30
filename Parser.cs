using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ConfigParser.ConfigurationUnits;
using ConfigParser.Data;
using ConfigParser.Types;

namespace ConfigParser
{
    public class Parser
    {
        public MillConfigUnit MillConfigUnit { get; set; }
        public SubscriptionsUnit SubscriptionsUnit { get; set; }
        public Collection<DataBlockUnit> ListDataBlockUnits { get; }
        public Collection<SignalUnit> ListSignalUnits { get; }
        public Collection<ThreadUnit> ListThreadUnits { get; }
        public Collection<RollgangUnit> ListRollgangUnits { get; }
        public Collection<LabelUnit> ListLabelUnits { get; }
        public Collection<SensorUnit> ListSensorUnits { get; }
        public Collection<StopperUnit> ListStopperUnits { get; }
        public Collection<LinearDisplacementUnit> ListLinearDisplacementUnits { get; }
        public Collection<DeleterUnit> ListDeleterUnits { get; }
        public Collection<CageUnit> ListCagesUnits { get; }
        public Collection<IngotParametersUnit> ListIngotParametersUnits { get; }
        public Collection<ConnectionStringUnit> ListConnectionStringUnits { get; }
        public Collection<AggregateUnit> ListAggregateUnits { get; }
        public Collection<TechnicalUnit> ListTechnicalUnits { get; }
        
        private Collection<ProductionThread> _listProductionThreads;

        private readonly string _cfgFileName;

        /// <summary>
        /// Открыть конфигурационный файл системы Дата-Трек
        /// </summary>
        /// <param name="fileName">Имя конфигурационного файла</param>
        public Parser(string fileName)
        {
            _listProductionThreads = new Collection<ProductionThread>();
            _cfgFileName = fileName.Trim();
            
            MillConfigUnit = new MillConfigUnit();
            SubscriptionsUnit = new SubscriptionsUnit();
            ListDataBlockUnits = new Collection<DataBlockUnit>();
            ListSignalUnits = new Collection<SignalUnit>();
            ListThreadUnits = new Collection<ThreadUnit>();
            ListRollgangUnits = new Collection<RollgangUnit>();
            ListLabelUnits = new Collection<LabelUnit>();
            ListSensorUnits = new Collection<SensorUnit>();
            ListStopperUnits = new Collection<StopperUnit>();
            ListLinearDisplacementUnits = new Collection<LinearDisplacementUnit>();
            ListDeleterUnits = new Collection<DeleterUnit>();
            ListCagesUnits = new Collection<CageUnit>();
            ListIngotParametersUnits = new Collection<IngotParametersUnit>();
            ListConnectionStringUnits = new Collection<ConnectionStringUnit>();
            ListAggregateUnits = new Collection<AggregateUnit>();
            ListTechnicalUnits = new Collection<TechnicalUnit>();            
        }
        
        /// <summary>
        /// Создание новой пустой конфигурации
        /// </summary>
        public Parser()
        {
            _listProductionThreads = new Collection<ProductionThread>();
            MillConfigUnit = new MillConfigUnit();
            SubscriptionsUnit = new SubscriptionsUnit();
            ListDataBlockUnits = new Collection<DataBlockUnit>();
            ListSignalUnits = new Collection<SignalUnit>();
            ListThreadUnits = new Collection<ThreadUnit>();
            ListRollgangUnits = new Collection<RollgangUnit>();
            ListLabelUnits = new Collection<LabelUnit>();
            ListSensorUnits = new Collection<SensorUnit>();
            ListStopperUnits = new Collection<StopperUnit>();
            ListLinearDisplacementUnits = new Collection<LinearDisplacementUnit>();
            ListDeleterUnits = new Collection<DeleterUnit>();
            ListCagesUnits = new Collection<CageUnit>();
            ListIngotParametersUnits = new Collection<IngotParametersUnit>();
            ListConnectionStringUnits = new Collection<ConnectionStringUnit>();
            ListAggregateUnits = new Collection<AggregateUnit>();
            ListTechnicalUnits = new Collection<TechnicalUnit>();            
        }
        
        /// <summary>
        /// Разбор конфигурационного файла на объекты
        /// </summary>
        /// <exception cref="NullReferenceException">Неверный формат конфигурационного файла</exception>
        public void Parse()
        {
            if (String.IsNullOrEmpty(_cfgFileName))
            {
                return;
            }
            
            List<ConfigurationUnit> objectsList = new List<ConfigurationUnit>();
            Stack<ConfigurationUnit> subobjects = new Stack<ConfigurationUnit>();
            bool openedMainObject = false;
            int subObjectsCount = 0;
            bool closedSubObject = true;
            string line; 
            ConfigurationUnit currConfigurationUnit = null;
            
            // Открываем файл конфигурации для чтения и читаем его построчно
            StreamReader reader = new StreamReader(_cfgFileName, System.Text.Encoding.Default);
            
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Replace("\t", " ").Trim();

                if (line == "" || line.StartsWith("//") || line == "(")
                    continue;

                if (line.Contains("=") && currConfigurationUnit != null)
                {
                    // Разбираем параметры объекта по первому вхождению символа '='
                    int separator = line.IndexOf('=');
                    string key = line.Substring(0, separator);
                    string val = line.Substring(separator + 1);
                    currConfigurationUnit.Parameters[key] = val;
                }
                else
                {
                    // Определяем тип операции с объектом
                    if (line == ")")
                    {
                        if(openedMainObject)
                        {
                            // Закрывем объект
                            if (subObjectsCount>0)
                            {
                                // Закрываем подобъект
                                if(subObjectsCount>1)
                                {
                                    // Забираем очередной подобъект из стека
                                    ConfigurationUnit topLevel = subobjects.Pop();
                                    topLevel.SubObjects.Add(currConfigurationUnit);
                                    currConfigurationUnit = topLevel;
                                    subObjectsCount--;
                                }
                                else
                                {
                                    // В стеке нет подобъектов, последний подобъект в переменной currObject
                                    ConfigurationUnit topLevel = objectsList[objectsList.Count-1];
                                    objectsList.Remove(topLevel);
                                    topLevel.SubObjects.Add(currConfigurationUnit);
                                    currConfigurationUnit = topLevel;
                                    subObjectsCount--;
                                    closedSubObject = true;
                                }
                            }
                            else
                            {
                                // Закрываем главный объект
                                if (!closedSubObject)
                                {
                                    ConfigurationUnit sub = subobjects.Pop();
                                    currConfigurationUnit?.SubObjects.Add(sub);
                                    closedSubObject = true;
                                }
                                objectsList.Add(currConfigurationUnit);
                                currConfigurationUnit = null;
                                openedMainObject = false;
                            }
                        }
                        else
                        {
                            // Нет созданных объектов
                            throw new NullReferenceException("Ошибка при разборе конфигурационного файла");
                        }
                    }
                    else
                    {
                        // Открываем объект
                        if (openedMainObject)
                        {
                            if(subObjectsCount==0)
                            {
                                // Это главный объект, и подобъектов пока нет
                                // Сохраняем главный объект и создаем подобъект
                                objectsList.Add(currConfigurationUnit);
                                currConfigurationUnit = new ConfigurationUnit();
                                ConfigurationUnitType type = GetObjectType(line);
                                currConfigurationUnit.Name = line;
                                currConfigurationUnit.Type = type;
                                closedSubObject = false;
                                subObjectsCount++;
                            }
                            else
                            {
                                // Это подобъект, сохроаняем его в стеке и создаем новый подобъект
                                subobjects.Push(currConfigurationUnit);
                                currConfigurationUnit = new ConfigurationUnit();
                                ConfigurationUnitType type = GetObjectType(line);
                                currConfigurationUnit.Name = line;
                                currConfigurationUnit.Type = type;
                                subObjectsCount++;
                                closedSubObject = false;
                            }
                        }
                        else
                        {
                            // Это главный объект
                            currConfigurationUnit = new ConfigurationUnit();
                            ConfigurationUnitType type = GetObjectType(line);
                            currConfigurationUnit.Name = line;
                            currConfigurationUnit.Type = type;
                            openedMainObject = true;
                        }
                    }
                }
            }

            reader.Close();

            foreach (ConfigurationUnit item in objectsList)
            {
                switch (item.Type)
                {
                    case ConfigurationUnitType.MillConfig: 
                        MillConfigUnit = new MillConfigUnit(item); 
                        break;
                    case ConfigurationUnitType.Subscription: 
                        SubscriptionsUnit = new SubscriptionsUnit(item);
                        break;
                    case ConfigurationUnitType.DataBlock:
                        ListDataBlockUnits.Add(new DataBlockUnit(item));
                        break;
                    case ConfigurationUnitType.Signal:
                        ListSignalUnits.Add(new SignalUnit(item));
                        break;
                    case ConfigurationUnitType.Thread:
                        ListThreadUnits.Add(new ThreadUnit(item));
                        break;
                    case ConfigurationUnitType.Rollgang:
                        ListRollgangUnits.Add(new RollgangUnit(item));
                        break;
                    case ConfigurationUnitType.Label:
                        ListLabelUnits.Add(new LabelUnit(item));
                        break;
                    case ConfigurationUnitType.Sensor:
                        ListSensorUnits.Add(new SensorUnit(item));
                        break;
                    case ConfigurationUnitType.Stopper:
                        ListStopperUnits.Add(new StopperUnit(item));
                        break;
                    case ConfigurationUnitType.LinearMoving:
                        ListLinearDisplacementUnits.Add(new LinearDisplacementUnit(item));
                        break;
                    case ConfigurationUnitType.Deleter:
                        ListDeleterUnits.Add(new DeleterUnit(item));
                        break;
                    case ConfigurationUnitType.Cage:
                        ListCagesUnits.Add(new CageUnit(item));
                        break;
                    case ConfigurationUnitType.IngotParams:
                        ListIngotParametersUnits.Add(new IngotParametersUnit(item));
                        break;
                    case ConfigurationUnitType.Connection:
                        ListConnectionStringUnits.Add(new ConnectionStringUnit(item));
                        break;
                    case ConfigurationUnitType.Aggregate:
                        ListAggregateUnits.Add(new AggregateUnit(item));
                        break;
                    case ConfigurationUnitType.TechnicalUnit:
                        ListTechnicalUnits.Add(new TechnicalUnit(item));
                        break;
                }
            }
        }

        /// <summary>
        /// Получить тип объекта по его имени
        /// </summary>
        /// <param name="name">Имя объекта</param>
        /// <returns>Тип объекта</returns>
        private ConfigurationUnitType GetObjectType(string name)
        {
            ConfigurationUnitType res = ConfigurationUnitType.Default;
            switch (name.ToUpper())
            {
                case "ОБЩИЕПАРАМЕТРЫСТАНА":
                    res = ConfigurationUnitType.MillConfig;
                    break;
                case "СИГНАЛ":
                    res = ConfigurationUnitType.Signal;
                    break;
                case "НИТЬ":
                    res = ConfigurationUnitType.Thread;
                    break;
                case "ДАТЧИК":
                    res = ConfigurationUnitType.Sensor;
                    break;
                case "РОЛЬГАНГ":
                    res = ConfigurationUnitType.Rollgang;
                    break;
                case "ПОДПИСКИ":
                    res = ConfigurationUnitType.Subscription;
                    break;
                case "БЛОКДАННЫХ":
                    res = ConfigurationUnitType.DataBlock;
                    break;
                case "МЕТКА":
                    res = ConfigurationUnitType.Label;
                    break;
                case "УПОР":
                    res = ConfigurationUnitType.Stopper;
                    break;
                case "АГРЕГАТЛИНЕЙНОГОПЕРЕМЕЩЕНИЯ":
                    res = ConfigurationUnitType.LinearMoving;
                    break;
                case "АГРЕГАТШАГОВОГОПЕРЕМЕЩЕНИЯ":
                    res = ConfigurationUnitType.StepperMoving;
                    break;
                case "УДАЛЕНИЕЗАСТРЯВШИХ":
                    res = ConfigurationUnitType.Deleter;
                    break;
                case "КЛЕТЬ":
                    res = ConfigurationUnitType.Cage;
                    break;
                case "ТЕХУЗЕЛ":
                    res = ConfigurationUnitType.TechnicalUnit;
                    break;
                case "АГРЕГАТ":
                    res = ConfigurationUnitType.Aggregate;
                    break;
                case "ПАРАМЕТРЕУ":
                    res = ConfigurationUnitType.IngotParams;
                    break;
                case "ПОДКЛЮЧЕНИЕ":
                    res = ConfigurationUnitType.Connection;
                    break;
                case "TCPSERVER":
                    res = ConfigurationUnitType.TcpServer;
                    break;
                case "MTSSERVICE":
                    res = ConfigurationUnitType.MtsService;
                    break;
            }

            return res;
        }

        /// <summary>
        /// Заполнить производственные линии объектами из конфигурационного файла
        /// </summary>
        public void FillProductionThreads()
        {
            // Обходим список нитей и для каждой нити обходим списки объектов,
            // которые могут находиться на нити. 

            for (int t=0; t<ListThreadUnits.GetItemsCount(); t++)
            {
                ThreadUnit thread = ListThreadUnits[t];
                ProductionThread productionThread = new ProductionThread();
                
                // Обходим список объектов Рольганг
                for (int i = 0; i < ListRollgangUnits.GetItemsCount(); i++)
                {
                    RollgangUnit rollgang = ListRollgangUnits[i];
                    if (rollgang.ThreadNumber == thread.ThreadNumber)
                    {
                        productionThread.ListRollgangUnits.Add(rollgang);
                    }
                }

                // Обходим список объектов Метка
                for (int i = 0; i < ListLabelUnits.GetItemsCount(); i++)
                {
                    LabelUnit label = ListLabelUnits[i];
                    if (label.ThreadNumber == thread.ThreadNumber)
                    {
                        productionThread.ListLabelUnits.Add(label);
                    }
                }

                // Обходим список объектов Датчик
                for (int i = 0; i < ListSensorUnits.GetItemsCount(); i++)
                {
                    SensorUnit sensor = ListSensorUnits[i];
                    if (sensor.ThreadNumber == thread.ThreadNumber)
                    {
                        productionThread.ListSensorUnits.Add(sensor);
                    }
                }

                // Обходим список объектов Упор
                for (int i = 0; i < ListStopperUnits.GetItemsCount(); i++)
                {
                    StopperUnit stopper = ListStopperUnits[i];
                    if (stopper.ThreadNumber == thread.ThreadNumber)
                    {
                        productionThread.ListStopperUnits.Add(stopper);
                    }
                }

                // Обходим список объектов АгрегатЛинейногоПеремещения
                for (int i = 0; i < ListLinearDisplacementUnits.GetItemsCount(); i++)
                {
                    LinearDisplacementUnit linear = ListLinearDisplacementUnits[i];
                    if (linear.ThreadNumber == thread.ThreadNumber)
                    {
                        productionThread.ListLinearDisplacementUnits.Add(linear);
                    }
                }

                // Обходим список объектов УдалениеЗастрявших
                for (int i = 0; i < ListDeleterUnits.GetItemsCount(); i++)
                {
                    DeleterUnit deleter = ListDeleterUnits[i];
                    if (deleter.ThreadNumber == thread.ThreadNumber)
                    {
                        productionThread.ListDeleterUnits.Add(deleter);
                    }
                }

                // Обходим список объектов Клеть
                for (int i = 0; i < ListCagesUnits.GetItemsCount(); i++)
                {
                    CageUnit cage = ListCagesUnits[i];
                    if (cage.ThreadNumber == thread.ThreadNumber)
                    {
                        productionThread.ListCagesUnits.Add(cage);
                    }
                }

                // Обходим список объектов ТехУзел
                for (int i = 0; i < ListTechnicalUnits.GetItemsCount(); i++)
                {
                    TechnicalUnit tech = ListTechnicalUnits[i];
                    if (tech.ThreadNumber == thread.ThreadNumber)
                    {
                        productionThread.ListTechnicalUnits.Add(tech);
                    }
                }

                productionThread.Uid = thread.Uid;
                productionThread.Name = thread.Name;
                productionThread.ThreadNumber = thread.ThreadNumber;
                productionThread.Direction = thread.Direction;
                productionThread.StartPos = thread.StartPos;
                productionThread.FinishPos = thread.FinishPos;
                productionThread.StopOnEnds = thread.StopOnEnds;
                productionThread.PrevThread = thread.PrevThread;
                productionThread.NextThread = thread.NextThread;
                
                _listProductionThreads.Add(productionThread);
            }
        }

        /// <summary>
        /// Удалить линию производства по имени
        /// </summary>
        /// <param name="name">Имя линии производства</param>
        /// <returns></returns>
        public void DeleteProductionThreadByName(string name)
        {
            int threadNumber = 0;

            FillProductionThreads();

            // Находим номер удаляемой нити
            foreach(ThreadUnit thread in ListThreadUnits)
            {
                if(thread.Name == name)
                {
                    threadNumber = thread.ThreadNumber;
                    break;
                }
            }

            foreach (ProductionThread productionThread in _listProductionThreads)
            {
                if (productionThread.Name == name)
                {
                    // Обходим список объектов, которые могут находиться на нити. 
                    // и удаляем объекты, принадлежащие удаляемой нити

                    // Обходим список объектов Рольганг
                    for (int i = 0; i < ListRollgangUnits.GetItemsCount(); i++)
                    {
                        if (ListRollgangUnits[i].ThreadNumber == threadNumber)
                        {
                            ListRollgangUnits.RemoveItem(i);
                        }
                    }

                    // Обходим список объектов Метка
                    for (int i = 0; i < ListLabelUnits.GetItemsCount(); i++)
                    {
                        if (ListLabelUnits[i].ThreadNumber == threadNumber)
                        {
                            ListLabelUnits.RemoveItem(i);
                        }
                    }

                    // Обходим список объектов Датчик
                    for (int i = 0; i < ListSensorUnits.GetItemsCount(); i++)
                    {
                        if (ListSensorUnits[i].ThreadNumber == threadNumber)
                        {
                            ListSensorUnits.RemoveItem(i);
                        }
                    }

                    // Обходим список объектов Упор
                    for (int i = 0; i < ListStopperUnits.GetItemsCount(); i++)
                    {
                        if (ListStopperUnits[i].ThreadNumber == threadNumber)
                        {
                            ListStopperUnits.RemoveItem(i);
                        }
                    }

                    // Обходим список объектов АгрегатЛинейногоПеремещения
                    for (int i = 0; i < ListLinearDisplacementUnits.GetItemsCount(); i++)
                    {
                        if (ListLinearDisplacementUnits[i].ThreadNumber == threadNumber)
                        {
                            ListLinearDisplacementUnits.RemoveItem(i);
                        }
                    }

                    // Обходим список объектов УдалениеЗастрявших
                    for (int i = 0; i < ListDeleterUnits.GetItemsCount(); i++)
                    {
                        if (ListDeleterUnits[i].ThreadNumber == threadNumber)
                        {
                            ListDeleterUnits.RemoveItem(i);
                        }
                    }

                    // Обходим список объектов Клеть
                    for (int i = 0; i < ListCagesUnits.GetItemsCount(); i++)
                    {
                        if (ListCagesUnits[i].ThreadNumber == threadNumber)
                        {
                            ListCagesUnits.RemoveItem(i);
                        }
                    }

                    // Обходим список объектов ТехУзел
                    for (int i = 0; i < ListTechnicalUnits.GetItemsCount(); i++)
                    {
                        if (ListTechnicalUnits[i].ThreadNumber == threadNumber)
                        {
                            ListTechnicalUnits.RemoveItem(i);
                        }
                    }

                    // Обходим список объектов Производственная линия
                    for (int i = 0; i < ListThreadUnits.GetItemsCount(); i++)
                    {
                        if (ListThreadUnits[i].ThreadNumber == threadNumber)
                        {
                            ListThreadUnits.RemoveItem(i);
                        }
                    }

                    FillProductionThreads();
                }
            }
        }

        /// <summary>
        /// Удалить нить производства
        /// </summary>
        /// <param name="name">Наименование нити</param>
        /// <param name="cascade">Удалить также все элементы нити</param>
        /// <returns></returns>
        public void DeleteProductionThreadByNumber(int threadNumber)
        {
            foreach(ThreadUnit thread in ListThreadUnits)
            {
                if(thread.ThreadNumber == threadNumber)
                {
                    // Обходим список объектов, которые могут находиться на нити. 
                    // и удаляем объекты, принадлежащие удаляемой нити

                    // Обходим список объектов Рольганг
                    for (int i = 0; i < ListRollgangUnits.GetItemsCount(); i++)
                    {
                        RollgangUnit rollgang = ListRollgangUnits[i];
                        if (rollgang.ThreadNumber == threadNumber)
                        {
                            ListRollgangUnits.RemoveItem(i);
                        }
                    }

                    // Обходим список объектов Метка
                    for (int i = 0; i < ListLabelUnits.GetItemsCount(); i++)
                    {
                        LabelUnit label = ListLabelUnits[i];
                        if (label.ThreadNumber == threadNumber)
                        {
                            ListLabelUnits.RemoveItem(i);
                        }
                    }

                    // Обходим список объектов Датчик
                    for (int i = 0; i < ListSensorUnits.GetItemsCount(); i++)
                    {
                        SensorUnit sensor = ListSensorUnits[i];
                        if (sensor.ThreadNumber == threadNumber)
                        {
                            ListSensorUnits.RemoveItem(i);
                        }
                    }

                    // Обходим список объектов Упор
                    for (int i = 0; i < ListStopperUnits.GetItemsCount(); i++)
                    {
                        StopperUnit stopper = ListStopperUnits[i];
                        if (stopper.ThreadNumber == threadNumber)
                        {
                            ListStopperUnits.RemoveItem(i);
                        }
                    }

                    // Обходим список объектов АгрегатЛинейногоПеремещения
                    for (int i = 0; i < ListLinearDisplacementUnits.GetItemsCount(); i++)
                    {
                        LinearDisplacementUnit linear = ListLinearDisplacementUnits[i];
                        if (linear.ThreadNumber == threadNumber)
                        {
                            ListLinearDisplacementUnits.RemoveItem(i);
                        }
                    }

                    // Обходим список объектов УдалениеЗастрявших
                    for (int i = 0; i < ListDeleterUnits.GetItemsCount(); i++)
                    {
                        DeleterUnit deleter = ListDeleterUnits[i];
                        if (deleter.ThreadNumber == threadNumber)
                        {
                            ListDeleterUnits.RemoveItem(i);
                        }
                    }

                    // Обходим список объектов Клеть
                    for (int i = 0; i < ListCagesUnits.GetItemsCount(); i++)
                    {
                        CageUnit cage = ListCagesUnits[i];
                        if (cage.ThreadNumber == threadNumber)
                        {
                            ListCagesUnits.RemoveItem(i);
                        }
                    }

                    // Обходим список объектов ТехУзел
                    for (int i = 0; i < ListTechnicalUnits.GetItemsCount(); i++)
                    {
                        TechnicalUnit tech = ListTechnicalUnits[i];
                        if (tech.ThreadNumber == threadNumber)
                        {
                            ListTechnicalUnits.RemoveItem(i);
                        }
                    }

                }
            }
        }


        /// <summary>
        /// Получить максимальный номер идентификатора
        /// </summary>
        /// <returns>Максимальный номер идентификатора</returns>
        public int GetLastUid(List<BaseConfigUnit> units)
        {
            int res = 0;
            foreach (BaseConfigUnit unit in units)
            {
                if (unit.Uid > res)
                {
                    res = unit.Uid;
                }
            }

            return res;
        }

        /// <summary>
        /// Получить список сигналов из конфигурационного файла
        /// </summary>
        /// <returns>Список номеров сигналов</returns>
        public List<ushort> GetSignals()
        {
            List<ushort> res = new List<ushort>();

            for(int i=0;i<ListSignalUnits.GetItemsCount();i++)
            {
                SignalUnit signal = ListSignalUnits[i];
                res.Add((ushort)signal.Uid);
            }
            return res;
        }

        /// <summary>
        /// Получить список сигналов из конфигурационного файла с описанием
        /// </summary>
        /// <returns>Список сигналов</returns>
        public Dictionary<ushort, string> GetSignalsList()
        {
            Dictionary<ushort, string> res = new Dictionary<ushort, string>();
            
            for(int i=0;i<ListSignalUnits.GetItemsCount();i++)
            {
                SignalUnit signal = ListSignalUnits[i];
                res.Add((ushort)signal.Uid, signal.Name);
            }

            return res;
        }

        /// <summary>
        /// Сохранить конфигурацию в файл
        /// </summary>
        public bool SaveConfiguration()
        {
            // Вывод на экран количество элементов каждой коллекции
            bool res = false;

            if (!String.IsNullOrEmpty(_cfgFileName))
            {
                Console.WriteLine("MillConfigUnit = 1");
                Console.WriteLine("SubscriptionsUnits = 1");
                Console.WriteLine($"ListDataBlockUnits = {ListDataBlockUnits.GetItemsCount()}");
                Console.WriteLine($"ListSignalUnits = {ListSignalUnits.GetItemsCount()}");
                Console.WriteLine($"ListThreadUnits = {ListThreadUnits.GetItemsCount()}");
                Console.WriteLine($"ListRollgangUnits = {ListRollgangUnits.GetItemsCount()}");
                Console.WriteLine($"ListLabelUnits = {ListLabelUnits.GetItemsCount()}");
                Console.WriteLine($"ListSensorUnits = {ListSensorUnits.GetItemsCount()}");
                Console.WriteLine($"ListStopperUnits = {ListStopperUnits.GetItemsCount()}");
                Console.WriteLine($"ListLinearDisplacementUnits = {ListLinearDisplacementUnits.GetItemsCount()}");
                Console.WriteLine($"ListDeleterUnits = {ListDeleterUnits.GetItemsCount()}");
                Console.WriteLine($"ListCagesUnits = {ListCagesUnits.GetItemsCount()}");
                Console.WriteLine($"ListIngotParametersUnits = {ListIngotParametersUnits.GetItemsCount()}");
                Console.WriteLine($"ListConnectionStringUnits = {ListConnectionStringUnits.GetItemsCount()}");
                Console.WriteLine($"ListAggregateUnits = {ListAggregateUnits.GetItemsCount()}");
                Console.WriteLine($"ListTechnicalUnits = {ListTechnicalUnits.GetItemsCount()}");

                res = true;
            }

            return res;
        }

        #region DataBlockUnit
        
        /// <summary>
        /// Найти блок данных по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataBlockUnit FindDataBlock(string name)
        {
            DataBlockUnit res = null;
            foreach (DataBlockUnit item in ListDataBlockUnits)
            {
                if (item.Name == name)
                {
                    res = item;
                    break;
                }
            }

            return res;
        }

        /// <summary>
        /// Найти блок данных по имени
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public DataBlockUnit FindDataBlock(int uid)
        {
            DataBlockUnit res = null;
            foreach (DataBlockUnit item in ListDataBlockUnits)
            {
                if (item.Uid == uid)
                {
                    res = item;
                    break;
                }
            }

            return res;
        }
        
        /// <summary>
        /// Изменить параметры блока данных
        /// </summary>
        /// <param name="dataBlock">Блок данных с измененными параметрами</param>
        public void ReplaceDataBlockUnit(DataBlockUnit dataBlock)
        {
            for (int i=0; i<ListDataBlockUnits.GetItemsCount(); i++)
            {
                if (ListDataBlockUnits[i].Uid == dataBlock.Uid)
                {
                    ListDataBlockUnits[i] = dataBlock;
                    break;
                }
            }
        }
        
        #endregion

        #region ThreadUnit
        
        /// <summary>
        /// Найти линию производства по ее наименованию
        /// </summary>
        /// <param name="name">Наименование линии производства</param>
        /// <returns>Линия производства</returns>
        public ThreadUnit FindThreadByName(string name)
        {
            ThreadUnit res = null;
            foreach (ThreadUnit unit in ListThreadUnits)
            {
                if (unit.Name == name)
                {
                    res = unit;
                    break;
                }
            }

            return res;
        }
        
        /// <summary>
        /// Найти линию производства по ID
        /// </summary>
        /// <param name="uid">ID линии производства</param>
        /// <returns>Линия производства</returns>
        public ThreadUnit FindThreadUnitByUid(int uid)
        {
            ThreadUnit res = null;
            foreach (ThreadUnit item in _listProductionThreads)
            {
                if (item.Uid == uid)
                {
                    res = item;
                    break;
                }
            }

            return res;
        }

        /// <summary>
        /// Найти линию производства по номеру
        /// </summary>
        /// <param name="number">Номер линии производства</param>
        /// <returns>Линия производства</returns>
        public ThreadUnit FindThreadUnitByNumber(int number)
        {
            ThreadUnit res = null;
            foreach (ThreadUnit item in _listProductionThreads)
            {
                if (item.ThreadNumber == number)
                {
                    res = item;
                    break;
                }
            }

            return res;
        }

        /// <summary>
        /// Изменить параметры линии проиводства
        /// </summary>
        /// <param name="threadUnit">Линия производства с измененными параметрами</param>
        public void ReplaceThreadUnit(ThreadUnit threadUnit)
        {
            for (int i=0; i<ListThreadUnits.GetItemsCount(); i++)
            {
                if (ListThreadUnits[i].Uid == threadUnit.Uid)
                {
                    ListThreadUnits[i] = threadUnit;
                    break;
                }
            }
        }
        
        /// <summary>
        /// Получить максимальный номер производственной линии 
        /// </summary>
        /// <returns></returns>
        public int GetLastThreadNumber()
        {
            int res = 0;
            for(int i=0;i<ListThreadUnits.GetItemsCount();i++)
            {
                ThreadUnit thread = ListThreadUnits[i];
                if (thread.Uid > res)
                {
                    res = thread.ThreadNumber;
                }
                Debug.WriteLine(res);
            }

            return res;
        }


        #endregion

        #region ProductionThread

        /// <summary>
        /// Изменить параметры производственной линии
        /// </summary>
        /// <param name="productionThread">Производственная линия с измененными параметрами</param>
        public void ReplaceProductionThread(ProductionThread productionThread)
        {
            for (int i=0; i<_listProductionThreads.GetItemsCount(); i++)
            {
                if (_listProductionThreads[i].Uid == productionThread.Uid)
                {
                    _listProductionThreads[i] = productionThread;
                    break;
                }
            }
        }

        /// <summary>
        /// Найти производственную линию по ID
        /// </summary>
        /// <param name="uid">ID производственной линии</param>
        /// <returns>Производственная линия</returns>
        public ProductionThread FindProductionThreadByUid(int uid)
        {
            ProductionThread res = null;
            foreach (ProductionThread item in _listProductionThreads)
            {
                if (item.Uid == uid)
                {
                    res = item;
                    break;
                }
            }

            return res;
        }

        /// <summary>
        /// Найти производственную линию по номеру 
        /// </summary>
        /// <param name="number">Номер производственной линии</param>
        /// <returns>Производственная линия</returns>
        public ProductionThread FindProductionThreadByNumber(int number)
        {
            ProductionThread res = null;
            foreach (ProductionThread unit in ListThreadUnits)
            {
                if (unit.ThreadNumber == number)
                {
                    res = unit;
                    break;
                }
            }

            return res;
        }

        /// <summary>
        /// Найти линию производства по наименованию
        /// </summary>
        /// <param name="name">Наименовавние производственной линии</param>
        /// <returns>Производственная линия</returns>
        public ProductionThread FindProductionThreadByName(string name)
        {
            ProductionThread res = null;
            foreach (ProductionThread item in _listProductionThreads)
            {
                if (item.Name == name)
                {
                    res = item;
                    break;
                }
            }

            return res;
        }
        
        /// <summary>
        /// Получить список всех производственных линий
        /// </summary>
        /// <returns>Список производственных линий</returns>
        public List<ProductionThread> GetProductionThreads()
        {
            return _listProductionThreads.GetItems();
        }

        /// <summary>
        /// Получить производственную линию по ее номеру
        /// </summary>
        /// <param name="number">Номер производственной линии</param>
        /// <returns>Производственная линия</returns>
        public ProductionThread GetProcessThread(int number)
        {
            ProductionThread thread = null;
            
            for (int i = 0; i < _listProductionThreads.GetItemsCount(); i++)
            {
                if (_listProductionThreads[i].ThreadNumber == number)
                {
                    thread = _listProductionThreads[i];
                    break;
                }
            }

            return thread;
        }
        
        /// <summary>
        /// Получить количество производственных линий, описанных в конфигурационном файле
        /// </summary>
        /// <returns>Количество производственных линий</returns>
        public int GetProductionThreadsCount()
        {
            return ListThreadUnits.GetItemsCount();
        }
        
        #endregion

        #region Signals

        

        #endregion

        #region Sensors

        

        #endregion
        
        public object FindElementByUid(int uid, UnitsTypes type)
        {
            object res = null;
            
            switch (type)
            {
                case UnitsTypes.Thread:
                    ThreadUnit t = FindThreadUnitByUid(uid);
                    res = t;
                    break;
                case UnitsTypes.DataBlock:
                    DataBlockUnit d = FindDataBlock(uid);
                    res = d;
                    break;
            }

            return res;
        }

        /// <summary>
        /// Найти рольганг по наименованию
        /// </summary>
        /// <param name="name">Наименование рольганга</param>
        /// <returns>Рольганг</returns>
        public RollgangUnit FindRollgang(string name)
        {
            RollgangUnit result = new RollgangUnit();
            foreach(RollgangUnit rollgang in ListRollgangUnits)
            {
                if (rollgang.Name == name)
                    result = rollgang;
            }

            return result;
        }

        /// <summary>
        /// Найти рольганг по идентификатору
        /// </summary>
        /// <param name="uid">Идентификатор рольганга</param>
        /// <returns>Рольганг</returns>
        public RollgangUnit FindRollgang(int uid)
        {
            RollgangUnit result = new RollgangUnit();
            foreach (RollgangUnit rollgang in ListRollgangUnits)
            {
                if (rollgang.Uid == uid)
                    result = rollgang;
            }

            return result;
        }

        /// <summary>
        /// Найти метку по тексту
        /// </summary>
        /// <param name="name">Текст метки</param>
        /// <returns>Метка</returns>
        public LabelUnit FindLabel(string name)
        {
            LabelUnit result = new LabelUnit();
            foreach(LabelUnit label in ListLabelUnits)
            {
                if (label.Text == name)
                    result = label;
            }

            return result;
        }
        
        /// <summary>
        /// Найти сигнал по имени
        /// </summary>
        /// <param name="name">Наименование сигнала</param>
        /// <returns>Сигнал</returns>
        public SignalUnit FindSignal(string name)
        {
            SignalUnit result = new SignalUnit();
            foreach(SignalUnit signal in ListSignalUnits)
            {
                if (signal.Name == name)
                    result = signal;
            }

            return result;
        }

        /// <summary>
        /// Найти сигнал по идентификатору
        /// </summary>
        /// <param name="uid">идентификатору сигнала</param>
        /// <returns>Сигнал</returns>
        public SignalUnit FindSignal(int uid)
        {
            SignalUnit result = new SignalUnit();
            foreach(SignalUnit signal in ListSignalUnits)
            {
                if (signal.Uid == uid)
                    result = signal;
            }

            return result;
        }
        
        /// <summary>
        /// Получить количество всех найденных элементов конфигурации
        /// </summary>
        /// <returns>Количество найденных элементов</returns>
        public int GetElementsCount()
        {
            int res = 0;
            res += string.IsNullOrEmpty(MillConfigUnit.Name) ? 0 : 1;
            res += SubscriptionsUnit.Threads > 0 ? 1 : 0;
            res += ListDataBlockUnits.GetItemsCount();
            res += ListSignalUnits.GetItemsCount();
            res += ListThreadUnits.GetItemsCount();
            res += ListRollgangUnits.GetItemsCount();
            res += ListLabelUnits.GetItemsCount();
            res += ListSensorUnits.GetItemsCount();
            res += ListStopperUnits.GetItemsCount();
            res += ListLinearDisplacementUnits.GetItemsCount();
            res += ListDeleterUnits.GetItemsCount();
            res += ListCagesUnits.GetItemsCount();
            res += ListIngotParametersUnits.GetItemsCount();
            res += ListConnectionStringUnits.GetItemsCount();
            res += ListAggregateUnits.GetItemsCount();
            res += ListTechnicalUnits.GetItemsCount();

            return res;
        }
    }
}