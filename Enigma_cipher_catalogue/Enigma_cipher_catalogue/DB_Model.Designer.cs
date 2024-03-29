﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

[assembly: EdmSchemaAttribute()]

namespace Enigma_cipher_catalogue
{
    #region Контексты
    
    /// <summary>
    /// Нет доступной документации по метаданным.
    /// </summary>
    public partial class Cipher_catEntities : ObjectContext
    {
        #region Конструкторы
    
        /// <summary>
        /// Инициализирует новый объект Cipher_catEntities, используя строку соединения из раздела "Cipher_catEntities" файла конфигурации приложения.
        /// </summary>
        public Cipher_catEntities() : base("name=Cipher_catEntities", "Cipher_catEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Инициализация нового объекта Cipher_catEntities.
        /// </summary>
        public Cipher_catEntities(string connectionString) : base(connectionString, "Cipher_catEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Инициализация нового объекта Cipher_catEntities.
        /// </summary>
        public Cipher_catEntities(EntityConnection connection) : base(connection, "Cipher_catEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Разделяемые методы
    
        partial void OnContextCreated();
    
        #endregion
    
        #region Свойства ObjectSet
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        public ObjectSet<Ciphers_Table> Ciphers_Table
        {
            get
            {
                if ((_Ciphers_Table == null))
                {
                    _Ciphers_Table = base.CreateObjectSet<Ciphers_Table>("Ciphers_Table");
                }
                return _Ciphers_Table;
            }
        }
        private ObjectSet<Ciphers_Table> _Ciphers_Table;

        #endregion
        #region Методы AddTo
    
        /// <summary>
        /// Устаревший метод для добавления новых объектов в набор EntitySet Ciphers_Table. Взамен можно использовать метод .Add связанного свойства ObjectSet&lt;T&gt;.
        /// </summary>
        public void AddToCiphers_Table(Ciphers_Table ciphers_Table)
        {
            base.AddObject("Ciphers_Table", ciphers_Table);
        }

        #endregion
    }
    

    #endregion
    
    #region Сущности
    
    /// <summary>
    /// Нет доступной документации по метаданным.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Cipher_catModel", Name="Ciphers_Table")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Ciphers_Table : EntityObject
    {
        #region Фабричный метод
    
        /// <summary>
        /// Создание нового объекта Ciphers_Table.
        /// </summary>
        /// <param name="id">Исходное значение свойства ID.</param>
        /// <param name="cycle">Исходное значение свойства Cycle.</param>
        /// <param name="current_pos">Исходное значение свойства Current_pos.</param>
        /// <param name="start_set">Исходное значение свойства Start_set.</param>
        public static Ciphers_Table CreateCiphers_Table(global::System.Int64 id, global::System.String cycle, global::System.String current_pos, global::System.String start_set)
        {
            Ciphers_Table ciphers_Table = new Ciphers_Table();
            ciphers_Table.ID = id;
            ciphers_Table.Cycle = cycle;
            ciphers_Table.Current_pos = current_pos;
            ciphers_Table.Start_set = start_set;
            return ciphers_Table;
        }

        #endregion
        #region Свойства-примитивы
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (_ID != value)
                {
                    OnIDChanging(value);
                    ReportPropertyChanging("ID");
                    _ID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("ID");
                    OnIDChanged();
                }
            }
        }
        private global::System.Int64 _ID;
        partial void OnIDChanging(global::System.Int64 value);
        partial void OnIDChanged();
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Cycle
        {
            get
            {
                return _Cycle;
            }
            set
            {
                OnCycleChanging(value);
                ReportPropertyChanging("Cycle");
                _Cycle = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Cycle");
                OnCycleChanged();
            }
        }
        private global::System.String _Cycle;
        partial void OnCycleChanging(global::System.String value);
        partial void OnCycleChanged();
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Current_pos
        {
            get
            {
                return _Current_pos;
            }
            set
            {
                OnCurrent_posChanging(value);
                ReportPropertyChanging("Current_pos");
                _Current_pos = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Current_pos");
                OnCurrent_posChanged();
            }
        }
        private global::System.String _Current_pos;
        partial void OnCurrent_posChanging(global::System.String value);
        partial void OnCurrent_posChanged();
    
        /// <summary>
        /// Нет доступной документации по метаданным.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Start_set
        {
            get
            {
                return _Start_set;
            }
            set
            {
                OnStart_setChanging(value);
                ReportPropertyChanging("Start_set");
                _Start_set = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Start_set");
                OnStart_setChanged();
            }
        }
        private global::System.String _Start_set;
        partial void OnStart_setChanging(global::System.String value);
        partial void OnStart_setChanged();

        #endregion
    
    }

    #endregion
    
}
